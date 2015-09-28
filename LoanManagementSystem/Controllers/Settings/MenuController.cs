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
    public class MenuController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: Menu
        public ActionResult Index()
        {
            var sdtoUrlInfoes = (
                                 from I in db.UrlInfoCollection.Where(x => x.IsMenu == true)
                                 from O in db.UrlInfoCollection.Where(w => w.UrlId == I.ParentId).DefaultIfEmpty()
                                 select new { UrlId = I.UrlId, ParentId = I.ParentId, UrlText = I.UrlText, ParentUrlId = O != null ? O.UrlId : 0, ParentUrlText = O != null ? O.UrlText : "Root" }
                                ).ToList().Select(x => new sdtoUrlInfo()
                                       {
                                           UrlId = x.UrlId,
                                           UrlText = x.UrlText,
                                           ParentId = x.ParentId,
                                           Parent = new sdtoUrlInfo()
                                           {
                                               UrlId = x.ParentUrlId,
                                               UrlText = x.ParentUrlText
                                           }
                                       });
            return View(sdtoUrlInfoes.ToList());
        }

        // GET: Menu/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUrlInfo sdtoUrlInfo = db.UrlInfoCollection.Find(id);
            if (sdtoUrlInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoUrlInfo);
        }

        // GET: Menu/Create
        public ActionResult Create()
        {
            var SelectList1 = db.UrlInfoCollection.ToList();
            SelectList1.Insert(0, new sdtoUrlInfo() { UrlId = 0, UrlText = "Root" });
            ViewBag.MenuList = new SelectList(SelectList1, "UrlId", "UrlText", 0);
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "UrlId,UrlText,Url,ParentId,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Status,IsDeleted,DeletedBy,DeletedOn")]
            sdtoUrlInfo sdtoUrlInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoUrlInfo.CreatedOn = DateTime.Now;
                sdtoUrlInfo.ModifiedOn = DateTime.Now;
                sdtoUrlInfo.DeletedOn = DateTime.Now;
                //sdtoUrlInfo.Status = Data.Models.Enumerations.UrlInfoStatus.Active;
                db.UrlInfoCollection.Add(sdtoUrlInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var SelectList1 = db.UrlInfoCollection.ToList();
            SelectList1.Insert(0, new sdtoUrlInfo() { UrlId = 0, UrlText = "Root" });
            ViewBag.MenuList = new SelectList(SelectList1, "UrlId", "UrlText", sdtoUrlInfo.ParentId);
            return View(sdtoUrlInfo);
        }

        // GET: Menu/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUrlInfo sdtoUrlInfo = db.UrlInfoCollection.Find(id);
            if (sdtoUrlInfo == null)
            {
                return HttpNotFound();
            }
            var SelectList1 = db.UrlInfoCollection.ToList();
            SelectList1.Insert(0, new sdtoUrlInfo() { UrlId = 0, UrlText = "Root" });
            ViewBag.MenuList = new SelectList(SelectList1, "UrlId", "UrlText", sdtoUrlInfo.ParentId);
            return View(sdtoUrlInfo);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UrlId,UrlText,Url,ParentId,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Status,IsDeleted,DeletedBy,DeletedOn")] sdtoUrlInfo sdtoUrlInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoUrlInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var SelectList1 = db.UrlInfoCollection.ToList();
            SelectList1.Insert(0, new sdtoUrlInfo() { UrlId = 0, UrlText = "Root" });
            ViewBag.MenuList = new SelectList(SelectList1, "UrlId", "UrlText", sdtoUrlInfo.ParentId);
            return View(sdtoUrlInfo);
        }

        // GET: Menu/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUrlInfo sdtoUrlInfo = db.UrlInfoCollection.Find(id);
            if (sdtoUrlInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoUrlInfo);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoUrlInfo sdtoUrlInfo = db.UrlInfoCollection.Find(id);
            db.UrlInfoCollection.Remove(sdtoUrlInfo);
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
