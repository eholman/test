#region Using directives

using System;
using DataAccess.MySQL;
using EventStore.ClientAPI;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Console
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            var connectionSettings = ConnectionSettings.Create();
            connectionSettings
                .EnableVerboseLogging()
                .UseDebugLogger()
                .KeepReconnecting()
                .SetHeartbeatTimeout(TimeSpan.FromSeconds(60))
                .SetHeartbeatInterval(TimeSpan.FromSeconds(30));

            return services.AddSingleton(a =>
            {
                var connection = EventStoreConnection.Create(
                    "ConnectTo=tcp://localhost:1115;DefaultUserCredentials=admin:changeit;UseSslConnection=true;TargetHost=eventstore.org;ValidateServer=false",
                    connectionSettings, "MyConName");
                connection.ConnectAsync()
                    .Wait();

                return connection;
            });
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>();

            using var context = services.BuildServiceProvider()
                .GetService<DataContext>();
            context.Database.EnsureCreated();

            return services;
        }
    }
}