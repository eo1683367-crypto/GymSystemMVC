using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
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
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
        {
            var sessions = gymDbContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category);

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
