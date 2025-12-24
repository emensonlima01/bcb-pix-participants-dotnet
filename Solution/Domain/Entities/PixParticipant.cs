namespace Domain.Entities;

public abstract class PixParticipant
{
    public int? Order { get; init; }
    public string ShortName { get; init; } = string.Empty;
    public string Ispb { get; init; } = string.Empty;
    public string Cnpj { get; init; } = string.Empty;
    public string InstitutionType { get; init; } = string.Empty;
    public string BcbAuthorized { get; init; } = string.Empty;
    public string SpiParticipation { get; init; } = string.Empty;
    public string PixParticipation { get; init; } = string.Empty;
    public string PixMode { get; init; } = string.Empty;
}
