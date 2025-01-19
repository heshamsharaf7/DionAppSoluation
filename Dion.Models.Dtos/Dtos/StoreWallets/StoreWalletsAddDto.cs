
namespace Dion.Models.Dtos.Dtos.StoreWallets
{
    public class StoreWalletsAddDto
    {
        public string AccountNo { get; set; }
        public string Details { get; set; }
        public string EnteredDate { get; set; }
        public int StoreId { get; set; }
        public int WalletId { get; set; }
    }
}
