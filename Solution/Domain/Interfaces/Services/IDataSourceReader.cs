namespace Domain.Interfaces.Services;

public interface IDataSourceReader
{
    IAsyncEnumerable<string> ReadLinesAsync(CancellationToken cancellationToken = default);
}
