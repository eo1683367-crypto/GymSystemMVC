using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using GymSystemMVC.DAL.Models;

namespace GymSystemMVC.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenaricRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? filter = null, CancellationToken ct = default);

        Task<Session> GetSessionByIdWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default);

        Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct);

    }
}
