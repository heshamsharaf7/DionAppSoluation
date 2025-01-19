using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetItems();
        Task<Store> AddItem(Store store);
        public  Task<Store> UpdateItem(int id, Store store);
        Task<Store> GetItem(int id);
        Task<IEnumerable<Store>> GetByUserId(int userId);

    }
}