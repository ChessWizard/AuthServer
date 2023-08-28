using AuthServer.Core.Entities.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities.Common
{
    public class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; set; }
    }
}
