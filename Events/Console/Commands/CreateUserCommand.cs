#region Using directives

using MediatR;

#endregion

namespace Console.Commands
{
    public class CreateUserCommand : IRequest<bool>
    {
    }
}