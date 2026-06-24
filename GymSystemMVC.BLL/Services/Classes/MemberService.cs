using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using AutoMapper;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        // GET
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(null, false, ct);

            // check if members is null or empty
            if (!members.Any()) return [];

            // Map members to MemberViewModel

            // Auto Mapped
            return mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);

            
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            // Get specific member by ID
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            // check if ID is Invalid !
            if (member == null) return null!;

            // Auto Mapped
            var memberViewModel = mapper.Map<Member, MemberViewModel>(member);

            // MemberShip => to get Plan Details
            var activeMembership = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(mb => mb.MemberId == memberId && mb.EndDate > DateTime.Now,false, ct);

            // check if member has active membership
            if (activeMembership is not null)
            {
                var planActive = await unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMembership.PlanId, ct);

                // Map Plan Details to MemberVM
                memberViewModel.PlanName = planActive?.Name;
                memberViewModel.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }

            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var healthRecord = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, false, ct);

            if (healthRecord is null) return null;

            return mapper.Map<HealthRecord, HealthRecordViewModel>(healthRecord);
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);

            if (member is null) return null;


            // Auto Mapped
            return mapper.Map<Member, MemberToUpdateViewModel>(member);
        }

        //POST
        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExisting = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            if (emailExisting)
                return Result.Fail("Email Already Exists", ResultKind.Conflict);

            var phoneExisting = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (phoneExisting)
                return Result.Fail("Phone Already Exists", ResultKind.Conflict);


            // Auti Mapped
            var member = mapper.Map<CreateMemberViewModel, Member>(model);

            // Add member to database
            unitOfWork.GetRepository<Member>().Add(member);

            var result = await unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create Member");
        }
        public async Task<Result> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");

            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct))
                return Result.Fail("Email Already Used By Another Member", ResultKind.Conflict);

            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct))
                return Result.Fail("Phone Already Used By Another Member", ResultKind.Conflict);

            // Map MemberToUpdateVM to Member Entity


            member.Phone = model.Phone;
            member.Email = model.Email;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().Update(member);

            var result = await unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Member");


        }
        public async Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");

            var hasFutureSessions = await unitOfWork.GetRepository<Booking>()
                .AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);

            if (hasFutureSessions)
                return Result.Fail("Cannot Delete Member With Future Booked Sessions");

            unitOfWork.GetRepository<Member>().Delete(memberId);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Member");
        }
      
    }
}
