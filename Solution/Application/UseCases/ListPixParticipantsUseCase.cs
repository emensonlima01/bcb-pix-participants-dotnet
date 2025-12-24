using Application.DTOs;
using Domain.Interfaces;

namespace Application.UseCases;

public sealed class ListPixParticipantsUseCase(IPixParticipantsSource source)
{
    public async IAsyncEnumerable<PixParticipantItem> Handle([global::System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var section = PixSection.None;
        var expectingHeader = false;

        await foreach (var rawLine in source.GetLinesAsync(cancellationToken).WithCancellation(cancellationToken))
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
                var participant = new PixActiveParticipant(
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
                    parts[10]);
                yield return new PixParticipantItem(PixParticipantKind.Active, participant, null);
            }
            else if (section == PixSection.Adhesion)
            {
                var parts = SplitLine(rawLine, 10);
                var participant = new PixAdhesionParticipant(
                    ParseOrder(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    parts[7],
                    parts[8],
                    parts[9]);
                yield return new PixParticipantItem(PixParticipantKind.Adhesion, null, participant);
            }
        }
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
