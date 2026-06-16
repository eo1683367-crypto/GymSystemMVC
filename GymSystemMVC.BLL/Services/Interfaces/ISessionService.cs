using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using GymSystemMVC.BLL.ViewModels.TrainerViewModels;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);

        Task<Result> CreateSessionAsync (CreateSessionViewModel model ,CancellationToken ct = default);

        Task<SessionViewModel?> GetSessionDetailsAsync(int id, CancellationToken ct = default);

        Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default);

        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);


        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);


        Task<Result> DeleteSesssionAsync(int sessionId, CancellationToken ct = default);


        Task<SessionViewModel> GetSessionById(int sessionId, CancellationToken ct);

    }
}
