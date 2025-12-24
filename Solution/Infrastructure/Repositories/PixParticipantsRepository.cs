using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Infrastructure.Repositories;

public sealed class PixParticipantsRepository(
    IDataSourceReader dataSourceReader,
    ICsvLineParser<ActivePixParticipant> activeParser,
    ICsvLineParser<AdhesionPixParticipant> adhesionParser) : IPixParticipantsRepository
{
    public async IAsyncEnumerable<ActivePixParticipant> StreamActiveParticipantsAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var section = ParticipantSection.None;
        var expectingHeader = false;

        await foreach (var rawLine in dataSourceReader.ReadLinesAsync(cancellationToken))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var line = rawLine.Trim();

            if (IsActiveSectionHeader(line))
            {
                section = ParticipantSection.Active;
                expectingHeader = true;
                continue;
            }

            if (IsAdhesionSectionHeader(line))
            {
                section = ParticipantSection.Adhesion;
                expectingHeader = true;
                continue;
            }

            if (section == ParticipantSection.None)
            {
                continue;
            }

            if (expectingHeader)
            {
                expectingHeader = false;
                continue;
            }

            if (section == ParticipantSection.Active)
            {
                yield return activeParser.Parse(rawLine);
            }
        }
    }

    public async IAsyncEnumerable<AdhesionPixParticipant> StreamAdhesionParticipantsAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var section = ParticipantSection.None;
        var expectingHeader = false;

        await foreach (var rawLine in dataSourceReader.ReadLinesAsync(cancellationToken))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var line = rawLine.Trim();

            if (IsActiveSectionHeader(line))
            {
                section = ParticipantSection.Active;
                expectingHeader = true;
                continue;
            }

            if (IsAdhesionSectionHeader(line))
            {
                section = ParticipantSection.Adhesion;
                expectingHeader = true;
                continue;
            }

            if (section == ParticipantSection.None)
            {
                continue;
            }

            if (expectingHeader)
            {
                expectingHeader = false;
                continue;
            }

            if (section == ParticipantSection.Adhesion)
            {
                yield return adhesionParser.Parse(rawLine);
            }
        }
    }

    private static bool IsActiveSectionHeader(string line)
    {
        return line.StartsWith("Lista de participantes ativos do Pix", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsAdhesionSectionHeader(string line)
    {
        return line.StartsWith("Lista de institu", StringComparison.OrdinalIgnoreCase)
               && line.Contains("processo de ades", StringComparison.OrdinalIgnoreCase);
    }

    private enum ParticipantSection
    {
        None,
        Active,
        Adhesion
    }
}
