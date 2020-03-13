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
                EmailAddress = notification.EmailAddress
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
            user.EmailAddress = notification.EmailAddress;
            user.IsVerified = false;
            await _writeRepository.UpdateAsync(user);
        }

        /// <inheritdoc />
        public async Task Handle(UserVerificationStateChangedEvent notification, CancellationToken cancellationToken)
        {
            // Validate
            var user = await _readRepository.GetByIdAsync(notification.AggregateId.Id);
            user.IsVerified = notification.VerificationState;
            await _writeRepository.UpdateAsync(user);
        }
    }
}