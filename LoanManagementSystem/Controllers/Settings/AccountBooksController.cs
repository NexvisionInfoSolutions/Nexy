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
    [Authorize()]
    public class AccountBooksController : ControllerBase
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: AccountBooks
        public ActionResult Index()
        {
            var sdtoAccountBooks = db.AccountBooks.Include(s => s.AccountBookType).Include(s => s.AccountHead);
            return View(sdtoAccountBooks.ToList());
        }
        public JsonResult AccountBooksInfo()
        {
            var dbResult = db.AccountBooks.Include(s => s.AccountBookType).Include(s => s.AccountHead).Where(x => x.IsDeleted == false).ToList();
            var AccountBooks = (from AccountBook in dbResult
                                select new
                                {
                                    AccountBook.AccountBookId,
                                    AccountBook.AccountBookType.AccountBookType,
                                    AccountBook.BankCharges,
                                    AccountBook.BankInterest,
                                    AccountBook.BookCode,
                                    AccountBook.BookName,
                                    AccountName = AccountBook.AccountHead == null ? string.Empty : AccountBook.AccountHead.AccountName,
                                    AccountBooksInfo = AccountBook.AccountBookId
                                });
            return Json(AccountBooks, JsonRequestBehavior.AllowGet);
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
            sdtoAccountBook objAccBook = new sdtoAccountBook();
            sdtoSettings settings = db.GeneralSettings.FirstOrDefault();
            ViewBag.AccountBookTypeId = new SelectList(db.AccountBookTypes, "AccountBookTypeId", "AccountBookType");
            ViewBag.AccountHeadId = new SelectList(db.AccountHeads, "AccountHeadId", "AccountCode");
            ViewBag.BankInterest = settings.BankInterest;
            ViewBag.BankCharges = settings.BankCharges;
            objAccBook.Status = Data.Models.Enumerations.AccountBookStatus.Active;
            return View(objAccBook);
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

                var jrnType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Journal", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (jrnType != null && AccountBook.AccountBookTypeId == jrnType.AccountBookTypeId)
                    AccountBook.AccountHeadId = null;

                db.AccountBooks.Add(AccountBook);
                db.SaveChanges();

                SetDisplayMessage("Account Book is created successfully");
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
        public ActionResult Edit(//[Bind(Include = "AccountBookId,BookCode,BookName,BookDescription,AccountBookTypeId,AccountHeadId,BankInterest,BankCharges,ReceiptVoucherPrefix,ReceiptVoucherSuffix,PaymentVoucherPrefix,PaymentVoucherSuffix,Status")] 
            sdtoAccountBook sdtoAccountBook)
        {
            if (ModelState.IsValid)
            {
                sdtoAccountBook.ModifiedOn = DateTime.Now;
                db.Entry(sdtoAccountBook).State = EntityState.Modified;
                db.SaveChanges();

                SetDisplayMessage("Account Book is saved successfully");
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
            sdtoAccountBook.IsDeleted = true;
            db.Entry(sdtoAccountBook).State = EntityState.Modified;
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
