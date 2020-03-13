#region Using directives

using System;
using System.Reflection;
using System.Threading.Tasks;
using Domain;
using Domain.Abstractions.DataAccess.EventSourcing;
using Domain.Abstractions.Events;
using Events.Repository.Abstractions;
using EventStore.ClientAPI;
using MediatR;

#endregion

namespace EventSourcingRepositories
{
    public class EventStoreRepository<TAggregate, TAggregateId> : IEventStoreRepository<TAggregate, TAggregateId>
        where TAggregate : BaseAggregate<TAggregateId>, IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        private readonly IEventStore _eventStore;
        private readonly IMediator _mediator;

        public EventStoreRepository(IEventStore eventStore, IMediator mediator)
        {
            _eventStore = eventStore;
            _mediator = mediator;
        }

        public async Task<TAggregate> GetByIdAsync(TAggregateId id)
        {
            var aggregate = CreateEmptyAggregate();

            foreach (var @event in await _eventStore.ReadEventsAsync(id))
            {
                aggregate.ApplyEvent(@event.DomainEvent, @event.EventNumber);
            }

            return aggregate;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            foreach (var @event in aggregate.GetUncommittedEvents())
            {
                await _eventStore.AppendEventAsync(@event);
                await _mediator.Publish(@event);
            }

            aggregate.ClearUncommittedEvents();
        }

        /// <inheritdoc />
        public IEventStoreConnection Connection => _eventStore.Connection;

        private static TAggregate CreateEmptyAggregate()
        {
            return (TAggregate)typeof(TAggregate)
                .GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[0], new ParameterModifier[0])
                ?.Invoke(new object[0]);
        }
    }
}