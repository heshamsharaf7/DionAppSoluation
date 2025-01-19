using Dion.Api.Data;
using Dion.Api.Repositories;
using Dion.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dion.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DionDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DionDbContext context)
        {
            _context = context;
            this.StoreTypeRepository= new StoreTypeRepository(_context);
            this.StoreRepository=new StoreRepository(_context);
            this.UserRepository=new UserRepository(_context);   
            this.StoreCustomersRepository = new StoreCustomersRepository(_context);
            this.CustomerParticipantRepository=new CustomerParticipantRepository(_context);

            this.CurrencyRepository = new CurrencyRepository(_context);

            this.InvoiceRepository = new InvoiceRepository(_context);

            this.InvoiceDetailsRepository = new InvoiceDetailsRepository(_context);

            this.TransactionDetailsRepository = new TransactionDetailsRepository(_context);

            this.WalletsRepository = new WalletsRepository(_context);
            this.StoreWalletsRepository=new StoreWalletsRepository(_context);

        }

        public IStoreTypeRepository StoreTypeRepository { get; private set; }

        public IStoreRepository StoreRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public IStoreCustomersRepository StoreCustomersRepository { get; private set; }
        
             public ICustomerParticipantRepository CustomerParticipantRepository { get; private set; }

        public ICurrencyRepository CurrencyRepository { get; private set; }

        public IInvoiceRepository InvoiceRepository { get; private set; }

        public IInvoiceDetailsRepository InvoiceDetailsRepository { get; private set; }

        public ITransactionDetailsRepository TransactionDetailsRepository { get; private set; }

        public IWalletsRepository WalletsRepository { get; private set; }

        public IStoreWalletsRepository StoreWalletsRepository { get; private set; }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.SaveChanges();
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        // Add repositories or methods for interacting with entities
        // For example:
        // public IAuthorRepository Authors => new AuthorRepository(_context);
        // public IBookRepository Books => new BookRepository(_context);
    }
}
