using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers.Account
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(sdtoUser u)
        {
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                using (LoanDBContext dc = new LoanDBContext())
                {
                    var v = dc.User.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserID"] = v.UserID.ToString();
                        //Session["LogedUserFullname"] = v.FirstName.ToString() + " " + v.LastName.ToString();
                        return View("~/Views/Home/Index.cshtml");
                    }
                }
            }
            return View("Index");
        }

        public ActionResult Home()
        {
            if (Session["LogedUserID"] != null)
            {
             return View();
            }
            else
            {
            return RedirectToAction("Index");
             }
        }


    }
}