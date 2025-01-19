using Microsoft.EntityFrameworkCore;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;
using Dion.Api.Data;
using Microsoft.OpenApi.Writers;

namespace Dion.Api.Repositories
{
    public class StoreCustomersRepository : IStoreCustomersRepository
    {
        private readonly DionDbContext _dionDbContext;

        public StoreCustomersRepository(DionDbContext dionDbContext)
        {
            _dionDbContext = dionDbContext;
        }

        public async Task<IEnumerable<StoreCustomers>> GetItemsByStore(int storeId)
        {
            return await _dionDbContext.StoreCustomers.Where(u => u.StoreId == storeId).ToListAsync();
        }

        public async Task<StoreCustomers> AddItem(StoreCustomers storeCustomers)
        {
            _dionDbContext.StoreCustomers.Add(storeCustomers);
            await _dionDbContext.SaveChangesAsync();
            return storeCustomers;
        }

        public async Task<StoreCustomers> UpdateItem(int id, StoreCustomers storeCustomers)
        {
            var existingItem = await _dionDbContext.StoreCustomers.FindAsync(id);
            if (existingItem == null)
            {
                return null;
            }

            existingItem.AccountCapacity = storeCustomers.AccountCapacity;
            //existingItem.IsLock = storeCustomers.IsLock;
            //existingItem.EnteredDate = storeCustomers.EnteredDate;
            //existingItem.PayNotification = storeCustomers.PayNotification;
            existingItem.CuName = storeCustomers.CuName;
            existingItem.CuAddress = storeCustomers.CuAddress;
            //existingItem.IsAccepted = storeCustomers.IsAccepted;
            //existingItem.StoreTypeId = storeCustomers.StoreTypeId;
            //existingItem.UserId = storeCustomers.UserId;
            //existingItem.StoreId = storeCustomers.StoreId;

            // Update navigation properties if needed
            //existingItem.User = storeCustomers.User;
            //existingItem.Store = storeCustomers.Store;

            await _dionDbContext.SaveChangesAsync();

            return existingItem;
        }

        public async Task<StoreCustomers> GetItem(int id)
        {
            return await _dionDbContext.StoreCustomers.FindAsync(id);
        }

        public async Task<bool> IsUserExist(int userId, int storeId)
        {
     
            var user = await  _dionDbContext.StoreCustomers.SingleOrDefaultAsync(u =>u.UserId==userId && u.StoreId == storeId);
            return user != null;

        }

        public async Task<IEnumerable<StoreCustomers>> GetItemsByUserId(int userId)
        {
            return await _dionDbContext.StoreCustomers.Where(u => u.UserId == userId & u.IsAccepted==false).ToListAsync();
        }

        public async Task<IEnumerable<StroreType>> GetStoreTypesByUserId(int userId)
        {
            
                var storeTypes = await _dionDbContext.StoreCustomers
                    .Where(u => u.UserId == userId && u.IsAccepted == true)
                    .Select(sc => sc.Store).Select(sc=>sc.StroreType)
                    .Distinct()
                    .ToListAsync();

                return storeTypes;
            
        }

        public async Task<IEnumerable<Store>> GetStoresByUserIdStoreAndTypeId(int userId, int storeTypeId)
        {
            var storeTypes = await _dionDbContext.StoreCustomers
                .Where(u => u.UserId == userId && u.StoreTypeId==storeTypeId && u.IsAccepted == true)
                .Select(sc => sc.Store)
                .ToListAsync();

            return storeTypes;
        }

        public async Task<StoreCustomers> GetItemByStoreAndUser(int userId, int storeId)
        {
            return await _dionDbContext.StoreCustomers.Where(u => u.UserId == userId && u.StoreId==storeId && u.IsAccepted == true).FirstOrDefaultAsync();
        }
        

        public async Task<bool> ChangeCustomerLock(int customerId, bool status)
        {
            // Assuming you have access to your data context/db context
            var customer = await _dionDbContext.StoreCustomers.FindAsync(customerId);

            if (customer == null)
            {
                // Handle case where customer with the provided ID is not found
                throw new ArgumentException("Customer not found.");
                return false;
            }

            // Update the lock status based on the provided status parameter
            customer.IsLock = status;

            // Save changes to the database
            await _dionDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeCustomerPayNotification(int customerId, bool status)
        {
            // Assuming you have access to your data context/db context
            var customer = await _dionDbContext.StoreCustomers.FindAsync(customerId);

            if (customer == null)
            {
                // Handle case where customer with the provided ID is not found
                throw new ArgumentException("Customer not found.");
                return false;
            }

            // Update the lock status based on the provided status parameter
            customer.PayNotification = status;

            // Save changes to the database
            await _dionDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<StoreCustomers> ConnectUserToCustomer(int customerId, int userId)
        {
            var existingItem = await _dionDbContext.StoreCustomers.FindAsync(customerId);
            if (existingItem == null)
            {
                return null;
            }

            existingItem.UserId = userId;
    

            await _dionDbContext.SaveChangesAsync();

            return existingItem;
        }

        public async Task<bool> CheckUserIdExisits(int userId)
        {
            var existingItem = await _dionDbContext.StoreCustomers.Where(c=>c.UserId==userId).FirstOrDefaultAsync();
                
            if (existingItem == null)
            {
                return false;
            }

            return true;
        }
    }
}