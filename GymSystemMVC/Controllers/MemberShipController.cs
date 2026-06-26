using GymSystemMVC.BLL.Services.Classes;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MemberShipsViewModels;
using GymSystemMVC.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystemMVC.Controllers
{
    [Authorize]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipService memberShipService;

        public MemberShipController(IMemberShipService memberShipService)
        {
            this.memberShipService = memberShipService;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberShips = await memberShipService.GetAllMembershipsAsync(ct);
            return View(memberShips);
        }      

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync(ct);
                return View(nameof(Create), model);
            }

            var result = await memberShipService.CreateMemberShipAsync(model, ct);

            if (result.Success)
            {
                TempData["Success"] = "MemberShip Created Succefully";
                return RedirectToAction(nameof(Index));
            }

            TempData["Failed"] = result.Error;

            await PopulateDropDownAsync(ct);
            return View(nameof(Create), model);

        }
        private async Task PopulateDropDownAsync(CancellationToken ct)
        {
            ViewBag.Members = new SelectList(await memberShipService.GetMembersForDropDownAsync(ct), "Id", "Name");
            ViewBag.Plans = new SelectList(await memberShipService.GetPlansForDropDownAsync(ct), "Id", "Name");

        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var result = await memberShipService.DeleteActiveMemberShipAsync(id, ct);

            TempData[result.Success ? "Success" : "Failed"] = result.Success ? "MemberShip Cancelled Succefully" : result.Error;
            return RedirectToAction(nameof(Index));
        }
    }
}
