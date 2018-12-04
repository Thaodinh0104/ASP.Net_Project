using FreeGrab.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace FreeGrab.Controllers
{

    public class AdminController : Controller
    {
        //public static string Aut(string usename, string avatar)
        //{
        //    return Formsauthentication.FormsCookieName(usename);
        ////}

        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        public ActionResult Dasboard()
        {
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Employee data)
        {
            // string message;
            if (ModelState.IsValid)
            {
                var email = db.Employees.Where(x => x.Email.Equals(data.Email)).FirstOrDefault();

                if (email == null)
                {
                    Employee employee = new Employee();
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
                        var checkexsitId = db.Employees.Count();
                        if (checkexsitId == 0)
                        {
                            id = data.Id + 1;
                        }
                        else
                        {
                            int max = db.Employees.Max(p => p.Id);
                            id = max + 1;
                        }



                        //  int id = data.Id;

                        string Folder = "/Content/Images/Admin" + year + "/" + month + "/" + day + "/" + id + "/";


                        bool IsExists = System.IO.Directory.Exists(Server.MapPath(Folder));
                        if (!IsExists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(Folder));


                        employee.Avatar = Folder + fileName;
                        fileName = Path.Combine(Server.MapPath(Folder), fileName);
                        data.ImageName.SaveAs(fileName);
                    }
                    else
                    {
                        employee.Avatar = null;
                    }
                    employee.IsActive = true;

                    employee.Password = Crypto.Hash(Crypto.Hash(data.Password));
                    employee.FirstName = data.FirstName;
                    employee.LastName = data.LastName;
                    employee.Phone = data.Phone;
                    employee.RoleID = data.RoleID;
                    employee.Email = data.Email;
                    employee.Gender = data.Gender;
                    employee.DateOfBirth = data.DateOfBirth;
                    db.Employees.Add(employee);

                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(data.Id.ToString(), false);
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
            ViewBag.RoleID = new SelectList(db.Roles, "Id", "Name", data.RoleID);
            
            return View(data);

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(Employee data, string ReturnUrl)
        {
          try { 
                data.Password = Crypto.Hash(Crypto.Hash(data.Password));
                var employee = db.Employees.Where(x =>
                    x.Email.Equals(data.Email)).SingleOrDefault();
                if (employee != null)
                {
                    if (employee.Password.Equals(data.Password))
                    {
                        if (employee.IsActive.Value)
                        {
                            //    save usename after login in cookie
                            var cookie = new HttpCookie("username", employee.LastName + employee.FirstName);
                            Response.Cookies.Add(cookie);
                            //save id after login  
                            FormsAuthentication.SetAuthCookie(employee.Id.ToString(), false);

                            if (string.IsNullOrEmpty(ReturnUrl))
                            {
                                return RedirectToAction("UpdateProfile");
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
            }
            catch { 
                return View(data);
            }
            return View(data);
        }

        [Authorize]
        public ActionResult UpdateProfile()
        {
            int id = int.Parse(User.Identity.Name);
            var employee = db.Employees.Find(id);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
            return View(employee);
        }

        [HttpPost]
        public ActionResult UpdateProfile(Employee data)
        {
            // lay id cua thang dang dang nhap
            int id = int.Parse(User.Identity.Name);
            // truy van vao co so du lieu lay thong tin nguoi nay
            var employee = db.Employees.Find(id);

            try
            {
                if (data.ImageName != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(data.ImageName.FileName);
                    string extention = Path.GetExtension(data.ImageName.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
                    DateTime dt = DateTime.Now;
                    // Get year, month, and day
                    int year = dt.Year;
                    int month = dt.Month;
                    int day = dt.Day;
                    string Folder = "/Content/Images/Admin" + year + "/" + month + "/" + day + "/" + id + "/";
                    employee.Avatar = Folder + fileName;
                    var photo = db.Employees.Find(id).Avatar;
                    string fullPath = Request.MapPath("~" + photo);
                    FileInfo file = new FileInfo(fullPath);
                    if (file.Exists)//check file exsit or not
                    {
                        file.Delete();

                    }

                    bool IsExists = System.IO.Directory.Exists(Server.MapPath(Folder));
                    if (!IsExists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(Folder));
                    fileName = Path.Combine(Server.MapPath(Folder), fileName);
                    data.ImageName.SaveAs(fileName);
                }




                employee.FirstName = data.FirstName;
                employee.LastName = data.LastName;
                employee.Phone = data.Phone;
                employee.Email = data.Email;
                employee.Gender = data.Gender;
                employee.DateOfBirth = data.DateOfBirth;


                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(data.Id.ToString(), false);
                ViewBag.Message = "Update successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            ViewBag.RolelId = new SelectList(db.Hospitals, "ID", "Name", data.RoleID);
            return View(employee);
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            int id = int.Parse(User.Identity.Name);
            var employee = db.Employees.Find(id);
      
            return View(employee);
        }
        [HttpPost]
        public ActionResult ChangePassword(Employee data)
        {
            int id = int.Parse(User.Identity.Name);
            var employee = db.Employees.Find(id);
            var old = db.Employees.Find(id).Password;

            data.NewPassword = Crypto.Hash(Crypto.Hash(data.NewPassword));
            data.OldPassword = Crypto.Hash(Crypto.Hash(data.OldPassword));
            if (old == data.OldPassword)
            {
                try
                {
                    employee.Password = data.NewPassword;



                    db.SaveChanges();
                    ViewBag.Message = "Password changed successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }

            }
            else
            {
                ViewBag.Message = "Old Password not correct";
            }

            return View(data);
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
                var account = db.Employees.Where(x =>
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
        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Admin/" + emailFor + "/" + activationCode;
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
                var user = db.Employees.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
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
                    var user = db.Employees.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(Crypto.Hash(model.NewPassword));
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
    }
}