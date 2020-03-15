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
                case UserVerificationStateChangedEvent changeEvent:
                    Apply(changeEvent);

                    break;
                case UserEmailAddressChangedEvent changeEvent:
                    Apply(changeEvent);

                    break;
                case UserDeletedEvent deleteEvent:
                    Apply(deleteEvent);

                    break;
            }
        }


        private void Apply(UserCreatedEvent createdeEvent)
        {
            Id = createdeEvent.AggregateId;
            EmailAddress = createdeEvent.EmailAddress;
        }

        private void Apply(UserVerificationStateChangedEvent changeEvent)
        {
            IsVerified = changeEvent.VerificationState;
        }
        private void Apply(UserEmailAddressChangedEvent changeEvent)
        {
            EmailAddress = changeEvent.EmailAddress;
        }
        private void Apply(UserDeletedEvent deleteEvent)
        {
            DeletionMoment = deleteEvent.DeletionMoment;
        }
    }
}