#region Using directives

using Domain.Abstractions.Events;

#endregion

namespace Domain.Events
{
    public class UserCreatedEvent : BaseEvent<UserId>
    {
        // ReSharper disable once UnusedMember.Local
        /// <summary>
        /// We need the private constructor for reflection
        /// </summary>
        private UserCreatedEvent()
        {
        }

        /// <inheritdoc />
        private UserCreatedEvent(UserId aggregateId, long aggregateVersion, string emailAddress) : base(aggregateId,
            aggregateVersion)
        {
            EmailAddress = emailAddress;
        }

        public UserCreatedEvent(UserId aggregateId, string emailAddress) : base(aggregateId)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; set; }

        /// <inheritdoc />
        public override IDomainEvent<UserId> WithAggregate(UserId aggregateId, long aggregateVersion)
        {
            return new UserCreatedEvent(aggregateId, aggregateVersion, EmailAddress);
        }
    }
}