using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Data.Models.Accounts;
using LoanManagementSystem.Models;
using System.Configuration;

namespace LoanManagementSystem.Controllers.Settings
{
    [Authorize()]
    public class SettingsController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        public ActionResult CountryList()
        {
            return View();
        }

        public ActionResult SaveCountry(long? CountryId = 0)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            sdtoCountry country = db.Countries.Find(CountryId);
            return View(country);
        }

        [HttpPost]
        public ActionResult SaveCountry([Bind(Include = "CountryName, CountryAbbr")] sdtoCountry country)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            if (ModelState.IsValid)
            {
                if (db.Countries.Count(x => x.CountryName.Equals(country.CountryName.Trim(), StringComparison.InvariantCultureIgnoreCase) && x.CountryId != country.CountryId) == 0)
                {
                    if (country.CountryId == 0)
                    {
                        db.Countries.Add(new sdtoCountry() { CountryName = country.CountryName, CountryAbbr = country.CountryAbbr, CreatedBy = userDetails.UserID, CreatedOn = DateTime.Now });
                    }
                    else
                    {
                        country.ModifiedBy = userDetails.UserID;
                        country.ModifiedOn = DateTime.Now;
                        db.Entry(country).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                else
                    ModelState.AddModelError("CountryName", "Country already exists!!!");

                return RedirectToAction("CountryList");
            }

            return View(country);
        }

        public ActionResult StateList(long? CountryId)
        {
            ViewBag.CountryList = new SelectList(db.Countries, "CountryId", "CountryName", CountryId == null ? 0 : CountryId.Value);
            ViewBag.CountryId = CountryId == null ? ((SelectList)ViewBag.CountryList).FirstOrDefault().Value : CountryId.Value.ToString();
            return View();
        }

        public ActionResult DistrictList(long? CountryId, long? StateId)
        {
            sdtoState state = new sdtoState() { CountryId = CountryId == null ? 0 : CountryId.Value, StateId = StateId == null ? 0 : StateId.Value };
            ViewBag.StateId = state.StateId;
            ViewBag.CountryId = state.CountryId;
            return View(state);
        }

        public ActionResult TalukList(long? CountryId, long? StateId, long? DistrictId)
        {
            sdtoDistrict district = new sdtoDistrict() { StateDetails = new sdtoState() { CountryId = CountryId == null ? 0 : CountryId.Value, StateId = StateId == null ? 0 : StateId.Value }, StateId = StateId == null ? 0 : StateId.Value, DistrictId = DistrictId == null ? 0 : DistrictId.Value };
            ViewBag.StateId = district.StateId;
            ViewBag.CountryId = district.StateDetails.CountryId;
            return View(district);
        }

        public ActionResult VillageList(long? TalukId)
        {
            sdtoTaluk taluk = db.Taluks.Find(TalukId);
            ViewBag.TalukId = 0;
            ViewBag.DistrictId = 0;
            ViewBag.StateId = 0;
            ViewBag.CountryId = 0;
            if (taluk != null)
            {
                var district = db.Districts.Find(taluk.DistrictId);
                var state = db.States.Find(district.StateId);

                taluk.DistrictDetails = district;
                taluk.DistrictDetails.StateDetails = state;

                ViewBag.TalukId = taluk.TalukId;
                ViewBag.DistrictId = taluk.DistrictId;
                ViewBag.StateId = district.StateId;
                ViewBag.CountryId = state.CountryId;
            }
            else
            {
                taluk = new sdtoTaluk()
                {
                    DistrictDetails = new sdtoDistrict()
                    {
                        StateDetails = new sdtoState()
                    }
                };
            }

            return View(taluk);
        }

        public ActionResult SaveState(long CountryId, long StateId = 0)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            sdtoCountry country = db.Countries.Find(CountryId);
            ViewBag.CountryList = new SelectList(db.Countries, "CountryId", "CountryName", CountryId);
            ViewBag.CountryId = CountryId;
            sdtoState state = db.States.Find(StateId);
            if (state == null)
                state = new sdtoState() { CountryId = country.CountryId, CountryDetails = country };
            else
                state.CountryDetails = country;
            return View(state);
        }

        [HttpPost]
        public ActionResult SaveState(sdtoState state)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            if (ModelState.IsValid)
            {
                if (db.States.Count(x => x.StateName.Equals(state.StateName.Trim(), StringComparison.InvariantCultureIgnoreCase) && x.StateId != state.StateId && x.CountryId == state.CountryId) == 0)
                {
                    if (state.StateId == 0)
                    {
                        db.States.Add(new sdtoState() { CountryId = state.CountryId, StateName = state.StateName, StateAbbr = state.StateAbbr, CreatedBy = userDetails.UserID, CreatedOn = DateTime.Now });
                    }
                    else
                    {
                        state.ModifiedBy = userDetails.UserID;
                        state.ModifiedOn = DateTime.Now;
                        db.Entry(state).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                else
                    ModelState.AddModelError("StateName", "State already exists!!!");

                return RedirectToAction("StateList", new { CountryId = state.CountryId });
            }

            sdtoCountry country = db.Countries.Find(state.CountryId);
            ViewBag.CountryList = new SelectList(db.Countries, "CountryId", "CountryName", state.CountryId);
            ViewBag.CountryId = state.CountryId;
            state.CountryDetails = country;
            return View(state);
        }

        public ActionResult SaveDistrict(long CountryId, long StateId, long DistrictId = 0)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();
            sdtoDistrict district = new sdtoDistrict();

            ViewBag.CountryId = CountryId;
            sdtoState parent = db.States.Find(StateId);
            if (parent != null)
            {
                ViewBag.ParentId = parent.StateId;
                district = db.Districts.Find(DistrictId);
                if (district == null)
                    district = new sdtoDistrict() { StateId = parent.StateId, StateDetails = parent };
                else
                    district.StateDetails = parent;
            }
            return View(district);
        }

        [HttpPost]
        public ActionResult SaveDistrict(sdtoDistrict district)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            if (ModelState.IsValid)
            {
                if (db.Districts.Count(x => x.DistrictName.Equals(district.DistrictName.Trim(), StringComparison.InvariantCultureIgnoreCase) && x.StateId == district.StateId && x.DistrictId != district.DistrictId) == 0)
                {
                    if (district.DistrictId == 0)
                    {
                        db.Districts.Add(new sdtoDistrict() { StateId = district.StateId, DistrictName = district.DistrictName, DistrictAbbr = district.DistrictAbbr, CreatedBy = userDetails.UserID, CreatedOn = DateTime.Now });
                    }
                    else
                    {
                        district.ModifiedBy = userDetails.UserID;
                        district.ModifiedOn = DateTime.Now;
                        db.Entry(district).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    var cn = db.States.Find(district.StateId);
                    return RedirectToAction("DistrictList", new { StateId = district.StateId, CountryId = cn.CountryId });
                }
                else
                    ModelState.AddModelError("DistrictName", "District already exists!!!");
            }

            var state = db.States.Find(district.StateId);
            ViewBag.CountryId = state.CountryId;
            return View(district);
        }

        public ActionResult SaveTaluk(long CountryId, long StateId, long DistrictId, long TalukId = 0)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();
            sdtoTaluk taluk = new sdtoTaluk()
            {
                DistrictId = DistrictId
            };

            ViewBag.CountryId = CountryId;
            ViewBag.StateId = StateId;
            ViewBag.DistrictId = DistrictId;
            sdtoDistrict parent = db.Districts.Find(DistrictId);
            if (parent != null)
            {
                ViewBag.ParentId = parent.DistrictId;
                taluk = db.Taluks.Find(TalukId);
                if (taluk == null)
                    taluk = new sdtoTaluk() { DistrictId = parent.DistrictId, DistrictDetails = parent };
                else
                    taluk.DistrictDetails = parent;
            }
            return View(taluk);
        }

        [HttpPost]
        public ActionResult SaveTaluk(sdtoTaluk taluk)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            if (ModelState.IsValid)
            {
                if (db.Taluks.Count(x => x.TalukName.Equals(taluk.TalukName.Trim(), StringComparison.InvariantCultureIgnoreCase) && x.DistrictId == taluk.DistrictId && x.TalukId != taluk.TalukId) == 0)
                {
                    if (taluk.TalukId == 0)
                    {
                        db.Taluks.Add(new sdtoTaluk() { DistrictId = taluk.DistrictId, TalukName = taluk.TalukName, TalukAbbr = taluk.TalukAbbr, CreatedBy = userDetails.UserID, CreatedOn = DateTime.Now });
                    }
                    else
                    {
                        taluk.ModifiedBy = userDetails.UserID;
                        taluk.ModifiedOn = DateTime.Now;
                        db.Entry(taluk).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    var district = db.Districts.Find(taluk.DistrictId);
                    var state = db.States.Find(district.StateId);
                    return RedirectToAction("TalukList", new { StateId = district.StateId, CountryId = state.CountryId, DistrictId = taluk.DistrictId });
                }
                else
                    ModelState.AddModelError("TalukName", "Taluk already exists!!!");
            }

            var districtst = db.Districts.Find(taluk.DistrictId);
            var statecn = db.States.Find(districtst.StateId);
            ViewBag.CountryId = statecn.CountryId;
            ViewBag.StateId = districtst.StateId;
            ViewBag.DistrictId = taluk.DistrictId;
            return View(taluk);
        }

        public ActionResult SaveVillage(long TalukId, long VillageId = 0)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            sdtoTaluk taluk = db.Taluks.Find(TalukId);
            if (taluk == null)
                taluk = new sdtoTaluk();
            var district = new sdtoDistrict();
            var state = new sdtoState();
            if (taluk != null)
                district = db.Districts.Find(taluk.DistrictId);

            if (district == null)
                district = new sdtoDistrict();

            if (district != null)
                state = db.States.Find(district.StateId);

            if (state == null)
                state = new sdtoState();

            sdtoVillage village = new sdtoVillage()
            {
                TalukId = taluk.TalukId
            };

            ViewBag.CountryId = state.CountryId;
            ViewBag.StateId = district.StateId;
            ViewBag.DistrictId = taluk.DistrictId;
            ViewBag.TalukId = taluk.TalukId;
            sdtoTaluk parent = db.Taluks.Find(taluk.TalukId);
            if (parent != null)
            {
                ViewBag.ParentId = parent.TalukId;
                village = db.Villages.Find(VillageId);
                if (village == null)
                    village = new sdtoVillage() { TalukId = parent.TalukId, TalukDetails = parent };
                else
                    village.TalukDetails = parent;
            }
            return View(village);
        }

        [HttpPost]
        public ActionResult SaveVillage(sdtoVillage village)
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            if (ModelState.IsValid)
            {
                if (db.Villages.Count(x => x.VillageName.Equals(village.VillageName.Trim(), StringComparison.InvariantCultureIgnoreCase) && x.TalukId == village.TalukId && x.VillageId != village.VillageId) == 0)
                {
                    if (village.VillageId == 0)
                    {
                        db.Villages.Add(new sdtoVillage() { TalukId = village.TalukId, VillageName = village.VillageName, VillageAbbr = village.VillageAbbr, CreatedBy = userDetails.UserID, CreatedOn = DateTime.Now });
                    }
                    else
                    {
                        village.ModifiedBy = userDetails.UserID;
                        village.ModifiedOn = DateTime.Now;
                        db.Entry(village).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    return RedirectToAction("VillageList", new { TalukId = village.TalukId });
                }
                else
                    ModelState.AddModelError("VillageName", "Village already exists!!!");
            }

            sdtoTaluk taluk = db.Taluks.Find(village.TalukId);
            var district = db.Districts.Find(taluk.DistrictId);
            var state = db.States.Find(district.StateId);
            ViewBag.CountryId = state.CountryId;
            ViewBag.StateId = district.StateId;
            ViewBag.DistrictId = taluk.DistrictId;
            ViewBag.TalukId = taluk.TalukId;
            return View(village);
        }

        // GET: Settings
        public ActionResult Index()
        {
            var list = db.GeneralSettings.Include(s => s.Company).Include(y => y.CreatedByUser);
            return View(list.ToList());
        }

        // GET: Settings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            sdtoUser userDetails = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (userDetails == null)
                userDetails = new sdtoUser();

            var items = new SelectList(db.Companies, "CompanyId", "CompanyName");
            ViewBag.CompanyList = items;
            if (items == null || items.Count() == 0)
            {
                TempData["ERR_PREMATURE_MESSAGE"] = "Please register a company for configuring settings";
                return RedirectToAction("Create", "Company");
            }

            var settings = db.GeneralSettings.Where(x => x.CompanyId == userDetails.CompanyId);
            if (settings != null && settings.Count() > 0)
                return EditOnCreate(settings.FirstOrDefault(x => x.CompanyId == userDetails.CompanyId) as sdtoSettings);
            else
                return View(new sdtoSettings() { CompanyId = userDetails.CompanyId, BankCharges = 0, BankInterest = 0 });
        }

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "SettingsId,CompanyId,BankInterest,BankCharges")] 
            sdtoSettings Settings)
        {
            if (ModelState.IsValid)
            {
                Settings.CreatedOn = DateTime.Now;

                db.GeneralSettings.Add(Settings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Settings);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            ViewBag.CompanyList = new SelectList(db.Companies, "CompanyId", "CompanyName");

            var assetSchedule = db.Schedules.Where(x => x.ShortName == "AS").FirstOrDefault();
            var SelectListAsset = db.Schedules.Where(x => x.BaseScheduleId == assetSchedule.ScheduleId || x.ScheduleId == assetSchedule.ScheduleId).ToList();
            ViewBag.AssetScheduleList = new SelectList(SelectListAsset, "ScheduleId", "ScheduleName", 0);

            var liabilitySchedule = db.Schedules.Where(x => x.ShortName == "LY").FirstOrDefault();
            var SelectListLiability = db.Schedules.Where(x => x.BaseScheduleId == liabilitySchedule.ScheduleId || x.ScheduleId == liabilitySchedule.ScheduleId).ToList();
            ViewBag.LiabilityScheduleList = new SelectList(SelectListLiability, "ScheduleId", "ScheduleName", 0);

            var expenditureSchedule = db.Schedules.Where(x => x.ShortName == "EX").FirstOrDefault();
            var SelectListExpenditure = db.Schedules.Where(x => x.BaseScheduleId == expenditureSchedule.ScheduleId || x.ScheduleId == expenditureSchedule.ScheduleId).ToList();
            ViewBag.ExpenditureScheduleList = new SelectList(SelectListExpenditure, "ScheduleId", "ScheduleName", 0);

            var incomeSchedule = db.Schedules.Where(x => x.ShortName == "IC").FirstOrDefault();
            var SelectListIncome = db.Schedules.Where(x => x.BaseScheduleId == incomeSchedule.ScheduleId || x.ScheduleId == incomeSchedule.ScheduleId).ToList();
            ViewBag.IncomeScheduleList = new SelectList(SelectListIncome, "ScheduleId", "ScheduleName", 0);

            var AccountList = db.AccountBooks.Where(x => x.IsDeleted == false).ToList();
            AccountList.Insert(0, new sdtoAccountBook() { AccountBookId = 0, BookName = "Select an Account" });
            ViewBag.AccountList = new SelectList(AccountList, "AccountBookId", "BookName", 0);

            var scheduleList = db.Schedules.ToList();
            scheduleList.Insert(0, new Data.Models.Accounts.Schedules.sdtoSchedule() { ScheduleId = 0, ScheduleName = "Select a Schedule" });
            ViewBag.ScheduleList = new SelectList(scheduleList, "ScheduleId", "ScheduleName", 0);

            var bookList = db.AccountBooks.ToList();
            bookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.BookList = new SelectList(bookList, "AccountBookId", "BookName", 0);

            var cashBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("CASH", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var cashBookList = db.AccountBooks.Where(x => x.AccountBookTypeId == cashBookType.AccountBookTypeId).ToList();
            cashBookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.CashBookList = new SelectList(cashBookList, "AccountBookId", "BookName", 0);

            var bankBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("BANK", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var bankBookList = db.AccountBooks.Where(x => x.AccountBookTypeId == bankBookType.AccountBookTypeId).ToList();
            bankBookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.BankBookList = new SelectList(bankBookList, "AccountBookId", "BookName", 0);

            var journalBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("journal", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var journalBookList = db.AccountBooks.Where(x => x.AccountBookTypeId == journalBookType.AccountBookTypeId).ToList();
            journalBookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.JournalBookList = new SelectList(journalBookList, "AccountBookId", "BookName", 0);

            var otherAccType = db.AccountTypes.Where(x => x.UniqueName.Equals("Other", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var otherAccountHeads = db.AccountHeads.Where(x => x.AccountTypeId == otherAccType.AccountTypeId).ToList();
            otherAccountHeads.Insert(0, new Data.Models.Accounts.sdtoAccountHead { AccountHeadId = 0, AccountName = "Select a Acount" });
            ViewBag.OtherAccountHeads = new SelectList(otherAccountHeads, "AccountHeadId", "AccountName", 0);

            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // GET: Settings/Edit/5
        public ActionResult EditOnCreate(sdtoSettings sdtoSettings)
        {
            if (sdtoSettings == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(//[Bind(Include = "SettingsId,CompanyId,BankInterest,BankCharges,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] 
            sdtoSettings Settings)
        {
            if (ModelState.IsValid)
            {
                Settings.CreatedOn = DateTime.Now;
                Settings.ModifiedOn = DateTime.Now;
                if (Settings.InterestAccountId == 0)
                    Settings.InterestAccountId = null;

                db.Entry(Settings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CompanyList = new SelectList(db.Companies, "CompanyId", "CompanyName");

            var assetSchedule = db.Schedules.Where(x => x.ShortName == "AS").FirstOrDefault();
            var SelectListAsset = db.Schedules.Where(x => x.BaseScheduleId == assetSchedule.ScheduleId || x.ScheduleId == assetSchedule.ScheduleId).ToList();
            ViewBag.AssetScheduleList = new SelectList(SelectListAsset, "ScheduleId", "ScheduleName", 0);

            var liabilitySchedule = db.Schedules.Where(x => x.ShortName == "LY").FirstOrDefault();
            var SelectListLiability = db.Schedules.Where(x => x.BaseScheduleId == liabilitySchedule.ScheduleId || x.ScheduleId == liabilitySchedule.ScheduleId).ToList();
            ViewBag.LiabilityScheduleList = new SelectList(SelectListLiability, "ScheduleId", "ScheduleName", 0);

            var expenditureSchedule = db.Schedules.Where(x => x.ShortName == "EX").FirstOrDefault();
            var SelectListExpenditure = db.Schedules.Where(x => x.BaseScheduleId == expenditureSchedule.ScheduleId || x.ScheduleId == expenditureSchedule.ScheduleId).ToList();
            ViewBag.ExpenditureScheduleList = new SelectList(SelectListExpenditure, "ScheduleId", "ScheduleName", 0);

            var incomeSchedule = db.Schedules.Where(x => x.ShortName == "IC").FirstOrDefault();
            var SelectListIncome = db.Schedules.Where(x => x.BaseScheduleId == incomeSchedule.ScheduleId || x.ScheduleId == incomeSchedule.ScheduleId).ToList();
            ViewBag.IncomeScheduleList = new SelectList(SelectListIncome, "ScheduleId", "ScheduleName", 0);

            var AccountList = db.AccountBooks.Where(x => x.IsDeleted == false).ToList();
            AccountList.Insert(0, new sdtoAccountBook() { AccountBookId = 0, BookName = "Select an Account" });
            ViewBag.AccountList = new SelectList(AccountList, "AccountBookId", "BookName", 0);

            var scheduleList = db.Schedules.ToList();
            scheduleList.Insert(0, new Data.Models.Accounts.Schedules.sdtoSchedule() { ScheduleId = 0, ScheduleName = "Select a Schedule" });
            ViewBag.ScheduleList = new SelectList(scheduleList, "ScheduleId", "ScheduleName", 0);

            var bookList = db.AccountBooks.ToList();
            bookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.BookList = new SelectList(bookList, "AccountBookId", "BookName", 0);

            var cashBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("CASH", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var cashBookList = db.AccountBooks.Where(x => x.AccountBookTypeId == cashBookType.AccountBookTypeId).ToList();
            cashBookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.CashBookList = new SelectList(cashBookList, "AccountBookId", "BookName", 0);

            var bankBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("BANK", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var bankBookList = db.AccountBooks.Where(x => x.AccountBookTypeId == bankBookType.AccountBookTypeId).ToList();
            bankBookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.BankBookList = new SelectList(bankBookList, "AccountBookId", "BookName", 0);

            var journalBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("journal", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var journalBookList = db.AccountBooks.Where(x => x.AccountBookTypeId == journalBookType.AccountBookTypeId).ToList();
            journalBookList.Insert(0, new Data.Models.Accounts.sdtoAccountBook { AccountBookId = 0, BookName = "Select a Book" });
            ViewBag.JournalBookList = new SelectList(journalBookList, "AccountBookId", "BookName", 0);

            var otherAccType = db.AccountTypes.Where(x => x.UniqueName.Equals("Other", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var otherAccountHeads = db.AccountHeads.Where(x => x.AccountTypeId == otherAccType.AccountTypeId).ToList();
            otherAccountHeads.Insert(0, new Data.Models.Accounts.sdtoAccountHead { AccountHeadId = 0, AccountName = "Select a Acount" });
            ViewBag.OtherAccountHeads = new SelectList(otherAccountHeads, "AccountHeadId", "AccountName", 0);

            return View(Settings);
        }

        // GET: Settings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            if (sdtoSettings == null)
            {
                return HttpNotFound();
            }
            return View(sdtoSettings);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoSettings sdtoSettings = db.GeneralSettings.Find(id);
            db.GeneralSettings.Remove(sdtoSettings);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult CountryInfo()
        {
            var list = db.Countries.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StateInfo(long CountryId)
        {
            //var list = db.States.Include(x => x.CountryId).Where(x => x.CountryId == CountryId).ToList();

            var list = db.Set<sdtoState>().Include("CountryDetails").Where(x => x.CountryId == CountryId).ToList().Select(x => new { x.CountryDetails.CountryName, x.CountryDetails.CountryAbbr, x.StateId, x.CountryId, x.StateName, x.StateAbbr });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DistrictInfo(long StateId)
        {
            //var list = db.States.Include(x => x.CountryId).Where(x => x.CountryId == CountryId).ToList();

            var list = db.Set<sdtoDistrict>().Include("StateDetails").Where(x => x.StateId == StateId).ToList().Select(x => new { x.StateDetails.StateName, x.StateDetails.StateAbbr, x.StateId, x.DistrictId, x.DistrictName, x.DistrictAbbr });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TalukInfo(long DistrictId)
        {
            //var list = db.States.Include(x => x.CountryId).Where(x => x.CountryId == CountryId).ToList();

            var list = db.Set<sdtoTaluk>().Include("DistrictDetails").Where(x => x.DistrictId == DistrictId).ToList().Select(x => new { x.DistrictDetails.DistrictName, x.DistrictDetails.DistrictAbbr, x.DistrictId, x.TalukId, x.TalukName, x.TalukAbbr });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VillageInfo(long TalukId)
        {
            //var list = db.States.Include(x => x.CountryId).Where(x => x.CountryId == CountryId).ToList();

            var list = db.Set<sdtoVillage>().Include("TalukDetails").Where(x => x.TalukId == TalukId).ToList().Select(x => new { x.TalukDetails.TalukName, x.TalukDetails.TalukAbbr, x.TalukId, x.VillageId, x.VillageName, x.VillageAbbr });
            return Json(list, JsonRequestBehavior.AllowGet);
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
