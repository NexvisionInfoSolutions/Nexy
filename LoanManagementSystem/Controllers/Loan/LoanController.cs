﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Models.Accounts;
using LoanManagementSystem.Models;
using Data.Models.Enumerations;

namespace LoanManagementSystem.Controllers.Loan
{
    public class LoanController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: Loan
        public ActionResult Index()
        {
            var sdtoLoanInfoes = db.sdtoLoanInfoes.Include(s => s.Member);
            return View(sdtoLoanInfoes.ToList());
        }

        // GET: Loan/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        // GET: Loan/Create
        public ActionResult Create()
        {
            var loan = new sdtoLoanInfo();
            loan.RePaymentInterval = Data.Models.Enumerations.RepaymentInterval.Monthly;
            loan.Status = Data.Models.Enumerations.LoanStatus.Active;
            ViewBag.UserList = new SelectList(db.User.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(loan);
        }
        // POST: Loan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "LoanId,UserId,RepaymentStartDate,RePaymentInterval,RequestedAmount,ProposedAmount,LoanAmount,TotalInstallments,Status,ChequeDetails,InteresRate,SanctionedDate,SanctionedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] 
            sdtoLoanInfo sdtoLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo.InstallmentAmount = sdtoLoanInfo.LoanAmount / sdtoLoanInfo.TotalInstallments;
                db.sdtoLoanInfoes.Add(sdtoLoanInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserList = new SelectList(db.User.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(sdtoLoanInfo);
        }

        // GET: Loan/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserList = new SelectList(db.User.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(sdtoLoanInfo);
        }

        // POST: Loan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanId,UserId,RepaymentStartDate,RePaymentInterval,RequestedAmount,ProposedAmount,LoanAmount,TotalInstallments,Status,ChequeDetails,InteresRate,SanctionedDate,SanctionedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] sdtoLoanInfo sdtoLoanInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoLoanInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserList = new SelectList(db.User.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(sdtoLoanInfo);
        }

        // GET: Loan/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            db.sdtoLoanInfoes.Remove(sdtoLoanInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LoanCancellation(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment repay = db.sdtoLoanRepayments.Where(x => x.LoanId == id).FirstOrDefault();
            ViewData["PaidAmount"] = repay.RepaymentAmount;
            ViewData["BalaceAmount"] = repay.PendingPrincipalAmount;

            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanCancellation(sdtoLoanInfo objLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo sdtoloanInfo = db.sdtoLoanInfoes.Find(objLoanInfo.LoanId);
                sdtoloanInfo.Status = LoanStatus.Inactive;
                db.Entry(sdtoloanInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoanRecall(long? id)
        {
            sdtoLoanRepayment repay = db.sdtoLoanRepayments.Where(x => x.LoanId == id).FirstOrDefault();
            ViewData["PaidAmount"] = repay.RepaymentAmount;
            ViewData["BalaceAmount"] = repay.PendingPrincipalAmount;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanRecall(sdtoLoanInfo objLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo sdtoloanInfo = db.sdtoLoanInfoes.Find(objLoanInfo.LoanId);
                sdtoloanInfo.Status = LoanStatus.Active;
                db.Entry(sdtoloanInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
