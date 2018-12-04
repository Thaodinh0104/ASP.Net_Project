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
   
    public class AccountCustomerController : Controller
    {
        //public static string Aut(string usename, string avatar)
        //{
        //    return Formsauthentication.FormsCookieName(usename);
        ////}

        private FreeGrabSocialEntities db = new FreeGrabSocialEntities();

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        // GET: AccountCustomer
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer data)
        {
           // string message;
                var email = db.Customers.Where(x => x.Email.Equals(data.Email)).FirstOrDefault();

                if (email == null)
                {
                    Customer cus = new Customer();
                if (data.ImageName != null) {
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

            return View(); 
        }

        [Authorize]
        public ActionResult UpdateProfile()
        {
            int id = int.Parse(User.Identity.Name);
            var customer = db.Customers.Find(id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult UpdateProfile(Customer data)
        {
            // lay id cua thang dang dang nhap
            int id = int.Parse(User.Identity.Name);
            // truy van vao co so du lieu lay thong tin nguoi nay
            var customer = db.Customers.Find(id);
         
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
                    string Folder = "/Content/Images/" + year + "/" + month + "/" + day + "/" + id + "/";
                    customer.Avatar = Folder + fileName;
                    var photo = db.Customers.Find(id).Avatar;
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

              
             

                customer.FirstName = data.FirstName;
                customer.LastName = data.LastName;
                customer.Phone = data.Phone;
                customer.Email = data.Email;
                customer.Gender = data.Gender;
                customer.DateOfBirth = data.DateOfBirth;


                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(data.Id.ToString(), false);
                ViewBag.Message = "Update successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View(customer);
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            int id = int.Parse(User.Identity.Name);
            var customer = db.Customers.Find(id);
            return View(customer);
        }
        [HttpPost]
        public ActionResult ChangePassword(Customer data)
        {
            int id = int.Parse(User.Identity.Name);
            var customer = db.Customers.Find(id);
            var old = db.Customers.Find(id).Password;

            data.NewPassword = Crypto.Hash(data.NewPassword);
            data.OldPassword = Crypto.Hash(data.OldPassword);
            if (old == data.OldPassword)
            {
                try
                {
                    customer.Password = data.NewPassword;



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
                var i =Email;
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
        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/AccountCustomer/" + emailFor + "/" + activationCode;
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
    }
}