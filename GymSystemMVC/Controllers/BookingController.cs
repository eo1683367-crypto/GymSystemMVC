using GymSystemMVC.BLL.Services.Classes;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.BookingViewModels;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemMVC.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await bookingService.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession(int id,CancellationToken ct)
        {
            var members = await bookingService.GetMembersForSessionsAsync(id, ct);
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
        {
            var members = await bookingService.GetMembersForSessionsAsync(id, ct);
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            var members = await GetMemberForDropDown(id, ct);

            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model,CancellationToken ct)
        {
            var result = await bookingService.CreateBookingAsync(model, ct);

            TempData[result.Success ? "Success" : "Failed"] = result.Success ? "Booking Created Succefully" : result.Error;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }


        [HttpPost]
        public async Task<IActionResult> Attended (int memberId, int sessionId, CancellationToken ct)
        {
            var result = await bookingService.MarkAttendedAsync(memberId, sessionId, ct);


            TempData[result.Success ? "Success" : "Failed"]
                                    = result.Success ? "Marked Attended Succefully" : result.Error;

            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken ct)
        {
            var result = await bookingService.CancelBookingAsync(memberId, sessionId, ct);


            TempData[result.Success ? "Success" : "Failed"]
                                    = result.Success ? "Booking Cancelled Succefully" : result.Error;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }

        private async Task<IEnumerable<MemberSelectListViewModel>> GetMemberForDropDown(int sessionId,CancellationToken ct)
        {
            var members = await bookingService.GetMembersForDropDownAsync(sessionId, ct);
            return members;
        }
    }
}
