
using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;

namespace Dion.Api.Repositories
{
    public class CustomerParticipantRepository : ICustomerParticipantRepository
    {
        private readonly DionDbContext dionDbContext;

        public CustomerParticipantRepository(DionDbContext dionDbContext)
        {
            this.dionDbContext = dionDbContext;
        }

        public async Task<CustomerParticipant> AddItem(CustomerParticipant customerParticipant)
        {
            var result = await this.dionDbContext.CustomerParticipant.AddAsync(customerParticipant);
            await this.dionDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> ChangeActive(int id, bool activeValue)
        {
            var item = await this.dionDbContext.CustomerParticipant.FindAsync(id);

            if (item != null)
            {
                item.IsActive = activeValue;
                await this.dionDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CustomerParticipant> GetItem(int id)
        {
            var item = await dionDbContext.CustomerParticipant
                                 .SingleOrDefaultAsync(p => p.Id == id);
            return item;
        }

        public async Task<IEnumerable<CustomerParticipant>> GetItemsByCustomerId(int customerId)
        {
            return await dionDbContext.CustomerParticipant.Where(u => u.CustomerId == customerId).ToListAsync();
        }
        public async Task<String> GetParticipantNameByInvoiceId(int invoiceId)
        {

            var invoice= await dionDbContext.Invoice.FindAsync(invoiceId);

            if(invoice.ParticipantId == 0)
            {
                return "0";
            }
            else
            {
             var item= await  GetItem(invoice.ParticipantId);
                return item.Name;
            }
        }
    }
}