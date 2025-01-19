using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using Dion.Api.UnitOfWork;
using Microsoft.AspNetCore.Mvc;


namespace Dion.Api.Repositories
{
    public class TransactionDetailsRepository : ITransactionDetailsRepository
    {
        private readonly DionDbContext _dionDbContext;

        public TransactionDetailsRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        public async Task<IEnumerable<TransactionDetails>> GetItems()
        {
            return await _dionDbContext.TransactionDetails.ToListAsync();
        }

        public async Task<TransactionDetails> AddItem(TransactionDetails transactionDetails)
        {
            _dionDbContext.TransactionDetails.Add(transactionDetails);
            await _dionDbContext.SaveChangesAsync();
            return transactionDetails;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var transactionDetails = await _dionDbContext.TransactionDetails.FindAsync(id);

            if (transactionDetails != null)
            {
                _dionDbContext.TransactionDetails.Remove(transactionDetails);
                await _dionDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<TransactionDetails> UpdateItem(int id, TransactionDetails transactionDetails)
        {
            var existingTransactionDetails = await _dionDbContext.TransactionDetails.FindAsync(id);

            if (existingTransactionDetails != null)
            {
                existingTransactionDetails.Statement = transactionDetails.Statement;
                existingTransactionDetails.Debit = transactionDetails.Debit;
                existingTransactionDetails.Credit = transactionDetails.Credit;
                existingTransactionDetails.LockStatus = transactionDetails.LockStatus;
                existingTransactionDetails.LockDate = transactionDetails.LockDate;
                existingTransactionDetails.InvoiceId = transactionDetails.InvoiceId;
                existingTransactionDetails.CurrencyId = transactionDetails.CurrencyId;
                existingTransactionDetails.CustomerId = transactionDetails.CustomerId;
                existingTransactionDetails.StoreId = transactionDetails.StoreId;

                await _dionDbContext.SaveChangesAsync();
                return existingTransactionDetails;
            }

            return null;
        }

        public async Task<TransactionDetails> GetItem(int id)
        {
            return await _dionDbContext.TransactionDetails.FindAsync(id);
        }
        public async Task<double> GetTotalDebtCreditDiscrepancyForCurrentDayStore(int storeId)
        {
            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.StoreId == storeId && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            // Calculate the total of debit and credit transactions for the current day
            double totalDebit = transactions
                .Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Debit);

            double totalCredit = transactions
                .Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Credit);

            // Calculate the discrepancy between total debit and total credit
            double totalDiscrepancy = totalDebit - totalCredit;

            return totalDiscrepancy;
        }
        public async Task<double> GetTotalDebtCreditDiscrepancyStore(int storeId)
        {
            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.StoreId == storeId && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            // Calculate the total of debit and credit transactions for the current day
            double totalDebit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Debit);

            double totalCredit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Credit);

            // Calculate the discrepancy between total debit and total credit
            double totalDiscrepancy = totalDebit - totalCredit;

            return totalDiscrepancy;
        }

        public async Task<double> GetTotalDebtCreditDiscrepancyForCurrentDayCustomer(int customerId)
        {
            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId &&t.CurrencyId==1 && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            // Calculate the total of debit and credit transactions for the current day
            double totalDebit = transactions
                .Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Debit);

            double totalCredit = transactions
                .Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Credit);

            // Calculate the discrepancy between total debit and total credit
            double totalDiscrepancy = totalDebit - totalCredit;

            return totalDiscrepancy;
        }
        public async Task<double> GetTotalDebtCreditDiscrepancyCustomer(int customerId)
        {
            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            // Calculate the total of debit and credit transactions for the current day
            double totalDebit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Debit);

            double totalCredit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Credit);

            // Calculate the discrepancy between total debit and total credit
            double totalDiscrepancy = totalDebit - totalCredit;

            return totalDiscrepancy;
        }

        public async Task<double> GetTotalDebtCustomer(int customerId)
        {
            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            // Calculate the total of debit and credit transactions for the current day
            double totalDebit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Debit);

        
           
            return totalDebit;
        }
        public async Task<double> GetTotalCreditCustomer(int customerId)
        {
            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            double totalCredit = transactions
                 //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                 .Sum(t => t.Credit);



            return totalCredit;
        }

        public async Task<IEnumerable<TransactionDetails>> GetAllStoreTransactions(int storeId)
        {
            // Fetch the data from the database first, ordered by date in descending order
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.StoreId == storeId && (t.Debit > 0 || t.Credit > 0))
                .OrderByDescending(t => t.EnteredDate) // Ordering by TransactionDate in descending order
                .ToListAsync();

            return transactions;
        }

        public async Task<IEnumerable<TransactionDetails>> GetAllCustomerTransactions(int customerId)
        {
            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId && (t.Debit > 0 || t.Credit > 0)).OrderByDescending(t => t.EnteredDate)
                .ToListAsync();

            return transactions;
        }

        public async Task<IEnumerable<TransactionDetails>> GetAllStoreCustomerTransactions(int customerId, int storeId)
        {

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId && t.StoreId==storeId &&
                (t.Debit > 0 || t.Credit > 0)).OrderBy(t => t.EnteredDate) // Ordering by TransactionDate in descending order

                .ToListAsync();

            return transactions;
        }

        public async Task<double> GetTotalDebtCreditDiscrepancyStoreCustomer(int customerId, int storeId)
        {


            DateTime today = DateTime.Today.Date;

            // Fetch the data from the database first
            var transactions = await _dionDbContext.TransactionDetails
                .Where(t => t.CustomerId == customerId && t.CurrencyId == 1 && (t.Debit > 0 || t.Credit > 0))
                .ToListAsync();

            // Calculate the total of debit and credit transactions for the current day
            double totalDebit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Debit);

            double totalCredit = transactions
                //.Where(t => DateTime.Parse(t.EnteredDate).Date == today)
                .Sum(t => t.Credit);

            // Calculate the discrepancy between total debit and total credit
            double totalDiscrepancy = totalDebit- totalCredit ;

            return totalDiscrepancy;
        }

        public async Task<TransactionDetails> GetItemByInvoiceId(int customerId)
        {
            return await _dionDbContext.TransactionDetails
                        .Where(id => id.InvoiceId == customerId).OrderBy(t => t.EnteredDate).FirstAsync();
                        
        }

        public async Task<double> GetTotalDebtCreditDiscrepancyByUserId(int userId)
        {
            // Get all StoreCustomer IDs based on the userId
            var storeCustomerIds = await _dionDbContext.StoreCustomers
                .Where(sc => sc.UserId == userId)
                .Select(sc => sc.Id)
                .ToListAsync();

            // Get all transactions linked to the fetched StoreCustomers
            var userTransactions = await _dionDbContext.TransactionDetails
                .Where(td => storeCustomerIds.Contains(td.CustomerId))
                .Select(td => new
                {
                    td.Debit,
                    td.Credit
                })
                .ToListAsync();

            // Calculate total debt and credit
            double totalDebit = userTransactions.Sum(td => td.Debit);
            double totalCredit = userTransactions.Sum(td => td.Credit);

            // Calculate discrepancy (difference between total debit and total credit)
            double discrepancy = totalDebit - totalCredit;

            return discrepancy;
        }
    }
}