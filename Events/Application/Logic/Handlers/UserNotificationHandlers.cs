#region Using directives

using System.Threading;
using System.Threading.Tasks;
using DataAccess.Entities;
using Domain.Events;
using Events.Repository.Abstractions;
using MediatR;

#endregion

namespace Logic.Handlers
{
    public class UserNotificationHandlers : INotificationHandler<UserCreatedEvent>,
        INotificationHandler<UserVerificationStateChangedEvent>, INotificationHandler<UserEmailAddressChangedEvent>,
        INotificationHandler<UserDeletedEvent>
    {
        private readonly IReadRepository<User> _readRepository;
        private readonly IWriteRepository<User> _writeRepository;

        public UserNotificationHandlers(IWriteRepository<User> writeRepository, IReadRepository<User> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        /// <inheritdoc />
        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Validate
            return _writeRepository.InsertAsync(new User
            {
                Id = notification.AggregateId.Id,
                EmailAddressUnverified = notification.EmailAddress
            });
        }

        /// <inheritdoc />
        public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _readRepository.GetByIdAsync(notification.AggregateId.Id);
            user.ScheduledDeletionMoment = notification.DeletionMoment;
            await _writeRepository.UpdateAsync(user);
        }

        /// <inheritdoc />
        public async Task Handle(UserEmailAddressChangedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _readRepository.GetByIdAsync(notification.AggregateId.Id);
            user.EmailAddressUnverified = notification.EmailAddress;
            await _writeRepository.UpdateAsync(user);
        }

        /// <inheritdoc />
        public async Task Handle(UserVerificationStateChangedEvent notification, CancellationToken cancellationToken)
        {
            // When user is verified update the email address 
            if (notification.VerificationState)
            {
                var user = await _readRepository.GetByIdAsync(notification.AggregateId.Id);
                user.EmailAddress = user.EmailAddressUnverified;
                user.EmailAddressUnverified = null;
                await _writeRepository.UpdateAsync(user);
            }
        }
    }
}