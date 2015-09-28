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
using System.IO;
using Business.Reports;
using Data.Models.Accounts;

namespace LoanManagementSystem.Controllers
{
    [Authorize()]
    public class MemberController : ControllerBase
    {
        private LoanDBContext db = new LoanDBContext();

        private void FileUpload(long UserId, HttpPostedFileBase uploadFile)
        {
            if (uploadFile.ContentLength > 0)
            {
                DirectoryInfo dirUser = new DirectoryInfo(HttpContext.Server.MapPath("~/").Trim("\\/ ".ToCharArray()) + "\\ContentUpload\\User\\Profile");
                if (!dirUser.Exists)
                    dirUser.Create();

                FileInfo objFile = new FileInfo(Path.Combine(dirUser.FullName, System.Guid.NewGuid() + ".logo"));
                ViewBag.UserProfileAvatar = objFile.FullName;

                if (objFile.Exists)
                    objFile.Delete();

                uploadFile.SaveAs(objFile.FullName);
            }
        }

        // GET: /Member/
        public ActionResult Index()
        {
            var user = db.User.Where(s => s.UserType == UserType.Member && s.IsDeleted == false); //.Include(s => s.UserAddress).Include(s => s.Contacts).Include(s => s.GuaranterAddress).Include(s => s.GuaranterContacts).Include(s => s.PermanentAddress).Include(s => s.PermanentContacts).Include(s => s.UserGroup);
            return View(user);
            //return View(user.Where(s => s.UserType == UserType.Member).ToList());
        }

        public JsonResult MemberInfo()
        {
            //db.Set<sdtoUser>().Include("UserAddress").Include("PermanentAddress").Include(
            var dbResult = db.Set<sdtoUser>().Include("UserAddress").Include("UserAddress.StateDetails").Include("UserAddress.Country").Include("Contacts").Include("UserAddress.DistrictDetails").Include("UserAddress.TalukDetails").Include("UserAddress.VillageDetails")
                .Include("PermanentAddress").Include("PermanentAddress.StateDetails").Include("PermanentAddress.Country").Include("PermanentAddress.DistrictDetails").Include("PermanentAddress.TalukDetails").Include("PermanentAddress.VillageDetails").Where(s => s.UserType == UserType.Member && s.IsDeleted == false).ToList();
            var Users = (from users in dbResult
                         select new
                         {
                             users.UserID,
                             users.FirstName,
                             users.LastName,
                             users.FatherName,
                             users.Code,
                             MemberInfo = users.UserID,
                             UserAddress = UtilityHelper.UtilityHelper.FormatAddress(users.UserAddress.Address1, users.UserAddress.Address2, users.UserAddress.Place, users.UserAddress.Post, users.UserAddress.DistrictDetails.DistrictName, users.UserAddress.Zipcode, users.UserAddress.TalukDetails.TalukName, users.UserAddress.VillageDetails.VillageName, users.UserAddress.StateDetails.StateName, users.UserAddress.Country.CountryName),
                             PermanentAddress = UtilityHelper.UtilityHelper.FormatAddress(users.PermanentAddress.Address1, users.PermanentAddress.Address2, users.PermanentAddress.Place, users.PermanentAddress.Post, users.PermanentAddress.DistrictDetails.DistrictName, users.PermanentAddress.Zipcode, users.PermanentAddress.TalukDetails.TalukName, users.PermanentAddress.VillageDetails.VillageName, users.PermanentAddress.StateDetails.StateName, users.PermanentAddress.Country.CountryName),
                             UserContactPhone = users.Contacts.Telephone1,
                             UserContactMobile = users.Contacts.Mobile1
                         });
            return Json(Users, JsonRequestBehavior.AllowGet);
        }

        // GET: /Member/Details/5
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

        // GET: /Member/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1");
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName");
            ViewBag.GuaranterAddressId = new SelectList(db.Address, "AddressId", "Address1");
            ViewBag.GuaranterContactId = new SelectList(db.Contacts, "ContactId", "ContactName");
            ViewBag.PermanentAddressId = new SelectList(db.Address, "AddressId", "Address1");
            ViewBag.PermanentContactId = new SelectList(db.Contacts, "ContactId", "ContactName");
            ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name");

            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.GuaranterAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.PermanentAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);

            sdtoSettings settings = db.GeneralSettings.Where(x => x.SettingsId == 1).FirstOrDefault();
            if (settings == null || settings.AssetScheduleId == null)
            {
                TempData["ShowHeaderInfo"] = true;
                TempData["ViewMessage"] = "Please configure the system setting before member is created";

                return RedirectToAction("Edit", "Settings", new { id = 1 });
            }

