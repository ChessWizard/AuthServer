using AuthServer.Core.Entities.Common;
using AuthServer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.UnitofWork
{
    public interface IUnitofWork
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity<Guid>, new();

        Task<int> SaveChangesAsync();
    }
}
