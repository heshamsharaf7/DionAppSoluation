
using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;

namespace Dion.Api.Repositories
{
    public class StoreTypeRepository : IStoreTypeRepository
    {
        private readonly DionDbContext dionDbContext;

        public StoreTypeRepository(DionDbContext dionDbContext)
        {
            this.dionDbContext = dionDbContext;
        }
        private async Task<bool> StoreTypeItemExists( string storeTypeName)
        {
            return await this.dionDbContext.StoreType.AnyAsync(c => c.Name.Equals( storeTypeName));

        }
        public async Task<StroreType> AddItem(StroreType stroreType)
        {
            if (await StoreTypeItemExists(stroreType.Name.ToString()) == false)
            {


                var result = await this.dionDbContext.StoreType.AddAsync(stroreType);
                await this.dionDbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<StroreType> GetItem(int id)
        {
            var store = await dionDbContext.StoreType
                       .SingleOrDefaultAsync(p => p.Id == id);
            return store;
        }

        public async Task<IEnumerable<StroreType>> GetItems()
        {
            var storeTypes = await this.dionDbContext.StoreType
                                     .ToListAsync();

            return storeTypes;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await this.dionDbContext.StoreType.FindAsync(id);

            if (item != null)
            {
                this.dionDbContext.StoreType.Remove(item);
                await this.dionDbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<StroreType> UpdateItem(int id, StroreType storeTypeItem)
        {
            var item = await this.dionDbContext.StoreType.FindAsync(id);

            if (item != null)
            {
                item.Name = storeTypeItem.Name;
                item.Details = storeTypeItem.Details;
                await this.dionDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}