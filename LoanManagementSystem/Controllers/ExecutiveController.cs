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
    public class ExecutiveController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: /Executive/
        public ActionResult Index()
        {
            var user = db.User.Include(s => s.Address).Include(s => s.Contacts).Include(s => s.UserGroup);
            return View(user.ToList());
        }

        // GET: /Executive/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUser sdtouser = db.User.Find(id);
            if (sdtouser == null)
            {
                return HttpNotFound();
            }
            return View(sdtouser);
        }

        // GET: /Executive/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1");
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName");
            ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name");
            return View();
        }

        // POST: /Executive/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sdtoUser sdtouser)//[Bind(Include="UserID,Code,Name,Description,UserGroupId,IsActive,Designation,UserType,ContactId,AddressId")] 
        {
            if (ModelState.IsValid)
            {
                if (sdtouser.Address != null)
                {
                    sdtouser.Address.CreatedOn = DateTime.Now;
                    sdtouser.Address.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.Contacts != null)
                {
                    sdtouser.Contacts.CreatedOn = DateTime.Now;
                    sdtouser.Contacts.ModifiedOn = DateTime.Now;
                }

                sdtouser.Contacts = db.Contacts.Add(sdtouser.Contacts);
                sdtouser.Address = db.Address.Add(sdtouser.Address);
                sdtouser.UserType = UserType.Executive;
                db.User.Add(sdtouser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.ContactId);
            //ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            return View(sdtouser);
        }

        // GET: /Executive/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUser sdtouser = db.User.Find(id);
            if (sdtouser == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.ContactId);
            ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            return View(sdtouser);
        }

        // POST: /Executive/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sdtoUser sdtouser)
        {
            if (ModelState.IsValid)
            {
                if (sdtouser.Address != null)
                {
                    sdtouser.Address.ModifiedOn = DateTime.Now;  
                }

                if (sdtouser.Contacts != null)
                {
                    sdtouser.Contacts.ModifiedOn = DateTime.Now;
                }
                db.Entry(sdtouser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.ContactId);
            //ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            return View(sdtouser);
        }

        // GET: /Executive/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoUser sdtouser = db.User.Find(id);
            if (sdtouser == null)
            {
                return HttpNotFound();
            }
            return View(sdtouser);
        }

        // POST: /Executive/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoUser sdtouser = db.User.Find(id);
            db.User.Remove(sdtouser);
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
