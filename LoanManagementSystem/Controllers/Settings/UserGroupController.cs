using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Controllers
{
    [Authorize()]
    public class UserGroupController : ControllerBase
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: /UserGroup/
        public ActionResult Index()
        {
            return View(db.Usergroup.Where(x => x.IsDeleted == false).ToList());
        }
        public JsonResult UsergroupInfo()
        {
            var dbResult = db.Usergroup.Where(x => x.IsDeleted == false).ToList();
            var Users = (from users in dbResult
                         select new
                         {
                             users.UserGroupId,
                             users.Code,
                             users.Description,
                             UsergroupInfo = users.UserGroupId
                         });
            return Json(Users, JsonRequestBehavior.AllowGet);
        }
        // GET: /UserGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUserGroup usergroup = db.Usergroup.Find(id);
            if (usergroup == null)
            {
                return HttpNotFound();
            }
            return View(usergroup);
        }

        // GET: /UserGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /UserGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Description")] sdtoUserGroup usergroup)
        {
            if (string.IsNullOrEmpty(usergroup.Code))
                ModelState.AddModelError("", "Code cannot be empty");
            else if (ModelState.IsValid)
            {
                usergroup.CreatedOn = DateTime.Now;
                usergroup.CreatedBy = CurrentUserSession.UserId;
                db.Usergroup.Add(usergroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usergroup);
        }

        // GET: /UserGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUserGroup usergroup = db.Usergroup.Find(id);
            if (usergroup == null)
            {
                return HttpNotFound();
            }
            return View(usergroup);
        }

        // POST: /UserGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sdtoUserGroup usergroup)
        {
            if (ModelState.IsValid)
            {
                usergroup.ModifiedOn = DateTime.Now;
                usergroup.ModifiedBy = CurrentUserSession.UserId;
                db.Entry(usergroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usergroup);
        }

        // GET: /UserGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUserGroup usergroup = db.Usergroup.Find(id);
            if (usergroup == null)
            {
                return HttpNotFound();
            }
            return View(usergroup);
        }

        // POST: /UserGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sdtoUserGroup usergroup = db.Usergroup.Find(id);
            usergroup.IsDeleted = true;
            usergroup.DeletedBy = CurrentUserSession.UserId;
            usergroup.DeletedOn = DateTime.Now;
            db.Entry(usergroup).State = EntityState.Modified;
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
