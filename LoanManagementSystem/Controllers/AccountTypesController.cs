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
    public class AccountTypesController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: AccountTypes
        public ActionResult Index()
        {
            return View(db.sdtoAccountTypes.ToList());
        }

        // GET: AccountTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountType sdtoAccountType = db.sdtoAccountTypes.Find(id);
            if (sdtoAccountType == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountType);
        }

        // GET: AccountTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sdtoAccountTypeId,AccountType,Status")] sdtoAccountType sdtoAccountType)
        {
            if (ModelState.IsValid)
            {
                db.sdtoAccountTypes.Add(sdtoAccountType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sdtoAccountType);
        }

        // GET: AccountTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountType sdtoAccountType = db.sdtoAccountTypes.Find(id);
            if (sdtoAccountType == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountType);
        }

        // POST: AccountTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sdtoAccountTypeId,AccountType,Status")] sdtoAccountType sdtoAccountType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoAccountType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sdtoAccountType);
        }

        // GET: AccountTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountType sdtoAccountType = db.sdtoAccountTypes.Find(id);
            if (sdtoAccountType == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountType);
        }

        // POST: AccountTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoAccountType sdtoAccountType = db.sdtoAccountTypes.Find(id);
            db.sdtoAccountTypes.Remove(sdtoAccountType);
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
