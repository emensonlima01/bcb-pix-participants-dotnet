namespace Domain.Interfaces;

public interface IPixParticipantsSource
{
    IAsyncEnumerable<string> GetLinesAsync(CancellationToken cancellationToken = default);
}
