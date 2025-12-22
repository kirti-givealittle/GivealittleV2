using GivealittleV2.Domain.Models.Cause;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Application.Interfaces
{
    public interface IEntityRepository
    {
        Task<CauseResponse> CreateCauseDraft(CauseDTO request);
        Task CreateCauseDraft(CauseDTO request);
    }
}
