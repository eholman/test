#region Using directives

using System;

#endregion

namespace Domain.Abstractions.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; }
    }
}