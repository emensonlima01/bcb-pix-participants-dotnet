using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface ICsvLineParser<out T> where T : PixParticipant
{
    T Parse(string line);
}
