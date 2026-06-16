using System.Diagnostics;
using GymSystemMVC.BLL.Services.Interfaces;
using GymSystemMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymSystemMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IAnalyticsService analyticsService;

        public HomeController(ILogger<HomeController> _logger, IAnalyticsService _analyticsService)
        {
            logger = _logger;
            analyticsService = _analyticsService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Data = await analyticsService.GetAnalyticsDataAsync(ct);
            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
