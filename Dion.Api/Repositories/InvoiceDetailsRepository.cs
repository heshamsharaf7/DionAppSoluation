using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dion.Api.Repositories
{
    public class InvoiceDetailsRepository : IInvoiceDetailsRepository
    {
        private readonly DionDbContext _dionDbContext;

        public InvoiceDetailsRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        public async Task<IEnumerable<InvoiceDetails>> GetItems()
        {
            return await _dionDbContext.InvoiceDetails.ToListAsync();
        }

        public async Task<InvoiceDetails> AddItem(InvoiceDetails invoiceDetails)
        {
            _dionDbContext.InvoiceDetails.Add(invoiceDetails);
            await _dionDbContext.SaveChangesAsync();
            return invoiceDetails;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var invoiceDetails = await _dionDbContext.InvoiceDetails.FindAsync(id);

            if (invoiceDetails != null)
            {
                _dionDbContext.InvoiceDetails.Remove(invoiceDetails);
                await _dionDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<InvoiceDetails> UpdateItem(int id, InvoiceDetails invoiceDetails)
        {
            var existingInvoiceDetails = await _dionDbContext.InvoiceDetails.FindAsync(id);

            if (existingInvoiceDetails != null)
            {
                existingInvoiceDetails.Statement = invoiceDetails.Statement;
                existingInvoiceDetails.UnitPrice = invoiceDetails.UnitPrice;
                existingInvoiceDetails.Quantity = invoiceDetails.Quantity;
                existingInvoiceDetails.InvoiceId = invoiceDetails.InvoiceId;

                await _dionDbContext.SaveChangesAsync();
                return existingInvoiceDetails;
            }

            return null;
        }

        public async Task<InvoiceDetails> GetItem(int id)
        {
            return await _dionDbContext.InvoiceDetails.FindAsync(id);
        }

        public async Task<IEnumerable<InvoiceDetails>> GetInvoiceDetailsByCustomerId(int customerId)
        {
            return await _dionDbContext.InvoiceDetails
                .Where(id => id.Invoice.CustomerId == customerId).OrderByDescending(t => t.Invoice.EnteredDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InvoiceDetails>> GetItemsByInvoiceId(int invoiceId)
        {
            return await _dionDbContext.InvoiceDetails
                        .Where(id => id.Invoice.Id == invoiceId).OrderByDescending(t => t.Invoice.EnteredDate)
                        .ToListAsync();
        }

       
    }
}