            return View();
        }

        // POST: /Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sdtoUser sdtouser, HttpPostedFileBase ProfileImage)//[Bind(Include="UserID,Code,Name,Description,UserGroupId,IsActive,Designation,UserType,ContactId,AddressId,PermanentAddressId,GuaranterAddressId,PermanentContactId,GuaranterContactId,FatherName,CreatedOn")]
        {
            sdtouser.ConfirmPassword = sdtouser.Password;
            if (string.IsNullOrEmpty(sdtouser.Code))
                ModelState.AddModelError("", "Code cannot be empty");
            else if (string.IsNullOrEmpty(sdtouser.FirstName))
                ModelState.AddModelError("", "FirstName cannot be empty");
            else if (string.IsNullOrEmpty(sdtouser.LastName))
                ModelState.AddModelError("", "LastName cannot be empty");
            else if (string.IsNullOrEmpty(sdtouser.UserAddress.Address1))
                ModelState.AddModelError("", "Communication Address cannot be empty");
            else if (string.IsNullOrWhiteSpace(sdtouser.PermanentAddress.Address1))
                ModelState.AddModelError("", "Permanent Address cannot be empty");
            else if (ModelState.IsValid)
            {
                var validImageTypes = new string[]
                                        {
                                            "image/gif",
                                            "image/jpeg",
                                            "image/pjpeg",
                                            "image/png"
                                        };

                if (ProfileImage != null && !validImageTypes.Contains(ProfileImage.ContentType))
                {
                    ModelState.AddModelError("", "Please choose either a GIF, JPG or PNG image for ProfileImage");
                }
                else
                {
                    sdtouser.UserType = UserType.Member;
                    if (sdtouser.UserAddress != null)
                    {
                        sdtouser.UserAddress.CreatedOn = DateTime.Now;
                    }

                    if (sdtouser.Contacts != null)
                    {
                        sdtouser.Contacts.CreatedOn = DateTime.Now;
                    }
                    if (sdtouser.PermanentAddress != null)
                    {
                        sdtouser.PermanentAddress.CreatedOn = DateTime.Now;
                    }

                    if (sdtouser.PermanentContacts != null)
                    {
                        sdtouser.PermanentContacts.CreatedOn = DateTime.Now;
                    }
                    if (sdtouser.GuaranterAddress != null)
                    {
                        sdtouser.GuaranterAddress.CreatedOn = DateTime.Now;
                    }

                    if (sdtouser.GuaranterContacts != null)
                    {
                        sdtouser.GuaranterContacts.CreatedOn = DateTime.Now;
                        sdtouser.GuaranterContacts.ModifiedOn = DateTime.Now;
                    }

                    sdtouser.CreatedOn = DateTime.Now;
                    sdtouser.CreatedBy = CurrentUserSession.UserId;
                    sdtouser.UserAddress = db.Address.Add(sdtouser.UserAddress);
                    sdtouser.Contacts = db.Contacts.Add(sdtouser.Contacts);

                    sdtouser.PermanentAddress = db.Address.Add(sdtouser.PermanentAddress);
                    sdtouser.PermanentContacts = db.Contacts.Add(sdtouser.PermanentContacts);

                    sdtouser.GuaranterAddress = db.Address.Add(sdtouser.GuaranterAddress);
                    sdtouser.GuaranterContacts = db.Contacts.Add(sdtouser.GuaranterContacts);

                    db.User.Add(sdtouser);
                    db.SaveChanges();

                    if (ProfileImage != null)
                    {
                        FileUpload(sdtouser.UserID, ProfileImage);

                        System.IO.FileInfo fInfo = new FileInfo(ViewBag.UserProfileAvatar);
                        fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, sdtouser.UserID + ".logo"), true);
                        fInfo.Delete();
                    }

                    bfTransaction objAccTransaction = new bfTransaction(db);
                    objAccTransaction.InitiateMemberAccounts(sdtouser);

                    SetDisplayMessage("Member is created successfully");
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.UserAddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.UserContactId);
            ViewBag.GuaranterAddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.GuaranterAddressId);
            ViewBag.GuaranterContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.GuaranterContactId);
            ViewBag.PermanentAddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.PermanentAddressId);
            ViewBag.PermanentContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.PermanentContactId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.UserAddress.CountryId);
            ViewBag.GuaranterAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.GuaranterAddress.CountryId);
            ViewBag.PermanentAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.PermanentAddress.CountryId);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
            //ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            return View(sdtouser);
        }

        // GET: /Member/Edit/5
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
            sdtouser.UserAddress = db.Address.Find(sdtouser.UserAddressId);
            sdtouser.PermanentAddress = db.Address.Find(sdtouser.PermanentAddressId);
            sdtouser.GuaranterAddress = db.Address.Find(sdtouser.GuaranterAddressId);
            sdtouser.Contacts = db.Contacts.Find(sdtouser.UserContactId);
            sdtouser.PermanentContacts = db.Contacts.Find(sdtouser.PermanentContactId);
            sdtouser.GuaranterContacts = db.Contacts.Find(sdtouser.GuaranterContactId);
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.UserAddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.UserContactId);
            ViewBag.GuaranterAddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.GuaranterAddressId);
            ViewBag.GuaranterContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.GuaranterContactId);
            ViewBag.PermanentAddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.PermanentAddressId);
            ViewBag.PermanentContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.PermanentContactId);
            ViewBag.UserGroupId = new SelectList(db.Usergroup, "UserGroupId", "Name", sdtouser.UserGroupId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.UserAddress.CountryId);
            ViewBag.GuaranterAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.GuaranterAddress.CountryId);
            ViewBag.PermanentAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.PermanentAddress.CountryId);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
            return View(sdtouser);
        }

        // POST: /Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sdtoUser sdtouser, HttpPostedFileBase ProfileImage)
        {
            sdtouser.Password = sdtouser.ConfirmPassword;
            if (ModelState.IsValid)
            {
                //var user  = db.User.Find(sdtouser.UserID);
                if (sdtouser.UserAddress != null)
                {
                    sdtouser.UserAddress.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.Contacts != null)
                {
                    sdtouser.Contacts.ModifiedOn = DateTime.Now;
                }
                if (sdtouser.PermanentAddress != null)
                {
                    sdtouser.PermanentAddress.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.PermanentContacts != null)
                {
                    sdtouser.PermanentContacts.ModifiedOn = DateTime.Now;
                }
                if (sdtouser.GuaranterAddress != null)
                {
                    sdtouser.GuaranterAddress.ModifiedOn = DateTime.Now;
                }

                if (sdtouser.GuaranterContacts != null)
                {
                    sdtouser.GuaranterContacts.ModifiedOn = DateTime.Now;
                }

                //sdtouser.UserAddress.AddressId = user.UserAddressId;
                //sdtouser.PermanentAddress = db.Address.Find(sdtouser.PermanentAddressId);
                //sdtouser.GuaranterAddress = db.Address.Find(sdtouser.GuaranterAddressId);
                //sdtouser.Contacts = db.Contacts.Find(sdtouser.ContactId);
                //sdtouser.PermanentContacts = db.Contacts.Find(sdtouser.PermanentContactId);
                //sdtouser.GuaranterContacts = db.Contacts.Find(sdtouser.GuaranterContactId);
                //db.User.Attach(user);

                sdtouser.ModifiedBy = CurrentUserSession.UserId;
                sdtouser.ModifiedOn = DateTime.Now;
                db.Entry(sdtouser).State = EntityState.Modified;
                db.Entry(sdtouser.PermanentAddress).State = EntityState.Modified;
                db.Entry(sdtouser.UserAddress).State = EntityState.Modified;
                db.Entry(sdtouser.GuaranterAddress).State = EntityState.Modified;
                db.Entry(sdtouser.Contacts).State = EntityState.Modified;
                db.Entry(sdtouser.GuaranterContacts).State = EntityState.Modified;
                db.Entry(sdtouser.PermanentContacts).State = EntityState.Modified;
                db.SaveChanges();

                if (ProfileImage != null)
                {
                    FileUpload(sdtouser.UserID, ProfileImage);

                    System.IO.FileInfo fInfo = new FileInfo(ViewBag.UserProfileAvatar);
                    fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, sdtouser.UserID + ".logo"), true);
                    fInfo.Delete();
                }

                SetDisplayMessage("Member is saved successfully");
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.UserAddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.UserContactId);
            ViewBag.GuaranterAddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.GuaranterAddressId);
            ViewBag.GuaranterContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.GuaranterContactId);
            ViewBag.PermanentAddressId = new SelectList(db.Address, "AddressId", "Address1", sdtouser.PermanentAddressId);
            ViewBag.PermanentContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtouser.PermanentContactId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.UserAddress.CountryId);
            ViewBag.GuaranterAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.GuaranterAddress.CountryId);
            ViewBag.PermanentAddressCountryList = new SelectList(countries, "CountryId", "CountryName", sdtouser.PermanentAddress.CountryId);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
            return View(sdtouser);
        }

        // GET: /Member/Delete/5
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

        // POST: /Member/Delete/5
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
