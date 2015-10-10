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
using System.Web.Security;
using System.IO;

namespace LoanManagementSystem.Controllers
{
    //[Authorize()]
    public class UserController : ControllerBase
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

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            sdtoUser user = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (user != null && user.UserSession != null)
            {
                user.UserSession.EndTime = DateTime.Now;
                db.Entry<sdtoUserSession>(user.UserSession).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Login", "Login");
        }

        // GET: /User/
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.UserGroup).Where(s => s.UserType == UserType.User && s.IsDeleted == false);
            return View(user);
        }

        public JsonResult UserInfo()
        {
            var dbResult = db.User.Include(u => u.UserGroup).Where(s => s.UserType == UserType.User && s.IsDeleted == false).ToList();
            var Users = (from users in dbResult
                         select new
                         {
                             users.UserID,
                             users.FirstName,
                             users.LastName,
                             users.UserName,
                             users.Code,
                             UserInfo = users.UserID,
                             UserGroup = users.UserGroup != null ? users.UserGroup.Name : string.Empty
                         });
            return Json(Users, JsonRequestBehavior.AllowGet);
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
            sdtoUser user = new sdtoUser() { CompanyId = 1 };
            ViewBag.UserGroupList = new SelectList(db.Usergroup, "UserGroupId", "Code");
            return View(user);
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sdtoUser user, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                var validImageTypes = new string[]
                                        {
                                            "image/gif",
                                            "image/jpeg",
                                            "image/pjpeg",
                                            "image/png"
                                        };

                //if (ProfileImage == null || ProfileImage.ContentLength == 0)
                //{
                //    ModelState.AddModelError("ProfileImage", "This field is required");
                //}
                //else
                if (string.IsNullOrWhiteSpace(user.UserName))
                {
                    ModelState.AddModelError("UserName", "Please enter username");
                }
                else if (string.IsNullOrWhiteSpace(user.Password))
                {
                    ModelState.AddModelError("Password", "Please enter password");
                }
                else if (string.IsNullOrWhiteSpace(user.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "Please enter confirm password");
                }
                else if (!user.ConfirmPassword.Equals(user.Password))
                {
                    ModelState.AddModelError("ConfirmPassword", "Confirm password entered is mismatching");
                }
                else if (ProfileImage != null && !validImageTypes.Contains(ProfileImage.ContentType))
                {
                    ModelState.AddModelError("ProfileImage", "Please choose either a GIF, JPG or PNG image");
                }
                else if (user != null && db.User.Count(x => x.UserName.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase)) > 0)
                {
                    ModelState.AddModelError("UserName", string.Format("User name {0} is unavailable. Please select a different user name.", user.UserName));
                }
                else
                {
                    user.UserType = UserType.User;
                    user.CreatedOn = DateTime.Now;
                    if (CurrentUserSession != null && CurrentUserSession.UserId > 0)
                        user.CreatedBy = CurrentUserSession.UserId;
                    if (user.UserAddress != null)
                        user.UserAddress = db.Address.Add(user.UserAddress);
                    if (user.Contacts != null)
                        user.Contacts = db.Contacts.Add(user.Contacts);
                    user.CreatedOn = DateTime.Now;
                    user.IsActive = true;

                    user.GuaranterAddressId = null;
                    user.PermanentAddressId = null;
                    user.UserAddressId = null;
                    user.UserAddress = null;
                    user.PermanentAddress = null;
                    user.GuaranterAddress = null;

                    if (user.UserAddress != null)
                    {
                        user.UserAddress.CountryId = user.UserAddress.CountryId == 0 ? null : user.UserAddress.CountryId;
                        user.UserAddress.StateId = user.UserAddress.StateId == 0 ? null : user.UserAddress.StateId;
                        user.UserAddress.DistrictId = user.UserAddress.DistrictId == 0 ? null : user.UserAddress.DistrictId;
                        user.UserAddress.TalukId = user.UserAddress.TalukId == 0 ? null : user.UserAddress.TalukId;
                        user.UserAddress.VillageId = user.UserAddress.VillageId == 0 ? null : user.UserAddress.VillageId;
                    }

                    if (user.PermanentAddress != null)
                    {
                        user.PermanentAddress.CountryId = user.PermanentAddress.CountryId == 0 ? null : user.PermanentAddress.CountryId;
                        user.PermanentAddress.StateId = user.PermanentAddress.StateId == 0 ? null : user.PermanentAddress.StateId;
                        user.PermanentAddress.DistrictId = user.PermanentAddress.DistrictId == 0 ? null : user.PermanentAddress.DistrictId;
                        user.PermanentAddress.TalukId = user.PermanentAddress.TalukId == 0 ? null : user.PermanentAddress.TalukId;
                        user.PermanentAddress.VillageId = user.PermanentAddress.VillageId == 0 ? null : user.PermanentAddress.VillageId;
                    }

                    if (user.GuaranterAddress != null)
                    {
                        user.GuaranterAddress.CountryId = user.GuaranterAddress.CountryId == 0 ? null : user.GuaranterAddress.CountryId;
                        user.GuaranterAddress.StateId = user.GuaranterAddress.StateId == 0 ? null : user.GuaranterAddress.StateId;
                        user.GuaranterAddress.DistrictId = user.GuaranterAddress.DistrictId == 0 ? null : user.GuaranterAddress.DistrictId;
                        user.GuaranterAddress.TalukId = user.GuaranterAddress.TalukId == 0 ? null : user.GuaranterAddress.TalukId;
                        user.GuaranterAddress.VillageId = user.GuaranterAddress.VillageId == 0 ? null : user.GuaranterAddress.VillageId;
                    }

                    db.User.Add(user);
                    db.SaveChanges();
                    if (ProfileImage != null)
                    {
                        FileUpload(user.UserID, ProfileImage);

                        System.IO.FileInfo fInfo = new FileInfo(ViewBag.UserProfileAvatar);
                        fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, user.UserID + ".logo"), true);
                        fInfo.Delete();
                    }
                    else
                    {
                        DirectoryInfo dirUser = new DirectoryInfo(HttpContext.Server.MapPath("~/").Trim("\\/ ".ToCharArray()) + "\\ContentUpload\\User\\Profile");
                        if (!dirUser.Exists)
                            dirUser.Create();

                        FileInfo objFile = new FileInfo(Path.Combine(HttpContext.Server.MapPath("~/").Trim("\\/ ".ToCharArray()) + "\\Content\\Images", "dummy-profile.png"));
                        objFile.CopyTo(Path.Combine(dirUser.FullName, user.UserID + ".logo"), true);
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        SetDisplayMessage("User is created successfully");
                        return RedirectToAction("Index");
                    }
                    else
                        return RedirectToAction("Login", "Login");
                }
            }

            ViewBag.UserGroupList = new SelectList(db.Usergroup, "UserGroupId", "Code", user.UserGroupId);
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
            user.ConfirmPassword = user.Password;
            ViewBag.UserGroupList = new SelectList(db.Usergroup, "UserGroupId", "Code", user.UserGroupId);
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sdtoUser user, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                user.UserType = UserType.User;
                user.ModifiedBy = CurrentUserSession.UserId;
                user.ModifiedOn = DateTime.Now;

                user.GuaranterAddressId = null;
                user.PermanentAddressId = null;
                user.UserAddressId = null;
                user.UserAddress = null;
                user.PermanentAddress = null;
                user.GuaranterAddress = null;

                if (user.UserAddress != null)
                {
                    user.UserAddress.CountryId = user.UserAddress.CountryId == 0 ? null : user.UserAddress.CountryId;
                    user.UserAddress.StateId = user.UserAddress.StateId == 0 ? null : user.UserAddress.StateId;
                    user.UserAddress.DistrictId = user.UserAddress.DistrictId == 0 ? null : user.UserAddress.DistrictId;
                    user.UserAddress.TalukId = user.UserAddress.TalukId == 0 ? null : user.UserAddress.TalukId;
                    user.UserAddress.VillageId = user.UserAddress.VillageId == 0 ? null : user.UserAddress.VillageId;
                }

                if (user.PermanentAddress != null)
                {
                    user.PermanentAddress.CountryId = user.PermanentAddress.CountryId == 0 ? null : user.PermanentAddress.CountryId;
                    user.PermanentAddress.StateId = user.PermanentAddress.StateId == 0 ? null : user.PermanentAddress.StateId;
                    user.PermanentAddress.DistrictId = user.PermanentAddress.DistrictId == 0 ? null : user.PermanentAddress.DistrictId;
                    user.PermanentAddress.TalukId = user.PermanentAddress.TalukId == 0 ? null : user.PermanentAddress.TalukId;
                    user.PermanentAddress.VillageId = user.PermanentAddress.VillageId == 0 ? null : user.PermanentAddress.VillageId;
                }

                if (user.GuaranterAddress != null)
                {
                    user.GuaranterAddress.CountryId = user.GuaranterAddress.CountryId == 0 ? null : user.GuaranterAddress.CountryId;
                    user.GuaranterAddress.StateId = user.GuaranterAddress.StateId == 0 ? null : user.GuaranterAddress.StateId;
                    user.GuaranterAddress.DistrictId = user.GuaranterAddress.DistrictId == 0 ? null : user.GuaranterAddress.DistrictId;
                    user.GuaranterAddress.TalukId = user.GuaranterAddress.TalukId == 0 ? null : user.GuaranterAddress.TalukId;
                    user.GuaranterAddress.VillageId = user.GuaranterAddress.VillageId == 0 ? null : user.GuaranterAddress.VillageId;
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                if (ProfileImage != null)
                {
                    FileUpload(user.UserID, ProfileImage);

                    System.IO.FileInfo fInfo = new FileInfo(ViewBag.UserProfileAvatar);
                    fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, user.UserID + ".logo"), true);
                    fInfo.Delete();
                }

                SetDisplayMessage("User is saved successfully");
                return RedirectToAction("Index");
            }
            ViewBag.UserGroupList = new SelectList(db.Usergroup, "UserGroupId", "Code", user.UserGroupId);
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
            user.ConfirmPassword = user.Password;
            user.IsDeleted = true;
            user.DeletedBy = CurrentUserSession.UserId;
            user.DeletedOn = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;

            db.SaveChanges();
            SetDisplayMessage("User is deleted successfully");
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
