using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FreeGrab.Models;

namespace FreeGrab.Controllers
{
    public class NewsController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        // GET: News
        public ActionResult Index()
        {
            var newses = db.Newses.Include(n => n.Customer).Include(n => n.TypeNews);
            return View(newses.ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newses.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Avatar");
            ViewBag.TypeId = new SelectList(db.TypeNewses, "Id", "Type");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,Content,TypeId,CustomerId,DateCreate,Dateupdate,IsPost,IsActive")] News news, List<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.Name != null)
                {
                    news.CustomerId = int.Parse(User.Identity.Name);
                    news.IsActive = true;
                    news.IsPost = false;
                    db.Newses.Add(news);
                    db.SaveChanges();
                    DateTime dt = DateTime.Now;
                    int year = dt.Year;
                    int month = dt.Month;

                    int day = dt.Day;
                    int id = news.Id;
                    string Folder = "/Content/Images/Newses/" + year + "/" + month + "/" + day + "/" + id + "/";
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            string extention = Path.GetExtension(file.FileName);
                            fileName = fileName + extention;
                            Photo photo = new Photo();
                            photo.Url = Folder + fileName;
                            photo.NewsId = id;
                            //      photo.DateUpdate = dt;
                            photo.DateCreate = dt;
                            db.Photos.Add(photo);

                            db.SaveChanges();
                            bool IsExists = System.IO.Directory.Exists(Server.MapPath(Folder));
                            if (!IsExists)
                                System.IO.Directory.CreateDirectory(Server.MapPath(Folder));
                            fileName = Path.Combine(Server.MapPath(Folder), fileName);
                            file.SaveAs(fileName);
                            //file.SaveAs(Path.Combine(Server.MapPath(Folder), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Avatar", news.CustomerId);
            ViewBag.TypeId = new SelectList(db.TypeNewses, "Id", "Type", news.TypeId);
            return View(news);
        }

        // GET: Taolao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newses.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Avatar", news.CustomerId);
            ViewBag.TypeId = new SelectList(db.TypeNewses, "Id", "Type", news.TypeId);
            return View(news);
        }

        // POST: Taolao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,Content,TypeId,CustomerId,DateCreate,Dateupdate,IsPost,IsActive")] News news, List<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                DateTime dt = DateTime.Now;
                int year = dt.Year;
                int month = dt.Month;

                int day = dt.Day;
                int id = news.Id;
                string Folder = "/Content/Images/Newses/" + year + "/" + month + "/" + day + "/" + id + "/";
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extention = Path.GetExtension(file.FileName);
                        fileName = fileName + extention;
                        Photo photo = new Photo();
                        photo.Url = Folder + fileName;
                        photo.NewsId = id;
                        //      photo.DateUpdate = dt;
                        photo.DateCreate = dt;
                        db.Photos.Add(photo);

                        db.SaveChanges();
                        bool IsExists = System.IO.Directory.Exists(Server.MapPath(Folder));
                        if (!IsExists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(Folder));
                        fileName = Path.Combine(Server.MapPath(Folder), fileName);
                        file.SaveAs(fileName);
                        //file.SaveAs(Path.Combine(Server.MapPath(Folder), Guid.NewGuid() + Path.GetExtension(file.FileName)));
                    }
                }
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", news.CustomerId);
            ViewBag.TypeId = new SelectList(db.TypeNewses, "Id", "Type", news.TypeId);
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newses.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.Newses.Find(id);
     
            news.IsActive = false;
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
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
