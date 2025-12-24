namespace Domain.Entities;

public sealed class ActivePixParticipant : PixParticipant
{
    public string PaymentInitiation { get; init; } = string.Empty;
    public string CashoutFacilitator { get; init; } = string.Empty;
}
