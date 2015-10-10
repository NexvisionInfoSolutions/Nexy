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
using Business.Reports;

namespace LoanManagementSystem.Controllers
{
    [Authorize()]
    public class AccountingController : ControllerBase
    {
        private LoanDBContext db = new LoanDBContext();

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
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
            var accHeads = db.AccountHeads.Where(x => x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).OrderBy(x => x.Text).ToList();
            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Account Name" });
            ViewBag.AccountHeads = new SelectList(accHeads, "Value", "Text", AccHeadId);
        }

        private void LoadAccountBookList(long AccBookId, long AccBookTypeId)
        {
            var accHeads = db.AccountBooks.Where(x => x.IsDeleted == false && (AccBookTypeId == 0 || AccBookTypeId == x.AccountBookTypeId)).ToList().Select(x => new SelectListItem() { Value = x.AccountBookId.ToString(), Text = x.BookName }).ToList();
            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Account Book" });
            ViewBag.AccountBookList = new SelectList(accHeads, "Value", "Text", AccBookId);
        }

        private void LoadJournalBookList(long AccBookId)
        {
            var journalBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Journal", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            var accHeads = db.AccountBooks.Where(x => x.AccountBookTypeId == journalBookType.AccountBookTypeId && x.IsDeleted == false).ToList().Select(x => new SelectListItem() { Value = x.AccountBookId.ToString(), Text = x.BookName }).ToList();
            accHeads.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Journal Book" });
            ViewBag.AccountBookList = new SelectList(accHeads, "Value", "Text", AccBookId);
        }

        public ActionResult BankDeposit(long? HeaderId)
        {
            var bankBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(0, bankBookType.AccountBookTypeId);
            sdtoViewAccDepositWithdrawal obj = new sdtoViewAccDepositWithdrawal() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccDepositWithdrawalDetails() { InstrumentDate = DateTime.Now });

            if (HeaderId != null && HeaderId.Value > 0)
            {
                var header = db.BankDepositHeader.Find(HeaderId);
                if (header != null)
                {
                    var accountBook = db.AccountBooks.Find(header.BookId);
                    var openingBalance = db.OpeningBalance.Where(x => x.AccountHeadId == accountBook.AccountHeadId).FirstOrDefault();

                    obj = new sdtoViewAccDepositWithdrawal()
                    {
                        VoucherTotal = header.VoucherTotal,
                        Book = new sdtoAccountBook() { AccountBookId = header.BookId },
                        Date = header.TransDate,
                        HeaderId = HeaderId.Value,
                        PreviousVoucherTotal = header.VoucherTotal,
                        Voucher = header.VoucherNo,
                        Balance = openingBalance == null ? 0 : openingBalance.ClosingBalance
                    };

                    var headerSubList = db.BankDepositDetails.Where(x => x.IsDeleted == false && x.BankDepositId == obj.HeaderId);
                    if (headerSubList != null)
                    {
                        foreach (sdtoBankDepositDetails dtl in headerSubList)
                        {
                            obj.Details.Add(new sdtoViewAccDepositWithdrawalDetails()
                            {
                                AccountHeadId = dtl.AccountId,
                                Amount = -1 * dtl.Amount,
                                Narration = dtl.Narration
                            });
                        }
                    }
                }
            }

