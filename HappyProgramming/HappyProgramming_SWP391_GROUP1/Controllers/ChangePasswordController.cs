using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class ChangePasswordController : Controller
    {
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string CfNewPassword)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var Email = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

                var EmailAccount = context.Accounts.Where(x => x.Email == Email && x.Password == oldPassword).FirstOrDefault();
                if (newPassword != CfNewPassword)
                {
                    ViewBag.error = "The Confirm new password must be same the new password";
                    return View("~/Views/ChangePassword/ChangePassword.cshtml");
                }
                else
                {
                    if (EmailAccount == null)
                    {
                        ViewBag.error = "Please input the correct old password";
                        return View("~/Views/ChangePassword/ChangePassword.cshtml");
                    }
                    else
                    {
                        EmailAccount.Password = newPassword;
                        context.SaveChanges();
                        ViewBag.success = "ChangePassword Successfully";

                    }
                }
                return View("~/Views/Profile/Profile.cshtml");
            }


        }
    }
}

