using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class MemberShipRepository : GenaricRepository<MemberShip>, IMemberShipRepository
    {
        private readonly GymDbContext gymDbContext;

        public MemberShipRepository(GymDbContext gymDbContext) : base(gymDbContext) 
        {
            this.gymDbContext = gymDbContext;
        }
        public async Task<IEnumerable<MemberShip>> GetMemberShipsWithMemebersAndPlansAsync(Expression<Func<MemberShip, bool>>? filter, CancellationToken ct)
        {
            var memberShips = gymDbContext.Memberships
                                          .Include(s => s.Member)
                                          .Include(s => s.Plan)
                                          .AsNoTracking();

            if (filter is not null)
            {
                memberShips = memberShips.Where(filter);
            }
            
            return await memberShips.ToListAsync(ct);
        }
    }
}
