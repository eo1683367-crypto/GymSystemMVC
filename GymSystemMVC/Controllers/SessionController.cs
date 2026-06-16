using GymSystemMVC.BLL.Common;
using GymSystemMVC.BLL.Services.Classes;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemMVC.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService _sessionService)
        {
            sessionService = _sessionService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(nameof(Create), model);
            }

            var result = await sessionService.CreateSessionAsync(model, ct);

            if (result.Success)
            {
                TempData["Success"] = "Session Created Succefully";
                return RedirectToAction(nameof(Index));
            }

                TempData["Failed"] = result.Error;

            await PopulateDropDownAsync(ct);
            return View(nameof(Create), model);

        }
        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await sessionService.GetTrainersForDropDownAsync(ct),"Id","Name");
            ViewBag.Categories = new SelectList(await sessionService.GetCategoriesForDropDownAsync(ct),"Id", "CategoryName");

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var session = await sessionService.GetSessionDetailsAsync(id, ct);

            if(session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken ct)
        {
            var session = await sessionService.GetSessionToUpdateAsync(id, ct);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Can Not Be Edit";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownAsync(ct);
            return View(session);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(model);
            }
            var result = await sessionService.UpdateSessionAsync(id, model, ct);

            if (result.Success)
            {
                TempData["Success"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Failed"] = result.Error;

            await PopulateDropDownAsync(ct);
            return View(model);

        }




        [HttpGet]
        public async Task<ActionResult> Delete(int sessionId,CancellationToken ct)
        {
            var session = await sessionService.GetSessionById(sessionId,ct);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found!";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int sessionId, CancellationToken ct)
        {
            var result = await sessionService.DeleteSesssionAsync(sessionId, ct);

            TempData[result.Success ? "Success" : "Failed"] = result.Success ? "Session Deleted Succefully" : result.Error;
            return RedirectToAction(nameof(Index));
        }
    }
}
