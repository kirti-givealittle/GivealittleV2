using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.Auth
{
    public class RegistrationDTO
    {
        public required string FName { get; set; } 
        public required string LName { get; set; }
        public required string Email { get; set; }
    }
}
