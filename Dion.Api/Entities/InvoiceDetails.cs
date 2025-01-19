using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class InvoiceDetails
    {
        public int Id { get; set; }
        public string Statement { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
        //[NotMapped]
        //public string EnteredDate { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }

    }
}
