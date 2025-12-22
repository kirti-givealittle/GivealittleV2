using GivealittleV2.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.Cause
{
    public class CauseGeneralDetails
    {
        public required bool IsIndividual { get; set; }
        public required string ContactNumber { get; set; }
        public DateTime? DOB { get; set; } //Individuals only
        public BusinessDetails? Business { get; set; } //Businesses only
        public SchoolDetails? School { get; set; } //Schools only
        public CharityDetails? Charity { get; set; } //Charities only
        public OtherOrganizationDetails? OtherOrganization { get; set; } //Other Organizations only
        //public required string ImageURL { get; set; }
    }

}
