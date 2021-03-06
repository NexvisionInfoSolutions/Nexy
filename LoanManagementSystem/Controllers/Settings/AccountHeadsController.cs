﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Models.Accounts;
using LoanManagementSystem.Models;
using Data.Models.Accounts.Schedules;

namespace LoanManagementSystem.Controllers
{
    [Authorize()]
    public class AccountHeadsController : ControllerBase
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: AccountHeads
        public ActionResult Index()
        {
            var accountHeads = db.AccountHeads.Include(s => s.AccountType).Include(s => s.Address).Include(s => s.Contacts);
            return View(accountHeads.ToList());
        }
        public JsonResult AccountHeadInfo()
        {
            var dbResult = db.AccountHeads.Include(s => s.AccountType).Include(s => s.Address).Include(s => s.Contacts).Where(x => x.IsDeleted == false).ToList();
            var AccountHeads = (from AccountHead in dbResult
                                select new
                                {
                                    AccountHead.AccountHeadId,
                                    AccountHead.AccountName,
                                    AccountHead.AccountType.AccountType,
                                    AccountHead.CreditDays,
                                    AccountHead.CreditLimit,
                                    AccountHead.AccountCode,

                                    AccountHeadInfo = AccountHead.AccountHeadId
                                });
            return Json(AccountHeads, JsonRequestBehavior.AllowGet);
        }
        // GET: AccountHeads/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Create
        public ActionResult Create()
        {
            var schedules = db.Schedules.ToList();
            schedules.Insert(0, new sdtoSchedule() { ScheduleId = 0, ScheduleName = "Select a Schedule" });
            ViewBag.ScheduleList = new SelectList(schedules, "ScheduleId", "ScheduleName", 0);

            var accountTypes = db.AccountTypes.ToList();
            accountTypes.Insert(0, new sdtoAccountType() { AccountTypeId = 0, AccountType = "Select an Account Type" });
            ViewBag.AccountTypeList = new SelectList(accountTypes, "AccountTypeId", "AccountType", 0);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
                        
            sdtoAccountHead accHead = new sdtoAccountHead()
            {
                Address = new sdtoAddress() { CountryId = 1 }
            };
            return View();
        }

