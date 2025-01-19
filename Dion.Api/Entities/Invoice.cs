using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string EnteredDate { get; set; }
        public bool LockStatus { get; set; }
        public string LockDate { get; set; }
        public int CustomerId { get; set; }
        public int CurrencyId { get; set; }
        //public bool IsCustomerBuyer { get; set; }
        public int ParticipantId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }
        //[ForeignKey("ParticipantId")]
        //public CustomerParticipant? CustomerParticipant { get; set; }
    }
}
