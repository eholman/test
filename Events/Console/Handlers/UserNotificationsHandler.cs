#region Using directives

using System.Threading;
using System.Threading.Tasks;
using Domain.Events;
using MediatR;

#endregion

namespace Console.Handlers
{
    public class UserNotificationsHandler : INotificationHandler<UserCreatedEvent>,
        INotificationHandler<UserVerificationStateChangedEvent>, INotificationHandler<UserEmailAddressChangedEvent>
    {
        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            System.Console.WriteLine("User has been created");

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Handle(UserEmailAddressChangedEvent notification, CancellationToken cancellationToken)
        {
            System.Console.WriteLine($"User is changed into {notification.EmailAddress}");

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Handle(UserVerificationStateChangedEvent notification, CancellationToken cancellationToken)
        {
            System.Console.WriteLine($"User verification state is: {notification.VerificationState}");

            return Task.CompletedTask;
        }
    }
}