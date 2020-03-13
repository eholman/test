#region Using directives

using System.Threading;
using System.Threading.Tasks;
using Console.Commands;
using MediatR;
using Microsoft.Extensions.Hosting;

#endregion

namespace Console
{
    public class ConsoleInterface : IHostedService
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IMediator _mediator;

        public ConsoleInterface(IMediator mediator, IHostApplicationLifetime applicationLifetime)
        {
            _mediator = mediator;
            _applicationLifetime = applicationLifetime;
        }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            bool stop;
            do
            {
                System.Console.WriteLine("Press q to quit. Press h for Help");
                var input = System.Console.ReadKey()
                    .KeyChar;

                stop = await ProcessInput(input) == false;
            } while (!stop);

            System.Console.WriteLine("Bye :-)");

            _applicationLifetime.StopApplication();
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task<bool> ProcessInput(char input) =>
            input switch
            {
                'q' => false,
                'h' => await _mediator.Send(new ListHelpCommand()),
                '1' => await _mediator.Send(new CreateUserCommand()),
                '2' => await _mediator.Send(new VerifyUserCommand()),
                '3' => await _mediator.Send(new SearchUserCommand()),
                '4' => await _mediator.Send(new ChangeUserEmailAddressCommand()),
                '5' => await _mediator.Send(new DeleteUserCommand()),
                _ => true
            };
    }
}