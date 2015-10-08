using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Models;
using Data.Models.Enumerations;
using System.Web.Security;
using System.IO;

namespace LoanManagementSystem.Controllers
{
    //[Authorize()]
    public class LoginController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            FormsAuthentication.SignOut();

            var accFY = db.FinancialPeriod.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.FinancialPeriodId.ToString(), Text = x.PeriodName }).ToList();
            accFY.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Period Name" });
            ViewBag.FinancialYears = new SelectList(accFY, "Value", "Text", 0);

            return View("Login");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(sdtoUser u)
        {
            FormsAuthentication.SignOut();
            // this action is for handle post (login)
            //if (ModelState.IsValid) // this is check validity
            {
                if ((u.FinancialYearId != null && u.FinancialYearId > 0))
                {
                    using (LoanDBContext dc = new LoanDBContext())
                    {
                        var v = dc.User.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                        if (v != null)
                        {
                            var userSession = new sdtoUserSession() { UserId = v.UserID, CompanyId = v.CompanyId, StartTime = DateTime.Now, FinancialYearId = u.FinancialYearId, Browser = UtilityHelper.UtilityHelper.GetBrowser(), SessionKey = System.Guid.NewGuid().ToString("N"), IPAddress = Request.ServerVariables["REMOTE_ADDR"] };
                            db.UserSessions.Add(userSession);
                            db.SaveChanges();
                            v.UserSession = userSession;
                            FormsAuthentication.SetAuthCookie(u.UserName, false);
                            UtilityHelper.UserSession.SetSession(UtilityHelper.UserSession.LoggedInUser, v);
                            //Session["LogedUserFullname"] = v.FirstName.ToString() + " " + v.LastName.ToString();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                            ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                else
                    ModelState.AddModelError("", "Please select a financial period.");

            }
            var accFY = db.FinancialPeriod.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.FinancialPeriodId.ToString(), Text = x.PeriodName }).ToList();
            accFY.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Period Name" });
            ViewBag.FinancialYears = new SelectList(accFY, "Value", "Text", 0);
            return View();
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
