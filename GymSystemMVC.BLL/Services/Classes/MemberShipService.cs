using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class MemberShipService : IMemberShipService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberShipService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberShips = await unitOfWork.MemberShipRepository.GetMemberShipsWithMemebersAndPlansAsync(mb => mb.EndDate > DateTime.Now,ct);

            if (!memberShips.Any()) return [];

            // Auto Mapped
            return mapper.Map<IEnumerable<MemberShip>, IEnumerable<MemberShipViewModel>>(memberShips);

            
        }
        public async Task<Result> CreateMemberShipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            // checks Business Rules

            var memberExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
            if (!memberExist)
                return Result.NotFound("Member Is Not Found");

            var planExist = await unitOfWork.GetRepository<Plan>().AnyAsync(m => m.Id == model.PlanId, ct);
            if (!planExist) 
                return Result.NotFound("Plan Is Not Found");


            var activeMemberShip = await unitOfWork.MemberShipRepository.AnyAsync(m=>m.Id == model.MemberId && m.EndDate > DateTime.Now,ct);
            if (activeMemberShip)
                return Result.Fail("Member Has aready Active MemberShip");

           var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct);
            if (!plan.IsActive)
                return Result.Fail("Plan Is NOt Active");


            var memberShip = new MemberShip
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                CreatedAt = DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.Duration),
            };


            unitOfWork.MemberShipRepository.Add(memberShip);

            var result = await unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Can Not Create MemberShip");
        }


        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(null, false, ct);

            return mapper.Map<IEnumerable<Member>, IEnumerable<MemberSelectListViewModel>>(members);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAllAsync(null, false, ct);

            return mapper.Map<IEnumerable<Plan>, IEnumerable<PlanSelectListViewModel>>(plans);
        }


        public async Task<Result> DeleteActiveMemberShipAsync(int memberId, CancellationToken ct = default)
        {
            var activeMemberShip = await unitOfWork.MemberShipRepository
                                  .FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now,true,ct);

            if (activeMemberShip is  null)
                return Result.Fail("Active MemberShip not found for Member");

            unitOfWork.MemberShipRepository.Delete(activeMemberShip.Id);

            var rowEffected = await unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Can Not Cancelled MemberShip");
        }

        
    }
}
