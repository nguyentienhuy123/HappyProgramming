using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;
using HappyProgramming_SWP391_GROUP1.Common;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");

        }
        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string rememberMe)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var account = context.Accounts.Where(a => a.UserName.Equals(username)
                                                    && a.Password.Equals(password)).FirstOrDefault();
                if (account == null)
                {

                    ViewBag.error = "Invalid username or password";
                    return View();
                }
                else
                {
                    var profile = context.Profiles.Where(x => x.Email.Equals(account.Email)).FirstOrDefault();
                    AuthenticationCustom(account, rememberMe, profile.Avatar);
                    Cache.message = "Login success";
                    HttpContext.Session.SetString("username", username);
                    return RedirectToAction("Course", "Course");
                }
            }


        }

        public void AuthenticationCustom(Account account, string remember, string profile)
        {
            bool check = false;
            if (remember != null)
            {
                check = true;
            }
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Email),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(ClaimTypes.Spn , profile),
                    new Claim(ClaimTypes.Dsa , account.Id.ToString())
                };
            ClaimsIdentity identity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = check
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
        }
    }
}
