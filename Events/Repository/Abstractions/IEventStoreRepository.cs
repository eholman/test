#region Using directives

using System.Threading.Tasks;
using Domain.Abstractions.Events;
using EventStore.ClientAPI;

#endregion

namespace Events.Repository.Abstractions
{
    public interface IEventStoreRepository<TAggregate, in T>
        where TAggregate : IAggregate<T>
    {
        IEventStoreConnection Connection { get; }
        Task<TAggregate> GetByIdAsync(T id);

        Task SaveAsync(TAggregate aggregate);
    }
}