using Dion.Api.Entities;

namespace Dion.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetItems();
        Task<User> AddItem(User user);
        Task<User> Login(User user);
        Task<User> GetUserByPhoneNo(int phoneNo);

        public Task<User> UpdateItem(int id, User user);
        Task<User> GetItem(int id);
        Task<bool> isPhoneNoExist(int phoneNo);
    }
}