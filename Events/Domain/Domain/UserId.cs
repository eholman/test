#region Using directives

using System;
using Domain.Abstractions.Events;

#endregion

namespace Domain
{
    public class UserId : IAggregateId
    {
        private const string IdPrefix = "User-";

        public UserId(string id)
        {
            Id = Guid.Parse(id.StartsWith(IdPrefix)
                ? id.Substring(IdPrefix.Length)
                : id);
        }

        private UserId(Guid id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public Guid Id { get; }

        public override string ToString()
        {
            return $"{IdPrefix}{Id}";
        }

        public static UserId NewId()
        {
            return new UserId(Guid.NewGuid());
        }
    }
}