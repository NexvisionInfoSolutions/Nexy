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
using Data.Models.Accounts;
using LoanManagementSystem.App_Start;

namespace LoanManagementSystem.Controllers
{
    public class AccountingController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // GET: /User/
        public ActionResult OpeningBalances()
        {
            var openingBalance = db.OpeningBalance.Include(x => x.AccountHead).Include(x => x.Schedule).Include(x => x.FinancialPeriod).Where(x => x.IsDeleted == false && x.FinancialPeriod.IsCurrentYear == true);
            return View(openingBalance);
        }

        public JsonResult ListOpeningBalances()
        {
            var openingBalance = db.OpeningBalance.Include(x => x.AccountHead).Include(x => x.Schedule).Include(x => x.FinancialPeriod).Where(x => x.IsDeleted == false && x.FinancialPeriod.IsCurrentYear == true);
            var Balances = (from sop in openingBalance
                            select new
                            {
                                sop.OpeningBalanceId,
                                OpeningBalanceIdInfo = sop.OpeningBalanceId,
                                sop.AccountHead.AccountName,
                                sop.Schedule.ScheduleName,
                                sop.DebitOpeningBalance,
                                sop.CreditOpeningBalance,
                                sop.ClosingBalance
                            });
            return Json(Balances, JsonRequestBehavior.AllowGet);
        }

