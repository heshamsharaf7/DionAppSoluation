namespace Dion.Api.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        public IStoreTypeRepository StoreTypeRepository { get; }
        public IStoreRepository StoreRepository { get; }
        public IUserRepository UserRepository { get; }
        public IStoreCustomersRepository StoreCustomersRepository {  get; }
        public ICustomerParticipantRepository CustomerParticipantRepository { get; }
        public ICurrencyRepository CurrencyRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IInvoiceDetailsRepository InvoiceDetailsRepository { get; }
        public ITransactionDetailsRepository TransactionDetailsRepository { get; }
        public IWalletsRepository WalletsRepository { get; }
        public IStoreWalletsRepository StoreWalletsRepository { get; }



        void BeginTransaction();
         void Commit();
         void Rollback();

    }
}
