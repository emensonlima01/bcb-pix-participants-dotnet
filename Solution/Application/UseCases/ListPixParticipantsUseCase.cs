using Application.DTOs;
using System.Globalization;
using System.Net;
using System.Text;

namespace Application.UseCases;

public sealed class ListPixParticipantsUseCase
{
    private readonly string? csvUrl;
    private readonly HttpClient httpClient;

    public ListPixParticipantsUseCase(string? csvUrl, HttpClient httpClient)
    {
        this.csvUrl = csvUrl;
        this.httpClient = httpClient;
    }

    public async Task<PixParticipantsResponse> Handle(CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(csvUrl))
        {
            var lines = await DownloadCsvLinesWithFallbackAsync(csvUrl, cancellationToken);
            return ParseParticipants(lines);
        }

        throw new InvalidOperationException("Pix participants CSV URL is not configured.");
    }

    private async Task<List<string>> DownloadCsvLinesWithFallbackAsync(string baseUrl, CancellationToken cancellationToken)
    {
        var today = DateTime.Now;
        var todayUrl = BuildCsvUrl(baseUrl, today);
        var response = await httpClient.GetAsync(todayUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            response.Dispose();
            var yesterdayUrl = BuildCsvUrl(baseUrl, today.AddDays(-1));
            response = await httpClient.GetAsync(yesterdayUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        using (response)
        {
            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = new StreamReader(responseStream, Encoding.GetEncoding("Windows-1252"), leaveOpen: false);

            var lines = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync(cancellationToken);
                if (line is not null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }

    private static string BuildCsvUrl(string baseUrl, DateTime date)
    {
        const string token = "pix-";
        var index = baseUrl.LastIndexOf(token, StringComparison.OrdinalIgnoreCase);
        if (index < 0)
        {
            return baseUrl;
        }

        var dateText = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var start = index + token.Length;
        if (start >= baseUrl.Length)
        {
            return baseUrl + dateText;
        }

        var suffix = baseUrl.Substring(start);
        var hasEightDigits = suffix.Length >= 8;
        if (hasEightDigits)
        {
            for (var i = 0; i < 8; i++)
            {
                if (!char.IsDigit(suffix[i]))
                {
                    hasEightDigits = false;
                    break;
                }
            }
        }

        if (hasEightDigits)
        {
            return baseUrl.Substring(0, start) + dateText + suffix.Substring(8);
        }

        return baseUrl.Substring(0, start) + dateText + suffix;
    }

    private PixParticipantsResponse ParseParticipants(List<string> lines)
    {
        var active = new List<PixActiveParticipant>();
        var adhesion = new List<PixAdhesionParticipant>();

        var section = PixSection.None;
        var expectingHeader = false;

        foreach (var rawLine in lines)
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var line = rawLine.Trim();

            if (line.StartsWith("Lista de participantes ativos do Pix", StringComparison.OrdinalIgnoreCase))
            {
                section = PixSection.Active;
                expectingHeader = true;
                continue;
            }

            if (line.StartsWith("Lista de institu", StringComparison.OrdinalIgnoreCase)
                && line.Contains("processo de ades", StringComparison.OrdinalIgnoreCase))
            {
                section = PixSection.Adhesion;
                expectingHeader = true;
                continue;
            }

            if (section == PixSection.None)
            {
                continue;
            }

            if (expectingHeader)
            {
                expectingHeader = false;
                continue;
            }

            if (section == PixSection.Active)
            {
                var parts = SplitLine(rawLine, 11);
                active.Add(new PixActiveParticipant(
                    ParseOrder(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    parts[7],
                    parts[8],
                    parts[9],
                    parts[10]));
            }
            else if (section == PixSection.Adhesion)
            {
                var parts = SplitLine(rawLine, 10);
                adhesion.Add(new PixAdhesionParticipant(
                    ParseOrder(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    parts[7],
                    parts[8],
                    parts[9]));
            }
        }

        return new PixParticipantsResponse(active, adhesion);
    }

    private static string[] SplitLine(string line, int expectedCount)
    {
        var parts = line.Split(';', StringSplitOptions.None);
        for (var i = 0; i < parts.Length; i++)
        {
            parts[i] = parts[i].Trim();
        }

        if (parts.Length < expectedCount)
        {
            Array.Resize(ref parts, expectedCount);
            for (var i = 0; i < parts.Length; i++)
            {
                parts[i] ??= string.Empty;
            }
        }

        return parts;
    }

    private static int? ParseOrder(string value)
    {
        if (int.TryParse(value, out var number))
        {
            return number;
        }

        return null;
    }

    private enum PixSection
    {
        None,
        Active,
        Adhesion
    }
}
