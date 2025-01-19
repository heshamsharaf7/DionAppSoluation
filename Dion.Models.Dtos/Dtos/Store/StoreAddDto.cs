namespace Dion.Models.Dtos.Dtos.User
{
    public class StoreAddDto
    {
        //user info 
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EnteredDate { get; set; }
        public int UserType { get; set; }
        public int PhoneNo { get; set; }
        public string UserAddress { get; set; }
        public string UserPassword { get; set; }

        //store info
        public string StoreName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool StoreVerified { get; set; }
        public int StoreTypeId { get; set; }
        public int StorePhoneNo { get; set; }


    }
}
