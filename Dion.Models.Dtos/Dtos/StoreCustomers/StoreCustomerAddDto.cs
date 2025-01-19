namespace Dion.Models.Dtos.Dtos.User
{
    public class StoreCustomersAddDto
    {
        public double AccountCapacity { get; set; }
        public bool IsLock { get; set; }
        public string EnteredDate { get; set; }
        public bool PayNotification { get; set; }
        public string CuName { get; set; }
        public string CuAddress { get; set; }
        public bool IsAccepted { get; set; }

        public int StoreTypeId { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }



    }
}
