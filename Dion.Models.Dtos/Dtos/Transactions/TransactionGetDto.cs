using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dion.Models.Dtos.Dtos.Transactions
{
    public class TransactionGetDto
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
        public string CustomerName { get; set; }

    }
}
