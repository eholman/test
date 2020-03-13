#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Abstractions.DataAccess.EventSourcing;
using Domain.Abstractions.Events;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace DataAccess.EventStore
{
    public class EventStoreDataContext : IEventStore
    {
        public EventStoreDataContext(IEventStoreConnection connection)
        {
            Connection = connection;
        }

        public IEventStoreConnection Connection { get; }

        public async Task<IEnumerable<IEvent<TAggregateId>>> ReadEventsAsync<TAggregateId>(TAggregateId id)
            where TAggregateId : IAggregateId
        {
            try
            {
                var ret = new List<Event<TAggregateId>>();
                StreamEventsSlice currentSlice;
                long nextSliceStart = StreamPosition.Start;

                do
                {
                    currentSlice =
                        await Connection.ReadStreamEventsForwardAsync(id.ToString(), nextSliceStart, 200, false);

                    if (currentSlice.Status != SliceReadStatus.Success)
                    {
                        throw new Exception($"Aggregate {id.ToString()} not found");
                    }

                    nextSliceStart = currentSlice.NextEventNumber;
                    foreach (var resolvedEvent in currentSlice.Events)
                    {
                        ret.Add(new Event<TAggregateId>(
                            Deserialize<TAggregateId>(resolvedEvent.Event.EventType, resolvedEvent.Event.Data),
                            resolvedEvent.Event.EventNumber));
                    }
                } while (!currentSlice.IsEndOfStream);

                return ret;
            }
            catch (EventStoreConnectionException ex)
            {
                throw new Exception($"Error while reading events for aggregate {id}", ex);
            }
        }

        public async Task<long> AppendEventAsync<TAggregateId>(IDomainEvent<TAggregateId> @event)
            where TAggregateId : IAggregateId
        {
            try
            {
                var eventData = new EventData(
                    @event.EventId,
                    @event.GetType()
                        .AssemblyQualifiedName,
                    true,
                    Serialize(@event),
                    new byte[] { });

                var writeResult = await Connection.AppendToStreamAsync(
                    @event.AggregateId.ToString(),
                    @event.AggregateVersion == BaseAggregate<TAggregateId>.NewAggregateVersion
                        ? ExpectedVersion.NoStream
                        : @event.AggregateVersion,
                    eventData);

                return writeResult.NextExpectedVersion;
            }
            catch (EventStoreConnectionException ex)
            {
                throw new Exception($"Error while appending event {@event.EventId} for aggregate {@event.AggregateId}",
                    ex);
            }
        }

        private static IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string eventType, byte[] data)
        {
            var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };

            return (IDomainEvent<TAggregateId>)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data),
                Type.GetType(eventType), settings);

            //return (IDomainEvent<TAggregateId>)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType(eventType), new JsonSerializerSettings
            //{
            //    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            //});
        }

        private static byte[] Serialize<TAggregateId>(IDomainEvent<TAggregateId> @event)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
        }
    }

    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }
    }
}