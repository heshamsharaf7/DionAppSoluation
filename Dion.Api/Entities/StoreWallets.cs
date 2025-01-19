using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class StoreWallets
    {
        public int Id { get; set; }
        public string AccountNo { get; set; }
        public string Details { get; set; }
        public string EnteredDate { get; set; }
        public int StoreId { get; set; }
        public int WalletId { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; }
        [ForeignKey("StoreId")]
        public Store Store { get; set; }
    }
}
