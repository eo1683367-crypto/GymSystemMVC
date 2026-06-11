using GymSystemMVC.BLL.Services.Classes;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.PlanViewModels;
using GymSystemMVC.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemMVC.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService _trainerService)
        {
            trainerService = _trainerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var trainers = await trainerService.GetAllTrainersAsync();


            return View(trainers);
        }

        [HttpGet]
        public async Task<IActionResult> TrainerDetails(int id)
        {
            var trainer = await trainerService.GetTrainerDetailsAsync(id);

            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model,CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create),model);
            }

            var result = await trainerService.CreateTrainerAsync(model, ct);

            if (result)
                TempData["Success"] = "Trainer Created Successfully";
            else
                TempData["Failed"] = "Failed To Create Trainer";

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken token)
        {
            var trainer = await trainerService.GetTrainerToUpdateAsync(id, token);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> EditTrainer(int id, TrainerToUpdateViewModel model, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }

            var result = await trainerService.UpdateTrainerDetailsAsync(id, model, token);

            if (result)
                TempData["Success"] = "Trainer Updated Successfully";
            else
                TempData["Failed"] = "Update Failed!";
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await trainerService.DeleteTrainerAsync(id, ct);
            if (result)
                TempData["Success"] = "Trainer Deleted Successfully";
            else
                TempData["Failed"] = "Delete Failed!";
            return RedirectToAction(nameof(Index));
        }

    }
}
