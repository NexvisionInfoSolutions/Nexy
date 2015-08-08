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

namespace LoanManagementSystem.Controllers
{
    public class AccountHeadsController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: AccountHeads
        public ActionResult Index()
        {
            var sdtoAccountHeads = db.sdtoAccountHeads.Include(s => s.AccountType).Include(s => s.Address).Include(s => s.ContactPerson).Include(s => s.Contacts);
            return View(sdtoAccountHeads.ToList());
        }

        // GET: AccountHeads/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.sdtoAccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Create
        public ActionResult Create()
        {
            ViewBag.AccountTypeId = new SelectList(db.sdtoAccountTypes, "sdtoAccountTypeId", "AccountType");
            ViewBag.AddressId = new SelectList(db.sdtoAddresses, "AddressId", "Address1");
            ViewBag.ContactUserId = new SelectList(db.User, "UserID", "Code");
            ViewBag.ContactId = new SelectList(db.sdtoContacts, "ContactId", "Telephone1");
            return View();
        }

        // POST: AccountHeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountHeadId,AccountCode,AccountName,ScheduleId,AccountTypeId,CreditLimit,CreditDays,ContactId,AddressId,ContactUserId,TIN,CST,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] sdtoAccountHead sdtoAccountHead)
        {
            if (ModelState.IsValid)
            {
                db.sdtoAccountHeads.Add(sdtoAccountHead);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountTypeId = new SelectList(db.sdtoAccountTypes, "sdtoAccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            ViewBag.AddressId = new SelectList(db.sdtoAddresses, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactUserId = new SelectList(db.User, "UserID", "Code", sdtoAccountHead.ContactUserId);
            ViewBag.ContactId = new SelectList(db.sdtoContacts, "ContactId", "Telephone1", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.sdtoAccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountTypeId = new SelectList(db.sdtoAccountTypes, "sdtoAccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            ViewBag.AddressId = new SelectList(db.sdtoAddresses, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactUserId = new SelectList(db.User, "UserID", "Code", sdtoAccountHead.ContactUserId);
            ViewBag.ContactId = new SelectList(db.sdtoContacts, "ContactId", "Telephone1", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // POST: AccountHeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountHeadId,AccountCode,AccountName,ScheduleId,AccountTypeId,CreditLimit,CreditDays,ContactId,AddressId,ContactUserId,TIN,CST,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] sdtoAccountHead sdtoAccountHead)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoAccountHead).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountTypeId = new SelectList(db.sdtoAccountTypes, "sdtoAccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            ViewBag.AddressId = new SelectList(db.sdtoAddresses, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactUserId = new SelectList(db.User, "UserID", "Code", sdtoAccountHead.ContactUserId);
            ViewBag.ContactId = new SelectList(db.sdtoContacts, "ContactId", "Telephone1", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.sdtoAccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountHead);
        }

        // POST: AccountHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoAccountHead sdtoAccountHead = db.sdtoAccountHeads.Find(id);
            db.sdtoAccountHeads.Remove(sdtoAccountHead);
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
