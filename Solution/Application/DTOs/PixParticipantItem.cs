namespace Application.DTOs;

public enum PixParticipantKind
{
    Active,
    Adhesion
}

public sealed record PixParticipantItem(
    PixParticipantKind Kind,
    PixActiveParticipant? ActiveParticipant,
    PixAdhesionParticipant? AdhesionParticipant);
