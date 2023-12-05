using Domain;
using Infrastructure.Persistence;

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
