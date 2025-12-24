using Domain.Entities;

namespace Application.DTOs;

public enum PixParticipantKind
{
    Active,
    Adhesion
}

public sealed record PixParticipantItem(
    PixParticipantKind Kind,
    ActivePixParticipant? ActiveParticipant,
    AdhesionPixParticipant? AdhesionParticipant);
