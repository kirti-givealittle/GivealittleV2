using GivealittleV2.Application.Interfaces;
using GivealittleV2.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext context;
        public EntityRepository(ApplicationDbContext _context)
        {
            context = _context;
        }
    }
}
