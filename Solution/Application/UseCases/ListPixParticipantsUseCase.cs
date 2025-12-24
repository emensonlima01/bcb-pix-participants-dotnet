using Application.DTOs;
using Domain.Entities;
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
                var participant = new ActivePixParticipant
                {
                    Order = ParseOrder(parts[0]),
                    ShortName = parts[1],
                    Ispb = parts[2],
                    Cnpj = parts[3],
                    InstitutionType = parts[4],
                    BcbAuthorized = parts[5],
                    SpiParticipation = parts[6],
                    PixParticipation = parts[7],
                    PixMode = parts[8],
                    PaymentInitiation = parts[9],
                    CashoutFacilitator = parts[10]
                };
                yield return new PixParticipantItem(PixParticipantKind.Active, participant, null);
            }
            else if (section == PixSection.Adhesion)
            {
                var parts = SplitLine(rawLine, 10);
                var participant = new AdhesionPixParticipant
                {
                    Order = ParseOrder(parts[0]),
                    ShortName = parts[1],
                    Ispb = parts[2],
                    Cnpj = parts[3],
                    InstitutionType = parts[4],
                    BcbAuthorized = parts[5],
                    SpiParticipation = parts[6],
                    PixParticipation = parts[7],
                    PixMode = parts[8],
                    AdhesionStatus = parts[9]
                };
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
