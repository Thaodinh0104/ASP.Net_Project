using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreeGrab.Controllers
{
    public class UploadAjaxController : Controller
    {
        [HttpPost]
        [ActionName("uploadImage")]
        public ActionResult uploadImagetinyMCE(HttpPostedFileBase file)
        {
            string path = "~/Content";
            string repath = "/Content";
            bool exist = System.IO.Directory.Exists(Server.MapPath(path));
            if (!exist) System.IO.Directory.CreateDirectory(Server.MapPath(path));
            path += "/Images";
            repath += "/Images";
            exist = System.IO.Directory.Exists(Server.MapPath(path));
            if (!exist) System.IO.Directory.CreateDirectory(Server.MapPath(path));
            path += "/Newses";
            repath += "/Newses/";
            exist = System.IO.Directory.Exists(Server.MapPath(path));
            if (!exist) System.IO.Directory.CreateDirectory(Server.MapPath(path));
            var location = SaveFileImg(Server.MapPath(path), repath, file);
            return Json(new { location }, JsonRequestBehavior.AllowGet);
        }
        private static string SaveFileImg(string pathfolder, string repath, HttpPostedFileBase file)
        {
            const int megabyte = 1024 * 1024;
            if (file.ContentLength > (8 * megabyte))
            {
                throw new InvalidOperationException("File size limit exceeded");
            }
            if (!file.ContentType.StartsWith("image/"))
            {
                throw new InvalidOperationException("Invalid MIME content type");
            }
            var extension = Path.GetExtension(file.FileName.ToLowerInvariant());
            string[] ar_extension = { ".gif", ".png", ".jpg", ".svg", ".webp" };
            if (!ar_extension.Contains(extension))
            {
                throw new InvalidOperationException("Invalid file extension");
            }
            var filename = Guid.NewGuid() + extension;
            var path = Path.Combine(pathfolder, filename);
            file.SaveAs(path);
            return Path.Combine(repath, filename).Replace('\\', '/');
        }
    }
}