#region Using directives

using MediatR;

#endregion

namespace Console.Commands
{
    public class ChangeUserEmailAddressCommand : IRequest<bool>
    {
    }
}