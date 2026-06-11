using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Classes;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // GET
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAll(false, ct);

            // check if Plans is null or empty
            if (!plans.Any()) return [];

            // Map members to MemberViewModel

            var plansViewModels = plans.Select(m => new PlanViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                DurationDays = m.Duration,
                Price = m.Price,
                IsActive = m.IsActive
            });

            return plansViewModels;
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default)
        {
            // Get specific plan by ID
            var plan = await unitOfWork.GetRepository<Plan>().GetById(planId, ct);

            // check if ID is Invalid !
            if (plan == null) return null!;

            // Create PlanVM Basiclly
            var planViewModel = new PlanViewModel()
            {
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.Duration,
                Price = plan.Price,
                IsActive = plan.IsActive

            };


            return planViewModel;
        }

        public async Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepository<Plan>().GetById(planId, ct);

            if (plan is null) return null!;

            return new UpdatePlanViewModel
            {
                Id = plan.Id,
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.Duration,
                Price = plan.Price
            };
        }
        // POST

        public async Task<bool> TogglePlanStatusAsync(int planId, CancellationToken ct = default)
        {
            // Get specific plan by ID
            var plan = await unitOfWork.GetRepository<Plan>().GetById(planId, ct);

            // check if ID is Invalid !
            if (plan is null) return false;

            if (plan.IsActive)
            {
                var activeMemberShip = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(
                    mb => mb.PlanId == planId && mb.EndDate > DateTime.Now, false, ct);

                if (activeMemberShip != null)
                    return false;
            }
            // Toggle the IsActive status
            plan.IsActive = !plan.IsActive;

            // Update the plan in the repository
            unitOfWork.GetRepository<Plan>().Update(plan);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }
       
        public async Task<bool> UpdatePlanDetailsAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepository<Plan>().GetById(planId, ct);

            if (plan is null) return false;

            // A plan with the same name already exists (excluding the current plan)
            if (plan.Name != model.PlanName)
            return false;

            var activeMemberShip = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(mb => mb.PlanId == planId && mb.EndDate > DateTime.Now, false, ct);
            if (activeMemberShip != null || !plan.IsActive)
                return false;
            
             plan.Duration = model.DurationDays;
             plan.Price = model.Price;
             plan.Description = model.Description;

            unitOfWork.GetRepository<Plan>().Update(plan);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }

    }
}
