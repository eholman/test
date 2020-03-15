#region Using directives

using System.Threading;
using System.Threading.Tasks;
using Console.Commands;
using MediatR;

#endregion

namespace Console.Handlers
{
    public class ListHelpCommandHandler : IRequestHandler<ListHelpCommand, bool>
    {
        /// <inheritdoc />
        public Task<bool> Handle(ListHelpCommand request, CancellationToken cancellationToken)
        {
            System.Console.WriteLine("\bPress:");
            System.Console.WriteLine("  1 - for Create User Command");
            System.Console.WriteLine("  2 - for Verify User Command");
            System.Console.WriteLine("  3 - for Search User Command");
            System.Console.WriteLine("  4 - for Change User Email Address Command");
            System.Console.WriteLine("  5 - for Delete User Command");
            System.Console.WriteLine("  q - to Quit Command prompt");

            return Task.FromResult(true);
        }
    }
}