            return View(obj);
        }

        public ActionResult BankWithdrawal(long? HeaderId)
        {
            var bankBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(0, bankBookType.AccountBookTypeId);
            sdtoViewAccDepositWithdrawal obj = new sdtoViewAccDepositWithdrawal() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccDepositWithdrawalDetails() { InstrumentDate = DateTime.Now });

            if (HeaderId != null && HeaderId.Value > 0)
            {
                var header = db.BankDepositHeader.Find(HeaderId);
                if (header != null)
                {
                    var accountBook = db.AccountBooks.Find(header.BookId);
                    var openingBalance = db.OpeningBalance.Where(x => x.AccountHeadId == accountBook.AccountHeadId).FirstOrDefault();

                    obj = new sdtoViewAccDepositWithdrawal()
                    {
                        VoucherTotal = header.VoucherTotal,
                        Book = new sdtoAccountBook() { AccountBookId = header.BookId },
                        Date = header.TransDate,
                        HeaderId = HeaderId.Value,
                        PreviousVoucherTotal = header.VoucherTotal,
                        Voucher = header.VoucherNo,
                        Balance = openingBalance == null ? 0 : openingBalance.ClosingBalance
                    };

                    var headerSubList = db.BankDepositDetails.Where(x => x.IsDeleted == false && x.BankDepositId == obj.HeaderId);
                    if (headerSubList != null)
                    {
                        foreach (sdtoBankDepositDetails dtl in headerSubList)
                        {
                            obj.Details.Add(new sdtoViewAccDepositWithdrawalDetails()
                            {
                                AccountHeadId = dtl.AccountId,
                                Amount = -1 * dtl.Amount,
                                Narration = dtl.Narration
                            });
                        }
                    }
                }
            }

            return View(obj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BankWithdrawal(sdtoViewAccDepositWithdrawal objDepositWithdrawal)
        {
            if (objDepositWithdrawal.SourceClick == 0)
                objDepositWithdrawal.Details.Add(new sdtoViewAccDepositWithdrawalDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    bfTransaction objTransaction = new bfTransaction(db);
                    if (objDepositWithdrawal.HeaderId > 0)
                        objTransaction.CancelBankAccountHeader(objDepositWithdrawal.HeaderId);

                    objTransaction.AddBankWithdrawal(objDepositWithdrawal);
                    return RedirectToAction("ListBankDepositWithdrawal");
                }
            }
            var bankBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(objDepositWithdrawal.Book != null ? objDepositWithdrawal.Book.AccountBookId : 0, bankBookType.AccountBookTypeId);

            return View(objDepositWithdrawal);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BankDeposit(sdtoViewAccDepositWithdrawal objDepositWithdrawal)
        {
            if (objDepositWithdrawal.SourceClick == 0)
                objDepositWithdrawal.Details.Add(new sdtoViewAccDepositWithdrawalDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    bfTransaction objTransaction = new bfTransaction(db);
                    objTransaction.AddBankDeposit(objDepositWithdrawal);

                    if (objDepositWithdrawal.HeaderId > 0)
                        objTransaction.CancelBankAccountHeader(objDepositWithdrawal.HeaderId);

                    return RedirectToAction("ListBankDepositWithdrawal");
                }
            }

            var bankBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(objDepositWithdrawal.Book != null ? objDepositWithdrawal.Book.AccountBookId : 0, bankBookType.AccountBookTypeId);

            return View(objDepositWithdrawal);
        }

        public ActionResult CashPayment(long? HeaderId)
        {
            var cashBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(0, cashBookType.AccountBookTypeId);
            sdtoViewAccCashReceiptPayment obj = new sdtoViewAccCashReceiptPayment() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccCashReceiptPaymentDetails() { });

            if (HeaderId != null && HeaderId.Value > 0)
            {
                var header = db.ReceiptHeader.Find(HeaderId);
                if (header != null)
                {
                    var accountBook = db.AccountBooks.Find(header.BookId);
                    var openingBalance = db.OpeningBalance.Where(x => x.AccountHeadId == accountBook.AccountHeadId).FirstOrDefault();

                    obj = new sdtoViewAccCashReceiptPayment()
                    {
                        VoucherTotal = header.VoucherTotal,
                        Book = new sdtoAccountBook() { AccountBookId = header.BookId },
                        Date = header.TransDate,
                        HeaderId = HeaderId.Value,
                        PreviousVoucherTotal = header.VoucherTotal,
                        Voucher = header.VoucherNo,
                        Balance = openingBalance == null ? 0 : openingBalance.ClosingBalance
                    };

                    var headerSubList = db.ReceiptDetails.Where(x => x.IsDeleted == false && x.ReceiptsId == obj.HeaderId);
                    if (headerSubList != null)
                    {
                        foreach (sdtoReceiptDetails dtl in headerSubList)
                        {
                            if (dtl.Display == 1)
                                obj.Details.Add(new sdtoViewAccCashReceiptPaymentDetails()
                                {
                                    AccountHeadId = dtl.AccountId,
                                    Amount = -1 * dtl.Amount,
                                    Narration = dtl.Narration
                                });
                        }
                    }
                }
            }
            return View(obj);
        }

        public ActionResult CashReceipt(long? HeaderId)
        {
            var cashBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(0, cashBookType.AccountBookTypeId);
            sdtoViewAccCashReceiptPayment obj = new sdtoViewAccCashReceiptPayment() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccCashReceiptPaymentDetails() { });

            if (HeaderId != null && HeaderId.Value > 0)
            {
                var header = db.ReceiptHeader.Find(HeaderId);
                if (header != null)
                {
                    var accountBook = db.AccountBooks.Find(header.BookId);
                    var openingBalance = db.OpeningBalance.Where(x => x.AccountHeadId == accountBook.AccountHeadId).FirstOrDefault();

                    obj = new sdtoViewAccCashReceiptPayment()
                    {
                        VoucherTotal = header.VoucherTotal,
                        Book = new sdtoAccountBook() { AccountBookId = header.BookId },
                        Date = header.TransDate,
                        HeaderId = HeaderId.Value,
                        PreviousVoucherTotal = header.VoucherTotal,
                        Voucher = header.VoucherNo,
                        Balance = openingBalance == null ? 0 : openingBalance.ClosingBalance
                    };

                    var headerSubList = db.ReceiptDetails.Where(x => x.IsDeleted == false && x.ReceiptsId == obj.HeaderId);
                    if (headerSubList != null)
                    {
                        foreach (sdtoReceiptDetails dtl in headerSubList)
                        {
                            if (dtl.Display == 1)
                                obj.Details.Add(new sdtoViewAccCashReceiptPaymentDetails()
                                {
                                    AccountHeadId = dtl.AccountId,
                                    Amount = -1 * dtl.Amount,
                                    Narration = dtl.Narration
                                });
                        }
                    }
                }
            }
            return View(obj);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CashReceipt(sdtoViewAccCashReceiptPayment objCashReceiptPayment)
        {
            if (objCashReceiptPayment.SourceClick == 0)
                objCashReceiptPayment.Details.Add(new sdtoViewAccCashReceiptPaymentDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    bfTransaction objTransaction = new bfTransaction(db);
                    if (objCashReceiptPayment.HeaderId > 0)
                        objTransaction.CancelCashAccountHeader(objCashReceiptPayment.HeaderId);

                    objTransaction.AddCashReceipt(objCashReceiptPayment);

                    return RedirectToAction("ListCashReceiptPayment");
                }
            }

            var cashBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(objCashReceiptPayment.Book != null ? objCashReceiptPayment.Book.AccountBookId : 0, cashBookType.AccountBookTypeId);

            return View(objCashReceiptPayment);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CashPayment(sdtoViewAccCashReceiptPayment objCashReceiptPayment)
        {
            if (objCashReceiptPayment.SourceClick == 0)
                objCashReceiptPayment.Details.Add(new sdtoViewAccCashReceiptPaymentDetails());
            else
            {
                if (ModelState.IsValid)
                {
                    bfTransaction objTransaction = new bfTransaction(db);
                    if (objCashReceiptPayment.HeaderId > 0)
                        objTransaction.CancelCashAccountHeader(objCashReceiptPayment.HeaderId);

                    objTransaction.AddCashPayment(objCashReceiptPayment);
                    return RedirectToAction("ListCashReceiptPayment");
                }
            }

            var cashBookType = db.AccountBookTypes.Where(x => x.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            LoadAccountHeadList(0);
            LoadAccountBookList(objCashReceiptPayment.Book != null ? objCashReceiptPayment.Book.AccountBookId : 0, cashBookType.AccountBookTypeId);

            return View(objCashReceiptPayment);
        }

        public ActionResult ExpenseEntry()
        {
            //LoadAccountHeadList(0);
            //LoadAccountBookList(0);

            //sdtoViewAccExpenseEntry obj = new sdtoViewAccExpenseEntry() { Date = DateTime.Now };
            //var CashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
            //var cashBook = db.AccountBooks.Where(x => x.AccountBookId == CashBookId).FirstOrDefault();
            //if (cashBook != null)
            //{
            //    obj.Book = cashBook;
            //}
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ExpenseEntry(sdtoViewAccExpenseEntry objExpenseEntry)
        {
            if (objExpenseEntry.SourceClick == 0)
                objExpenseEntry.Details.Add(new sdtoViewAccExpenseEntryDetails() { AccountHead = new sdtoAccountHead() { AccountHeadId = objExpenseEntry.Book.AccountHeadId.Value }, Amount = objExpenseEntry.Amount, Narration = objExpenseEntry.Description });
            else
            {
                if (ModelState.IsValid)
                {
                    //sdtoUser user = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
                    //var settingCashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
                    ////Post for Bank book
                    //var accBankBook = db.AccountBooks.Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                    //if (accBankBook != null)
                    //{
                    //    sdtoReceiptHeader hdrCashReceiptPayment = new sdtoReceiptHeader()
                    //    {
                    //        BookId = objExpenseEntry.Book.AccountBookId,
                    //        Cancelled = 0,
                    //        CreatedOn = DateTime.Now,
                    //        CreatedBy = user.UserID,
                    //        FinancialYearId = CurrentUser.UserSession.FinancialYearId.Value,
                    //        FromModule = 0,
                    //        IsDeleted = false,
                    //        TransDate = objExpenseEntry.Date
                    //    };

                    //    db.ReceiptHeader.Add(hdrCashReceiptPayment);
                    //    db.SaveChanges();

                    //    //hdrBankDeposit.VoucherNo = accBankBook.ReceiptVoucherPrefix + hdrBankDeposit.BankTransId + accBankBook.ReceiptVoucherSuffix;
                    //    //db.Entry(hdrBankDeposit).State = EntityState.Modified;
                    //    //db.SaveChanges();

                    //    foreach (sdtoViewAccExpenseEntryDetails dtl in objExpenseEntry.Details)
                    //    {
                    //        sdtoReceiptDetails bankDepositDtlCr = new sdtoReceiptDetails()
                    //        {
                    //            ReceiptsId = hdrCashReceiptPayment.Id,
                    //            AccountId = dtl.AccountHead.AccountHeadId,
                    //            Narration = dtl.Narration,
                    //            Amount = dtl.Amount,
                    //            CreatedBy = user.UserID,
                    //            CreatedOn = DateTime.Now,
                    //            Display = 1,
                    //            IsDeleted = false
                    //        };

                    //        sdtoReceiptDetails bankDepositDtlDb = new sdtoReceiptDetails()
                    //        {
                    //            ReceiptsId = hdrCashReceiptPayment.Id,
                    //            AccountId = accBankBook.AccountHeadId,
                    //            Narration = dtl.Narration,
                    //            Amount = dtl.Amount,
                    //            CreatedBy = user.UserID,
                    //            CreatedOn = DateTime.Now,
                    //            Display = 1,
                    //            IsDeleted = false,
                    //        };

                    //        db.ReceiptDetails.Add(bankDepositDtlCr);
                    //        db.ReceiptDetails.Add(bankDepositDtlDb);
                    //    }

                    //    if (db.BankDepositDetails.Count() > 0)
                    //        db.SaveChanges();

                    //    //var receiptDetails = new sdtoReceiptDetails()
                    //    //{
                    //    //    ReceiptId = receipt.ReceiptId,
                    //    //    AccountId = accHead.AccountHeadId,
                    //    //    Narration = "Loan dispatched",
                    //    //    CrAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                    //    //    Display = 1
                    //    //};                        
                    //}

                    return RedirectToAction("ListBankDepositWithdrawal");
                }
            }

            // LoadAccountHeadList(0);
            // LoadAccountBookList(objExpenseEntry.Book != null ? objExpenseEntry.Book.AccountBookId : 0);

            return View(objExpenseEntry);
        }

        public ActionResult JournalEntry(long? HeaderId)
        {
            LoadAccountHeadList(0);
            LoadJournalBookList(0);
            //LoadAccountBookList(0);
            //var accJournalBookAccHeadId = db.GeneralSettings.FirstOrDefault().PurchaseJournalId;
            //var accBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountBookType.UniqueName.Equals("Journal", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            sdtoViewAccJournalEntry obj = new sdtoViewAccJournalEntry() { Date = DateTime.Now };
            obj.Details.Add(new sdtoViewAccJournalEntryDetails() { });

            if (HeaderId != null && HeaderId.Value > 0)
            {
                var header = db.JournalHeader.Find(HeaderId);
                if (header != null)
                {
                    obj = new sdtoViewAccJournalEntry()
                    {
                        Balance = header.VoucherTotal,
                        Book = new sdtoAccountBook() { AccountBookId = header.BookId },
                        Date = header.TransDate.Value,
                        HeaderId = HeaderId.Value,
                        PreviousVoucherTotal = header.VoucherTotal,
                        Voucher = header.VoucherNo
                    };

                    var headerSubList = db.JournalDetails.Where(x => x.IsDeleted == false && x.JournalId == obj.HeaderId);
                    if (headerSubList != null)
                    {
                        foreach (sdtoJournalDetails dtl in headerSubList)
                        {
                            obj.Details.Add(new sdtoViewAccJournalEntryDetails()
                            {
                                AccountHeadId = dtl.AccountId,
                                DebitAmount = -1 * dtl.DrAmount,
                                CreditAmount = -1 * dtl.CrAmount,
                                Narration = dtl.Narration
                            });
                        }
                    }
                }
            }

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
                    var cr = objCashReceiptPayment.Details.Sum(x => x.CreditAmount);
                    var dr = objCashReceiptPayment.Details.Sum(x => x.CreditAmount);

                    if (dr + (-1 * cr) > 0)
                        ModelState.AddModelError("", "The voucher credit total and debit total does not balance.");
                    else
                    {

                        bfTransaction objTransaction = new bfTransaction(db);
                        if (objCashReceiptPayment.HeaderId > 0)
                            objTransaction.CancelJournalAccountHeader(objCashReceiptPayment.HeaderId);

                        objTransaction.AddJournalEntry(objCashReceiptPayment);
                        return RedirectToAction("ListJournalEntry");
                    }
                }
            }
            LoadAccountHeadList(0);
            LoadJournalBookList(objCashReceiptPayment.Book != null ? objCashReceiptPayment.Book.AccountBookId : 0);

            return View(objCashReceiptPayment);
        }

        public ActionResult NewOpeningBalance(long? ScheduleId)
        {
            LoadSelectListOpeningBalance(0, 0, 0);
            var openingBalance = db.OpeningBalance.Include(x => x.AccountHead).Include(x => x.Schedule).Include(x => x.FinancialPeriod).Where(x => x.IsDeleted == false && x.FinancialPeriod.IsCurrentYear == true && (x.ScheduleId == ((ScheduleId == null || ScheduleId.Value == 0) ? x.ScheduleId : ScheduleId))).ToList();

            sdtoViewOpeningBalance openingBalances = new sdtoViewOpeningBalance() { ScheduleId = ScheduleId == null ? 0 : ScheduleId.Value, OpeningBalances = openingBalance };

            return View(openingBalances);
        }

        //[HttpParamAction]
        //[AcceptVerbs(HttpVerbs.Post)]

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DepositWithdrawalSave")]
        //[ValidateAntiForgeryToken]
        public ActionResult DepositWithdrawalSave(sdtoViewAccDepositWithdrawal DepositWithdrawal)
        {
            LoadAccountHeadList(0);
            LoadAccountBookList(DepositWithdrawal != null && DepositWithdrawal.Book != null ? DepositWithdrawal.Book.AccountBookId : 0, 0);
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
            LoadAccountBookList(objDepositWithdrawal.Book != null ? objDepositWithdrawal.Book.AccountBookId : 0, 0);

            return View(objDepositWithdrawal);
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOpeningBalance(sdtoViewOpeningBalance ViewOpeningBalance)
        {
            if (ModelState.IsValid)
            {
                var OpeningBalances = ViewOpeningBalance.OpeningBalances;
                if (OpeningBalances != null)
                {
                    for (int i = 0; i < OpeningBalances.Count(); i++)
                    {
                        var CurrentOpeningBalance = OpeningBalances[i];
                        var opnBalance = db.OpeningBalance.Find(CurrentOpeningBalance.OpeningBalanceId);

                        var prevCreditOpeningBalance = opnBalance.CreditOpeningBalance;
                        var prevDebitOpeningBalance = opnBalance.DebitOpeningBalance;

                        opnBalance.ScheduleId = CurrentOpeningBalance.ScheduleId;
                        opnBalance.AccountHeadId = CurrentOpeningBalance.AccountHeadId;
                        opnBalance.CreditOpeningBalance = CurrentOpeningBalance.CreditOpeningBalance;
                        opnBalance.DebitOpeningBalance = CurrentOpeningBalance.DebitOpeningBalance;
                        opnBalance.ModifiedBy = CurrentUserSession.UserId;
                        opnBalance.ModifiedOn = DateTime.Now;
                        opnBalance.ClosingBalance = opnBalance.ClosingBalance - (prevCreditOpeningBalance + prevDebitOpeningBalance) + (opnBalance.CreditOpeningBalance + opnBalance.DebitOpeningBalance);

                        if (opnBalance.CreditOpeningBalance > 0 && opnBalance.DebitOpeningBalance > 0)
                        {
                            ModelState.AddModelError("", "Please enter either credit or debit opening balance");
                            LoadSelectListOpeningBalance(0, 0, 0);
                            return View(ViewOpeningBalance);
                        }

                        opnBalance.CreatedOn = DateTime.Now;
                        sdtoUser session = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
                        if (session != null)
                            opnBalance.CreatedBy = session.UserID;
                        opnBalance.IsDeleted = false;

                        db.Entry(opnBalance).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    if (User.Identity.IsAuthenticated)
                        return RedirectToAction("OpeningBalances");
                    else
                        return RedirectToAction("Login", "Login");
                }
            }

            LoadSelectListOpeningBalance(0, 0, 0);
            return View(ViewOpeningBalance);
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
                sdtoUser session = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
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
                    return RedirectToAction("Login", "Login");
            }

            LoadSelectListOpeningBalance(OpeningBalance.AccountHeadId, OpeningBalance.ScheduleId, OpeningBalance.FinancialYearId);
            return View();
        }

        public ActionResult ListBankDepositWithdrawal()
        {
            return View();
        }

        public JsonResult BankDepositWithdrawalInfo()
        {
            long CompanyId = 0;
            sdtoUser session = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            if (session != null)
                CompanyId = session.CompanyId.Value;

            bfReport objReport = new bfReport(db);
            var Deposits = objReport.GetBankDepositWithdrawal(CompanyId, string.Empty);
            var result = Json(Deposits, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public ActionResult ListCashReceiptPayment()
        {
            return View();
        }

        public JsonResult CashReceiptPaymentInfo()
        {
            long CompanyId = 0;
            sdtoUser session = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            if (session != null)
                CompanyId = session.CompanyId.Value;

            bfReport objReport = new bfReport(db);
            var CashReceiptPayments = objReport.GetCashReceiptPayments(CompanyId, string.Empty);
            var result = Json(CashReceiptPayments, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public ActionResult ListJournalEntry()
        {
            return View();
        }

        public JsonResult JournalEntryInfo()
        {
            long CompanyId = 0;
            sdtoUser session = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            if (session != null)
                CompanyId = session.CompanyId.Value;

            bfReport objReport = new bfReport(db);
            var CashReceiptPayments = objReport.GetJournalEntries(CompanyId, string.Empty);
            var result = Json(CashReceiptPayments, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public JsonResult GetBookAccountDetails(long AccountBookId, string TransType)
        {
            bfTransaction objReport = new bfTransaction(db);
            var accDetails = objReport.GetAccountDetails(AccountBookId, TransType);
            return Json(accDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BaseSchedulesInfo()
        {
            var list = db.Schedules.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ScheduleAccountInfo(long ScheduleId)
        {
            var list = db.AccountHeads.Include(x => x.Schedule).Where(x => x.IsDeleted == false && (x.ScheduleId == ScheduleId || x.Schedule.BaseScheduleId == ScheduleId)).ToList();
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
