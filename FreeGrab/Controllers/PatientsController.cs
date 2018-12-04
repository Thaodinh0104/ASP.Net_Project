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
    public class PatientsController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        // GET: Patients
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.Hospital).Include(p => p.PatientStatus);
            return View(patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.HospitalId = new SelectList(db.Hospitals, "ID", "Name");
            ViewBag.StatusId = new SelectList(db.PatientStatuses, "Id", "Status");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient data)
        {
            if (ModelState.IsValid)
            {
                Patient patient = new Patient();
                DateTime dt = DateTime.Now;
              
                patient.DateUpdate = dt;
                patient.FullName = data.FullName;
                patient.Age = data.Age;
                patient.Gender = data.Gender;
                patient.HospitalId = data.HospitalId;
                patient.Phone = data.Phone;
                patient.DateDeparture = data.DateDeparture;
                patient.Destination = data.Destination;
               
                var results = db.PatientStatuses.Where(x => x.Status == "Not pick-up").Select(x=>x.Id).Single();
                patient.StatusId = results;
                int result = DateTime.Compare(dt, patient.DateDeparture);
                if (result < 0)
                {
                    db.Patients.Add(patient);

                    db.SaveChanges();
                    int year = dt.Year;
                    int month = dt.Month;

                    int day = dt.Day;
                    int id = patient.Id;
                    string Folder = "/Content/Images/Patients/" + year + "/" + month + "/" + day + "/" + id + "/";
                    //foreach (string file in Request.Files)
                    //{
                    //    HttpPostedFile hpf = Request.Files[data.files] as HttpPostedFile;
                    //    if ((data.files).ContentLength == 0)
                    //        continue;
                    //    string savedFileName = Path.Combine(
                    //       AppDomain.CurrentDomain.BaseDirectory,
                    //       Path.GetFileName(hpf.FileName));
                    //    hpf.SaveAs(savedFileName);
                    //}


                    foreach (var file in data.files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            string extention = Path.GetExtension(file.FileName);
                            fileName = fileName + extention;
                            Photo photo = new Photo();
                            photo.Url = Folder + fileName;
                            photo.PatientId = id;
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

                    return View("PatientSuccess");
                }
                else
                {
                    ViewBag.Message = "Please check your Date DateDeparture!";
                }
            }

            ViewBag.HospitalId = new SelectList(db.Hospitals, "ID", "Name", data.HospitalId);
            ViewBag.StatusId = new SelectList(db.PatientStatuses, "Id", "Status", data.StatusId);
            return View(data);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.HospitalId = new SelectList(db.Hospitals, "ID", "Name", patient.HospitalId);
            ViewBag.StatusId = new SelectList(db.PatientStatuses, "Id", "Status", patient.StatusId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Age,Gender,HospitalId,DateDeparture,Phone,Destination,StatusId,DateCreate,DateUpdate")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                patient.DateUpdate = dt;
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HospitalId = new SelectList(db.Hospitals, "ID", "Name", patient.HospitalId);
            ViewBag.StatusId = new SelectList(db.PatientStatuses, "Id", "Status", patient.StatusId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            DateTime dt = DateTime.Now;
            patient.DateUpdate = dt;
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
