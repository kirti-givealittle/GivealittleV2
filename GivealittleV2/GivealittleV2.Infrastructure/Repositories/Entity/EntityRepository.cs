using GivealittleV2.Application.Interfaces.Entity;
using GivealittleV2.Domain.Models.Auth;
using GivealittleV2.Infrastructure.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GivealittleV2.Infrastructure.Repositories.Entity
{
    public class EntityRepository : IEntityRepository
    {
        private readonly ApplicationDbContext _db;
        public EntityRepository(ApplicationDbContext _context)
        {
            _db = _context;
        }

        
    }
}
