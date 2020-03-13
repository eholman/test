#region Using directives

using Domain.Abstractions.Events;

#endregion

namespace DataAccess.EventStore
{
    public class Event<TAggregateId> : IEvent<TAggregateId>
    {
        public Event(IDomainEvent<TAggregateId> domainEvent, long eventNumber)
        {
            DomainEvent = domainEvent;
            EventNumber = eventNumber;
        }

        public long EventNumber { get; }

        public IDomainEvent<TAggregateId> DomainEvent { get; }
    }
}