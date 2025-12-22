using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Domain.Models.Cause
{
    public class CauseBeneficiaryDetails
    {
        public MyAccount? MyAccount { get; set; }
        public OtherAccount? OtherAccount { get; set; }

    }
}
