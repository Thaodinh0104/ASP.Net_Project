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
    public class PatientStatusController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();
        // GET: PatientStatus
        public ActionResult Index()
        {
            return View(db.PatientStatuses.ToList());
        }

        // GET: PatientStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientStatus patientStatus = db.PatientStatuses.Find(id);
            if (patientStatus == null)
            {
                return HttpNotFound();
            }
            return View(patientStatus);
        }

        // GET: PatientStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Status,DateCreate,DateUpdate")] PatientStatus patientStatus)
        {
            if (ModelState.IsValid)
            {
                db.PatientStatuses.Add(patientStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patientStatus);
        }

        // GET: PatientStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientStatus patientStatus = db.PatientStatuses.Find(id);
            if (patientStatus == null)
            {
                return HttpNotFound();
            }
            return View(patientStatus);
        }

        // POST: PatientStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Status,DateCreate,DateUpdate")] PatientStatus patientStatus)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                patientStatus.DateUpdate=dt;
                db.Entry(patientStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patientStatus);
        }

        // GET: PatientStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientStatus patientStatus = db.PatientStatuses.Find(id);
            if (patientStatus == null)
            {
                return HttpNotFound();
            }
            return View(patientStatus);
        }

        // POST: PatientStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientStatus patientStatus = db.PatientStatuses.Find(id);
            db.PatientStatuses.Remove(patientStatus);
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
