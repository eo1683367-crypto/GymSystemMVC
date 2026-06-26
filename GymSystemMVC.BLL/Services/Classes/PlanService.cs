using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using GymSystemMVC.DAL.Models;
using GymSystemMVC.DAL.Repositories.Classes;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        // GET
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(null,false, ct);

            // check if Plans is null or empty
            if (!plans.Any()) return [];

            // Map members to MemberViewModel

            // Auto Mapped
           return mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(plans);

         
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default)
        {
            // Get specific plan by ID
            var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            // check if ID is Invalid !
            if (plan == null) return null!;

            return mapper.Map<Plan, PlanViewModel>(plan);
        }

        public async Task<UpdatePlanViewModel> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) return null!;

            return mapper.Map<Plan, UpdatePlanViewModel>(plan);
        }
        // POST

        public async Task<Result> TogglePlanStatusAsync(int planId, CancellationToken ct = default)
        {
            // Get specific plan by ID
            var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            // check if ID is Invalid !
            if (plan is null) return Result.NotFound("Plan Not Found");

            if (plan.IsActive)
            {
                var activeMemberShip = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(
                    mb => mb.PlanId == planId && mb.EndDate > DateTime.Now, false, ct);

                if (activeMemberShip != null)
                    return Result.Fail("Cannot Deactivate Plan With Active Memberships");
            }
            // Toggle the IsActive status
            plan.IsActive = !plan.IsActive;

            // Update the plan in the repository
            unitOfWork.GetRepository<Plan>().Update(plan);

            var result = await unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Toggle Plan Status");
        }

        public async Task<Result> UpdatePlanDetailsAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);

            if (plan is null) 
                return Result.NotFound("Plan Not Found");

            if (plan.Name != model.PlanName)
                return Result.Validation("Plan Name Cannot Be Changed");

            if (!plan.IsActive)
                return Result.Fail("Cannot Update An Inactive Plan");

            var activeMemberShip = await unitOfWork.GetRepository<MemberShip>()
                .FirstOrDefaultAsync(mb => mb.PlanId == planId && mb.EndDate > DateTime.Now, false, ct);

            if (activeMemberShip is not null)
                return Result.Fail("Cannot Update Plan With Active Memberships");


            plan.Duration = model.DurationDays;
            plan.Price = model.Price;
            plan.Description = model.Description;


            unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Plan");
        }

    }
}
