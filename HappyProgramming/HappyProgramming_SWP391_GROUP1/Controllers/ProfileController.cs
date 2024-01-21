using Microsoft.AspNetCore.Mvc;
using HappyProgramming_SWP391_GROUP1.Models;
using System.Security.Claims;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using HappyProgramming_SWP391_GROUP1.Common;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{

    public class ProfileController : Controller
	{
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;
        public ProfileController(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        [HttpGet]
		public IActionResult Profile()
		{
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var profile = context.Profiles
                    .Where(x => x.Email.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)))
                    .FirstOrDefault();

                ViewBag.data = profile;
                if (profile.Name == null && profile.Dob == null && profile.Phone == null)
                {
                    ViewBag.Type = "edit";
                }
                if (!Cache.message.Equals(""))
                {
                    ViewBag.success = Cache.message;
                    Cache.message = "";
                }
                return View();
		}

        }
        [HttpGet]
        public IActionResult Edit()
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var profile = context.Profiles
                    .Where(x => x.Email.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)))
                    .FirstOrDefault();

                ViewBag.data = profile;
                ViewBag.Type = "edit1";
                return View("~/Views/Profile/Profile.cshtml");

            }
        }

        [HttpPost]
        public IActionResult Update(Profile profile)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var profileDb = context.Profiles
                    .Where(x => x.Email.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)))
                    .FirstOrDefault();

                profileDb.Description = profile.Description;
                profileDb.Address = profile.Address;
                profileDb.Phone = profile.Phone;
                if (profile.Address != null && profile.Name != null && profile.Dob != null)
                {
                    profileDb.Name = profile.Name;
                    profileDb.Dob = profile.Dob;
                   
                }
                context.SaveChanges();
                ViewBag.Data = profileDb;
                ViewBag.success = "Update successful";
                return View("~/Views/Profile/Profile.cshtml");
            }
        }
        [HttpPost]
        public IActionResult UploadImage(IFormFile Image)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads/" + claimsPrincipal.FindFirstValue(ClaimTypes.Name));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Path.GetFileName(Image.FileName);

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    Image.CopyTo(stream);

                }
                var profile = context.Profiles
                    .Where(x => x.Email.Equals(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)))
                    .FirstOrDefault();
                profile.Avatar = "Uploads/" + claimsPrincipal.FindFirstValue(ClaimTypes.Name) + "/" + fileName;
                context.SaveChanges();
                var account = context.Accounts
                         .Where(x => x.Email.Equals(profile.Email))
                         .FirstOrDefault();
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Email),
                    new Claim(ClaimTypes.Role, account.Role),
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(ClaimTypes.Spn , profile.Avatar),
                    new Claim(ClaimTypes.Dsa , account.Id.ToString())
                };
                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = false

                };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);
                Cache.message = "Update success";
                return RedirectToAction("Profile", "Profile");
            }
        }
	}
}
