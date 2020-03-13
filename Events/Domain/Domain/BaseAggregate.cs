#region Using directives

using System.Collections.Generic;
using System.Linq;
using Domain.Abstractions.Events;
using Domain.Events;

#endregion

namespace Domain
{
    public abstract class BaseAggregate<T> : IAggregate<T>, IEventSourcingAggregate<T>
    {
        public const long NewAggregateVersion = -1;
        private readonly ICollection<IDomainEvent<T>> _uncommittedEvents = new LinkedList<IDomainEvent<T>>();
        private long _version = NewAggregateVersion;

        /// <inheritdoc />
        public T Id { get; set; }

        /// <inheritdoc />
        public void ApplyEvent(IDomainEvent<T> @event, long version)
        {
            if (_uncommittedEvents.Any(x => Equals(x.EventId, @event.EventId)))
            {
                return;
            }

            Apply(@event);
            //((dynamic)this).Apply((dynamic)@event);
            _version = version;
        }

        /// <inheritdoc />
        public IEnumerable<IDomainEvent<T>> GetUncommittedEvents() => _uncommittedEvents;

        /// <inheritdoc />
        public void ClearUncommittedEvents() => _uncommittedEvents.Clear();

        protected abstract void Apply(IDomainEvent<T> @event);

        protected void RaiseEvent<TEvent>(TEvent @event)
            where TEvent : BaseEvent<T>
        {
            var eventWithAggregate = @event.WithAggregate(Equals(Id, default(T))
                ? @event.AggregateId
                : Id, _version);

            ((IEventSourcingAggregate<T>)this).ApplyEvent(eventWithAggregate, _version + 1);
            _uncommittedEvents.Add(eventWithAggregate);
        }
    }
}