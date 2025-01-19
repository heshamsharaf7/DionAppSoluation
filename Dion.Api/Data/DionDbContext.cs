using Microsoft.EntityFrameworkCore;
using Dion.Api.Entities;

namespace Dion.Api.Data
{
    public class DionDbContext : DbContext
    {
      
        public DionDbContext(DbContextOptions<DionDbContext> options) : base(options)
        {

        }
     
        public DbSet<StroreType> StoreType { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<StoreCustomers> StoreCustomers { get; set; }
        public DbSet<CustomerParticipant> CustomerParticipant { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public DbSet<TransactionDetails> TransactionDetails { get; set; }
        public DbSet<Wallets> Wallets { get; set; }

        public DbSet<StoreWallets> StoreWallets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This method is intentionally left empty to prevent automatic table creation
        }


    }
}