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
    public class UserController : Controller
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
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(sdtoUser u)
        {
            FormsAuthentication.SignOut();
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                using (LoanDBContext dc = new LoanDBContext())
                {
                    var v = dc.User.Where(a => a.UserName.Equals(u.UserName) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        FormsAuthentication.SetAuthCookie(u.UserName, false);
                        Session["UserDetails"] = v;
                        //Session["LogedUserFullname"] = v.FirstName.ToString() + " " + v.LastName.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

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
            ViewBag.UserGroupList = new SelectList(db.Usergroup, "UserGroupId", "Code");
            return View();
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

                if (ProfileImage == null || ProfileImage.ContentLength == 0)
                {
                    ModelState.AddModelError("ProfileImage", "This field is required");
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
                        return RedirectToAction("Index");
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
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                if (ProfileImage != null)
                {
                    FileUpload(user.UserID, ProfileImage);

                    System.IO.FileInfo fInfo = new FileInfo(ViewBag.UserProfileAvatar);
                    fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, user.UserID + ".logo"), true);
                    fInfo.Delete();
                }

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