        private void LoadSelectListOpeningBalance(long AccHeadId, long AccScheduleId, long accFYId)
        {
            LoadAccountHeadList(AccHeadId);

            var accSch = db.Schedules.ToList().Select(x => new SelectListItem() { Value = x.ScheduleId.ToString(), Text = x.ScheduleName }).ToList();
            var accFY = db.FinancialPeriod.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.FinancialPeriodId.ToString(), Text = x.PeriodName }).ToList();

            accSch.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Schedule Name" });
            accFY.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Period Name" });

            ViewBag.AccountSchedules = new SelectList(accSch, "Value", "Text", AccScheduleId);
            ViewBag.FinancialYears = new SelectList(accFY, "Value", "Text", accFYId);
        }

        private void LoadAccountHeadList(long AccHeadId)
        {
            var accHeads = db.AccountHeads.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Account Name" });
            ViewBag.AccountHeads = new SelectList(accHeads, "Value", "Text", AccHeadId);
        }

        private void LoadAccountBookList(long AccBookId)
        {
            var accHeads = db.AccountBooks.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.AccountBookId.ToString(), Text = x.BookName }).ToList();
            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Account Book" });
            ViewBag.AccountBookList = new SelectList(accHeads, "Value", "Text", AccBookId);
        }

        private void LoadJournalBookList(long AccBookId)
        {
            var accHeads = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountBookType.UniqueName.Equals("Journal", StringComparison.InvariantCultureIgnoreCase) && x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.AccountBookId.ToString(), Text = x.BookName }).ToList();
            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Journal Book" });
            ViewBag.AccountBookList = new SelectList(accHeads, "Value", "Text", AccBookId);
        }

        public ActionResult DepositWithdrawal()
        {
            LoadAccountHeadList(0);
            LoadAccountBookList(0);
            sdtoViewAccDepositWithdrawal obj = new sdtoViewAccDepositWithdrawal() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccDepositWithdrawalDetails() { InstrumentDate = DateTime.Now });
            return View(obj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DepositWithdrawal(sdtoViewAccDepositWithdrawal objDepositWithdrawal)
        {
            if (objDepositWithdrawal.SourceClick == 0)
                objDepositWithdrawal.Details.Add(new sdtoViewAccDepositWithdrawalDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    sdtoUser user = Session["UserDetails"] as sdtoUser;
                    var settingCashBookId = db.GeneralSettings.FirstOrDefault().BankBookId;
                    //Post for Bank book
                    var accBankBook = db.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accBankBook != null)
                    {
                        sdtoBankDepositHeader hdrBankDeposit = new sdtoBankDepositHeader()
                        {
                            BookId = objDepositWithdrawal.Book.AccountBookId,
                            Cancelled = 0,
                            CreatedOn = DateTime.Now,
                            CreatedBy = user.UserID,
                            TransDate = objDepositWithdrawal.Date,
                            FinYear = 1,
                            FromModule = 0,
                            IsDeleted = false,
                            VoucherNo = objDepositWithdrawal.Voucher,
                            VoucherTotal = objDepositWithdrawal.VoucherTotal
                        };

                        db.BankDepositHeader.Add(hdrBankDeposit);
                        db.SaveChanges();

                        //hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.BankTransId + accBankBook.ReceiptVoucherSuffix;
                        //db.Entry(hdrBankDeposit).State = EntityState.Modified;
                        //db.SaveChanges();

                        foreach (sdtoViewAccDepositWithdrawalDetails dtl in objDepositWithdrawal.Details)
                        {
                            sdtoBankDepositDetails bankDepositDtlCr = new sdtoBankDepositDetails()
                            {
                                BankDepositId = hdrBankDeposit.Id,
                                AccountId = dtl.AccountHead.AccountHeadId,
                                Narration = dtl.Narration,
                                Amount = -1 * dtl.Amount,
                                CreatedBy = user.UserID,
                                CreatedOn = DateTime.Now,
                                Display = 1,
                                IsDeleted = false,
                                Instrument = dtl.InstrumentType,
                                InstrumentNo = dtl.InstrumentNo,
                                InstrumentDate = dtl.InstrumentDate
                            };
                            db.BankDepositDetails.Add(bankDepositDtlCr);
                        }

                        sdtoBankDepositDetails bankDepositDtlDb = new sdtoBankDepositDetails()
                        {
                            BankDepositId = hdrBankDeposit.Id,
                            AccountId = accBankBook.AccountHeadId,
                            Narration = "Bank deposit/withdrawal",
                            Amount = objDepositWithdrawal.Details.Sum(x => x.Amount),
                            CreatedBy = user.UserID,
                            CreatedOn = DateTime.Now,
                            Display = 1,
                            IsDeleted = false,
                            Instrument = objDepositWithdrawal.Details.FirstOrDefault().InstrumentType,
                            InstrumentNo = objDepositWithdrawal.Details.FirstOrDefault().InstrumentNo,
                            InstrumentDate = objDepositWithdrawal.Details.FirstOrDefault().InstrumentDate
                        };

                        db.BankDepositDetails.Add(bankDepositDtlDb);
                        db.SaveChanges();
                    }
                }
            }
            LoadAccountHeadList(0);
            LoadAccountBookList(objDepositWithdrawal.Book != null ? objDepositWithdrawal.Book.AccountBookId : 0);

            return View(objDepositWithdrawal);
        }

        public ActionResult CashReceiptPayment()
        {
            LoadAccountHeadList(0);
            LoadAccountBookList(0);
            sdtoViewAccDepositWithdrawal obj = new sdtoViewAccDepositWithdrawal() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccDepositWithdrawalDetails() { InstrumentDate = DateTime.Now });
            return View(obj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CashReceiptPayment(sdtoViewAccCashReceiptPayment objCashReceiptPayment)
        {
            if (objCashReceiptPayment.SourceClick == 0)
                objCashReceiptPayment.Details.Add(new sdtoViewAccCashReceiptPaymentDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    sdtoUser user = Session["UserDetails"] as sdtoUser;
                    var settingCashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
                    //Post for Bank book
                    var accBankBook = db.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accBankBook != null)
                    {
                        sdtoReceiptHeader hdrBankDeposit = new sdtoReceiptHeader()
                        {
                            BookId = objCashReceiptPayment.Book.AccountBookId,
                            Cancelled = 0,
                            CreatedOn = DateTime.Now,
                            CreatedBy = user.UserID,
                            TransDate = objCashReceiptPayment.Date,
                            FinYear = 1,
                            FromModule = 0,
                            IsDeleted = false,
                            VoucherNo = objCashReceiptPayment.Voucher,
                            VoucherTotal = objCashReceiptPayment.Details.Sum(x => x.Amount)
                        };

                        db.ReceiptHeader.Add(hdrBankDeposit);
                        db.SaveChanges();

                        foreach (sdtoViewAccCashReceiptPaymentDetails dtl in objCashReceiptPayment.Details)
                        {
                            sdtoReceiptDetails bankDepositDtlCr = new sdtoReceiptDetails()
                            {
                                ReceiptsId = hdrBankDeposit.Id,
                                AccountId = dtl.AccountHead.AccountHeadId,
                                Narration = dtl.Narration,
                                Amount = -1 * dtl.Amount,
                                CreatedBy = user.UserID,
                                CreatedOn = DateTime.Now,
                                Display = 1,
                                IsDeleted = false,
                            };
                            db.ReceiptDetails.Add(bankDepositDtlCr);
                        }

                        sdtoReceiptDetails bankDepositDtlDb = new sdtoReceiptDetails()
                        {
                            ReceiptsId = hdrBankDeposit.Id,
                            AccountId = accBankBook.AccountHeadId,
                            Narration = "Bank deposit/withdrawal",
                            Amount = objCashReceiptPayment.Details.Sum(x => x.Amount),
                            CreatedBy = user.UserID,
                            CreatedOn = DateTime.Now,
                            Display = 1,
                            IsDeleted = false
                        };

                        db.ReceiptDetails.Add(bankDepositDtlDb);
                        db.SaveChanges();
                    }
                }
            }
            LoadAccountHeadList(0);
            LoadAccountBookList(objCashReceiptPayment.Book != null ? objCashReceiptPayment.Book.AccountBookId : 0);

            return View(objCashReceiptPayment);
        }

        public ActionResult ExpenseEntry()
        {
            LoadAccountHeadList(0);
            //LoadAccountBookList(0);

            sdtoViewAccExpenseEntry obj = new sdtoViewAccExpenseEntry() { Date = DateTime.Now };
            var CashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
            var cashBook = db.AccountBooks.Include(x => x.AccountHeadId).Where(x => x.AccountBookId == CashBookId).FirstOrDefault();
            if (cashBook != null)
            {
                obj.Book = cashBook;
            }
            return View(obj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ExpenseEntry(sdtoViewAccExpenseEntry objExpenseEntry)
        {
            if (objExpenseEntry.SourceClick == 0)
                objExpenseEntry.Details.Add(new sdtoViewAccExpenseEntryDetails() { AccountHead = new sdtoAccountHead() { AccountHeadId = objExpenseEntry.Book.AccountHeadId }, Amount = objExpenseEntry.Amount, Narration = objExpenseEntry.Description });
            else
            {
                if (ModelState.IsValid)
                {
                    sdtoUser user = Session["UserDetails"] as sdtoUser;
                    var settingCashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
                    //Post for Bank book
                    var accBankBook = db.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accBankBook != null)
                    {
                        sdtoReceiptHeader hdrCashReceiptPayment = new sdtoReceiptHeader()
                        {
                            BookId = objExpenseEntry.Book.AccountBookId,
                            Cancelled = 0,
                            CreatedOn = DateTime.Now,
                            CreatedBy = user.UserID,
                            FinYear = 1,
                            FromModule = 0,
                            IsDeleted = false
                        };

                        db.ReceiptHeader.Add(hdrCashReceiptPayment);
                        db.SaveChanges();

                        //hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.BankTransId + accBankBook.ReceiptVoucherSuffix;
                        //db.Entry(hdrBankDeposit).State = EntityState.Modified;
                        //db.SaveChanges();

                        foreach (sdtoViewAccExpenseEntryDetails dtl in objExpenseEntry.Details)
                        {
                            sdtoReceiptDetails bankDepositDtlCr = new sdtoReceiptDetails()
                            {
                                ReceiptsId = hdrCashReceiptPayment.Id,
                                AccountId = dtl.AccountHead.AccountHeadId,
                                Narration = dtl.Narration,
                                Amount = dtl.Amount,
                                CreatedBy = user.UserID,
                                CreatedOn = DateTime.Now,
                                Display = 1,
                                IsDeleted = false
                            };

                            sdtoReceiptDetails bankDepositDtlDb = new sdtoReceiptDetails()
                            {
                                ReceiptsId = hdrCashReceiptPayment.Id,
                                AccountId = accBankBook.AccountHeadId,
                                Narration = dtl.Narration,
                                Amount = dtl.Amount,
                                CreatedBy = user.UserID,
                                CreatedOn = DateTime.Now,
                                Display = 1,
                                IsDeleted = false,
                            };

                            db.ReceiptDetails.Add(bankDepositDtlCr);
                            db.ReceiptDetails.Add(bankDepositDtlDb);
                        }

                        if (db.BankDepositDetails.Count() > 0)
                            db.SaveChanges();

                        //var receiptDetails = new sdtoReceiptDetails()
                        //{
                        //    ReceiptId = receipt.ReceiptId,
                        //    AccountId = accHead.AccountHeadId,
                        //    Narration = "Loan dispatched",
                        //    CrAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                        //    Display = 1
                        //};                        
                    }
                }
            }
            LoadAccountHeadList(0);
            LoadAccountBookList(objExpenseEntry.Book != null ? objExpenseEntry.Book.AccountBookId : 0);

            return View(objExpenseEntry);
        }

        public ActionResult JournalEntry()
        {
            LoadAccountHeadList(0);
            LoadJournalBookList(0);
            //LoadAccountBookList(0);
            //var accJournalBookAccHeadId = db.GeneralSettings.FirstOrDefault().PurchaseJournalId;
            //var accBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountBookType.UniqueName.Equals("Journal", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            sdtoViewAccJournalEntry obj = new sdtoViewAccJournalEntry() { Date = DateTime.Now };
            //var CashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
            //var cashBook = db.AccountBooks.Include(x => x.AccountHeadId).Where(x => x.AccountBookId == CashBookId).FirstOrDefault();
            //if (cashBook != null)
            //{
            //    obj.Book = cashBook;
            //}
            return View(obj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult JournalEntry(sdtoViewAccJournalEntry objCashReceiptPayment)
        {
            if (objCashReceiptPayment.SourceClick == 0)
                objCashReceiptPayment.Details.Add(new sdtoViewAccJournalEntryDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    sdtoUser user = Session["UserDetails"] as sdtoUser;
                    var settingCashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
                    //Post for Bank book
                    var accBankBook = db.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    if (accBankBook != null)
                    {
                        sdtoJournalHeader hdrCashReceiptPayment = new sdtoJournalHeader()
                        {
                            BookId = objCashReceiptPayment.Book.AccountBookId,
                            Cancelled = 0,
                            CreatedOn = DateTime.Now,
                            CreatedBy = user.UserID,
                            FinYear = 1,
                            FromModule = 0,
                            IsDeleted = false,
                            VoucherNo = objCashReceiptPayment.Voucher,
                            VoucherTotal = objCashReceiptPayment.CreditVoucherTotal + objCashReceiptPayment.DebitVoucherTotal
                        };

                        db.JournalHeader.Add(hdrCashReceiptPayment);
                        db.SaveChanges();

                        //hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.BankTransId + accBankBook.ReceiptVoucherSuffix;
                        //db.Entry(hdrBankDeposit).State = EntityState.Modified;
                        //db.SaveChanges();

                        foreach (sdtoViewAccJournalEntryDetails dtl in objCashReceiptPayment.Details)
                        {
                            sdtoJournalDetails bankDepositDtlCr = new sdtoJournalDetails()
                            {
                                JournalId = hdrCashReceiptPayment.Id,
                                AccountId = dtl.AccountHead.AccountHeadId,
                                Narration = dtl.Narration,
                                CrAmount = dtl.CreditAmount,
                                CreatedBy = user.UserID,
                                CreatedOn = DateTime.Now,
                                IsDeleted = false
                            };

                            sdtoJournalDetails bankDepositDtlDb = new sdtoJournalDetails()
                            {
                                JournalId = hdrCashReceiptPayment.Id,
                                AccountId = accBankBook.AccountHeadId,
                                Narration = dtl.Narration,
                                DrAmount = dtl.DebitAmount,
                                CreatedBy = user.UserID,
                                CreatedOn = DateTime.Now,
                                IsDeleted = false,
                            };

                            db.JournalDetails.Add(bankDepositDtlCr);
                            db.JournalDetails.Add(bankDepositDtlDb);
                        }

                        if (db.JournalDetails.Count() > 0)
                            db.SaveChanges();

                        //var receiptDetails = new sdtoReceiptDetails()
                        //{
                        //    ReceiptId = receipt.ReceiptId,
                        //    AccountId = accHead.AccountHeadId,
                        //    Narration = "Loan dispatched",
                        //    CrAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                        //    Display = 1
                        //};                        
                    }
                }
            }
            LoadAccountHeadList(0);
            LoadAccountBookList(objCashReceiptPayment.Book != null ? objCashReceiptPayment.Book.AccountBookId : 0);

            return View(objCashReceiptPayment);
        }

        public ActionResult NewOpeningBalance()
        {
            LoadSelectListOpeningBalance(0, 0, 0);
            return View();
        }

        //[HttpParamAction]
        //[AcceptVerbs(HttpVerbs.Post)]

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DepositWithdrawalSave")]
        //[ValidateAntiForgeryToken]
        public ActionResult DepositWithdrawalSave(sdtoViewAccDepositWithdrawal DepositWithdrawal)
        {
            LoadAccountHeadList(0);
            LoadAccountBookList(DepositWithdrawal != null && DepositWithdrawal.Book != null ? DepositWithdrawal.Book.AccountBookId : 0);
            return View(DepositWithdrawal);
        }

        //[HttpParamAction]
        //[AcceptVerbs(HttpVerbs.Post)]

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DepositWithdrawalNewRow")]
        //[ValidateAntiForgeryToken]
        public ActionResult DepositWithdrawalNewRow(sdtoViewAccDepositWithdrawal objDepositWithdrawal)
        {
            objDepositWithdrawal.Details.Add(new sdtoViewAccDepositWithdrawalDetails());
            //if (TempData["PrevItems"] == null)
            //{
            //    TempData["PrevItems"] = objDepositWithdrawal.Details;
            //}
            //else
            //{
            //    List<sdtoViewAccDepositWithdrawalDetails> tempList = TempData["PrevItems"] as List<sdtoViewAccDepositWithdrawalDetails>;
            //    foreach (sdtoViewAccDepositWithdrawalDetails dtl in tempList)
            //    {
            //        objDepositWithdrawal.Details.Add(dtl);
            //    }
            //    objDepositWithdrawal.Details.Add(new sdtoViewAccDepositWithdrawalDetails());
            //    TempData["PrevItems"] = objDepositWithdrawal.Details;
            //}
            LoadAccountHeadList(0);
            LoadAccountBookList(objDepositWithdrawal.Book != null ? objDepositWithdrawal.Book.AccountBookId : 0);

            return View(objDepositWithdrawal);
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOpeningBalance(sdtoOpeningBalance OpeningBalance)
        {
            if (ModelState.IsValid)
            {
                if (OpeningBalance.CreditOpeningBalance > 0 && OpeningBalance.DebitOpeningBalance > 0)
                {
                    ModelState.AddModelError("CreditOpeningBalance", "Please enter either credit or debit opening balance");
                    LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
                    return View(OpeningBalance);
                }

                OpeningBalance.CreatedOn = DateTime.Now;
                sdtoUser session = Session["UserDetails"] as sdtoUser;
                if (session != null)
                    OpeningBalance.CreatedBy = session.UserID;
                OpeningBalance.IsDeleted = false;

                OpeningBalance.ClosingBalance = OpeningBalance.CreditOpeningBalance > 0 ? OpeningBalance.CreditOpeningBalance : OpeningBalance.DebitOpeningBalance;

                db.OpeningBalance.Add(OpeningBalance);
                db.SaveChanges();
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("OpeningBalances");
                else
                    return RedirectToAction("Login");
            }

            LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
            return View(OpeningBalance);
        }

        public ActionResult UpdateOpeningBalance(long? OpeningBalanceId)
        {
            var openingBalance = db.OpeningBalance.Find(OpeningBalanceId);
            LoadSelectListOpeningBalance(openingBalance.AccountHeadId, openingBalance.ScheduleId, openingBalance.FinancialYearId);
            return View(openingBalance);
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOpeningBalance(sdtoOpeningBalance OpeningBalance, long PrevDebit, long PrevCredit)
        {
            if (ModelState.IsValid)
            {
                sdtoUser session = Session["UserDetails"] as sdtoUser;
                if (session != null)
                    OpeningBalance.ModifiedBy = session.UserID;
                OpeningBalance.ModifiedOn = DateTime.Now;

                OpeningBalance.ClosingBalance = (OpeningBalance.ClosingBalance - (PrevDebit > 0 ? PrevDebit : PrevCredit)) + (OpeningBalance.CreditOpeningBalance > 0 ? (-1 * OpeningBalance.CreditOpeningBalance) : OpeningBalance.DebitOpeningBalance);

                // ModelState.AddModelError("ProfileImage", "This field is required");
                db.Entry(OpeningBalance).State = EntityState.Modified;
                db.SaveChanges();
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Login");
            }

            LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
            return View();
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
