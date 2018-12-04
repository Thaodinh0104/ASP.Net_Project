using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FreeGrab.Models;

namespace FreeGrab.Controllers
{
    public class TaolaoController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        // GET: Taolao
        public ActionResult Index()
        {
            var newses = db.Newses.Include(n => n.Customer).Include(n => n.TypeNews);
            return View(newses.ToList());
        }

        // GET: Taolao/Details/5
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

        // GET: Taolao/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Avatar");
            ViewBag.TypeId = new SelectList(db.TypeNewses, "Id", "Type");
            return View();
        }

        // POST: Taolao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,Content,TypeId,CustomerId,DateCreate,Dateupdate,IsPost,IsActive")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Newses.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", news.CustomerId);
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
        public ActionResult Edit([Bind(Include = "Id,Subject,Content,TypeId,CustomerId,DateCreate,Dateupdate,IsPost,IsActive")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", news.CustomerId);
            ViewBag.TypeId = new SelectList(db.TypeNewses, "Id", "Type", news.TypeId);
            return View(news);
        }

        // GET: Taolao/Delete/5
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

        // POST: Taolao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.Newses.Find(id);
            db.Newses.Remove(news);
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
