using LogisticaVertical.Domain.Entities;

namespace LogisticaVertical.Domain.Interfaces
{
    public interface IFileParser
    {
        IReadOnlyCollection<User> Parse(Stream fileStream);
    }
}
