#region Using directives

using MediatR;

#endregion

namespace Console.Commands
{
    public class ListHelpCommand : IRequest<bool>
    {
    }
}