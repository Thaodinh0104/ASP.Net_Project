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
    public class TypeNewsController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();
        // GET: TypeNews
        public ActionResult Index()
        {
            return View(db.TypeNewses.ToList());
        }

        // GET: TypeNews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeNews typeNews = db.TypeNewses.Find(id);
            if (typeNews == null)
            {
                return HttpNotFound();
            }
            return View(typeNews);
        }

        // GET: TypeNews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeNews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ParentId,Type,DateCreate,DateUpdate,IsActive")] TypeNews typeNews)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = new DateTime();
                typeNews.DateCreate = dt;
                typeNews.DateCreate = dt;
                db.TypeNewses.Add(typeNews);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeNews);
        }

        // GET: TypeNews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeNews typeNews = db.TypeNewses.Find(id);
            if (typeNews == null)
            {
                return HttpNotFound();
            }
            return View(typeNews);
        }

        // POST: TypeNews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ParentId,Type,DateCreate,DateUpdate,IsActive")] TypeNews typeNews)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = new DateTime();
           
                typeNews.DateCreate = dt;
                db.Entry(typeNews).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeNews);
        }

        // GET: TypeNews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeNews typeNews = db.TypeNewses.Find(id);
            if (typeNews == null)
            {
                return HttpNotFound();
            }
            return View(typeNews);
        }

        // POST: TypeNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeNews typeNews = db.TypeNewses.Find(id);
            db.TypeNewses.Remove(typeNews);
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