        // POST: AccountHeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "AccountHeadId,AccountCode,AccountName,ScheduleId,AccountTypeId,CreditLimit,CreditDays,TIN,CST,Contacts.ContactName,Contacts.Mobile1,Contacts.Telephone1,Address.Address1")]
            sdtoAccountHead accHead)
        {
            if (ModelState.IsValid)
            {
                long? userCreatedBy = null;
                var userSession = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
                if (userSession != null)
                    userCreatedBy = userSession.UserID;

                accHead.CreatedOn = DateTime.Now;
                accHead.ModifiedOn = null;
                accHead.CreatedBy = userCreatedBy;

                if (accHead.Address != null)
                {
                    accHead.Address.CreatedOn = accHead.CreatedOn;
                }

                if (accHead.Contacts != null)
                {
                    accHead.Contacts.CreatedOn = accHead.CreatedOn;
                }

                if (accHead.Address != null)
                {
                    accHead.Address.CountryId = accHead.Address.CountryId == 0 ? null : accHead.Address.CountryId;
                    accHead.Address.StateId = accHead.Address.StateId == 0 ? null : accHead.Address.StateId;
                    accHead.Address.DistrictId = accHead.Address.DistrictId == 0 ? null : accHead.Address.DistrictId;
                    accHead.Address.TalukId = accHead.Address.TalukId == 0 ? null : accHead.Address.TalukId;
                    accHead.Address.VillageId = accHead.Address.VillageId == 0 ? null : accHead.Address.VillageId;
                }

                accHead.Contacts = db.Contacts.Add(accHead.Contacts);
                accHead.Address = db.Address.Add(accHead.Address);
                db.AccountHeads.Add(accHead);
                db.SaveChanges();

                sdtoOpeningBalance memberOpeniningBalance = new sdtoOpeningBalance()
                {
                    AccountHeadId = accHead.AccountHeadId,
                    ClosingBalance = 0,
                    CreditOpeningBalance = 0,
                    DebitOpeningBalance = 0,
                    FinancialYearId = 1,
                    ScheduleId = accHead.ScheduleId,
                    IsDeleted = false,
                    CreatedBy = userCreatedBy,
                    CreatedOn = DateTime.Now
                };

                db.OpeningBalance.Add(memberOpeniningBalance);
                db.SaveChanges();

                SetDisplayMessage("The account head is created successfully");

                return RedirectToAction("Index");
            }

            var schedules = db.Schedules.ToList();
            schedules.Insert(0, new sdtoSchedule() { ScheduleId = 0, ScheduleName = "Select a Schedule" });
            ViewBag.ScheduleList = new SelectList(schedules, "ScheduleId", "ScheduleName", 0);

            var accountTypes = db.AccountTypes.ToList();
            accountTypes.Insert(0, new sdtoAccountType() { AccountTypeId = 0, AccountType = "Select an Account Type" });
            ViewBag.AccountTypeList = new SelectList(accountTypes, "AccountTypeId", "AccountType", 0);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);

            return View(accHead);
        }

        // GET: AccountHeads/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }

            var schedules = db.Schedules.ToList();
            schedules.Insert(0, new sdtoSchedule() { ScheduleId = 0, ScheduleName = "Select a Schedule" });
            ViewBag.ScheduleList = new SelectList(schedules, "ScheduleId", "ScheduleName", sdtoAccountHead.ScheduleId);

            var accountTypes = db.AccountTypes.ToList();
            accountTypes.Insert(0, new sdtoAccountType() { AccountTypeId = 0, AccountType = "Select an Account Type" });
            ViewBag.AccountTypeList = new SelectList(accountTypes, "AccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);

            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // POST: AccountHeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountHeadId,AccountCode,AccountName,ScheduleId,AccountTypeId,CreditLimit,CreditDays,ContactId,AddressId,TIN,CST,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] sdtoAccountHead sdtoAccountHead)
        {
            if (ModelState.IsValid)
            {
                long? userCreatedBy = null;
                var userSession = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
                if (userSession != null)
                    userCreatedBy = userSession.UserID;

                sdtoAccountHead.ModifiedOn = DateTime.Now;
                sdtoAccountHead.ModifiedBy = userCreatedBy;

                if (sdtoAccountHead.Address != null)
                {
                    sdtoAccountHead.Address.CountryId = sdtoAccountHead.Address.CountryId == 0 ? null : sdtoAccountHead.Address.CountryId;
                    sdtoAccountHead.Address.StateId = sdtoAccountHead.Address.StateId == 0 ? null : sdtoAccountHead.Address.StateId;
                    sdtoAccountHead.Address.DistrictId = sdtoAccountHead.Address.DistrictId == 0 ? null : sdtoAccountHead.Address.DistrictId;
                    sdtoAccountHead.Address.TalukId = sdtoAccountHead.Address.TalukId == 0 ? null : sdtoAccountHead.Address.TalukId;
                    sdtoAccountHead.Address.VillageId = sdtoAccountHead.Address.VillageId == 0 ? null : sdtoAccountHead.Address.VillageId;
                }

                db.Entry(sdtoAccountHead).State = EntityState.Modified;
                db.SaveChanges();

                var accOpeningBalance = db.OpeningBalance.Where(x => x.AccountHeadId == sdtoAccountHead.AccountHeadId).FirstOrDefault();
                if (accOpeningBalance != null && accOpeningBalance.ScheduleId != sdtoAccountHead.ScheduleId)
                {
                    accOpeningBalance.ScheduleId = sdtoAccountHead.ScheduleId;
                    accOpeningBalance.ModifiedBy = userCreatedBy;

                    db.Entry(accOpeningBalance).State = EntityState.Modified;
                    db.SaveChanges();
                }

                SetDisplayMessage("The account head is saved successfully");

                return RedirectToAction("Index");
            }
            var schedules = db.Schedules.ToList();
            schedules.Insert(0, new sdtoSchedule() { ScheduleId = 0, ScheduleName = "Select a Schedule" });
            ViewBag.ScheduleList = new SelectList(schedules, "ScheduleId", "ScheduleName", sdtoAccountHead.ScheduleId);

            var accountTypes = db.AccountTypes.ToList();
            accountTypes.Insert(0, new sdtoAccountType() { AccountTypeId = 0, AccountType = "Select an Account Type" });
            ViewBag.AccountTypeList = new SelectList(accountTypes, "AccountTypeId", "AccountType", sdtoAccountHead.AccountTypeId);
            var countries = db.Countries.ToList();
            countries.Insert(0, new sdtoCountry() { CountryId = 0, CountryName = "Select Country" });
            ViewBag.UserAddressCountryList = new SelectList(countries, "CountryId", "CountryName", 0);
            ViewBag.StateList = new SelectList(db.States, "StateId", "StateName", 0);
            ViewBag.AddressId = new SelectList(db.Address, "AddressId", "Address1", sdtoAccountHead.AddressId);
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "ContactName", sdtoAccountHead.ContactId);
            return View(sdtoAccountHead);
        }

        // GET: AccountHeads/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);
            var createdUser = db.User.Find(sdtoAccountHead.CreatedBy);
            if (createdUser != null)
                sdtoAccountHead.CreatedByUser = createdUser;
            if (sdtoAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(sdtoAccountHead);
        }

        // POST: AccountHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoAccountHead sdtoAccountHead = db.AccountHeads.Find(id);

            if (db.ReceiptDetails.Count(x => x.AccountId == id && x.IsDeleted == false) == 0 && db.BankDepositDetails.Count(x => x.AccountId == id && x.IsDeleted == false) == 0 && db.JournalDetails.Count(x => x.AccountId == id && x.IsDeleted == false) == 0)
            {
                var accountHd = db.AccountHeads.Find(id);
                if (accountHd != null)
                    accountHd.IsDeleted = true;
                db.Entry(accountHd).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                SetDisplayMessage("The account head cannot be deleted!!!");
            }
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
