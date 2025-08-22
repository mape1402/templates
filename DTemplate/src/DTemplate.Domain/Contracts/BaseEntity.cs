using DTemplate.Domain.Identifier;

namespace DTemplate.Domain.Contracts
{
    public abstract class BaseEntity : IEntity<CId>
    {
        public CId Id { get; set; }
    }
}
