﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities.Common.Interfaces
{
    public interface IAuditEntity<T> : IEntity<T>, IAuditEntity
    {
        
    }
}
