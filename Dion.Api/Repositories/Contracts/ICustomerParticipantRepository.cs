using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface ICustomerParticipantRepository
    {
        Task<IEnumerable<CustomerParticipant>> GetItemsByCustomerId(int customerId);
        Task<CustomerParticipant> AddItem(CustomerParticipant customerParticipant);
        Task<bool> ChangeActive(int id,bool activeValue);

        Task<CustomerParticipant> GetItem(int id);
        Task<String> GetParticipantNameByInvoiceId(int invoiceId);

    }
}