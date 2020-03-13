#region Using directives

using System.Threading.Tasks;
using Domain.Abstractions.Entities;

#endregion

namespace Events.Repository.Abstractions
{
    public interface IWriteRepository<T>
        where T : IBaseEntity
    {
        Task<int> InsertAsync(T entity);

        Task UpdateAsync(T entity);
    }
}