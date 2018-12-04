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
    public class GrabersController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();
        public ActionResult Index()
        {
            return View(db.Grabers.ToList());
        }

        // GET: Grabers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graber graber = db.Grabers.Find(id);
            if (graber == null)
            {
                return HttpNotFound();
            }
            return View(graber);
        }

        // GET: Grabers/Create
        public ActionResult Create(int? id)
        {

            return View();
        }

        // POST: Grabers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Graber data, Patient patient, int? id)
        {

            try
            {
                Graber graber = new Graber();
                DateTime dt = DateTime.Now;
                // Get year, month, and day

                int year = dt.Year;
                int month = dt.Month;

                int day = dt.Day;

                if (data.ImageName != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(data.ImageName.FileName);
                    string extention = Path.GetExtension(data.ImageName.FileName);
                    fileName = fileName + extention;
                  
                    string Folder = "/Content/Images/Grabers/" + year + "/" + month + "/" + day + "/";
                    graber.Avatar = Folder + fileName;
                    bool IsExists = System.IO.Directory.Exists(Server.MapPath(Folder));
                    if (!IsExists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(Folder));
                    fileName = Path.Combine(Server.MapPath(Folder), fileName);
                    data.ImageName.SaveAs(fileName);
                }
                graber.FullName = data.FullName;
                graber.IDCard = data.IDCard;
                graber.Phone = data.Phone;

                graber.IsActive = true;
                db.Grabers.Add(graber);

                db.SaveChanges();
                //change staus of patient from not pick-up become wait pick up
                var patients = db.Patients.Find(id);
                var results = db.PatientStatuses.Where(x => x.Status == "Wait pick up").Select(x => x.Id).Single();
                patients.StatusId = results;       
                db.Entry(patients).State = EntityState.Modified;
                db.SaveChanges();
                // insert history graber
                HistoryGrab historyGrab = new HistoryGrab();
                historyGrab.GrabId = graber.Id;
                historyGrab.PatientId = patients.Id;
                historyGrab.IsActive = true;
                db.HistoryGrabs.Add(historyGrab);
                db.SaveChanges();

                return RedirectToAction("Index","Home");

            }
            catch { 
            return View(data);
            }
        }

        // GET: Grabers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graber graber = db.Grabers.Find(id);
            if (graber == null)
            {
                return HttpNotFound();
            }
            return View(graber);
        }

        // POST: Grabers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,IDCard,Phone,Avatar,DateCreate,IsActive")] Graber graber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(graber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(graber);
        }

        // GET: Grabers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Graber graber = db.Grabers.Find(id);
            if (graber == null)
            {
                return HttpNotFound();
            }
            return View(graber);
        }

        // POST: Grabers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Graber graber = db.Grabers.Find(id);
            db.Grabers.Remove(graber);
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
