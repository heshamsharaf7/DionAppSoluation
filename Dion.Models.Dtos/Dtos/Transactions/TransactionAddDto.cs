using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dion.Models.Dtos.Dtos.Transactions
{
    public class TransactionAddtDto
    {
        public string Statement { get; set; }
        public string EnteredDate { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public int CurrencyId { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }

    }
}
