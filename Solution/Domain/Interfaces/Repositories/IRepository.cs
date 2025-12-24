namespace Domain.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<T> StreamAllAsync(CancellationToken cancellationToken = default);
}
