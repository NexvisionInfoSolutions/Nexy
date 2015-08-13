using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Utility
        public ActionResult CompanyLogo(long CompanyId)
        {
            FileInfo fInfo = new FileInfo(Server.MapPath("~/").Trim("\\/ ".ToCharArray()) + "\\ContentUpload\\Company\\" + CompanyId + ".logo");
            if (fInfo.Exists)
            {
                FileStream fs = fInfo.OpenRead();
                byte[] bImages = new byte[fs.Length];
                fs.Read(bImages, 0, bImages.Length);
                fs.Close();
                return File(bImages, "image/" + (fInfo.Extension.Trim(". /\\".ToCharArray())));
            }
            else
                return View();
        }
    }
}