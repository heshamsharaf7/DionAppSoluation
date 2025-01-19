using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class CustomerParticipant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnteredDate { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public StoreCustomers StoreCustomers { get; set; }
    }
}
