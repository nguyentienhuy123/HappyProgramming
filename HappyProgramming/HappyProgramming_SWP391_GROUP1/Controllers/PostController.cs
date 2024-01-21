using HappyProgramming_SWP391_GROUP1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace HappyProgramming_SWP391_GROUP1.Controllers
{
    public class PostController : Controller
    {

        public IActionResult Post()
        {
            return View("Post");
        }

        public IActionResult CreatePost(Guid CourseId)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            ViewBag.accountId = claimsPrincipal.FindFirstValue(ClaimTypes.Dsa);
            ViewBag.CourseId = CourseId;
            return View();
        }

        public IActionResult ViewEditPost()
        {
            return View("ViewEdit");
        }

        [HttpGet("ViewPost/{postId}")]
        [Route("ViewPost")]
        public IActionResult ViewPost(string postId)
        {
            Post post = new Post();
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                //post = context.Posts.Include(x => x.Comments).Where(p => p.Id == Guid.Parse(postId)).SingleOrDefault();

                post = context.Posts.Include(x => x.Comments).SingleOrDefault(p => p.Id == Guid.Parse(postId));
                if (post != null)
                {
                    if (post.Comments == null)
                    {
                        {
                            post.Comments = new List<Comment>(); // Tạo một danh sách rỗng nếu thuộc tính Comments là null
                        }
                    }
                        //post = context.Posts.Include(x => x.Comments).Where(p => p.Id == Guid.Parse(postId)).FirstOrDefault();
                        var ListComment = context.Comments.Where(c => c.PostId == post.Id).ToList();
                        if (ListComment == null)
                        {
                            ViewBag.listComment = null;
                        }
                        ViewBag.listComment = ListComment;
                                        

                }
                return View("Post", post);
            }
        }

        [HttpGet]
        public ActionResult ViewEditPost(Guid postId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var post = context.Posts.Find(postId);
                if (post == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.postId = postId;
                return View("ViewEdit", post);
            }

        }


        [HttpGet]
        public ActionResult ViewEditComment(Guid CommentId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var comment = context.Posts.Find(CommentId);
                if (comment == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.postId = CommentId;
                return View("ViewEditComment", CommentId);
            }

        }

        public IActionResult ListPostByMe()
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                Console.WriteLine(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
                List<Post> ListPost = context.Posts.ToList();
                ViewBag.data = ListPost;
            }
            return View();
        }


        // list add post in course
        public IActionResult GetPosts(Guid courseId)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            Console.WriteLine(claimsPrincipal.FindFirstValue(ClaimTypes.Dsa));
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                List<Post> postsInCourse = context.Posts.Where(p => p.CourseId == courseId).ToList();
                ViewBag.data = postsInCourse;
            }
            return View();
        }


        [HttpPost]
        public ActionResult CreatePost([FromQuery(Name = "account")] Guid account, string title, string content, string attachmentPath, string CourseId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
                {
                    var newPost = new Post
                    {
                        Id = Guid.NewGuid(),
                        Title = title,
                        Content = content,
                        AccountId = account,
                        AttachmentPath = attachmentPath,
                        CreateDate = DateTime.Now,
                        CourseId = new Guid(CourseId)

                    };
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        var fileInfo = new FileInfo(attachmentPath);
                        if (fileInfo.Exists && fileInfo.Length > 0)
                        {
                            var fileName = Path.GetFileName(attachmentPath);
                            var folderPath = "C:\\";

                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            var filePath = Path.Combine(folderPath, fileName);
                            System.IO.File.Copy(attachmentPath, filePath, true);
                            newPost.AttachmentPath = filePath;
                        }
                    }
                    context.Add(newPost);
                    context.SaveChanges();
                    
                    return RedirectToAction("ViewPost", new { postId = newPost.Id });
                    // return Redirect("~/Course/CourseClassroom?postId=" +newPost.Id);
                }
                return View("~/Views/Launch/Launch.cshtml");
            }
        }

        [HttpGet]
        public IActionResult DownloadAttachment(Guid postId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                var post = context.Posts.FirstOrDefault(p => p.Id == postId);
                if (post != null && !string.IsNullOrEmpty(post.AttachmentPath))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), post.AttachmentPath);
                    if (System.IO.File.Exists(filePath))
                    {
                        var fileBytes = System.IO.File.ReadAllBytes(filePath);
                        var fileName = Path.GetFileName(filePath);
                        return File(fileBytes, "application/octet-stream", fileName);
                    }
                }
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult UpdatePost(Guid postId, Post post)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var existingPost = context.Posts.Find(postId);
                if (existingPost == null)
                {
                    return RedirectToAction("Index");
                }

                existingPost.Title = post.Title;
                existingPost.Content = post.Content;
                existingPost.AttachmentPath = post.AttachmentPath;
                context.SaveChanges();
                return RedirectToAction("ViewPost", new { postId = existingPost.Id });
            }
        }

        public ActionResult DeletePost(Guid postId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {

                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var existingPost = context.Posts.FirstOrDefault(p => p.Id == postId);
                string courseId = existingPost.CourseId.ToString();
                if (existingPost != null)
                {
                    var listComment = context.Comments.Where(c => c.PostId == postId).ToList();
                  foreach(var comment in listComment)
                    {
                        context.Remove(comment);
                        context.SaveChanges();
                    }                             
                    context.Remove(existingPost);
                    context.SaveChanges();
                }
                return RedirectToAction("CourseClassroom", "Course", new { courseId = courseId });
            }
         
        }

        // COMMENT POST

        [HttpPost]
        public ActionResult CommentPost(Guid postId, string content)
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            var accountId = claimsPrincipal.FindFirstValue(ClaimTypes.Dsa);
            var routeData = HttpContext.GetRouteData();
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {

                if (!string.IsNullOrEmpty(content))
                {
                    var newComment = new Comment
                    {
                        Id = Guid.NewGuid(),
                        PostId = postId,
                        AccountId = Guid.Parse(accountId),
                        Content = content,
                        CreateDate = DateTime.Now
                    };
                    context.Comments.Add(newComment);
                    context.SaveChanges();
                    return RedirectToAction("ViewPost", new { postId }) ;
                }
                return View("~/Views/Post/Post.cshtml");
            }
        }
        [HttpPost]
        public ActionResult DeleteComment(string commentId, Guid postId)
        {
            using (HappyProgrammingContext context = new HappyProgrammingContext())
            {
                ClaimsPrincipal claimsPrincipal = HttpContext.User;
                var comment = context.Comments.FirstOrDefault(c => c.Id == Guid.Parse(commentId));
                if (comment != null)
                {
                    context.Comments.Remove(comment);
                    context.SaveChanges();
                }
                return RedirectToAction("ViewPost", new { postId = postId });
            }
        }
    }

}


