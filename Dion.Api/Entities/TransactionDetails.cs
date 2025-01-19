using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class TransactionDetails
    {
        public int Id { get; set; }
        public string Statement { get; set; }
        public string EnteredDate { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public bool LockStatus { get; set; }
        public string LockDate { get; set; }
        public int InvoiceId { get; set; }
        public int CurrencyId { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }

        //[ForeignKey("InvoiceId")]
        //public Invoice Invoice { get; set; }

    }
}
