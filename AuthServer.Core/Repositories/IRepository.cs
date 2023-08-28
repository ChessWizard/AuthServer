using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity<Guid>
    {
        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();

        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}
