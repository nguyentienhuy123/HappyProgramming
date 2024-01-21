using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class SignUpController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Verify()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Verify(string pin)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var accoutByPin = context.Accounts.AsNoTracking().Where(x => x.Pin == pin).FirstOrDefault();

                if (accoutByPin == null)
                {
                    ViewBag.error = "PIN code is wrong !";
                    return View("~/Views/SignUp/Verify.cshtml");
                    //return RedirectToAction("Verify", "SignUp");
                }
                else
                {
                    var profile = context.Profiles.Where(x => x.Email.Equals(accoutByPin.Email)).FirstOrDefault();
                    AuthenticationCustom(accoutByPin, "check", profile.Avatar);
                    ViewBag.success = "Login success";
                    return View("~/Views/Launch/Launch.cshtml");
                }
            }
        }



        [HttpPost]
        public IActionResult SignUp(string username, string password, string email)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var userCount = context.Accounts.AsNoTracking().Where(x => x.UserName == username || x.Email == email).Count();
                if (userCount > 1)
                {
                    ViewBag.error = "Email already exit !";
                    return View("~/Views/SignUp/Index.cshtml");
                }
                else
                {

                    int pin = GenPin();
                    var Account = new Account()
                    {
                        UserName = username,
                        Password = password,
                        Email = email,
                        Role = "Mentee",
                        Pin = pin.ToString(),
                    };
                    var Profile = new Profile()
                    {
                        Email = email,
                        Avatar = "/hometemplate/images/149071.png",
                        
                    };
                    context.Accounts.Add(Account);
                    context.Profiles.Add(Profile);
                    context.SaveChanges();
                    SendEmail(Account.Email, "Verify Sign Up", "Mã pin của bạn: " + pin);

                }
                return View("~/Views/SignUp/Verify.cshtml");
            }

        }
        //     return Guid.NewGuid().ToString();
        //const string chars = "0123456789!@#$%^&*()";
        //Random random = new Random();
        //int length = 8; // Độ dài chuỗi ngẫu nhiên bạn muốn tạo
        //string pin = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        //return pin; 

        private static int GenPin()
        {
            Random random = new Random();
            int pin = random.Next(1000, 9999);
            return pin;
        }

        public async void SendEmail(string toEmail, string subject, string body)
        {
            //   var apiKey = Environment.GetEnvironmentVariable("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");
            var client = new SendGridClient("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");
            var from = new EmailAddress("cuongbui0110@gmail.com");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "Content-Type:text/plain; charset=UTF-8", body);
            var response = await client.SendEmailAsync(msg);
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
