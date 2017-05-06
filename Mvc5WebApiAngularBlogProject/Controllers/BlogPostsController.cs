using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5WebApiAngularBlogProject.Models;
using PagedList;

namespace Mvc5WebApiAngularBlogProject.Controllers
{
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index(int? page)
        {
            ViewBag.ReturnUrl = Request.Url.LocalPath;
            //BlogVM BlogPost = new BlogVM();
            //BlogPost.BlogPost = db.Posts.ToList();
            //if (db.Topics.Count() == 0)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //else
            //{
            //    BlogPost.Topics = db.Topics.ToList();
            //};
            //BlogPost.Comments = db.Comments.ToList();
            //return View(BlogPost);
            int pageSize = 3; // display three blog posts at a time on this page
            int pageNumber = (page ?? 1);
            return View(db.Posts.AsQueryable().OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
        }

        // GET: BlogPosts/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BlogPost blogPost = db.Posts.Find(id);
        //    if (blogPost == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(blogPost);
        //}

        public ActionResult Details(string Slug)
        {
            ViewBag.ReturnUrl = Request.Url.AbsolutePath; //Request.Url.AbsolutePath is the URL that is fed to the controller from whatever was clicked it to send it here (a.k.a. the URL of the location the controller is directing you to.)

            if (Slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(b => b.Slug == Slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            ViewBag.ReturnUrl = Request.Url.LocalPath;
            ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Author,Created,Modified,ModifiedNote,Private,Title,Slug,Body,MediaUrl,published")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Author,Created,Modified,ModifiedNote,Private,Title,Slug,Body,MediaUrl,published")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
