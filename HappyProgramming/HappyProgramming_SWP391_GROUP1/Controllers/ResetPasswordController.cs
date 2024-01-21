using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Principal;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
	public class ResetPasswordController : Controller
	{
		[HttpGet]
		public IActionResult Confirm()
		{
			return View();
		}

		[HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
		public IActionResult Confirm(string email)
		{
			using (HappyProgrammingContext context = new HappyProgrammingContext())
			{
				var EmailAccount = context.Accounts.Where(x => x.Email == email).FirstOrDefault();
				if (EmailAccount == null)
				{
					ViewBag.error = "Email not exit !";
                    return View("~/Views/Login/ResetPassword.cshtml");
                }
				else
				{
					Random random = new Random();
					int pin = random.Next(1000, 9999);
					EmailAccount.Pin = pin.ToString();
                    context.SaveChanges();
					SendEmail(EmailAccount.Email, "Reset Password", "Mã pin của bạn: " + pin);
				}
				return View("~/Views/ResetPassword/Confirm.cshtml");
			}
		}

        [HttpPost]
        public IActionResult ConfirmChange(string pin, string newPass, string cfPass)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
				
				var account = context.Accounts.AsNoTracking().Where(x => x.Pin == pin).FirstOrDefault();
                if (account == null)
				{
                    ViewBag.error = "PIN not exit !";
					return View("~/Views/ResetPassword/Confirm.cshtml");
                }
				if (newPass != cfPass)
				{
                    ViewBag.error = "The password must be the same!";
                    return View("~/Views/ResetPassword/Confirm.cshtml");
                }
				else
				{ 
					account.Password = cfPass;
					context.SaveChanges();
				}
				ViewBag.success = "Change password successful"; 
                return View("~/Views/Login/Login.cshtml");
            }
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
	}
	
}
