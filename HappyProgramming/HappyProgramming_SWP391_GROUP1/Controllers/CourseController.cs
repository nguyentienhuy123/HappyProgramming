using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HappyProgramming_SWP391_GROUP1.Models.ViewModel;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class CourseController : Controller
    {

        [HttpPost]
        public IActionResult SaveCourseId(string CourseName, string courseId)
        {
            HttpContext.Session.SetString("CourseName", CourseName);
            HttpContext.Session.SetString("courseId", courseId);
            return Redirect($"/Course/CourseClassroom?CourseId={courseId}");
        }
        public IActionResult Course(string message)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                Console.WriteLine(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
                List<Course> ListCourse = context.Courses.ToList();
                var listMentor = context.Accounts.Where(a => a.Role == "Mentor").ToList();
                ViewBag.listAllCourse = ListCourse;
                ViewBag.listMentor = listMentor;
                ViewBag.Enroll = message;
                ViewBag.noti = TempData["noti"];
            }
            return View();
        }
        public IActionResult MyCourse()
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                Guid accId = new Guid(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
                List<Course> listCourse = context.Courses.ToList();
                List<Course> listMyCourse = new List<Course>();
                if (claimsPrincipal.FindFirstValue(ClaimTypes.Role) == "Mentee")
                {
                    foreach (Course course in listCourse)
                    {
                        List<StudentOfCourse> listStudentOfCourse = context.StudentOfCourses.Where(s => s.CourseId == course.Id).ToList();
                        foreach (var s in listStudentOfCourse)
                        {
                            if (s.AccountId == accId)
                            {
                                listMyCourse.Add(course);
                            }
                        }

                    }
                }
                if (claimsPrincipal.FindFirstValue(ClaimTypes.Role) == "Mentor")
                {
                    foreach (var s in listCourse)
                    {
                        if (s.AccountId == accId)
                        {
                            listMyCourse.Add(s);
                        }
                    }
                }



                ViewBag.listMyCourse = listMyCourse;

            }
            return View();
        }

        public IActionResult MentorInfor(Guid MentorId)
        {
            var mentorProfile = new Profile();
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var mentorAcc = context.Accounts.FirstOrDefault(a => a.Id == MentorId);
                mentorProfile = context.Profiles.FirstOrDefault(p => p.Id == mentorAcc.ProfileId);

            }
            ViewBag.infor = mentorProfile;
            return View();
        }
        public IActionResult CreateCourse()
        {

            return View();
        }
        [HttpPost]
        public IActionResult CreateCourse(CourseViewModel courseViewModel)
        {

            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var course = new Course();
                course.Description = courseViewModel.Description;
                course.StartDate = courseViewModel.StartDate;
                course.EndDate = courseViewModel.EndDate;
                course.Name = courseViewModel.Name;
                var email = courseViewModel.Email;
                course.AccountId = context.Accounts.FirstOrDefault(a => a.Email.Equals(email)).Id;
                context.Courses.Add(course);
                var listMentor = context.Accounts.Where(a => a.Role == "Mentor").ToList();
                ViewBag.listMentor = listMentor;
                if (context.SaveChanges() > 0)
                {
                    List<Course> ListCourse = context.Courses.ToList();
                    ViewBag.listAllCourse = ListCourse;

                    return View("~/Views/Course/Course.cshtml");
                }
                else
                {
                    return RedirectToAction("CreateCourse", "Course");

                }
            }

        }


        public IActionResult DeleteCourse(Guid courseId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var courseDelete = context.Courses.SingleOrDefault(c => c.Id == courseId);
                var listPostInCourse = context.Posts.Where(p => p.CourseId == courseId).ToList();
                if (listPostInCourse != null)
                {
                    foreach (var post in listPostInCourse)
                    {
                        var listComment = context.Comments.Where(c => c.PostId == post.Id).ToList();
                        foreach (var comment in listComment)
                        {
                            context.Remove(comment);
                            context.SaveChanges();
                        }
                        context.Remove(post);
                        context.SaveChanges();
                    }
                }
                var listStudentOfCourse = context.StudentOfCourses.Where(s => s.CourseId == courseId).ToList();
                foreach (var student in listStudentOfCourse)
                {
                    context.Remove(student);
                    context.SaveChanges();
                }
                context.Courses.Remove(courseDelete);
                context.SaveChanges();

            }
            TempData["noti"] = "Delete successfull";

            return RedirectToAction("Course", "Course");
        }
        [HttpPost]
        public IActionResult UpdateMentor(Guid courseId, Guid mentorId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var courseUpdate = context.Courses.SingleOrDefault(c => c.Id == courseId);
                courseUpdate.AccountId = mentorId;
                context.Courses.Update(courseUpdate);
                context.SaveChanges();
            }
            return RedirectToAction("Course", "Course");
        }
        public IActionResult CourseClassroom(string CourseId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var listPostOfCourse = context.Posts.Where(p => p.CourseId == Guid.Parse(CourseId)).ToList();
                ViewBag.listPostOfCourse = listPostOfCourse;
            }
            ViewBag.CourseId = CourseId;
            return View();
        }

    }
}