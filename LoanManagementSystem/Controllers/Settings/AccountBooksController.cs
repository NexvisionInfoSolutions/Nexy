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

namespace LoanManagementSystem.Controllers.Settings
{
    public class AccountBooksController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: AccountBooks
        public ActionResult Index()
        {
            var sdtoAccountBooks = db.AccountBooks.Include(s => s.AccountBookType).Include(s => s.AccountHead);
            return View(sdtoAccountBooks.ToList());
        }

        // GET: AccountBooks/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountBook sdtoAccountBook = db.AccountBooks.Find(id);
            if (sdtoAccountBook == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountBook);
        }

        // GET: AccountBooks/Create
        public ActionResult Create()
        {
            sdtoSettings settings = db.GeneralSettings.Where(x => x.CompanyId == 1).FirstOrDefault();
            ViewBag.AccountBookTypeId = new SelectList(db.AccountBookTypes, "AccountBookTypeId", "AccountBookType");
            ViewBag.AccountHeadId = new SelectList(db.AccountHeads, "AccountHeadId", "AccountCode");
            ViewBag.BankInterest = settings.BankInterest;
            ViewBag.BankCharges = settings.BankCharges;
            return View();
        }

        // POST: AccountBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "AccountBookId,BookCode,BookName,BookDescription,AccountBookTypeId,AccountHeadId,BankInterest,BankCharges,ReceiptVoucherPrefix,ReceiptVoucherSuffix,PaymentVoucherPrefix,PaymentVoucherSuffix,Status")] 
            sdtoAccountBook AccountBook)
        {
            if (ModelState.IsValid)
            {
                AccountBook.CreatedOn = DateTime.Now;
                AccountBook.ModifiedOn = null;
                db.AccountBooks.Add(AccountBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountBookTypeId = new SelectList(db.AccountBookTypes, "AccountBookTypeId", "AccountBookType", AccountBook.AccountBookTypeId);
            ViewBag.AccountHeadId = new SelectList(db.AccountHeads, "AccountHeadId", "AccountCode", AccountBook.AccountHeadId);
            return View(AccountBook);
        }

        // GET: AccountBooks/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountBook sdtoAccountBook = db.AccountBooks.Find(id);
            if (sdtoAccountBook == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountBookTypeId = new SelectList(db.AccountBookTypes, "AccountBookTypeId", "AccountBookType", sdtoAccountBook.AccountBookTypeId);
            ViewBag.AccountHeadId = new SelectList(db.AccountHeads, "AccountHeadId", "AccountCode", sdtoAccountBook.AccountHeadId);
            return View(sdtoAccountBook);
        }

        // POST: AccountBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountBookId,BookCode,BookName,BookDescription,AccountBookTypeId,AccountHeadId,BankInterest,BankCharges,ReceiptVoucherPrefix,ReceiptVoucherSuffix,PaymentVoucherPrefix,PaymentVoucherSuffix,Status")] sdtoAccountBook sdtoAccountBook)
        {
            if (ModelState.IsValid)
            {
                sdtoAccountBook.ModifiedOn = DateTime.Now;
                db.Entry(sdtoAccountBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountBookTypeId = new SelectList(db.AccountBookTypes, "AccountBookTypeId", "AccountBookType", sdtoAccountBook.AccountBookTypeId);
            ViewBag.AccountHeadId = new SelectList(db.AccountHeads, "AccountHeadId", "AccountCode", sdtoAccountBook.AccountHeadId);
            return View(sdtoAccountBook);
        }

        // GET: AccountBooks/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountBook sdtoAccountBook = db.AccountBooks.Find(id);
            if (sdtoAccountBook == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountBook);
        }

        // POST: AccountBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoAccountBook sdtoAccountBook = db.AccountBooks.Find(id);
            db.AccountBooks.Remove(sdtoAccountBook);
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
