using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetItems();
        Task<Currency> AddItem(Currency currency);
        Task<bool> DeleteItem(int id);
        public  Task<Currency> UpdateItem(int id, Currency currency);
        Task<Currency> GetItem(int id);


    }
}