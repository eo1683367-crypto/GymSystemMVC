using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default);

        Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default);

        Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct);

    }
}
