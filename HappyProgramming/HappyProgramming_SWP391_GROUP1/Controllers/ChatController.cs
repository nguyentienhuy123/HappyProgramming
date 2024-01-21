using HappyProgramming_SWP391_GROUP1.Controllers;
using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
//using Microsoft.AspNetCore.SignalR;

public class ChatController : Controller
{
    //private readonly IHubContext<ChatHub> _chatHubContext;

    public ChatController()
    {
        //_chatHubContext = chatHubContext;
    }

    public IActionResult listChat()
    {
        using (HappyProgrammingContext context = new HappyProgrammingContext())
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            Guid accId = new Guid(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
            var listMyCourse = context.Courses.Where(c => c.Id == accId).ToList();

        }

        return View();
    }

    [HttpGet]
    public IActionResult Chat(Guid CourseId)
    {
        //int sendId = 1;
        //int receiveId = 2;
        //ViewBag.SendId = sendId;
        //ViewBag.ReceiveId = receiveId;
        using (HappyProgrammingContext context = new HappyProgrammingContext())
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            Guid sendId = new Guid(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
            var receiveId = CourseId;
            ViewBag.SendId = sendId;
            ViewBag.ReceiveId = receiveId;

        }

        return View("Chat");

    }

    public async Task<IActionResult> SendClassMessage(Guid sendId, Guid courseId, string content)
    {
        using (HappyProgrammingContext context = new HappyProgrammingContext())
        {

            var message = new MessageDetail
            {
                SendId = sendId,
                ReceiveId = courseId,
                Content = content,
                CreateDate = DateTime.Now,
                IsClass = true
            };

            context.MessageDetails.Add(message);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
    //receive mess
    [HttpPost]
    public async Task<IActionResult> ReceiveMessage(Guid sendId, Guid receiveId, string content)
    {
        using (HappyProgrammingContext context = new HappyProgrammingContext())
        {
            var message = new MessageDetail
            {
                SendId = sendId,
                ReceiveId = receiveId,
                Content = content,
                CreateDate = DateTime.Now,
                IsClass = true
            };

            context.MessageDetails.Add(message);
            await context.SaveChangesAsync();
            return Ok(new { message = "Tin nhắn đã được nhận thành công." });
        }
    }
}
