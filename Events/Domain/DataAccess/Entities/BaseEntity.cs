#region Using directives

using System;
using System.ComponentModel.DataAnnotations;
using Domain.Abstractions.Entities;

#endregion

namespace DataAccess.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// Identifier of the entity
        /// </summary>
        [Key]
        public Guid Id { get; set; }
    }
}