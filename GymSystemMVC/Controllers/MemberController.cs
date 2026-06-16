using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemMVC.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService memberSevice;

        public MemberController(IMemberService _memberSevice)
        {
            memberSevice = _memberSevice;
        }
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await memberSevice.GetAllMembersAsync(ct);
            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            var result = await memberSevice.CreateMemberAsync(model, ct);

            if (result.Success)
                TempData["Success"] = "Member Created Succefully";
            else
                TempData["Failed"] = result.Error;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await memberSevice.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await memberSevice.GetMemberHealthRecordAsync(id, ct);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var member = await memberSevice.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(nameof(Edit), model);

            var result = await memberSevice.UpdateMemberDetailsAsync(id, model, ct);

            if (result.Success)
                TempData["Success"] = "Member Updated Successfully";
            else
                TempData["Failed"] = result.Error;

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
            var result = await memberSevice.DeleteMemberAsync(id, ct);

            TempData[result.Success ? "Success" : "Failed"] = result.Success ? "Member Deleted Successfully" : result.Error;

            return RedirectToAction(nameof(Index));
        }
    }
}
