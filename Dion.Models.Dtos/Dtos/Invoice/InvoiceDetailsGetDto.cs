using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dion.Models.Dtos.Dtos.Accounting
{
    public class InvoiceDetailsGetDto
    {
        public int Id { get; set; }
        public string Statement { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
        public string ParticipantName { get; set; }
        public int ParticipantId { get; set; }
        public string EnteredDate { get; set; }


    }
}
