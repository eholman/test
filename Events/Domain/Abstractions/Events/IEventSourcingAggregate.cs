#region Using directives

using System.Collections.Generic;

#endregion

namespace Domain.Abstractions.Events
{
    public interface IEventSourcingAggregate<T>
    {
        void ApplyEvent(IDomainEvent<T> @event, long version);
        IEnumerable<IDomainEvent<T>> GetUncommittedEvents();
        void ClearUncommittedEvents();
    }
}