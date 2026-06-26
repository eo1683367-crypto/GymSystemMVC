using System;
using System.Collections.Generic;
using System.Text;
using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.ViewModels.BookingViewModels;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;

namespace GymSystemMVC.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetMembersForSessionsAsync(int sessionId,CancellationToken ct = default); 
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(int sessionId, CancellationToken ct = default);
        Task<Result> CreateBookingAsync(CreateBookingViewModel model,CancellationToken ct = default);
        Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
        Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
    }
}
