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
using Data.Models.Accounts;

namespace LoanManagementSystem.Controllers
{
    public class AccountingController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // GET: /User/
        public ActionResult OpeningBalances()
        {
            var openingBalance = db.OpeningBalance.Include(x => x.AccountHead).Include(x => x.Schedule).Include(x => x.FinancialPeriod).Where(x => x.IsDeleted == false && x.FinancialPeriod.IsCurrentYear == true);
            return View(openingBalance);
        }

        public JsonResult ListOpeningBalances()
        {
            var openingBalance = db.OpeningBalance.Include(x => x.AccountHead).Include(x => x.Schedule).Include(x => x.FinancialPeriod).Where(x => x.IsDeleted == false && x.FinancialPeriod.IsCurrentYear == true);
            var Balances = (from sop in openingBalance
                            select new
                            {
                                sop.OpeningBalanceId,
                                OpeningBalanceIdInfo = sop.OpeningBalanceId,
                                sop.AccountHead.AccountName,
                                sop.Schedule.ScheduleName,
                                sop.DebitOpeningBalance,
                                sop.CreditOpeningBalance,
                                sop.ClosingBalance
                            });
            return Json(Balances, JsonRequestBehavior.AllowGet);
        }

        private void LoadSelectListOpeningBalance(long AccHeadId, long AccScheduleId, long accFYId)
        {
            var accHeads = db.AccountHeads.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            var accSch = db.Schedules.ToList().Select(x => new SelectListItem() { Value = x.ScheduleId.ToString(), Text = x.ScheduleName }).ToList();
            var accFY = db.FinancialPeriod.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.FinancialPeriodId.ToString(), Text = x.PeriodName }).ToList();

            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Account Name" });
            accSch.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Schedule Name" });
            accFY.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Period Name" });

            ViewBag.AccountHeads = new SelectList(accHeads, "Value", "Text", AccHeadId);
            ViewBag.AccountSchedules = new SelectList(accSch, "Value", "Text", AccScheduleId);
            ViewBag.FinancialYears = new SelectList(accFY, "Value", "Text", accFYId);
        }

        public ActionResult NewOpeningBalance()
        {
            LoadSelectListOpeningBalance(0, 0, 0);
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOpeningBalance(sdtoOpeningBalance OpeningBalance)
        {
            if (ModelState.IsValid)
            {
                if (OpeningBalance.CreditOpeningBalance > 0 && OpeningBalance.DebitOpeningBalance > 0)
                {
                    ModelState.AddModelError("CreditOpeningBalance", "Please enter either credit or debit opening balance");
                    LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
                    return View(OpeningBalance);
                }

                OpeningBalance.CreatedOn = DateTime.Now;
                sdtoUser session = Session["UserDetails"] as sdtoUser;
                if (session != null)
                    OpeningBalance.CreatedBy = session.UserID;
                OpeningBalance.IsDeleted = false;

                OpeningBalance.ClosingBalance = OpeningBalance.CreditOpeningBalance > 0 ? OpeningBalance.CreditOpeningBalance : OpeningBalance.DebitOpeningBalance;

                db.OpeningBalance.Add(OpeningBalance);
                db.SaveChanges();
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("ListOpeningBalances");
                else
                    return RedirectToAction("Login");
            }

            LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
            return View(OpeningBalance);
        }

        public ActionResult UpdateOpeningBalance(long? OpeningBalanceId)
        {
            var openingBalance = db.OpeningBalance.Find(OpeningBalanceId);
            LoadSelectListOpeningBalance(openingBalance.AccountHeadId, openingBalance.ScheduleId, openingBalance.FinancialYearId);
            return View(openingBalance);
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOpeningBalance(sdtoOpeningBalance OpeningBalance, long PrevDebit, long PrevCredit)
        {
            if (ModelState.IsValid)
            {
                sdtoUser session = Session["UserDetails"] as sdtoUser;
                if (session != null)
                    OpeningBalance.ModifiedBy = session.UserID;
                OpeningBalance.ModifiedOn = DateTime.Now;

                OpeningBalance.ClosingBalance = (OpeningBalance.ClosingBalance - (PrevDebit > 0 ? PrevDebit : PrevCredit)) + (OpeningBalance.CreditOpeningBalance > 0 ? (-1 * OpeningBalance.CreditOpeningBalance) : OpeningBalance.DebitOpeningBalance);

                // ModelState.AddModelError("ProfileImage", "This field is required");
                db.Entry(OpeningBalance).State = EntityState.Modified;
                db.SaveChanges();
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Login");
            }

            LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
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
