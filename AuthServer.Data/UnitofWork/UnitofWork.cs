using AuthServer.Core.Entities.Common;
using AuthServer.Core.Repositories;
using AuthServer.Core.UnitofWork;
using AuthServer.Data.Context;
using AuthServer.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.UnitofWork
{
    public class UnitofWork : IUnitofWork
    {
        private readonly AuthServerDbContext _context;
        public UnitofWork(AuthServerDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity<Guid>, new()
        {
            return new Repository<T>(_context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
