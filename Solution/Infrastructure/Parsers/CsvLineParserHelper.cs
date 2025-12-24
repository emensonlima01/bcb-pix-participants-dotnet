namespace Infrastructure.Parsers;

internal static class CsvLineParserHelper
{
    public static string[] SplitAndTrim(string line, int expectedCount)
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

    public static int? ParseOrder(string value)
    {
        return int.TryParse(value, out var number) ? number : null;
    }
}
