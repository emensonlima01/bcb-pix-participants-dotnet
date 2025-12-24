using Domain.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Globalization;
using System.IO.Pipelines;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace Infrastructure.PixParticipants;

public sealed class BcbPixParticipantsSource(HttpClient httpClient, IOptions<PixParticipantsOptions> options) : IPixParticipantsSource
{
    private static readonly Encoding CsvEncoding = Encoding.GetEncoding("Windows-1252");
    private readonly PixParticipantsOptions options = options.Value;

    public async IAsyncEnumerable<string> GetLinesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(options.CsvUrl))
        {
            throw new InvalidOperationException("Pix participants CSV URL is not configured.");
        }

        var today = DateTime.Now;
        var response = await GetResponseAsync(options.CsvUrl, today, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            response.Dispose();
            response = await GetResponseAsync(options.CsvUrl, today.AddDays(-1), cancellationToken);
        }

        using (response)
        {
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var reader = PipeReader.Create(responseStream);
            await foreach (var line in ReadLinesAsync(reader, cancellationToken))
            {
                yield return line;
            }
        }
    }

    private Task<HttpResponseMessage> GetResponseAsync(string baseUrl, DateTime date, CancellationToken cancellationToken)
    {
        var url = BuildCsvUrl(baseUrl, date);
        return httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    }

    private static string BuildCsvUrl(string baseUrl, DateTime date)
    {
        var dateText = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        return $"{baseUrl}-{dateText}.csv";
    }

    private static async IAsyncEnumerable<string> ReadLinesAsync(
        PipeReader reader,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (true)
        {
            var readResult = await reader.ReadAsync(cancellationToken);
            var buffer = readResult.Buffer;

            while (TryReadLine(ref buffer, out var line))
            {
                if (line.IsEmpty)
                {
                    continue;
                }

                yield return DecodeLine(line);
            }

            reader.AdvanceTo(buffer.Start, buffer.End);

            if (readResult.IsCompleted)
            {
                break;
            }
        }

        await reader.CompleteAsync();
    }

    private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
    {
        var position = buffer.PositionOf((byte)'\n');
        if (position is null)
        {
            line = default;
            return false;
        }

        line = buffer.Slice(0, position.Value);
        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));

        if (line.Length > 0)
        {
            var lastByte = line.Slice(line.Length - 1, 1).FirstSpan[0];
            if (lastByte == (byte)'\r')
            {
                line = line.Slice(0, line.Length - 1);
            }
        }

        return true;
    }

    private static string DecodeLine(ReadOnlySequence<byte> line)
    {
        if (line.IsSingleSegment)
        {
            return CsvEncoding.GetString(line.FirstSpan);
        }

        var length = checked((int)line.Length);
        var buffer = ArrayPool<byte>.Shared.Rent(length);
        try
        {
            line.CopyTo(buffer);
            return CsvEncoding.GetString(buffer, 0, length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
