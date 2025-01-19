namespace Dion.Models.Dtos.Dtos.User
{
    public class StoreCustomerOrderGetDto
    {
        public int Id { get; set; }
        public double AccountCapacity { get; set; }
        public bool IsLock { get; set; }
        public string EnteredDate { get; set; }
        public bool PayNotification { get; set; }
        public string CuName { get; set; }
        public string CuAddress { get; set; }
        public bool IsAccepted { get; set; }

        public int StoreTypeId { get; set; }
        public string StoreTypeName { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }



    }
}
