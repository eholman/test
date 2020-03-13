namespace Domain.Abstractions.Events
{
    public interface IEvent<TAggregateId>
    {
        long EventNumber { get; }

        IDomainEvent<TAggregateId> DomainEvent { get; }
    }
}