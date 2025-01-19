
namespace Dion.Models.Dtos.Dtos.StoreWallets
{
    public class StoreWalletsGetDto
    {
        public int Id { get; set; }
        public string AccountNo { get; set; }
        public string Details { get; set; }
        public string EnteredDate { get; set; }
        public int StoreId { get; set; }
        public int WalletId { get; set; }
        public string IconPath { get; set; }


    }
}
