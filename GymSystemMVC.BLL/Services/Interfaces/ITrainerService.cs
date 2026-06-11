using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.BLL.ViewModels.TrainerViewModels;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        // GET
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default);

        Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default);

        Task<TrainerToUpdateViewModel> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default);
        // POST

        Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);

        Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> DeleteTrainerAsync(int memberId, CancellationToken ct = default);
    }
}
