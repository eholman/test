#region Using directives

using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DataAccess.EventStore;
using DatabaseRepositories;
using Domain;
using Domain.Abstractions.DataAccess.EventSourcing;
using Events.Repository.Abstractions;
using EventSourcingRepositories;
using Logic.Handlers;
using Logic.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

namespace Console
{
    internal static class Program
    {
        private static Task Main()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json", false, true);
            var configuration = configurationBuilder.Build();

            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddSingleton<IConfiguration>(configuration)
                        .AddSingleton<IUserWriter, UserWriter>()
                        .AddSingleton<IUserReader, UserReader>()
                        .AddSingleton<IWriteRepository<DataAccess.Entities.User>,
                            MySqlRepository<DataAccess.Entities.User>>()
                        .AddSingleton<IReadRepository<DataAccess.Entities.User>,
                            MySqlRepository<DataAccess.Entities.User>>()
                        .AddSingleton<IEventStoreRepository<User, UserId>, EventStoreRepository<User, UserId>>()
                        .AddSingleton<IEventStore, EventStoreDataContext>()
                        .AddEventStore()
                        .AddDatabase()
                        .AddMediatR(Assembly.GetAssembly(typeof(UserNotificationHandlers)),
                            Assembly.GetAssembly(typeof(ConsoleInterface)))
                        .AddHostedService<ConsoleInterface>();
                });

            return hostBuilder.RunConsoleAsync();
        }
    }
}