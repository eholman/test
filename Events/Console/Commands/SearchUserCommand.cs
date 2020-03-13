#region Using directives

using MediatR;

#endregion

namespace Console.Commands
{
    public class SearchUserCommand : IRequest<bool>
    {
    }
}