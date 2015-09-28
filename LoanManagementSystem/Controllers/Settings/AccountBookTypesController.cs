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
    public class AccountBookTypesController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: AccountBookTypes
        public ActionResult Index()
        {
            return View(db.AccountBookTypes.ToList());
        }

        // GET: AccountBookTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountBookType sdtoAccountBookType = db.AccountBookTypes.Find(id);
            if (sdtoAccountBookType == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountBookType);
        }

        // GET: AccountBookTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountBookTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountBookTypeId,AccountBookType,Status")] sdtoAccountBookType sdtoAccountBookType)
        {
            if (ModelState.IsValid)
            {
                db.AccountBookTypes.Add(sdtoAccountBookType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sdtoAccountBookType);
        }

        // GET: AccountBookTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountBookType sdtoAccountBookType = db.AccountBookTypes.Find(id);
            if (sdtoAccountBookType == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountBookType);
        }

        // POST: AccountBookTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountBookTypeId,AccountBookType,Status")] sdtoAccountBookType sdtoAccountBookType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoAccountBookType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sdtoAccountBookType);
        }

        // GET: AccountBookTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountBookType sdtoAccountBookType = db.AccountBookTypes.Find(id);
            if (sdtoAccountBookType == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountBookType);
        }

        // POST: AccountBookTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoAccountBookType sdtoAccountBookType = db.AccountBookTypes.Find(id);
            db.AccountBookTypes.Remove(sdtoAccountBookType);
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
