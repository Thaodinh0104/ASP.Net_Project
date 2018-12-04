using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using FreeGrab.Models;
using System.Web.Helpers;
using System.Web.Security;
using System.Net.Mail;
using System.Data.Entity.Migrations;

namespace FreeGrab.Controllers
{
    public class UserManangerController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        // GET: UserMananger
        [Authorize]
        public ActionResult Dasboard()
        {
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            var data = new ShowData();
            int id = int.Parse(User.Identity.Name);
          data.news = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.CustomerId==id).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToList();
            data.newspost = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.IsPost == true && p.CustomerId == id).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToList();
            data.notpost = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.IsPost == false && p.CustomerId == id).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToList();

            return View(data);
        }
    }
}