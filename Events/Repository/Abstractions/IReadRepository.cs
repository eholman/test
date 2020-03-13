#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Abstractions.Entities;

#endregion

namespace Events.Repository.Abstractions
{
    public interface IReadRepository<T>
        where T : IBaseEntity
    {
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdAsync(Guid id);
    }
}