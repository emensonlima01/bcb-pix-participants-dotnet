using Domain.Entities;
using Domain.Interfaces.Services;

namespace Infrastructure.Parsers;

public sealed class AdhesionPixParticipantParser : ICsvLineParser<AdhesionPixParticipant>
{
    private const int ExpectedFieldCount = 10;

    public AdhesionPixParticipant Parse(string line)
    {
        var fields = CsvLineParserHelper.SplitAndTrim(line, ExpectedFieldCount);

        return new AdhesionPixParticipant
        {
            Order = CsvLineParserHelper.ParseOrder(fields[0]),
            ShortName = fields[1],
            Ispb = fields[2],
            Cnpj = fields[3],
            InstitutionType = fields[4],
            BcbAuthorized = fields[5],
            SpiParticipation = fields[6],
            PixParticipation = fields[7],
            PixMode = fields[8],
            AdhesionStatus = fields[9]
        };
    }
}
