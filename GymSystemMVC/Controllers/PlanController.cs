using GymSystemMVC.BLL.Services.Classes;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.DAL.Entities;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {

        private readonly IPlanService planService;

        public PlanController(IPlanService _planService)
        {
            planService = _planService;
        }



        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planService.GetAllPlansAsync(token);

            return View(plans);  // Return View With Model.
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planService.GetPlanDetailsAsync(id, token);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);  // Return View With Model.
        }


        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken token)
        {
            var result = await planService.TogglePlanStatusAsync(id, token);

            if (result)
                TempData["Success"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Plan Not Found";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            var plan = await planService.GetPlanToUpdateAsync(id, token);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlan(int id, UpdatePlanViewModel model, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }
            var result = await planService.UpdatePlanDetailsAsync(id, model, token);

            if (result)
                TempData["Success"] = "Plan Updated Successfully";
            else
                TempData["Failed"] = "Update Failed!";
            return RedirectToAction(nameof(Index));

        }

    }
}