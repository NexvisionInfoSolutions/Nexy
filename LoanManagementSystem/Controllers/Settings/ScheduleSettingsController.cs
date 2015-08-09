using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Models.Accounts.Schedules;
using LoanManagementSystem.Models;
using Business.SysBase.Tree;
using Data.Models.SysBase.Tree;

namespace LoanManagementSystem.Controllers
{
    public class ScheduleSettingsController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: ScheduleSettings
        public ActionResult Index()
        {
            bfTree<sdtoSchedule> objBfTree = new bfTree<sdtoSchedule>(db);
            var schedules = objBfTree.GetData();
            return View(schedules);
        }

        // GET: ScheduleSettings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sdtoSchedules = (
                                 from I in db.Schedules.Where(w => w.ScheduleId == id)
                                 from O in db.Schedules.Where(w => w.ScheduleId == I.ParentId).DefaultIfEmpty()
                                 select new { ScheduleId = I.ScheduleId, ParentId = I.ParentId, ScheduleName = I.ScheduleName, ParentScheduleId = O.ScheduleId, ParentScheduleName = O.ScheduleName }
                                ).ToList().Select(x => new sdtoSchedule()
                                       {
                                           ScheduleId = x.ScheduleId,
                                           ScheduleName = x.ScheduleName,
                                           ParentId = x.ParentId,
                                           Parent = new sdtoSchedule()
                                           {
                                               ScheduleId = x.ParentScheduleId,
                                               ScheduleName = x.ParentScheduleName
                                           }
                                       }); ;




            if (sdtoSchedules == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSchedules.FirstOrDefault());
        }

        // GET: ScheduleSettings/Create
        public ActionResult Create()
        {
            var SelectList1 = db.Schedules.ToList();
            SelectList1.Insert(0, new sdtoSchedule() { ScheduleId = 0, ScheduleName = "Root" });
            ViewBag.ScheduleList = new SelectList(SelectList1, "ScheduleId", "ScheduleName", 0);
            return View();
        }

        // POST: ScheduleSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleId,ScheduleName,ParentId")] sdtoSchedule sdtoSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedules.Add(sdtoSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScheduleList = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", sdtoSchedule.ParentId);
            return View(sdtoSchedule);
        }

        // GET: ScheduleSettings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSchedule sdtoSchedule = db.Schedules.Find(id);
            if (sdtoSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScheduleList = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", sdtoSchedule.ParentId);
            return View(sdtoSchedule);
        }

        // POST: ScheduleSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleId,ScheduleName,ParentId")] sdtoSchedule sdtoSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "ScheduleId", "ScheduleName", sdtoSchedule.ScheduleId);
            return View(sdtoSchedule);
        }

        // GET: ScheduleSettings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSchedule sdtoSchedule = db.Schedules.Find(id);
            if (sdtoSchedule == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSchedule);
        }

        // POST: ScheduleSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoSchedule sdtoSchedule = db.Schedules.Find(id);
            db.Schedules.Remove(sdtoSchedule);
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
