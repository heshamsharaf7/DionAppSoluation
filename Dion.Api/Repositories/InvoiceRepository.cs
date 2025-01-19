using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;

namespace Dion.Api.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DionDbContext _dionDbContext;

        public InvoiceRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        public async Task<IEnumerable<Invoice>> GetItems()
        {
            return await _dionDbContext.Invoice.OrderByDescending(t => t.EnteredDate).ToListAsync();
        }

        public async Task<Invoice> AddItem(Invoice invoice)
        {
            _dionDbContext.Invoice.Add(invoice);
            await _dionDbContext.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> DeleteItem(int id)
        {
            var invoice = await _dionDbContext.Invoice.FindAsync(id);

            if (invoice != null)
            {
                _dionDbContext.Invoice.Remove(invoice);
                await _dionDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Invoice> UpdateItem(int id, Invoice invoice)
        {
            var existingInvoice = await _dionDbContext.Invoice.FindAsync(id);

            if (existingInvoice != null)
            {
                existingInvoice.EnteredDate = invoice.EnteredDate;
                //existingInvoice.Details = invoice.Details;
                existingInvoice.LockStatus = invoice.LockStatus;
                existingInvoice.LockDate = invoice.LockDate;
                existingInvoice.CustomerId = invoice.CustomerId;
                existingInvoice.CurrencyId = invoice.CurrencyId;

                await _dionDbContext.SaveChangesAsync();
                return existingInvoice;
            }

            return null;
        }

        public async Task<Invoice> GetItem(int id)
        {
            return await _dionDbContext.Invoice.FindAsync(id);
        }

      
    }
}