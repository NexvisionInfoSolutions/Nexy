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
using Data.Models.Enumerations;

namespace LoanManagementSystem.Controllers.Loan
{
    public class LoanRepaymentsController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: LoanRepayments
        public ActionResult Index(long? LoanId)
        {
            //var query = from c in db.sdtoLoanRepayments
            //join o in db.sdtoLoanInfoes on c.LoanId equals o.LoanId
            //where c.LoanId == ((LoanId == null || LoanId.Value == 0) ? c.LoanId : LoanId)
            //group c by c.LoanId into g
            //select new
            //{
            //  Name = g.Key,
            //  Sum = g.Sum(oi => oi.RepaymentAmount),
            //  RepaymentCode = c.RepaymentCode
            //};

            sdtoLoanRepayment repay = new sdtoLoanRepayment();
            var itemsLoan = db.sdtoLoanInfoes.Include(x => x.Member).Where(x => x.Status == LoanStatus.Active).ToList();
            var itemsLoans = itemsLoan.Select(x => new SelectListItem() { Value = x.LoanId.ToString(), Text = x.LoanId + " - " + x.LoanAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsLoans.Insert(0, new SelectListItem() { Value = "0", Text = "Select a loan" });
            ViewBag.LoanList = new SelectList(itemsLoans, "Value", "Text");

            ViewBag.LoanDetails = db.sdtoLoanInfoes.Where(x => x.LoanId == LoanId).FirstOrDefault();

            //var sdtoLoanRepayments = db.sdtoLoanRepayments.Where(x => x.LoanId == ((LoanId == null || LoanId.Value == 0) ? x.LoanId : LoanId)).Include(s => s.LoanDetails);
            var sdtoLoanRepayments = db.sdtoLoanRepayments.Where(x => x.LoanId == LoanId).Include(s => s.LoanDetails);
            if (sdtoLoanRepayments != null && sdtoLoanRepayments.Count() > 0)
            {
                ViewBag.TotalPaid = sdtoLoanRepayments.Sum(y => y.RepaymentAmount);
                ViewBag.TotalPendingPrincipal = sdtoLoanRepayments.OrderByDescending(x => x.LoanRepaymentId).FirstOrDefault().PendingPrincipalAmount;
            }
            return View(sdtoLoanRepayments.ToList());
        }

        // GET: LoanRepayments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            if (sdtoLoanRepayment == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanRepayment);
        }

        //// GET: LoanRepayments/Create
        //public ActionResult Create()
        //{
        //    sdtoLoanRepayment repay = new sdtoLoanRepayment();
        //    var itemsLoan = db.sdtoLoanInfoes.Include(x => x.Member).ToList();
        //    itemsLoan.Insert(0, new sdtoLoanInfo() { LoanId = 0, Member = new sdtoUser() { UserID = 0, FirstName = "Select Loan" } });

        //    ViewBag.LoanList = new SelectList(itemsLoan.Select(x => new { LoanId = x.LoanId, Display = x.LoanId + " - " + x.LoanAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }),
        //            "LoanId", "Display");
        //    return View(repay);
        //}

        [HttpGet]
        public ActionResult Create(int? LoanId)
        {
            sdtoLoanRepayment repay = new sdtoLoanRepayment();
            var itemsLoan = db.sdtoLoanInfoes.Include(x => x.Member).Where(x => x.Status == LoanStatus.Active).ToList();
            var itemsLoans = itemsLoan.Select(x => new SelectListItem() { Value = x.LoanId.ToString(), Text = x.LoanId + " - " + x.LoanAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsLoans.Insert(0, new SelectListItem() { Value = "0", Text = "Select a loan" });
            ViewBag.LoanList = new SelectList(itemsLoans, "Value", "Text");

            if (LoanId != null && LoanId.Value > 0)
            {
                var loandetails = db.sdtoLoanInfoes.Find(LoanId);

                var loanPendingAmt = loandetails.LoanAmount;
                var loanInterest = loandetails.InteresRate;
                var loanPendingInstallments = loandetails.TotalInstallments;

                var loanRepayment = db.sdtoLoanRepayments.Where(x => x.LoanId == LoanId).OrderByDescending(x => x.LoanRepaymentId).FirstOrDefault();
                var repaymentInterest = loanInterest;
                var repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;

                if (loanRepayment != null && loanRepayment.LoanRepaymentId > 0)
                {
                    loanPendingAmt = loanRepayment.PendingPrincipalAmount;
                    loanPendingInstallments = loanRepayment.PendingInstallments;
                    repaymentInterest = loanRepayment.InterestRate;

                    repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;
                    repay.RepaymentCode = loanRepayment.RepaymentCode;
                }

                repay.InterestRate = repaymentInterest;
                repay.InterestAmount = Math.Round(repaymentInterestAmt, 2);
                repay.PendingPrincipalAmount = loanPendingAmt;
                repay.PendingInstallments = loanPendingInstallments;
                repay.PrincipalAmount = loandetails.LoanAmount;

                repay.Status = Data.Models.Enumerations.RepaymentStatus.Paid;
                repay.PaymentMode = ModeOfPayment.Cash;

                repay.RepaymentAmount = loandetails.InstallmentAmount + repay.InterestAmount;
            }

            return View(repay);
        }

        // POST: LoanRepayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "LoanRepaymentId,LoanId,RepaymentCode,PrincipalAmount,InterestAmount,InterestRate,RepaymentAmount,PendingPrincipalAmount,Status,PaymentMode,ChequeDetails,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] 
            sdtoLoanRepayment sdtoLoanRepayment)
        {
            if (ModelState.IsValid)
            {
                if (sdtoLoanRepayment.LoanId > 0)
                {
                    var loandetails = db.sdtoLoanInfoes.Find(sdtoLoanRepayment.LoanId);

                    var loanPendingAmt = loandetails.LoanAmount;
                    var loanInterest = loandetails.InteresRate;
                    var loanPendingInstallments = loandetails.TotalInstallments;

                    var loanRepayment = db.sdtoLoanRepayments.Where(x => x.LoanId == sdtoLoanRepayment.LoanId).OrderByDescending(x => x.LoanRepaymentId).FirstOrDefault();
                    var repaymentInterest = loanInterest;
                    var repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;

                    if (loanRepayment != null && loanRepayment.LoanRepaymentId > 0)
                    {
                        loanPendingAmt = loanRepayment.PendingPrincipalAmount;
                        loanPendingInstallments = loanRepayment.PendingInstallments;
                        repaymentInterest = loanRepayment.InterestRate;

                        repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;
                        sdtoLoanRepayment.RepaymentCode = loanRepayment.RepaymentCode;
                    }

                    sdtoLoanRepayment.InterestAmount = Math.Round(repaymentInterestAmt, 2);
                    sdtoLoanRepayment.PendingPrincipalAmount = loanPendingAmt - (sdtoLoanRepayment.RepaymentAmount - repaymentInterestAmt);
                    sdtoLoanRepayment.PendingInstallments -= Convert.ToInt32(Math.Floor(sdtoLoanRepayment.PendingPrincipalAmount / loandetails.InstallmentAmount));
                    sdtoLoanRepayment.PrincipalAmount = loandetails.LoanAmount;

                    sdtoLoanRepayment.Status = Data.Models.Enumerations.RepaymentStatus.Paid;
                    sdtoLoanRepayment.CreatedOn = DateTime.Now;

                    db.sdtoLoanRepayments.Add(sdtoLoanRepayment);
                    db.SaveChanges();

                    var loan = db.sdtoLoanInfoes.Find(sdtoLoanRepayment.LoanId);

                    //Account Posting
                    var member = db.User.Include(x => x.AccountHeadId).Where(x => x.UserID == loan.UserId).FirstOrDefault();
                    if (member != null)
                    {
                        var accHead = member.AccountHead;
                        if (accHead != null)
                        {
                            // Post for member cash book
                            var accCashBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                            if (accCashBook != null)
                            {
                                var receipt = new sdtoReceiptHeader()
                                {
                                    BookId = accCashBook.AccountBookId,
                                    TransDate = DateTime.Now,
                                    VoucherTotal = sdtoLoanRepayment.RepaymentAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                    TransType = ReceiptType.Payment,
                                    FinYear = 1,
                                    FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                    Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                    TransId = sdtoLoanRepayment.LoanRepaymentId, //Doubt: Is transaction id loan id?
                                    Cancelled = 0

                                };
                                db.ReceiptHeader.Add(receipt);
                                db.SaveChanges();

                                receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.ReceiptId + accCashBook.ReceiptVoucherSuffix;
                                db.Entry(receipt).State = EntityState.Modified;
                                db.SaveChanges();

                                var receiptDetailsPayment = new sdtoReceiptDetails()
                                {
                                    ReceiptId = receipt.ReceiptId,
                                    AccountId = accHead.AccountHeadId,
                                    Narration = "Loan repayment amount received",
                                    CrAmount = sdtoLoanRepayment.RepaymentAmount - sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                    Display = 1
                                };

                                var receiptDetailsInterest = new sdtoReceiptDetails()
                                {
                                    ReceiptId = receipt.ReceiptId,
                                    AccountId = accHead.AccountHeadId,
                                    Narration = "Loan interest received",
                                    CrAmount = sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                    Display = 1
                                };

                                db.ReceiptDetails.Add(receiptDetailsPayment);
                                db.ReceiptDetails.Add(receiptDetailsInterest);
                                db.SaveChanges();
                            }

                            //Post for Bank book
                            var accBankBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                            if (accBankBook != null)
                            {
                                var receipt = new sdtoReceiptHeader()
                                {
                                    BookId = accBankBook.AccountBookId,
                                    TransDate = DateTime.Now,
                                    VoucherTotal = sdtoLoanRepayment.RepaymentAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                    TransType = ReceiptType.Receipt,
                                    FinYear = 1,
                                    FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                    Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                    TransId = sdtoLoanRepayment.LoanRepaymentId, //Doubt: Is transaction id loan id?
                                    Cancelled = 0

                                };
                                db.ReceiptHeader.Add(receipt);
                                db.SaveChanges();

                                receipt.VoucherNo = accBankBook.ReceiptVoucherPrefix + receipt.ReceiptId + accBankBook.ReceiptVoucherSuffix;
                                db.Entry(receipt).State = EntityState.Modified;
                                db.SaveChanges();

                                var receiptDetailsPayment = new sdtoReceiptDetails()
                                 {
                                     ReceiptId = receipt.ReceiptId,
                                     AccountId = accHead.AccountHeadId,
                                     Narration = "Loan repayment amount received",
                                     DbAmount = sdtoLoanRepayment.RepaymentAmount - sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                     Display = 1
                                 };

                                var receiptDetailsInterest = new sdtoReceiptDetails()
                                {
                                    ReceiptId = receipt.ReceiptId,
                                    AccountId = accHead.AccountHeadId,
                                    Narration = "Loan interest received",
                                    DbAmount = sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                    Display = 1
                                };

                                db.ReceiptDetails.Add(receiptDetailsPayment);
                                db.ReceiptDetails.Add(receiptDetailsInterest);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("Index", new { LoanId = sdtoLoanRepayment.LoanId });
                }
            }

            var itemsLoan = db.sdtoLoanInfoes.Include(x => x.Member).ToList();
            var itemsLoans = itemsLoan.Select(x => new SelectListItem() { Value = x.LoanId.ToString(), Text = x.LoanId + " - " + x.LoanAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsLoans.Insert(0, new SelectListItem() { Value = "0", Text = "Select a loan" });
            ViewBag.LoanList = new SelectList(itemsLoans, "Value", "Text");

            return View(sdtoLoanRepayment);
        }

        // GET: LoanRepayments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            if (sdtoLoanRepayment == null)
            {
                return HttpNotFound();
            }
            var itemsLoan = db.sdtoLoanInfoes.Include(x => x.Member).ToList();
            var itemsLoans = itemsLoan.Select(x => new SelectListItem() { Value = x.LoanId.ToString(), Text = x.LoanId + " - " + x.LoanAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsLoans.Insert(0, new SelectListItem() { Value = "0", Text = "Select a loan" });
            ViewBag.LoanList = new SelectList(itemsLoans, "Value", "Text");

            return View(sdtoLoanRepayment);
        }

        // POST: LoanRepayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(//[Bind(Include = "LoanRepaymentId,LoanId,RepaymentCode,PrincipalAmount,InterestAmount,InterestRate,RepaymentAmount,PendingPrincipalAmount,Status,PaymentMode,ChequeDetails,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")]
            sdtoLoanRepayment sdtoLoanRepayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sdtoLoanRepayment).State = EntityState.Modified;
                db.SaveChanges();

                var loan = db.sdtoLoanInfoes.Find(sdtoLoanRepayment.LoanId);

                var member = db.User.Include(x => x.AccountHeadId).Where(x => x.UserID == loan.UserId).FirstOrDefault();
                if (member != null)
                {
                    var accHead = member.AccountHead;
                    if (accHead != null)
                    {
                        // Post for member cash book
                        var accCashBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (accCashBook != null)
                        {
                            var header = db.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && x.BookId == accCashBook.AccountBookId && x.TransId == sdtoLoanRepayment.LoanRepaymentId && x.Transaction == TransactionType.LoanRepayment).FirstOrDefault();

                            header.Cancelled = 1;
                            db.Entry(header).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        var accBankBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (accBankBook != null)
                        {
                            var header = db.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && x.BookId == accBankBook.AccountBookId && x.TransId == sdtoLoanRepayment.LoanRepaymentId && x.Transaction == TransactionType.LoanRepayment).FirstOrDefault();

                            header.Cancelled = 1;
                            db.Entry(header).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                //Account Posting
                //var member = db.User.Include(x => x.AccountHeadId).Where(x => x.UserID == loan.UserId).FirstOrDefault();
                if (member != null)
                {
                    var accHead = member.AccountHead;
                    if (accHead != null)
                    {
                        // Post for member cash book
                        var accCashBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (accCashBook != null)
                        {
                            var receipt = new sdtoReceiptHeader()
                            {
                                BookId = accCashBook.AccountBookId,
                                TransDate = DateTime.Now,
                                VoucherTotal = sdtoLoanRepayment.RepaymentAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                TransType = ReceiptType.Payment,
                                FinYear = 1,
                                FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                TransId = sdtoLoanRepayment.LoanRepaymentId, //Doubt: Is transaction id loan id?
                                Cancelled = 0

                            };
                            db.ReceiptHeader.Add(receipt);
                            db.SaveChanges();

                            receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.ReceiptId + accCashBook.ReceiptVoucherSuffix;
                            db.Entry(receipt).State = EntityState.Modified;
                            db.SaveChanges();

                            var receiptDetailsPayment = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan repayment amount received",
                                CrAmount = sdtoLoanRepayment.RepaymentAmount - sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            var receiptDetailsInterest = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan interest received",
                                CrAmount = sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            db.ReceiptDetails.Add(receiptDetailsPayment);
                            db.ReceiptDetails.Add(receiptDetailsInterest);
                            db.SaveChanges();
                        }

                        //Post for Bank book
                        var accBankBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (accBankBook != null)
                        {
                            var receipt = new sdtoReceiptHeader()
                            {
                                BookId = accBankBook.AccountBookId,
                                TransDate = DateTime.Now,
                                VoucherTotal = sdtoLoanRepayment.RepaymentAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                TransType = ReceiptType.Receipt,
                                FinYear = 1,
                                FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                TransId = sdtoLoanRepayment.LoanRepaymentId, //Doubt: Is transaction id loan id?
                                Cancelled = 0

                            };
                            db.ReceiptHeader.Add(receipt);
                            db.SaveChanges();

                            receipt.VoucherNo = accBankBook.ReceiptVoucherPrefix + receipt.ReceiptId + accBankBook.ReceiptVoucherSuffix;
                            db.Entry(receipt).State = EntityState.Modified;
                            db.SaveChanges();

                            var receiptDetailsPayment = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan repayment amount received",
                                DbAmount = sdtoLoanRepayment.RepaymentAmount - sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            var receiptDetailsInterest = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan interest received",
                                DbAmount = sdtoLoanRepayment.InterestAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            db.ReceiptDetails.Add(receiptDetailsPayment);
                            db.ReceiptDetails.Add(receiptDetailsInterest);
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index", new { LoanId = sdtoLoanRepayment.LoanId });
            }
            var itemsLoan = db.sdtoLoanInfoes.Include(x => x.Member).ToList();
            var itemsLoans = itemsLoan.Select(x => new SelectListItem() { Value = x.LoanId.ToString(), Text = x.LoanId + " - " + x.LoanAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsLoans.Insert(0, new SelectListItem() { Value = "0", Text = "Select a loan" });
            ViewBag.LoanList = new SelectList(itemsLoans, "Value", "Text");

            return View(sdtoLoanRepayment);
        }

        // GET: LoanRepayments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            if (sdtoLoanRepayment == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanRepayment);
        }

        // POST: LoanRepayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoLoanRepayment sdtoLoanRepayment = db.sdtoLoanRepayments.Find(id);
            db.sdtoLoanRepayments.Remove(sdtoLoanRepayment);
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
