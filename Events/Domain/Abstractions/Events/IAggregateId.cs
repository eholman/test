#region Using directives

using System;

#endregion

namespace Domain.Abstractions.Events
{
    public interface IAggregateId
    {
        Guid Id { get; }
        string ToString();
    }
}