using System.ComponentModel.DataAnnotations.Schema;

namespace Dion.Api.Entities
{
    public class Wallets
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }
        public string EnteredDate { get; set; }

    }
}
