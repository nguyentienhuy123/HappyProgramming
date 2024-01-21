using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HappyProgramming_SWP391_GROUP1.Common;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class LaunchController : Controller
    {
        [Authorize]
        public IActionResult Launch()
        {
            if (!Cache.message.Equals(""))
            {
                ViewBag.success = Cache.message;
                Cache.message = "";
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
