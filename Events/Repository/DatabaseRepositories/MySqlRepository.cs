#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.MySQL;
using Domain.Abstractions.Entities;
using Events.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

#endregion

namespace DatabaseRepositories
{
    public class MySqlRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : class, IBaseEntity
    {
        private readonly DataContext _dbContext;

        public MySqlRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }

        /// <inheritdoc />
        public Task<T> GetByIdAsync(Guid id)
        {
            return _dbContext.Set<T>()
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        /// <inheritdoc />
        public async Task<int> InsertAsync(T entity)
        {
            await _dbContext.AddAsync(entity);

            return await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public Task UpdateAsync(T entity)
        {
            _dbContext.Update(entity);

            return _dbContext.SaveChangesAsync();
        }
    }
}