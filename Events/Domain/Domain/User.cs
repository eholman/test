#region Using directives

using System;
using Domain.Abstractions.Events;
using Domain.Events;

#endregion

namespace Domain
{
    public class User : BaseAggregate<UserId>
    {
        private User()
        {
        }

        public User(UserId id, string emailAddress) : this()
        {
            EmailAddress = emailAddress;
            RaiseEvent(new UserCreatedEvent(id, emailAddress));
        }

        public DateTime DeletionMoment { get; set; }

        public bool IsVerified { get; private set; }
        public string EmailAddress { get; private set; }

        public void SetVerificationState(bool verificationState)
        {
            IsVerified = verificationState;
            RaiseEvent(new UserVerificationStateChangedEvent(verificationState));
        }

        public void SetEmailAddress(string emailAddress)
        {
            EmailAddress = emailAddress;
            RaiseEvent(new UserEmailAddressChangedEvent(emailAddress));
        }

        public void MarkForDeletion(DateTime deletionMoment)
        {
            DeletionMoment = deletionMoment;
            RaiseEvent(new UserDeletedEvent(deletionMoment));
        }

        /// <inheritdoc />
        protected override void Apply(IDomainEvent<UserId> @event)
        {
            switch (@event)
            {
                case UserCreatedEvent createdEvent:
                    Apply(createdEvent);

                    break;
                case UserVerificationStateChangedEvent createdEvent:
                    Apply(createdEvent);

                    break;
            }
        }

        private void Apply(UserCreatedEvent @event)
        {
            Id = @event.AggregateId;
            EmailAddress = @event.EmailAddress;
        }

        private void Apply(UserVerificationStateChangedEvent @event)
        {
            IsVerified = @event.VerificationState;
        }
    }
}