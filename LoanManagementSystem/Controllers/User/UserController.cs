﻿using System;
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

        public ActionResult Login()
        {
            FormsAuthentication.SignOut();

            var accFY = db.FinancialPeriod.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.FinancialPeriodId.ToString(), Text = x.PeriodName }).ToList();
            accFY.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Period Name" });
            ViewBag.FinancialYears = new SelectList(accFY, "Value", "Text", 0);

            return View("Login");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(sdtoUser u)
        {
            FormsAuthentication.SignOut();
            // this action is for handle post (login)
            //if (ModelState.IsValid) // this is check validity
            {
                if ((u.FinancialYearId != null && u.FinancialYearId > 0))
                {
                    using (LoanDBContext dc = new LoanDBContext())
                    {
                        var v = dc.User.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                        if (v != null)
                        {
                            var userSession = new sdtoUserSession() { UserId = v.UserID, CompanyId = v.CompanyId, StartTime = DateTime.Now, FinancialYearId = u.FinancialYearId, Browser = UtilityHelper.UtilityHelper.GetBrowser(), SessionKey = System.Guid.NewGuid().ToString("N"), IPAddress = Request.ServerVariables["REMOTE_ADDR"] };
                            db.UserSessions.Add(userSession);
                            db.SaveChanges();
                            v.UserSession = userSession;
                            FormsAuthentication.SetAuthCookie(u.UserName, false);
                            UtilityHelper.UserSession.SetSession(UtilityHelper.UserSession.LoggedInUser, v);
                            //Session["LogedUserFullname"] = v.FirstName.ToString() + " " + v.LastName.ToString();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                            ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                else
                    ModelState.AddModelError("", "Please select a financial period.");

            }
            var accFY = db.FinancialPeriod.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.FinancialPeriodId.ToString(), Text = x.PeriodName }).ToList();
            accFY.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Period Name" });
            ViewBag.FinancialYears = new SelectList(accFY, "Value", "Text", 0);
            return View();
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

            return RedirectToAction("Login");
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
                if (ProfileImage != null && !validImageTypes.Contains(ProfileImage.ContentType))
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
                    db.User.Add(user);
                    db.SaveChanges();
                    if (ProfileImage != null)
                    {
                        FileUpload(user.UserID, ProfileImage);

                        System.IO.FileInfo fInfo = new FileInfo(ViewBag.UserProfileAvatar);
                        fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, user.UserID + ".logo"), true);
                        fInfo.Delete();
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        SetDisplayMessage("User is created successfully");
                        return RedirectToAction("Index");
                    }
                    else
                        return RedirectToAction("Login");
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
