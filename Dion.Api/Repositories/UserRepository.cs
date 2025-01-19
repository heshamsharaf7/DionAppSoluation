
using Microsoft.EntityFrameworkCore;
using Dion.Api.Data;
using Dion.Api.Entities;
using Dion.Api.Repositories.Contracts;

namespace Dion.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DionDbContext dionDbContext;

        public UserRepository(DionDbContext dionDbContext)
        {
            this.dionDbContext = dionDbContext;
        }

        public async Task<User> AddItem(User user)
        {
            var result = await this.dionDbContext.User.AddAsync(user);
            await this.dionDbContext.SaveChangesAsync();
            if (result != null) { return result.Entity; }
            return null;
            
        }

        public async Task<User> GetItem(int id)
        {
            var user = await dionDbContext.User
                      .SingleOrDefaultAsync(p => p.Id == id);
            return user;
        }

        public Task<IEnumerable<User>> GetItems()
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByPhoneNo(int phoneNo)
        {
            var user = await dionDbContext.User
                .FirstOrDefaultAsync(p => p.PhoneNo == phoneNo);

            return user;
        }

        public async Task<bool> isPhoneNoExist(int phoneNo)
        {
            
            var user = await dionDbContext.User
                .FirstOrDefaultAsync(p => p.PhoneNo == phoneNo); 

            return user != null;
        }

        public async Task<User> Login(User user)
        {
            var newUser = await dionDbContext.User
                      .SingleOrDefaultAsync(p => p.PhoneNo == user.PhoneNo && p.Password == user.Password&& p.Type == user.Type);
            return newUser;
        }

        public Task<User> UpdateItem(int id, User user)
        {
            throw new NotImplementedException();
        }

       
    }
}