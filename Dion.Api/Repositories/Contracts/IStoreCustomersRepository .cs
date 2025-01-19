using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface IStoreCustomersRepository
    {
        Task<IEnumerable<StoreCustomers>> GetItemsByStore(int storeId);
        Task<StoreCustomers> AddItem(StoreCustomers storeCustomers);
        public Task<StoreCustomers> UpdateItem(int id, StoreCustomers storeCustomers);
        Task<StoreCustomers> GetItem(int id);
        Task<bool> IsUserExist(int userId,int storeId);
        Task<IEnumerable<StoreCustomers>> GetItemsByUserId(int userId);
        Task<IEnumerable<StroreType>> GetStoreTypesByUserId(int userId);
        Task<IEnumerable<Store>> GetStoresByUserIdStoreAndTypeId(int userId, int storeTypeId);
        Task<StoreCustomers> GetItemByStoreAndUser(int userId, int storeId);
        Task<bool> ChangeCustomerLock(int customerId, bool statues);
        Task<bool> ChangeCustomerPayNotification(int customerId, bool statues);
        Task<bool> CheckUserIdExisits(int userId);



        Task<StoreCustomers> ConnectUserToCustomer(int customerId, int userId);





    }
}