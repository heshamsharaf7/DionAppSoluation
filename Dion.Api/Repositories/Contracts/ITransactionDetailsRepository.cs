using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface ITransactionDetailsRepository
    {
        Task<IEnumerable<TransactionDetails>> GetItems();
        Task<TransactionDetails> AddItem(TransactionDetails transactionDetails);
        Task<bool> DeleteItem(int id);
        public  Task<TransactionDetails> UpdateItem(int id, TransactionDetails transactionDetails);
        Task<TransactionDetails> GetItem(int id);
        Task<IEnumerable<TransactionDetails>> GetAllStoreTransactions(int storeId); 
        Task<IEnumerable<TransactionDetails>> GetAllCustomerTransactions(int customerId);
        Task<TransactionDetails> GetItemByInvoiceId(int customerId);

        Task<IEnumerable<TransactionDetails>> GetAllStoreCustomerTransactions(int customerId,int storeId);
        Task<double> GetTotalDebtCreditDiscrepancyStoreCustomer(int customerId, int storeId);
        Task<double> GetTotalDebtCreditDiscrepancyForCurrentDayStore(int storeId);
        Task<double> GetTotalDebtCreditDiscrepancyStore(int storeId);
        Task<double> GetTotalDebtCreditDiscrepancyForCurrentDayCustomer(int customerId);
        Task<double> GetTotalDebtCreditDiscrepancyCustomer(int customerId);
        Task<double> GetTotalDebtCustomer(int customerId);
        Task<double> GetTotalCreditCustomer(int customerId);
        Task<double> GetTotalDebtCreditDiscrepancyByUserId(int userId);



    }
}