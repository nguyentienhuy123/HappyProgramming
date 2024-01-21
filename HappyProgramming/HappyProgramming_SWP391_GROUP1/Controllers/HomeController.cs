using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.success = "Hello " + claimsPrincipal.FindFirstValue(ClaimTypes.Name);
                return View("~/Views/Launch/Launch.cshtml");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
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