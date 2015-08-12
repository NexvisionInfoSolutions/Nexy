using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Models;
using Data.Models.Enumerations; 

namespace LoanManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: /User/
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.UserGroup);
            return View(user.Where(s => s.UserType == UserType.User).ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUser user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            ViewBag.UserGroupID = new SelectList(db.Usergroup, "UserGroupId", "Code");
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sdtoUser user)
        {
            if (ModelState.IsValid)
            {
                user.UserType = UserType.User;
                if (user.UserAddress != null)
                    user.UserAddress = db.Address.Add(user.UserAddress);
                if (user.Contacts != null)
                    user.Contacts = db.Contacts.Add(user.Contacts);
                user.CreatedOn = DateTime.Now;
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserGroupID = new SelectList(db.Usergroup, "UserGroupId", "Code", user.UserGroupId);
            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUser user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserGroupID = new SelectList(db.Usergroup, "UserGroupId", "Code", user.UserGroupId);
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Code,Name,Description,UserGroupId,IsActive")] sdtoUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserGroupID = new SelectList(db.Usergroup, "UserGroupId", "Code", user.UserGroupId);
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUser user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sdtoUser user = db.User.Find(id);
            db.User.Remove(user);
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
