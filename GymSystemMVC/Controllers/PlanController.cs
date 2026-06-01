using GymSystem.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {

        private readonly GymDbContext gymDbContext = new GymDbContext(); // Composition Relation

        // First Action => Get All Plan
        public async Task<IActionResult> Index()
        {
            var plans = await gymDbContext.Plans.ToListAsync();

            return View(plans);  // Return View With Model.
        }


        // Second Action => Get Details Of Specific Plan
        public async Task<IActionResult> Details(int id)
        {
            var plan = await gymDbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
            {
                 RedirectToAction(nameof(Index));
            }

            return View(plan);  // Return View With Model.
        }


     

    }
}
