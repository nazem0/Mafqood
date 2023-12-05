using Castle.Core.Logging;
using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EntitiesContext _entitiesContext;
        public UnitOfWork(EntitiesContext entitiesContext)
        {
            _entitiesContext = entitiesContext;
        }

        public int SaveChanges()
        {
            return _entitiesContext.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _entitiesContext.SaveChangesAsync();
        }
    }
}
