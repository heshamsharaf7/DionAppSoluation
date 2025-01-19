using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dion.Api.Repositories
{
    public class WalletsRepository : IWalletsRepository
    {
        private readonly DionDbContext _dionDbContext;

        public WalletsRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        private async Task<bool> WalletItemExists(string walletName)
        {
            return await _dionDbContext.Wallets.AnyAsync(w => w.Name.Equals(walletName));
        }

        public async Task<Wallets> AddItem(Wallets wallet)
        {
            var result = await _dionDbContext.Wallets.AddAsync(wallet);
            await _dionDbContext.SaveChangesAsync();
            return result.Entity;

        }

        public async Task<Wallets> GetItem(int id)
        {
            return await _dionDbContext.Wallets.SingleOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Wallets>> GetItems()
        {
            return await _dionDbContext.Wallets.ToListAsync();
        }

        public async Task<bool> DeleteItem(int id)
        {
            var item = await _dionDbContext.Wallets.FindAsync(id);

            if (item != null)
            {
                _dionDbContext.Wallets.Remove(item);
                await _dionDbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<Wallets> UpdateItem(int id, Wallets wallet)
        {
            var item = await _dionDbContext.Wallets.FindAsync(id);

            if (item != null)
            {
                item.Name = wallet.Name;
                item.EnteredDate = wallet.EnteredDate;
                await _dionDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}