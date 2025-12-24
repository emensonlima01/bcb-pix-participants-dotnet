using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.UseCases;

public sealed class StreamPixParticipantsUseCase(
    IPixParticipantsRepository repository,
    IMapper<ActivePixParticipant, PixActiveParticipantDto> activeMapper,
    IMapper<AdhesionPixParticipant, PixAdhesionParticipantDto> adhesionMapper)
{
    public async IAsyncEnumerable<object> ExecuteAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var participant in repository.StreamActiveParticipantsAsync(cancellationToken))
        {
            yield return activeMapper.Map(participant);
        }

        await foreach (var participant in repository.StreamAdhesionParticipantsAsync(cancellationToken))
        {
            yield return adhesionMapper.Map(participant);
        }
    }

    public IAsyncEnumerable<PixActiveParticipantDto> StreamActiveAsync(CancellationToken cancellationToken = default)
    {
        return StreamAsync(repository.StreamActiveParticipantsAsync(cancellationToken), activeMapper, cancellationToken);
    }

    public IAsyncEnumerable<PixAdhesionParticipantDto> StreamAdhesionAsync(CancellationToken cancellationToken = default)
    {
        return StreamAsync(repository.StreamAdhesionParticipantsAsync(cancellationToken), adhesionMapper, cancellationToken);
    }

    private static async IAsyncEnumerable<TDto> StreamAsync<TEntity, TDto>(
        IAsyncEnumerable<TEntity> entities,
        IMapper<TEntity, TDto> mapper,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var entity in entities.WithCancellation(cancellationToken))
        {
            yield return mapper.Map(entity);
        }
    }
}
