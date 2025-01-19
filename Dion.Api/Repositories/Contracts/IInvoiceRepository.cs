using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetItems();
        Task<Invoice> AddItem(Invoice invoice);
        Task<bool> DeleteItem(int id);
        public  Task<Invoice> UpdateItem(int id, Invoice invoice);
        Task<Invoice> GetItem(int id);


    }
}