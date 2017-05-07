using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mvc5WebApiAngularBlogProject.Models;
using Microsoft.AspNet.Identity;

namespace Mvc5WebApiAngularBlogProject.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            //var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPost);
            var comments = db.Comments.Include(c => c.Author);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName");
            //ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Author");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Author");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,AuthorId,BlogPostId,Created,Modified,ModifiedNote,Body,ProfilePicUrl,Private")] Comments comments)
        public ActionResult Create([Bind(Include = "BlogPostId, Body, ProfilePicUrl, Private")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                var slug = db.Posts.FirstOrDefault(x => x.Id == comments.BlogPostId).Slug;
                var user = db.Users.Find(User.Identity.GetUserId());
                comments.AuthorId = user.Id;
                comments.Created = DateTimeOffset.Now;
                db.Comments.Add(comments);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
            }

            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", comments.AuthorId);
            //ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Author", comments.BlogPostId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comments.AuthorId);
            ViewBag.BlogPostId = new SelectList(db.Posts, "Id", "Title", comments.BlogPostId);
            return View(comments);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", comments.AuthorId);
            //ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Author", comments.BlogPostId);
            var slug = db.Posts.FirstOrDefault(x => x.Id == comments.BlogPostId).Slug;
            ViewBag.Slug = slug;
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comments.AuthorId);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,AuthorId,BlogPostId,Created,Modified,ModifiedNote,Body,ProfilePicUrl,Private")] Comments comments)
        public ActionResult Edit([Bind(Include = "Modified, ModifiedNote, Body, Private, Id")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                var modCom = db.Comments.Find(comments.Id);
                modCom.Body = comments.Body;
                var slug = db.Posts.FirstOrDefault(x => x.Id == modCom.BlogPostId).Slug;
                modCom.Modified = DateTimeOffset.Now;

                db.Entry(comments).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
            }
            //ViewBag.AuthorId = new SelectList(db.ApplicationUsers, "Id", "FirstName", comments.AuthorId);
            //ViewBag.BlogPostId = new SelectList(db.BlogPosts, "Id", "Author", comments.BlogPostId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", comments.AuthorId);
            return View(comments);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            var slug = db.Posts.FirstOrDefault(x => x.Id == comments.BlogPostId).Slug;
            ViewBag.Slug = slug;
            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comments comments = db.Comments.Find(id);
            var slug = db.Posts.FirstOrDefault(x => x.Id == comments.BlogPostId).Slug;

            db.Comments.Remove(comments);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Details", "BlogPosts", new { Slug = slug });
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
