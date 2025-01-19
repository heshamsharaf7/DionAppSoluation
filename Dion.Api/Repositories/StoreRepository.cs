
using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;

namespace Dion.Api.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DionDbContext dionDbContext;

        public StoreRepository(DionDbContext dionDbContext)
        {
            this.dionDbContext = dionDbContext;
        }

        public async Task<Store> AddItem(Store store)
        {
            var result = await this.dionDbContext.Store.AddAsync(store);
            await this.dionDbContext.SaveChangesAsync();
            if (result != null) { return result.Entity; }
            return null;
            
        }

        public async Task<IEnumerable<Store>> GetByUserId(int userId)
        {
            var stores = await this.dionDbContext.Store.Where(s => s.UserId == userId)
                         .ToListAsync();

            return stores;
        }

        public async Task<Store> GetItem(int id)
        {
            var store = await dionDbContext.Store
                      .SingleOrDefaultAsync(p => p.Id == id);
            return store;
        }

        public Task<IEnumerable<Store>> GetItems()
        {
            throw new NotImplementedException();
        }

      
        public Task<Store> UpdateItem(int id, Store store)
        {
            throw new NotImplementedException();
        }
    }
}