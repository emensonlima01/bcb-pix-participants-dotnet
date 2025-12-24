using Domain.Entities;
using Domain.Interfaces.Services;

namespace Infrastructure.Parsers;

public sealed class ActivePixParticipantParser : ICsvLineParser<ActivePixParticipant>
{
    private const int ExpectedFieldCount = 11;

    public ActivePixParticipant Parse(string line)
    {
        var fields = CsvLineParserHelper.SplitAndTrim(line, ExpectedFieldCount);

        return new ActivePixParticipant
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
            PaymentInitiation = fields[9],
            CashoutFacilitator = fields[10]
        };
    }
}
