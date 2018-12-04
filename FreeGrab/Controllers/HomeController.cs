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
using PagedList;
namespace FreeGrab.Controllers
{
    public class HomeController : Controller
    {
        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();


        int pageSize = 4;



        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer data)
        {
            if (ModelState.IsValid)
            {


                // string message;
                var email = db.Customers.Where(x => x.Email.Equals(data.Email)).FirstOrDefault();

                if (email == null)
                {
                    Customer cus = new Customer();
                    if (data.ImageName != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(data.ImageName.FileName);
                        string extention = Path.GetExtension(data.ImageName.FileName);
                        fileName = fileName + extention;
                        DateTime dt = DateTime.Now;
                        // Get year, month, and day

                        int year = dt.Year;
                        int month = dt.Month;

                        int day = dt.Day;
                        int id;
                        var checkexsitId = db.Customers.Count();
                        if (checkexsitId == 0)
                        {
                            id = data.Id + 1;
                        }
                        else
                        {
                            int max = db.Customers.Max(p => p.Id);
                            id = max + 1;
                        }



                        //  int id = data.Id;

                        string Folder = "/Content/Images/" + year + "/" + month + "/" + day + "/" + id + "/";


                        bool IsExists = System.IO.Directory.Exists(Server.MapPath(Folder));
                        if (!IsExists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(Folder));


                        cus.Avatar = Folder + fileName;
                        fileName = Path.Combine(Server.MapPath(Folder), fileName);
                        data.ImageName.SaveAs(fileName);
                    }
                    else
                    {
                        cus.Avatar = null;
                    }
                    cus.IsActive = true;

                    cus.Password = Crypto.Hash(data.Password);
                    cus.FirstName = data.FirstName;
                    cus.LastName = data.LastName;
                    cus.Phone = data.Phone;
                    cus.Email = data.Email;
                    cus.Gender = data.Gender;
                    cus.DateOfBirth = data.DateOfBirth;
                    db.Customers.Add(cus);

                    db.SaveChanges();

                    var cookie = new HttpCookie("username", cus.LastName + cus.FirstName);
                    Response.Cookies.Add(cookie);
                    var avatar= new HttpCookie("avatar", cus.Avatar);
                    Response.Cookies.Add(avatar);
                    //save id after login  
                    FormsAuthentication.SetAuthCookie(cus.Id.ToString(), false);

                    //   string resetCode = Guid.NewGuid().ToString();
                    //   SendVerificationLinkEmail(data.Email, resetCode, "VerifyAccount");
                    //   data.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    //  db.Configuration.ValidateOnSaveEnabled = false;
                    // db.SaveChanges();
                    // ViewBag.message = "Reset password link has been sent to your email id.";
                    return View("RegisterSuccess");

                }
                else
                {
                    ViewBag.Message = "Email is esixted";
                }
            }

            return View(data);

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer data, string ReturnUrl)
        {
          
                data.Password = Crypto.Hash(data.Password);
                var customer = db.Customers.Where(x =>
                    x.Email.Equals(data.Email)).SingleOrDefault();
                if (customer != null)
                {
                    if (customer.Password.Equals(data.Password))
                    {
                        if (customer.IsActive.Value)
                        {
                            //    save usename after login in cookie
                            var cookie = new HttpCookie("username", customer.LastName + customer.FirstName);
                            Response.Cookies.Add(cookie);
                           
                            //save id after login  
                            FormsAuthentication.SetAuthCookie(customer.Id.ToString(), false);

                            if (string.IsNullOrEmpty(ReturnUrl))
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return Redirect(ReturnUrl);
                            }

                        }
                        else
                        {
                            ViewBag.Message = "Account has been blocked!";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Password wrong!";
                    }
                }
                else
                {
                    ViewBag.Message = "User name not exist!";
                }
            
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {

            //Verify Email ID
            //Generate Reset Password link
            //send email

            //  bool status = false;
            using (db)
            {
                var i = Email;
                var account = db.Customers.Where(x =>
                x.Email.Equals(Email)).SingleOrDefault();
                if (account != null)
                {
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    ViewBag.message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.message = "Account not found";
                }
            }
            return View();
        }


        public ActionResult Index( int? page)
        {
            int pageNumber = (page ?? 1);
            var data = new ShowData();
          data.patients = db.Patients.Include(p => p.Hospital).Where(p => p.StatusId == 3).Include(p => p.PatientStatus).Include(p => p.Photos).OrderByDescending(s => s.DateDeparture).ToPagedList(pageNumber, pageSize).ToList();
            data.news = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.IsPost == true).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToPagedList(pageNumber, pageSize).ToList();

            return View(data);
        }
        public ActionResult ListNewses()
        {
          
       var news = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.IsPost == true).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToList();
            return View(news);
        }
    
    public ActionResult FormSearch()
    {

            return View();

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AllNewses(int? page)
        {
       
            int pageNumber = (page ?? 1);
    var news = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.IsPost == true).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToPagedList(pageNumber, pageSize);

            return View(news);
        }
        public ActionResult AllPatients(string sortOrder, int? page)
        {
            
            int pageNumber = (page ?? 1);
            var patients = db.Patients.Include(p => p.Hospital).Where(p => p.StatusId == 3).Include(p => p.PatientStatus).Include(p => p.Photos).OrderByDescending(s => s.DateDeparture).ToPagedList(pageNumber, pageSize);
            return View(patients);
        }
        public ActionResult NewsDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = new ShowData();
            data.newDetail = db.Newses.Find(id);
            data.news = db.Newses.Include(p => p.TypeNews).Where(p => p.IsActive == true && p.IsPost == true).Include(p => p.Customer).Include(p => p.Photos).OrderByDescending(s => s.Dateupdate).ToList();
            data.comments = db.Comments.Where(s => (s.CustomerId != null || s.EmployeeId != null) && s.NewsId == id).OrderByDescending(s => s.DateCreate).AsQueryable().ToList();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }
        [HttpGet]
        public JsonResult GetComment(int id)
        {

            db.Configuration.ProxyCreationEnabled = false;
            var commentResult = db.Comments.Where(s => (s.CustomerId != null || s.EmployeeId != null) && s.NewsId == id)
           .Select(c => new
           {
               Id = c.Id,
               DateCreate = c.DateCreate,
               Contents = c.Contents,
               UserLastName = c.Customer.LastName,
               UserFirstName = c.Customer.FirstName,

               UserId = c.CustomerId,
               Avatar = c.Customer.Avatar



           }).OrderByDescending(s => s.DateCreate).AsQueryable().ToList();


            return Json(commentResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult addComment([Bind(Exclude = "Id")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CustomerId = int.Parse(User.Identity.Name);

                db.Comments.Add(comment);

                db.SaveChanges();
            }

            return Json(comment, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
  
        public ActionResult addGraber([Bind(Exclude = "Id")] Graber grab, int id)
        {
            // Checking no of files injected in Request object  
            
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        grab.Avatar = "~/Content/Images/Grabers/" + fname;
                        grab.IsActive = true;
                        db.Grabers.Add(grab);
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("/Content/Images/Grabers/"), fname);
                        db.SaveChanges();
                        var patients = db.Patients.Find(id);
                        var results = db.PatientStatuses.Where(x => x.Status == "Wait pick up").Select(x => x.Id).Single();
                        patients.StatusId = results;
                        db.Entry(patients).State = EntityState.Modified;
                        db.SaveChanges();
                        // insert history graber
                        HistoryGrab historyGrab = new HistoryGrab();
                        historyGrab.GrabId = grab.Id;
                        historyGrab.PatientId = patients.Id;
                        historyGrab.IsActive = true;
                        db.HistoryGrabs.Add(historyGrab);
                        db.SaveChanges();
                        file.SaveAs(fname);
                       

                       
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Home/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("thao.dinhpn@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "thaovyphuong"; // Replace with actual password

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
            ViewBag.Message = "Please check mail";

        }
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (db)
            {
                var user = db.Customers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (db)
                {
                    var user = db.Customers.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                        return View("ResetPasswordSuccess");
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            //this.ControllerContext.HttpContext.Response.Cookies.Clear();
            return RedirectToAction("Login");
        }
 
        public ActionResult ResultSearch(string searchString)
        {
            var news = from s in db.Newses
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                news = news.Where(s => s.Subject.Contains(searchString) || s.Content.Contains(searchString));
            }
            return View(news.ToList());
        }
        //public ActionResult Search(string content)
        //{
        //    var news = from s in db.Newses
        //               select s;
        //    if (!String.IsNullOrEmpty(content))
        //    {
        //        news = news.Where(s => s.Subject.Contains(content) || s.Content.Contains(content));

        //    }
        //    return Json(news.ToList(), JsonRequestBehavior.AllowGet);

        //}

        public JsonResult Search(string search)
        {
           
            List<NewsModel> allsearch = db.Newses.Where(x => x.Subject.Contains(search)).Select(x => new NewsModel
            {
                Id = x.Id,
                Subject = x.Subject
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

}