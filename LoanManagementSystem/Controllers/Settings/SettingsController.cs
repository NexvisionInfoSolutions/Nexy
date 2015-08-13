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
    public class SettingsController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: Settings
        public ActionResult Index()
        {
            return View(db.GeneralSettings.ToList());
        }

        // GET: Settings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            ViewBag.CompanyList = new SelectList(db.Companies, "CompanyId", "CompanyName");

            var settings = db.GeneralSettings.Where(x => x.CompanyId == userDetails.CompanyId);
            if (settings != null && settings.Count() > 0)
                return EditOnCreate(settings.FirstOrDefault(x => x.CompanyId == userDetails.CompanyId) as sdtoSettings);
            else
                return View(new sdtoSettings() { CompanyId = userDetails.CompanyId, BankCharges = 0, BankInterest = 0 });
        }

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SettingsId,CompanyId,BankInterest,BankCharges")] sdtoSettings Settings)
        {
            if (ModelState.IsValid)
            {
                Settings.CreatedOn = DateTime.Now;
                Settings.ModifiedOn = Settings.CreatedOn;

                db.GeneralSettings.Add(Settings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Settings);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // GET: Settings/Edit/5
        public ActionResult EditOnCreate(sdtoSettings sdtoSettings)
        {
            if (sdtoSettings == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SettingsId,CompanyId,BankInterest,BankCharges,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] sdtoSettings sdtoSettings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoSettings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sdtoSettings);
        }

        // GET: Settings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            db.GeneralSettings.Remove(sdtoSettings);
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
