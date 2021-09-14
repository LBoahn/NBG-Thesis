﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBG.Visitor_Registration.Shared.Models;

namespace NBG.Visitor_Registration.Server.Repositories
{
    public interface IDataRepository
    {

        Task<Visitor> Get(int id);
        Task<IEnumerable<Visitor>> GetAll();
        Task Add(Visitor visitor);
        Task Delete(int id);
        Task Update(Visitor visitor);
    }
}
