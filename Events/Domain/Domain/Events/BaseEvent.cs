#region Using directives

using System;
using Domain.Abstractions.Events;
using MediatR;

#endregion

namespace Domain.Events
{

    public abstract class BaseEvent<T> : IDomainEvent<T>, INotification
    {
        protected BaseEvent()
        {
            EventId = Guid.NewGuid();
        }

        protected BaseEvent(T aggregateId) : this()
        {
            AggregateId = aggregateId;
        }

        protected BaseEvent(T aggregateId, long aggregateVersion) : this(aggregateId)
        {
            AggregateVersion = aggregateVersion;
        }

        /// <inheritdoc />
        public Guid EventId { get; }

        /// <inheritdoc />
        public T AggregateId { get; }

        /// <inheritdoc />
        public long AggregateVersion { get; }

        public abstract IDomainEvent<T> WithAggregate(T aggregateId, long aggregateVersion);
    }
}