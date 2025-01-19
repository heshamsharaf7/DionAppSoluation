using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dion.Models.Dtos.Dtos.CustomerParticipant
{
    public class CustomerParticipantAddDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EnteredDate { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }
    }
}
