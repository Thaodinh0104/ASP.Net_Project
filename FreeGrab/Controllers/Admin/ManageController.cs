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

namespace FreeGrab.Controllers.Admin
{
    public class ManageController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();
        // GET: Manage
        //public ActionResult AllPatients()
        //{
        //    var patients = db.Patients.Include(p => p.Hospital).Include(p => p.PatientStatus);
        //    return View(patients.ToList());

        //}
      

    }
}