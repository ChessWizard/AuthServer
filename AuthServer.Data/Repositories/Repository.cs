using AuthServer.Core.Entities.Common;
using AuthServer.Core.Entities.Common.Interfaces;
using AuthServer.Core.Repositories;
using AuthServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<Guid>
    {
        private DbSet<TEntity> _table { get; set; }

        private AuthServerDbContext _context { get; set; }

        public Repository(AuthServerDbContext context)
        {
            _context = context;
            _table = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity) => await _table.AddAsync(entity);

        public IQueryable<TEntity> GetAll() => _table.AsNoTracking();

        public async Task<TEntity> GetByIdAsync(Guid id) => await GetAll().FirstOrDefaultAsync(x => x.Id.Equals(id));

        public void Remove(TEntity entity) => _table.Remove(entity);

        public void Update(TEntity entity) => _table.Update(entity);

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate) => _table.FirstOrDefaultAsync(predicate);
    }
}
