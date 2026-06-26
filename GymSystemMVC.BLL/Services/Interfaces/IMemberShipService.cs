using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface IMemberShipService
    {
        Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);

        // create
        Task<Result> CreateMemberShipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);

        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default);

        // Delete

        Task<Result> DeleteActiveMemberShipAsync(int memberId,CancellationToken ct = default); 
    }
}
