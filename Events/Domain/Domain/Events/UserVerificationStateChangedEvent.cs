#region Using directives

using Domain.Abstractions.Events;

#endregion

namespace Domain.Events
{
    public class UserVerificationStateChangedEvent : BaseEvent<UserId>
    {
        public UserVerificationStateChangedEvent(bool verificationState)
        {
            VerificationState = verificationState;
        }

        private UserVerificationStateChangedEvent(UserId aggregateId, long aggregateVersion, bool verificationState) :
            base(aggregateId, aggregateVersion)
        {
            VerificationState = verificationState;
        }

        public bool VerificationState { get; }

        /// <inheritdoc />
        public override IDomainEvent<UserId> WithAggregate(UserId aggregateId, long aggregateVersion)
        {
            return new UserVerificationStateChangedEvent(aggregateId, aggregateVersion, VerificationState);
        }
    }
}