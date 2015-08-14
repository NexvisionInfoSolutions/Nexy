using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Models;
using System.IO;

namespace LoanManagementSystem.Controllers
{
    public class CompanyController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        private void FileUpload(long CompanyId, HttpPostedFileBase uploadFile)
        {
            if (uploadFile.ContentLength > 0)
            {
                DirectoryInfo dirComp = new DirectoryInfo(HttpContext.Server.MapPath("~/").Trim("\\/ ".ToCharArray()) + "\\ContentUpload\\Company");
                if (!dirComp.Exists)
                    dirComp.Create();

                FileInfo objFile = new FileInfo(Path.Combine(dirComp.FullName, System.Guid.NewGuid() + ".logo"));
                ViewBag.CompanyLogoFile = objFile.FullName;

                if (objFile.Exists)
                    objFile.Delete();

                uploadFile.SaveAs(objFile.FullName);
            }
        }

        // GET: /Company/
        public ActionResult Index()
        {
            var companies = db.Companies.Include(s => s.Address).Include(s => s.Contacts);
            return View(companies.ToList());
        }

        // GET: /Company/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoCompany sdtocompany = db.Companies.Find(id);
            if (sdtocompany == null)
            {
                return HttpNotFound();
            }
            return View(sdtocompany);
        }

        // GET: /Company/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1");
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName");
            return View();
        }

        // POST: /Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(sdtoCompany objcompany, HttpPostedFileBase Logo)//[Bind(Include="CompanyId,Code,Name,AddressId,Owner,TIN,ContactId,IsDeleted,WebUrl,LogoUrl,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")]
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

                if (Logo == null || Logo.ContentLength == 0)
                {
                    ModelState.AddModelError("Logo", "This field is required");
                }
                else if (Logo != null && !validImageTypes.Contains(Logo.ContentType))
                {
                    ModelState.AddModelError("Logo", "Please choose either a GIF, JPG or PNG image");
                }
                else
                {
                    objcompany.CreatedOn = DateTime.Now;
                    objcompany.ModifiedOn = DateTime.Now;

                    if (objcompany.Address != null)
                    {
                        objcompany.Address.CreatedOn = objcompany.CreatedOn;
                    }

                    if (objcompany.Contacts != null)
                    {
                        objcompany.Contacts.CreatedOn = objcompany.CreatedOn;
                    }

                    objcompany.Contacts = db.Contacts.Add(objcompany.Contacts);
                    objcompany.Address = db.Address.Add(objcompany.Address);

                    objcompany = db.Companies.Add(objcompany);
                    db.SaveChanges();
                    if (Logo != null)
                    {
                        FileUpload(objcompany.CompanyId, Logo);

                        System.IO.FileInfo fInfo = new FileInfo(ViewBag.CompanyLogoFile);
                        fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, objcompany.CompanyId + ".logo"), true);
                        fInfo.Delete();
                    }
                    // objcompany.LogoUrl = ViewBag.CompanyLogoFile
                    return RedirectToAction("Index");
                }
            }

            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", objcompany.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", objcompany.ContactId);
            return View(objcompany);
        }

        // GET: /Company/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoCompany sdtocompany = db.Companies.Include(s => s.Address).Include(s => s.Contacts).Where(x => x.CompanyId == id).FirstOrDefault();
            if (sdtocompany == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtocompany.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtocompany.ContactId);
            return View(sdtocompany);
        }

        // POST: /Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(sdtoCompany objcompany, HttpPostedFileBase Logo)//[Bind(Include="CompanyId,Code,Name,AddressId,Owner,TIN,ContactId,IsDeleted,WebUrl,LogoUrl,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")]
        {
            if (ModelState.IsValid)
            {
                objcompany.ModifiedOn = DateTime.Now;

                if (objcompany.Address != null)
                {
                    objcompany.Address.AddressId = objcompany.AddressId;
                    objcompany.Address.ModifiedOn = DateTime.Now;
                    objcompany.Address.ModifiedBy = objcompany.ModifiedBy;
                }

                if (objcompany.Contacts != null)
                {
                    objcompany.Contacts.ContactId = objcompany.ContactId;
                    objcompany.Contacts.ModifiedOn = DateTime.Now;
                    objcompany.Contacts.ModifiedBy = objcompany.ModifiedBy;
                }

                if (Logo != null)
                {
                    FileUpload(objcompany.CompanyId, Logo);

                    System.IO.FileInfo fInfo = new FileInfo(ViewBag.CompanyLogoFile);
                    fInfo.CopyTo(Path.Combine(fInfo.Directory.FullName, objcompany.CompanyId + ".logo"), true);
                    fInfo.Delete();

                    objcompany.LogoUrl = "ContentUpload/Company/" + objcompany.CompanyId + ".logo";
                }

                db.Contacts.Attach(objcompany.Contacts);
                db.Address.Attach(objcompany.Address);
                db.Companies.Attach(objcompany);

                db.Entry<sdtoContact>(objcompany.Contacts).State = EntityState.Modified;
                db.Entry<sdtoAddress>(objcompany.Address).State = EntityState.Modified;
                db.Entry<sdtoCompany>(objcompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", objcompany.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", objcompany.ContactId);
            return View(objcompany);
        }

        // GET: /Company/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoCompany sdtocompany = db.Companies.Find(id);
            if (sdtocompany == null)
            {
                return HttpNotFound();
            }
            return View(sdtocompany);
        }

        // POST: /Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoCompany sdtocompany = db.Companies.Find(id);
            db.Companies.Remove(sdtocompany);
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
