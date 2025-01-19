using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface IStoreTypeRepository
    {
        Task<IEnumerable<StroreType>> GetItems();
        Task<StroreType> AddItem(StroreType stroreType);
        Task<bool> DeleteItem(int id);
        public  Task<StroreType> UpdateItem(int id, StroreType storeTypeItem);
        Task<StroreType> GetItem(int id);

    }
}