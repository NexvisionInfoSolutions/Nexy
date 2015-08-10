using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Models.Accounts;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers.Loan
{
    public class LoanRepaymentsController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: LoanRepayments
        public ActionResult Index()
        {
            var sdtoLoanRepayments = db.sdtoLoanRepayments.Include(s => s.LoanDetails);
            return View(sdtoLoanRepayments.ToList());
        }

        // GET: LoanRepayments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            if (sdtoLoanRepayment == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanRepayment);
        }

        // GET: LoanRepayments/Create
        public ActionResult Create()
        {
            ViewBag.LoanId = new SelectList(db.sdtoLoanInfoes, "LoanId", "ChequeDetails");
            return View();
        }

        // POST: LoanRepayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanRepaymentId,LoanId,RepaymentCode,PrincipalAmount,InterestAmount,InterestRate,RepaymentAmount,PendingPrincipalAmount,Status,PaymentMode,ChequeDetails,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] sdtoLoanRepayment sdtoLoanRepayment)
        {
            if (ModelState.IsValid)
            {
                db.sdtoLoanRepayments.Add(sdtoLoanRepayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoanId = new SelectList(db.sdtoLoanInfoes, "LoanId", "ChequeDetails", sdtoLoanRepayment.LoanId);
            return View(sdtoLoanRepayment);
        }

        // GET: LoanRepayments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            if (sdtoLoanRepayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoanId = new SelectList(db.sdtoLoanInfoes, "LoanId", "ChequeDetails", sdtoLoanRepayment.LoanId);
            return View(sdtoLoanRepayment);
        }

        // POST: LoanRepayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanRepaymentId,LoanId,RepaymentCode,PrincipalAmount,InterestAmount,InterestRate,RepaymentAmount,PendingPrincipalAmount,Status,PaymentMode,ChequeDetails,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] sdtoLoanRepayment sdtoLoanRepayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoLoanRepayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoanId = new SelectList(db.sdtoLoanInfoes, "LoanId", "ChequeDetails", sdtoLoanRepayment.LoanId);
            return View(sdtoLoanRepayment);
        }

        // GET: LoanRepayments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            if (sdtoLoanRepayment == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanRepayment);
        }

        // POST: LoanRepayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            db.sdtoLoanRepayments.Remove(sdtoLoanRepayment);
            db.SaveChanges();
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
