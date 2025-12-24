namespace Domain.Interfaces;

public interface IPixParticipantsSource
{
    Task<List<string>> GetLinesAsync(CancellationToken cancellationToken);
}
