using Mvc5WebApiAngularBlogProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc5WebApiAngularBlogProject.Controllers
{    
    //[RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //var max = db.Posts.OrderByDescending(p => p.Id).FirstOrDefault();
            //BlogPost blogPost = max;
            //if (blogPost == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.ReturnUrl = Request.Url.LocalPath;
            return View(db.Posts.ToList());
        }

        //public ActionResult Topics()
        //{
        //    ViewBag.Message = "Feel free to browse through the blog posts by topic.";
        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Here is my story, short version: I took the scenic route";
            ViewBag.ReturnUrl = Request.Url.LocalPath;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.ReturnUrl = Request.Url.LocalPath;
            ViewBag.StatusMessage = "";
            ViewBag.Message = "If you wish to contact me for some reason...";
            EmailModel email = new Models.EmailModel();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold>({1})</p><p> Message: </p><p>{2}</p>";
                    var from = "MyBlog<soileep@hotmail.com>";
                    model.Body = "This is a messag from your blog site. The name and the email of the contacting person is above";

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Blog Contact Email",
                        Body = String.Format(body, model.FromName, model.FromEmail, model.Body),
                        // Body = "test",
                        IsBodyHtml = true,
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    ViewBag.StatusMessage = "Success! Thanks for reaching out to me!";
                    // return RedirectToAction("Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }

        //public ActionResult Search(int? page)
        //{
        //    var Model = db.Posts.AsQueryable();
        //    int pageSize = 5; // display three blog posts at a time on this page
        //    int pageNumber = (page ?? 1);
        //    return View(Model.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult Search(int? page, string search)
        {
            ViewBag.ReturnUrl = Request.Url.LocalPath;
            var blogResults = db.Posts.AsQueryable(); // Most efficient method
            if (!String.IsNullOrEmpty(search))
            {
                blogResults = db.Posts.Where(p => p.Body.Contains(search) || p.Title.Contains(search) || p.Slug.Contains(search) || p.Comments.Any(c => c.Body.Contains(search) || c.Author.DisplayName.Contains(search) || c.Author.FirstName.Contains(search) || c.Author.LastName.Contains(search)) || p.Topics.Any(t => t.Name.Contains(search)));

                // Session["search"] = blogResults;
                int pageSize = 3; // display three blog posts at a time on this page
                int pageNumber = (page ?? 1);
                // ViewBag.search = blogResults;
                return View(blogResults.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
                // List<BlogPost> resultList = blogResults.ToList();
                // return View(resultsList);
            }
            //else if (page != null && String.IsNullOrEmpty(search))
            //{
            //    blogResults = Session["search"] as IQueryable<BlogPost>;

            //    int pageSize = 3; // display three blog posts at a time on this page
            //    int pageNumber = (page ?? 1);
            //    ViewBag.search = blogResults;
            //    return View(blogResults.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize);
            //}
            else
            {
                //Session["search"] = null;
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(blogResults.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
                // return RedirectToLocal(Request.Url.LocalPath);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}