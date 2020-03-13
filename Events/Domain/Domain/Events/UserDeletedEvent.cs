#region Using directives

using System;
using Domain.Abstractions.Events;

#endregion

namespace Domain.Events
{
    public class UserDeletedEvent : BaseEvent<UserId>
    {
        public UserDeletedEvent(DateTime deletionMoment)
        {
            DeletionMoment = deletionMoment;
        }

        private UserDeletedEvent(UserId aggregateId, long aggregateVersion, DateTime deletionMoment) : base(aggregateId,
            aggregateVersion)
        {
            DeletionMoment = deletionMoment;
        }

        public DateTime DeletionMoment { get; }

        /// <inheritdoc />
        public override IDomainEvent<UserId> WithAggregate(UserId aggregateId, long aggregateVersion)
        {
            return new UserDeletedEvent(aggregateId, aggregateVersion, DeletionMoment);
        }
    }
}