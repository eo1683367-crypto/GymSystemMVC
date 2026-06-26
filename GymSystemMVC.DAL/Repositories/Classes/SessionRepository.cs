using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class SessionRepository : GenaricRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext gymDbContext;

        public SessionRepository(GymDbContext _gymDbContext) : base(_gymDbContext)
        {
            gymDbContext = _gymDbContext;
        }

        // Special Method
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? filter, CancellationToken ct = default)
        {
            var sessions =  gymDbContext.Sessions
                                        .Include(s => s.Trainer)
                                        .Include(s => s.Category)
                                        .AsNoTracking();

            if (filter is not null)
            {
                sessions = sessions.Where(filter);
            }

            return await sessions.ToListAsync(ct);
        }

        public Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct)
        {
            return gymDbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId);
        }

        public async Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default)
        {
            var session = gymDbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category)
                .FirstOrDefaultAsync(s=>s.Id == sessionId);

            return await session;
        }
    }
}
