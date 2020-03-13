namespace Domain.Abstractions.Events
{
    public interface IAggregate<T>
    {
        T Id { get; }
    }
}