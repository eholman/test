#region Using directives

using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Abstractions.Events;
using EventStore.ClientAPI;

#endregion

namespace Domain.Abstractions.DataAccess.EventSourcing
{
    public interface IEventStore
    {
        IEventStoreConnection Connection { get; }

        Task<IEnumerable<IEvent<TAggregateId>>> ReadEventsAsync<TAggregateId>(TAggregateId id)
            where TAggregateId : IAggregateId;

        Task<long> AppendEventAsync<TAggregateId>(IDomainEvent<TAggregateId> @event)
            where TAggregateId : IAggregateId;
    }
}