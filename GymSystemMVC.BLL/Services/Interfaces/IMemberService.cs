using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.DAL.Entities;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        // GET
        // Model --> ViewModel --> VIEW

        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);

        // POST
        // ViewModel --> Model --> DB

        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
