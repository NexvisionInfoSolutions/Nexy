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
    [Authorize()]
    public class ExecutiveController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: /Executive/
        public ActionResult Index()
        {
            var user = db.User.Include(s => s.UserAddress).Include(s => s.Contacts).Include(s => s.UserGroup);

            return View(user.Where(s => s.UserType == UserType.Executive).ToList());
        }
        public JsonResult ExecutiveInfo()
        {
            var dbResult = db.Set<sdtoUser>().Include("UserAddress").Include("UserAddress.StateDetails").Include("UserAddress.Country").Include("Contacts").Include("UserAddress.DistrictDetails").Include("UserAddress.TalukDetails").Include("UserAddress.VillageDetails").Where(s => s.UserType == UserType.Executive).ToList();
            var Users = (from users in dbResult
                         select new
                         {
                             users.UserID,
                             users.FirstName,
                             users.LastName,
                             users.Designation,
                             users.Code,
                             ExecutiveInfo = users.UserID,
                             UserAddress = UtilityHelper.UtilityHelper.FormatAddress(users.UserAddress.Address1, users.UserAddress.Address2, users.UserAddress.Place, users.UserAddress.Post, users.UserAddress.DistrictDetails == null ? "" : users.UserAddress.DistrictDetails.DistrictName, users.UserAddress.Zipcode, users.UserAddress.TalukDetails == null ? "" : users.UserAddress.TalukDetails.TalukName, users.UserAddress.VillageDetails == null ? "" : users.UserAddress.VillageDetails.VillageName, users.UserAddress.StateDetails == null ? "" : users.UserAddress.StateDetails.StateName, users.UserAddress.Country == null ? "" : users.UserAddress.Country.CountryName),
                             UserContactPhone = users.Contacts.Telephone1,
                             UserContactMobile = users.Contacts.Mobile1
                         });
            return Json(Users, JsonRequestBehavior.AllowGet);
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

            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
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
                sdtouser.UserType = UserType.Executive;
                if (sdtouser.UserAddress != null)
                {
                    sdtouser.UserAddress.CreatedOn = DateTime.Now;
                    sdtouser.UserAddress.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.Contacts != null)
                {
                    sdtouser.Contacts.CreatedOn = DateTime.Now;
                    sdtouser.Contacts.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.UserAddress != null)
                {
                    sdtouser.UserAddress.CountryId = sdtouser.UserAddress.CountryId == 0 ? null : sdtouser.UserAddress.CountryId;
                    sdtouser.UserAddress.StateId = sdtouser.UserAddress.StateId == 0 ? null : sdtouser.UserAddress.StateId;
                    sdtouser.UserAddress.DistrictId = sdtouser.UserAddress.DistrictId == 0 ? null : sdtouser.UserAddress.DistrictId;
                    sdtouser.UserAddress.TalukId = sdtouser.UserAddress.TalukId == 0 ? null : sdtouser.UserAddress.TalukId;
                    sdtouser.UserAddress.VillageId = sdtouser.UserAddress.VillageId == 0 ? null : sdtouser.UserAddress.VillageId;
                }

                sdtouser.GuaranterAddress = null;
                sdtouser.PermanentAddress = null;
                sdtouser.PermanentAddressId = null;
                sdtouser.GuaranterAddressId = null;

                sdtouser.Contacts = db.Contacts.Add(sdtouser.Contacts);
                sdtouser.UserAddress = db.Address.Add(sdtouser.UserAddress);
                sdtouser.UserType = UserType.Executive;
                db.User.Add(sdtouser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.UserAddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.UserContactId);
            //ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
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
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.UserAddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.UserContactId);
            ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
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
                sdtouser.UserType = UserType.Executive;
                if (sdtouser.UserAddress != null)
                {
                    sdtouser.UserAddress.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.Contacts != null)
                {
                    sdtouser.Contacts.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.UserAddress != null)
                {
                    sdtouser.UserAddress.CountryId = sdtouser.UserAddress.CountryId == 0 ? null : sdtouser.UserAddress.CountryId;
                    sdtouser.UserAddress.StateId = sdtouser.UserAddress.StateId == 0 ? null : sdtouser.UserAddress.StateId;
                    sdtouser.UserAddress.DistrictId = sdtouser.UserAddress.DistrictId == 0 ? null : sdtouser.UserAddress.DistrictId;
                    sdtouser.UserAddress.TalukId = sdtouser.UserAddress.TalukId == 0 ? null : sdtouser.UserAddress.TalukId;
                    sdtouser.UserAddress.VillageId = sdtouser.UserAddress.VillageId == 0 ? null : sdtouser.UserAddress.VillageId;
                }

                sdtouser.GuaranterAddress = null;
                sdtouser.PermanentAddress = null;
                sdtouser.PermanentAddressId = null;
                sdtouser.GuaranterAddressId = null;

                db.User.Attach(sdtouser);
                db.Entry(sdtouser).State = EntityState.Modified;
                db.Entry(sdtouser.UserAddress).State = EntityState.Modified;
                db.Entry(sdtouser.Contacts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.UserAddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.UserContactId);
            //ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
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
