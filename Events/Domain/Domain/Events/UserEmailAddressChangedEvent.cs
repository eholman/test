#region Using directives

using Domain.Abstractions.Events;

#endregion

namespace Domain.Events
{
    public class UserEmailAddressChangedEvent : BaseEvent<UserId>
    {
        public UserEmailAddressChangedEvent(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        private UserEmailAddressChangedEvent(UserId aggregateId, long aggregateVersion, string emailAddress) : base(
            aggregateId, aggregateVersion)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }

        /// <inheritdoc />
        public override IDomainEvent<UserId> WithAggregate(UserId aggregateId, long aggregateVersion)
        {
            return new UserEmailAddressChangedEvent(aggregateId, aggregateVersion, EmailAddress);
        }
    }
}