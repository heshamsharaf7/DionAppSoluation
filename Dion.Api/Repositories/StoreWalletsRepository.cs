using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dion.Api.Repositories
{
    public class StoreWalletsRepository : IStoreWalletsRepository
    {
        private readonly DionDbContext _dionDbContext;

        public StoreWalletsRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        //private async Task<bool> StoreWalletItemExists(string walletName)
        //{
        //    return await _dionDbContext.StoreWallets.AnyAsync(w => w.Name.Equals(walletName));
        //}

        public async Task<StoreWallets> AddItem(StoreWallets storeWallet)
        {
            var result = await _dionDbContext.StoreWallets.AddAsync(storeWallet);
            await _dionDbContext.SaveChangesAsync();
            return result.Entity;

            return null;
        }

        public async Task<StoreWallets> GetItem(int id)
        {
            return await _dionDbContext.StoreWallets.SingleOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<StoreWallets>> GetItems()
        {
            return await _dionDbContext.StoreWallets.ToListAsync();
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await _dionDbContext.StoreWallets.FindAsync(id);

            if (item != null)
            {
                _dionDbContext.StoreWallets.Remove(item);
                await _dionDbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<StoreWallets> UpdateItem(int id, StoreWallets storeWallet)
        {
            var item = await _dionDbContext.StoreWallets.FindAsync(id);

            if (item != null)
            {
                item.AccountNo = storeWallet.AccountNo;
                item.EnteredDate = storeWallet.EnteredDate;
                item.StoreId = storeWallet.StoreId;

                // You can also include updating the Store reference here if needed

                await _dionDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }

        public async Task<IEnumerable<StoreWallets>> GetItemsByStoreId(int storeId)
        {
            return await _dionDbContext.StoreWallets.Where(w => w.StoreId == storeId).ToListAsync();
        }
    }
}