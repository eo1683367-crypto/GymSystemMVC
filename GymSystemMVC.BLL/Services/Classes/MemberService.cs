using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;

namespace GymSystemMVC.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);

            // check if members is null or empty
            if (!members.Any()) return [];

            // Map members to MemberViewModel

            var membersViewModel = members.Select(m => new MemberViewModel()
            {  
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            });

            return membersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            // Get specific member by ID
            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);

            // check if ID is Invalid !
            if (member == null) return null!;

            // Create MemberVM Basiclly
            var memberViewModel = new MemberViewModel()
            {       
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBith.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.City} - {member.Address.Street}"
            };

            // MemberShip => to get Plan Details
            var activeMembership = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(mb => mb.MemberId == memberId && mb.EndDate > DateTime.Now,false, ct);

            // check if member has active membership
            if (activeMembership is not null)
            {
                var planActive = await unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId, ct);

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

            return new HealthRecordViewModel
            {
                Height = healthRecord.Height,
                Weight = healthRecord.Weight,
                BloodType = healthRecord.BloodType,
                Note = healthRecord.Note
            };
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);

            if (member is null) return null;

            return new MemberToUpdateViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber,
                Photo = member.Photo
            };
        }

        //POST
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExisting = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExisting = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExisting || phoneExisting) return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBith = model.DateOfBirth,
                Address = new Address
                {
                    Street = model.Street,
                    City = model.City,
                    BuildingNumber = model.BuildingNumber
                },
                HealthRecord = new HealthRecord
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            // Add member to database
            unitOfWork.GetRepository<Member>().Add(member);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }
        public async Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);

            if (member is null) return false;

            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct))
                return false;
            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct))
                return false;

            // Map MemberToUpdateVM to Member Entity


            member.Phone = model.Phone;
            member.Email = model.Email;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().Update(member);

            var Result = await unitOfWork.CompleteAsync();

            return Result > 0;


        }
        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var hasFutureSessions = await unitOfWork.GetRepository<Booking>().
                AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);

            if (hasFutureSessions) return false;

            unitOfWork.GetRepository<Member>().Delete(memberId);

            var result = await unitOfWork.CompleteAsync();

            return result > 0;
        }
      
    }
}
