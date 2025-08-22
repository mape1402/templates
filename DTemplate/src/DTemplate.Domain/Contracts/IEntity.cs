namespace DTemplate.Domain.Contracts
{
    public interface IEntity { }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
