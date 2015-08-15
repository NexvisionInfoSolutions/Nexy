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
            var accountHeads = db.AccountHeads.Include(s => s.AccountType).Include(s => s.Address).Include(s => s.Contacts);
            return View(accountHeads.ToList());
        }

        // GET: AccountHeads/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Create
        public ActionResult Create()
        {
            ViewBag.ScheduleList = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", 0);
            ViewBag.AccountTypeList = new SelectList(db.AccountTypes, "AccountTypeId", "AccountType");
            return View();
        }

        // POST: AccountHeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "AccountHeadId,AccountCode,AccountName,ScheduleId,AccountTypeId,CreditLimit,CreditDays,TIN,CST,Contacts.ContactName,Contacts.Mobile1,Contacts.Telephone1,Address.Address1")]
            sdtoAccountHead accHead)
        {
            if (ModelState.IsValid)
            {
                accHead.CreatedOn = DateTime.Now;
                accHead.ModifiedOn = null;

                if (accHead.Address != null)
                {
                    accHead.Address.CreatedOn = accHead.CreatedOn;
                }

                if (accHead.Contacts != null)
                {
                    accHead.Contacts.CreatedOn = accHead.CreatedOn;
                }

                accHead.Contacts = db.Contacts.Add(accHead.Contacts);
                accHead.Address = db.Address.Add(accHead.Address);
                db.AccountHeads.Add(accHead);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScheduleList = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", accHead.ScheduleId);
            ViewBag.AccountTypeList = new SelectList(db.AccountTypes, "AccountTypeId", "AccountType", accHead.AccountTypeId);

            return View(accHead);
        }

        // GET: AccountHeads/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchedulesList = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", sdtoAccountHead.ScheduleId);
            ViewBag.AccountTypeId = new SelectList(db.AccountTypes, "AccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // POST: AccountHeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountHeadId,AccountCode,AccountName,ScheduleId,AccountTypeId,CreditLimit,CreditDays,ContactId,AddressId,TIN,CST,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] sdtoAccountHead sdtoAccountHead)
        {
            if (ModelState.IsValid)
            {
                sdtoAccountHead.ModifiedOn = DateTime.Now;
                db.Entry(sdtoAccountHead).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SchedulesList = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", sdtoAccountHead.ScheduleId);
            ViewBag.AccountTypeId = new SelectList(db.AccountTypes, "AccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
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
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
            db.AccountHeads.Remove(sdtoAccountHead);
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
