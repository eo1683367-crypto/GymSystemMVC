using GymSystemMVC.DAL.Contexts;
using GymSystemMVC.DAL.Repositories.Classes;
using GymSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository planRepository;

        public PlanController(IPlanRepository _planRepository)
        {
            planRepository = _planRepository;
        }


        // First Action => Get All Plan
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepository.GetAll(false , token);

            return View(plans);  // Return View With Model.
        }


        // Second Action => Get Details Of Specific Plan
        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetById(id, token);

            if (plan == null)
            {
                 RedirectToAction(nameof(Index));
            }

            return View(plan);  // Return View With Model.
        }


     

    }
}
