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

namespace LoanManagementSystem.Controllers.Deposit
{
    public class DepositController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: sdtoDepositInfoes
        public ActionResult Index()
        {
            var sdtoDepositInfoes = db.sdtoDepositInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser);
            return View(sdtoDepositInfoes.ToList());
        }

        // GET: sdtoDepositInfoes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            if (sdtoDepositInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoDepositInfo);
        }

        // GET: sdtoDepositInfoes/Create
        public ActionResult Create()
        {
            ViewBag.CreatedBy = new SelectList(db.User, "UserID", "Code");
            ViewBag.DeletedBy = new SelectList(db.User, "UserID", "Code");
            ViewBag.UserId = new SelectList(db.User, "UserID", "Code");
            ViewBag.ModifiedBy = new SelectList(db.User, "UserID", "Code");
            return View();
        }

        // POST: sdtoDepositInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepositId,UserId,Duration,DepositType,MaturityDate,TotalInstallments,DepositAmount,MatureAmount,InstallmentAmount,ClosedDate,RecurringDepositDate,Status,ChequeDetails,InteresRate,ApprovedDate,ApprovedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] sdtoDepositInfo sdtoDepositInfo)
        {
            if (ModelState.IsValid)
            {
                db.sdtoDepositInfoes.Add(sdtoDepositInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.CreatedBy);
            ViewBag.DeletedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.DeletedBy);
            ViewBag.UserId = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.UserId);
            ViewBag.ModifiedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.ModifiedBy);
            return View(sdtoDepositInfo);
        }

        // GET: sdtoDepositInfoes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            if (sdtoDepositInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.CreatedBy);
            ViewBag.DeletedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.DeletedBy);
            ViewBag.UserId = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.UserId);
            ViewBag.ModifiedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.ModifiedBy);
            return View(sdtoDepositInfo);
        }

        // POST: sdtoDepositInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepositId,UserId,Duration,DepositType,MaturityDate,TotalInstallments,DepositAmount,MatureAmount,InstallmentAmount,ClosedDate,RecurringDepositDate,Status,ChequeDetails,InteresRate,ApprovedDate,ApprovedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] sdtoDepositInfo sdtoDepositInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoDepositInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.CreatedBy);
            ViewBag.DeletedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.DeletedBy);
            ViewBag.UserId = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.UserId);
            ViewBag.ModifiedBy = new SelectList(db.User, "UserID", "Code", sdtoDepositInfo.ModifiedBy);
            return View(sdtoDepositInfo);
        }

        // GET: sdtoDepositInfoes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            if (sdtoDepositInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoDepositInfo);
        }

        // POST: sdtoDepositInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            db.sdtoDepositInfoes.Remove(sdtoDepositInfo);
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
