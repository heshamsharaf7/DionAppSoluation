using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Models.Dtos.Dtos.Accounting
{
    public class InvoiceAddDto
    {

        public string EnteredDate { get; set; }
        //public string Details { get; set; }
        public int CustomerId { get; set; }
        public int CurrencyId { get; set; }
        public int ParticipantId { get; set; }
        public int StoreId { get; set; }
        //public bool IsCustomerBuyer { get; set; }

        public List<InvoiceItems> InvoiceItems { get; set; }




    }



    public class InvoiceItems
    {
        public string Statement { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }


    }
}
