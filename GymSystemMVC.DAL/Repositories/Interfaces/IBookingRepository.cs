using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenaricRepository<Booking>
    {
        Task<List<Booking>> GetBookingWithMemberBySessionIdAsync(int sessionId , CancellationToken ct = default);
    }
}
