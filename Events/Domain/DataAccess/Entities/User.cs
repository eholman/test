#region Using directives

using System;

#endregion

// ReSharper disable UnusedAutoPropertyAccessor.Local
namespace DataAccess.Entities
{
    public class User : BaseEntity
    {
        public string EmailAddress { get; set; }

        /// <summary>
        /// Unverified email address
        /// </summary>
        public string EmailAddressUnverified { get; set; }

        /// <summary>
        /// Indicated whether a user's email address is verified or not
        /// </summary>
        public bool IsVerified { get; private set; }

        /// <summary>
        /// Moment in time when the user should be completely removed. If property has a value, user is functional wise deleted
        /// </summary>
        public DateTime? ScheduledDeletionMoment { get; set; }

        /// <summary>
        /// Indication whether a user is deleted or not
        /// </summary>
        public bool IsDeleted { get; private set; }
    }
}