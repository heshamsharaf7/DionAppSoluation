using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface IInvoiceDetailsRepository
    {
        Task<IEnumerable<InvoiceDetails>> GetItems();
        Task<InvoiceDetails> AddItem(InvoiceDetails invoiceDetails);
        Task<bool> DeleteItem(int id);
        Task<InvoiceDetails> UpdateItem(int id, InvoiceDetails invoiceDetails);
        Task<InvoiceDetails> GetItem(int id);
        Task<IEnumerable<InvoiceDetails>> GetInvoiceDetailsByCustomerId(int customerId);

        Task<IEnumerable<InvoiceDetails>> GetItemsByInvoiceId(int invoiceId);


    }
}