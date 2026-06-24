using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemMVC.DAL.Repositories.Classes
{
    public class BookingRepository : GenaricRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext gymDbContext;

        public BookingRepository(GymDbContext gymDbContext) : base(gymDbContext) 
        {
            this.gymDbContext = gymDbContext;
        }
        public async Task<List<Booking>> GetBookingWithMemberBySessionIdAsync(int sessionId, CancellationToken ct = default)
        {
            return await gymDbContext.Bookings
                                     .AsNoTracking()
                                     .Include(b => b.Member)
                                     .Where(b => b.SessionId == sessionId)
                                     .ToListAsync(ct);
        }
    }
}
