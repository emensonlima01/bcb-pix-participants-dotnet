using Domain.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Text;

namespace Infrastructure.PixParticipants;

public sealed class BcbPixParticipantsSource(HttpClient httpClient, IOptions<PixParticipantsOptions> options) : IPixParticipantsSource
{
    private readonly PixParticipantsOptions options = options.Value;

    public async Task<List<string>> GetLinesAsync(CancellationToken cancellationToken)
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = new StreamReader(responseStream, Encoding.GetEncoding("Windows-1252"), leaveOpen: false);

            var lines = new List<string>();
            while (true)
            {
                var line = await reader.ReadLineAsync(cancellationToken);
                if (line is null)
                {
                    break;
                }

                lines.Add(line);
            }

            return lines;
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
}
