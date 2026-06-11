using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        // GET
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default);

        Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);

        // POST
        Task<bool> TogglePlanStatusAsync(int planId, CancellationToken ct = default);
        Task<bool> UpdatePlanDetailsAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default);

    }
}
