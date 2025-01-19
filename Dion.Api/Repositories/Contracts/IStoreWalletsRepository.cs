using Dion.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dion.Api.Repositories.Contracts
{
    public interface IStoreWalletsRepository
    {
        Task<IEnumerable<StoreWallets>> GetItems();
        Task<StoreWallets> AddItem(StoreWallets storeWallet);
        Task<bool> DeleteItem(int id);
        Task<StoreWallets> UpdateItem(int id, StoreWallets storeWallet);
        Task<StoreWallets> GetItem(int id);
        Task<IEnumerable<StoreWallets>> GetItemsByStoreId(int storeId);

    }
}