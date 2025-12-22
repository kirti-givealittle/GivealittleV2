using GivealittleV2.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.Cause
{
    public class CauseDTO
    {
        public Guid IndividualEntityID { get; set; }
        public required CauseGeneralDetails CauseGeneralDetails { get; set; }
        //public required CauseBeneficiaryDetailsDTO CauseBeneficiaryDetails { get; set; }
        //public required CausePageDetailsDTO CausePageDetails { get; set; }
        //public required string ImageURL { get; set; }
    }
}
