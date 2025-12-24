namespace Application.Interfaces;

public interface IUseCase<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IStreamUseCase<in TRequest, TResponse>
{
    IAsyncEnumerable<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IUseCase<TResponse>
{
    Task<TResponse> ExecuteAsync(CancellationToken cancellationToken = default);
}

public interface IStreamUseCase<TResponse>
{
    IAsyncEnumerable<TResponse> ExecuteAsync(CancellationToken cancellationToken = default);
}
