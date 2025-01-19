using Dion.Api.Entities;


namespace Dion.Api.Repositories.Contracts
{
    public interface IWalletsRepository
    {
        Task<IEnumerable<Wallets>> GetItems();
        Task<Wallets> AddItem(Wallets wallet);
        Task<bool> DeleteItem(int id);
        Task<Wallets> UpdateItem(int id, Wallets wallet);
        Task<Wallets> GetItem(int id);
    }
}