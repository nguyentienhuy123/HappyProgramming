using HappyProgramming_SWP391_GROUP1.Models;
using HappyProgramming_SWP391_GROUP1.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class RequestController : Controller
    {
        [HttpGet]
        public IActionResult SendRequest(Guid receiveId, Guid courseId)
        {
            ViewBag.ReceiveId = receiveId;
            ViewBag.CourseId = courseId;
            return PartialView("Privacy");
        }

        [HttpPost]
        public IActionResult SendRequest2([FromQuery(Name = "receiveId")] Guid receiveId, [FromQuery(Name = "courseId")] Guid courseId)
        {

            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            Request request = new Request();

            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {

                var listRequest = context.Requests.ToList();

                Guid studentId = new Guid(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
                List<Course> listCourse = context.Courses.ToList();
                List<StudentOfCourse> listStudentOfCourse = context.StudentOfCourses.Where(s => s.CourseId == courseId).ToList();

                foreach (var s in listStudentOfCourse)
                {
                    if (s.AccountId == studentId)
                    {
                        return RedirectToAction("Course", "Course", new { message = "You have join to this course before." });
                    }
                }

                foreach (var item in listRequest)
                {
                    if (item.SendId.ToString() == claimsPrincipal.FindFirstValue(ClaimTypes.Dsa) && item.CourseId == courseId)
                    {

                        return RedirectToAction("Course", "Course", new { message = "You have send a request to this course before." });
                    }
                }
                Guid sendId = new Guid(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
                request.CreateDate = DateTime.Now;
                request.SendId = sendId;
                request.ReceiveId = receiveId;
                request.Status = "Pending";
                request.CourseId = courseId;
                context.Requests.Add(request);
                int count = context.SaveChanges();
                if (count > 0)
                {
                    ViewBag.Message = "Enroll successful.";
                }
                else
                {
                    ViewBag.Message = "Xảy ra lỗi.";
                }
                ViewBag.listAllCourse = context.Courses.ToList();
                ViewBag.listMentor = context.Accounts.Where(a => a.Role == "Mentee");

            }

            return RedirectToAction("Course", "Course");
        }

        public IActionResult ListRequestByMentee()
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())

            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var mentee = context.Accounts.SingleOrDefault
                                (m => m.Email == claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
                var listRequest = context.Requests.Where(r => r.SendId == mentee.Id).ToList();
                List<RequestViewModel> listRequestViewModel = new List<RequestViewModel>();
                foreach (var request in listRequest)
                {
                    var requestViewModel = new RequestViewModel();
                    requestViewModel.Id = request.Id;
                    requestViewModel.CreateDate = request.CreateDate;
                    requestViewModel.Status = request.Status;
                    var senderEmail = context.Accounts.FirstOrDefault(a => a.Id == request.SendId).Email;
                    var reiceverEmail = context.Accounts.FirstOrDefault(a => a.Id == request.ReceiveId).Email;
                    requestViewModel.Sender = (context.Profiles.FirstOrDefault
                                              (p => p.Email == senderEmail)).Name;
                    requestViewModel.Receiver = (context.Profiles.FirstOrDefault
                                                (p => p.Email == reiceverEmail)).Name;
                    requestViewModel.CourseName = (context.Courses.FirstOrDefault
                                                (p => p.Id == request.CourseId)).Name;

                    listRequestViewModel.Add(requestViewModel);
                }
                ViewBag.data = listRequestViewModel;
            }

            return View();
        }
        public IActionResult DeleteRequest(Guid requestId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var requestDelete = context.Requests.SingleOrDefault(r => r.Id == requestId);
                context.Requests.Remove(requestDelete);
                context.SaveChanges();
            }
            return RedirectToAction("ListRequestByMentee", "Request");
        }
        public IActionResult ListRequestMentor()
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())

            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var mentor = context.Accounts.SingleOrDefault
                                (m => m.Email == claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
                var listRequestMentor = context.Requests.Where(r => r.ReceiveId == mentor.Id).ToList();
                List<RequestViewModel> listRequestViewModel = new List<RequestViewModel>();
                foreach (var request in listRequestMentor)
                {
                    var requestViewModel = new RequestViewModel();
                    requestViewModel.Id = request.Id;
                    requestViewModel.CreateDate = request.CreateDate;
                    requestViewModel.Status = request.Status;
                    var senderEmail = context.Accounts.FirstOrDefault(a => a.Id == request.SendId).Email;
                    var reiceverEmail = context.Accounts.FirstOrDefault(a => a.Id == request.ReceiveId).Email;
                    requestViewModel.Sender = (context.Profiles.FirstOrDefault
                                              (p => p.Email == senderEmail)).Name;
                    requestViewModel.Receiver = (context.Profiles.FirstOrDefault
                                                (p => p.Email == reiceverEmail)).Name;
                    requestViewModel.CourseName = (context.Courses.FirstOrDefault
                                                (p => p.Id == request.CourseId)).Name;

                    listRequestViewModel.Add(requestViewModel);
                }
                ViewBag.listRequestMentor = listRequestViewModel;
            }
            return View("~/Views/Request/ListRequestMentor.cshtml");
        }
        public IActionResult ProcessResponse(Guid requestId, string button)
        {

            switch (button)
            {
                case "approve":
                    using (HappyProgrammingContext context = new HappyProgrammingContext())
                    {
                        ClaimsPrincipal claimsPrincipal = HttpContext.User;
                        StudentOfCourse studentOfCourse = new StudentOfCourse();
                        var request = context.Requests.SingleOrDefault(a => a.Id == requestId);
                        request.Status = "Approved";
                        var newStudentOfCourse = new StudentOfCourse();
                        newStudentOfCourse.AccountId = request.SendId;
                        newStudentOfCourse.CourseId = request.CourseId;
                        context.StudentOfCourses.Add(newStudentOfCourse);
                        context.Requests.Update(request);
                        context.SaveChanges();
                        return RedirectToAction("ListRequestMentor", "Request");
                    }
                    break;

                case "reject":
                    // Xử lý khi người dùng chọn reject
                    using (HappyProgrammingContext context = new HappyProgrammingContext())
                    {
                        var requestDelete = context.Requests.SingleOrDefault(r => r.Id == requestId);
                        requestDelete.Status = "Rejected";
                        context.Requests.Update(requestDelete);
                        context.SaveChanges();
                        return RedirectToAction("ListRequestMentor", "Request");
                    }
                    break;
            }

            return RedirectToAction("ListRequestMentor", "Request");
        }

    }
}
