using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Verified { get; set; }
        public string EnteredDate { get; set; }
        public int StoreTypeId { get; set; }
        public int UserId { get; set; }
        public int StorePhoneNo { get; set; }
        [ForeignKey("StoreTypeId")]
        public StroreType StroreType { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
