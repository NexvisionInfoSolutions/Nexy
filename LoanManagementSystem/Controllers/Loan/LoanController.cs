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
using System.Text;
using System.IO;
using Business.Reports;

namespace LoanManagementSystem.Controllers.Loan
{
    public class LoanController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: Loan
        public ActionResult Index()
        {
            var sdtoLoanInfoes = db.sdtoLoanInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser);
            return View(sdtoLoanInfoes.ToList());
        }

        public JsonResult LoansInfo()
        {
            var dbResult = db.sdtoLoanInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser).ToList();
            var loans = (from loan in dbResult
                         select new
                         {
                             loan.LoanId,
                             loan.Member.FirstName,
                             loan.Member.LastName,
                             loan.LoanAmount,
                             loan.CreatedOn,
                             loan.InstallmentAmount,
                             loan.InteresRate,
                             loan.Notes,
                             LoanInfo = loan.LoanId
                         });
            return Json(loans, JsonRequestBehavior.AllowGet);
        }

        // GET: Loan/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        public ActionResult ExportView()
        {
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            long CompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                CompanyId = sessionUser.CompanyId.Value;

            DataTable dtRptParams = new DataTable();
            dtRptParams.Columns.Add(new DataColumn("EntityId", typeof(long)));
            dtRptParams.Columns.Add(new DataColumn("EntityStartDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityEndDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityIntVal", typeof(int)));
            dtRptParams.Columns.Add(new DataColumn("EntityStrVal", typeof(string)));
            dtRptParams.Columns.Add(new DataColumn("EntityType", typeof(string)));

            bfReport objReport = new bfReport(null);
            var loanInfoList = objReport.GetRptLoanSummary(CompanyId, dtRptParams).ToList().Select(x => new sdtoLoanRepayment() { LoanId = x.LoanId, LoanDetails = new sdtoLoanInfo() { Member = new sdtoUser() { FirstName = x.FirstName, LastName = x.LastName } }, PendingPrincipalAmount = x.BalanceLoanAmount });

            return View(loanInfoList);
        }

        public ActionResult ImportView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportView(HttpPostedFileBase DeviceInput)
        {
            StreamReader reader = new StreamReader(DeviceInput.InputStream);
            while (!reader.EndOfStream)
            {
                string lineInput = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(lineInput))
                {
                    string[] arrLines = lineInput.Split("\0".ToCharArray());
                    if (arrLines != null && arrLines.Length > 0)
                    {
                        foreach (string strLine in arrLines)
                        {
                            string[] arrValues = strLine.Split(",".ToCharArray());
                            if (arrValues != null && arrValues.Length > 10)
                            {
                                sdtoLoanRepayment repayment = new sdtoLoanRepayment();
                                long iLoanId = 0;
                                long.TryParse(arrValues[2], out iLoanId);
                                repayment.LoanId = iLoanId;
                                if (repayment.LoanId > 0)
                                {
                                    float fPaymentAmount = 0;
                                    float.TryParse(arrValues[3], out fPaymentAmount);

                                    DateTime dt = DateTime.Now;
                                    DateTime.TryParse(arrValues[8], out dt);

                                    repayment.RepaymentDate = dt;

                                    var loandetails = db.sdtoLoanInfoes.Find(repayment.LoanId);

                                    if (loandetails != null)
                                    {
                                        var loanPendingAmt = loandetails.LoanAmount;
                                        var loanInterest = loandetails.InteresRate;
                                        var loanPendingInstallments = loandetails.TotalInstallments;

                                        var loanRepayment = db.sdtoLoanRepayments.Where(x => x.LoanId == repayment.LoanId).OrderByDescending(x => x.LoanRepaymentId).FirstOrDefault();
                                        var repaymentInterest = loanInterest;
                                        var repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;

                                        if (loanRepayment != null && loanRepayment.LoanRepaymentId > 0)
                                        {
                                            loanPendingAmt = loanRepayment.PendingPrincipalAmount;
                                            loanPendingInstallments = loanRepayment.PendingInstallments;
                                            repaymentInterest = loanRepayment.InterestRate;

                                            repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;
                                            repayment.RepaymentCode = loanRepayment.RepaymentCode;
                                        }

                                        repayment.InterestAmount = Math.Round(repaymentInterestAmt, 2);
                                        repayment.PendingPrincipalAmount = loanPendingAmt - (repayment.RepaymentAmount - repaymentInterestAmt);
                                        repayment.PendingInstallments -= Convert.ToInt32(Math.Floor(repayment.PendingPrincipalAmount / loandetails.InstallmentAmount));
                                        repayment.PrincipalAmount = loandetails.LoanAmount;

                                        repayment.Status = Data.Models.Enumerations.RepaymentStatus.Paid;
                                        repayment.CreatedOn = DateTime.Now;

                                        db.sdtoLoanRepayments.Add(repayment);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            reader.Close();

            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            long CompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                CompanyId = sessionUser.CompanyId.Value;

            DataTable dtRptParams = new DataTable();
            dtRptParams.Columns.Add(new DataColumn("EntityId", typeof(long)));
            dtRptParams.Columns.Add(new DataColumn("EntityStartDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityEndDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityIntVal", typeof(int)));
            dtRptParams.Columns.Add(new DataColumn("EntityStrVal", typeof(string)));
            dtRptParams.Columns.Add(new DataColumn("EntityType", typeof(string)));

            bfReport objReport = new bfReport(null);
            var loanInfoList = objReport.GetRptLoanSummary(CompanyId, dtRptParams).ToList().Select(x => new sdtoLoanRepayment() { LoanId = x.LoanId, LoanDetails = new sdtoLoanInfo() { Member = new sdtoUser() { FirstName = x.FirstName, LastName = x.LastName } }, PendingPrincipalAmount = x.BalanceLoanAmount });

            return View(loanInfoList);
        }

        public FileStreamResult Export()
        {
            //var sdtoLoanInfoes = db.sdtoLoanInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser).Where(x => x.IsDeleted == false && x.Status == LoanStatus.Active);
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            long CompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                CompanyId = sessionUser.CompanyId.Value;

            DataTable dtRptParams = new DataTable();
            dtRptParams.Columns.Add(new DataColumn("EntityId", typeof(long)));
            dtRptParams.Columns.Add(new DataColumn("EntityStartDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityEndDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityIntVal", typeof(int)));
            dtRptParams.Columns.Add(new DataColumn("EntityStrVal", typeof(string)));
            dtRptParams.Columns.Add(new DataColumn("EntityType", typeof(string)));

            bfReport objReport = new bfReport(null);
            var loanInfoList = objReport.GetRptLoanSummary(CompanyId, dtRptParams).ToList().Select(x => new sdtoLoanRepayment() { LoanId = x.LoanId, LoanDetails = new sdtoLoanInfo() { Member = new sdtoUser() { FirstName = x.FirstName, LastName = x.LastName } }, PendingPrincipalAmount = x.BalanceLoanAmount }).ToList();

            //string HeaderData = "";
            string Data = "";
            //HeaderData = "Member ID" + "  " + "Name of customer" + "  " + "STOL" + "  " + "Amount" + "  " + "Interest" + Environment.NewLine;
            //Data = HeaderData;
            foreach (sdtoLoanRepayment Linfo in loanInfoList)
            {
                Data += Linfo.LoanId + "                 " + Linfo.LoanDetails.Member.FirstName + " " + Linfo.LoanDetails.Member.FirstName + "       STOL            " + Linfo.PendingPrincipalAmount + "                             " + Linfo.LoanDetails.InteresRate + "                                                                                                                                                      ";
                //Data += Linfo.Member.UserID.ToString() + " " + Linfo.Member.FirstName + " " + "STOL" + " " + Linfo.LoanAmount.ToString() + " " + Linfo.InteresRate.ToString() + Environment.NewLine;
            }
            var byteArray = Encoding.ASCII.GetBytes(Data);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "LoanData.DAT");

        }
        // GET: Loan/Create
        public ActionResult Create(long? UserId)
        {
            var loan = new sdtoLoanInfo();
            loan.RePaymentInterval = Data.Models.Enumerations.RepaymentInterval.Monthly;
            loan.Status = Data.Models.Enumerations.LoanStatus.Active;

            loan.InteresRate = db.GeneralSettings.FirstOrDefault().BankInterest;

            var listUsers = db.User.Where(x => x.UserType == UserType.Member && (UserId == null || UserId.Value == 0 || x.UserID == UserId));

            if (listUsers == null || listUsers.Count(x => x.UserID > 0) == 0)
                return RedirectToAction("Create", "Member");

            var users = listUsers.Select(x => new SelectListItem() { Value = x.UserID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            users.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Member" });
            ViewBag.UserList = new SelectList(users, "Value", "Text");
            return View(loan);
        }
        // POST: Loan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "LoanId,UserId,RepaymentStartDate,RePaymentInterval,RequestedAmount,ProposedAmount,LoanAmount,TotalInstallments,Status,ChequeDetails,InteresRate,SanctionedDate,SanctionedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] 
            sdtoLoanInfo sdtoLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo.InstallmentAmount = sdtoLoanInfo.LoanAmount / sdtoLoanInfo.TotalInstallments;
                db.sdtoLoanInfoes.Add(sdtoLoanInfo);
                db.SaveChanges();

                var member = db.User.Include(x => x.AccountHeadId).Where(x => x.UserID == sdtoLoanInfo.UserId).FirstOrDefault();
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
                                VoucherTotal = sdtoLoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                TransType = ReceiptType.Receipt,
                                FinYear = 1,
                                FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                TransId = sdtoLoanInfo.LoanId, //Doubt: Is transaction id loan id?
                                Cancelled = 0

                            };
                            db.ReceiptHeader.Add(receipt);
                            db.SaveChanges();

                            receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.ReceiptId + accCashBook.ReceiptVoucherSuffix;
                            db.Entry(receipt).State = EntityState.Modified;
                            db.SaveChanges();

                            var receiptDetails = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan amount received",
                                DbAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            db.ReceiptDetails.Add(receiptDetails);
                            db.SaveChanges();
                        }

                        var settingCashBookId = db.GeneralSettings.FirstOrDefault().CashBookId;
                        //Post for Bank book
                        var accBankBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountBookId == settingCashBookId).FirstOrDefault();
                        if (accBankBook != null)
                        {
                            var receipt = new sdtoReceiptHeader()
                            {
                                BookId = accBankBook.AccountBookId,
                                TransDate = DateTime.Now,
                                VoucherTotal = sdtoLoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                TransType = ReceiptType.Receipt,
                                FinYear = 1,
                                FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                Transaction = TransactionType.LoanEntry, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                TransId = sdtoLoanInfo.LoanId, //Doubt: Is transaction id loan id?
                                Cancelled = 0

                            };
                            db.ReceiptHeader.Add(receipt);
                            db.SaveChanges();

                            receipt.VoucherNo = accBankBook.ReceiptVoucherPrefix + receipt.ReceiptId + accBankBook.ReceiptVoucherSuffix;
                            db.Entry(receipt).State = EntityState.Modified;
                            db.SaveChanges();

                            var receiptDetails = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan dispatched",
                                CrAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            db.ReceiptDetails.Add(receiptDetails);
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            var listUsers = db.User.Where(x => x.UserType == UserType.Member);
            var users = listUsers.Select(x => new SelectListItem() { Value = x.UserID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            users.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Member" });
            ViewBag.UserList = new SelectList(users, "Value", "Text");
            return View(sdtoLoanInfo);
        }

        // GET: Loan/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }

            var listUsers = db.User.Where(x => x.UserType == UserType.Member);
            var users = listUsers.Select(x => new SelectListItem() { Value = x.UserID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            users.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Member" });
            //ViewBag.UserList = new SelectList(users, "Value", "Text");
            ViewBag.UserList = new SelectList(listUsers.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(sdtoLoanInfo);
        }

        // POST: Loan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanId,UserId,RepaymentStartDate,RePaymentInterval,RequestedAmount,ProposedAmount,LoanAmount,TotalInstallments,Status,ChequeDetails,InteresRate,SanctionedDate,SanctionedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] sdtoLoanInfo sdtoLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo.InstallmentAmount = sdtoLoanInfo.LoanAmount / sdtoLoanInfo.TotalInstallments;
                db.Entry(sdtoLoanInfo).State = EntityState.Modified;
                db.SaveChanges();

                var member = db.User.Include(x => x.AccountHeadId).Where(x => x.UserID == sdtoLoanInfo.UserId).FirstOrDefault();
                if (member != null)
                {
                    var accHead = member.AccountHead;
                    if (accHead != null)
                    {
                        // Post for member cash book
                        var accCashBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Cash", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (accCashBook != null)
                        {
                            var header = db.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && x.BookId == accCashBook.AccountBookId && x.TransId == sdtoLoanInfo.LoanId && x.Transaction == TransactionType.LoanEntry).FirstOrDefault();

                            header.Cancelled = 1;
                            db.Entry(header).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        var accBankBook = db.AccountBooks.Include(x => x.AccountBookTypeId).Where(x => x.AccountHeadId == accHead.AccountHeadId && x.AccountBookType.UniqueName.Equals("Bank", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (accBankBook != null)
                        {
                            var header = db.ReceiptHeader.Where(x => x.IsDeleted == false && x.Cancelled == 0 && x.BookId == accBankBook.AccountBookId && x.TransId == sdtoLoanInfo.LoanId && x.Transaction == TransactionType.LoanEntry).FirstOrDefault();

                            header.Cancelled = 1;
                            db.Entry(header).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                //var member = db.User.Include(x => x.AccountHeadId).Where(x => x.UserID == sdtoLoanInfo.UserId).FirstOrDefault();
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
                                VoucherTotal = sdtoLoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                TransType = ReceiptType.Receipt,
                                FinYear = 1,
                                FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                Transaction = TransactionType.LoanRepayment, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                TransId = sdtoLoanInfo.LoanId, //Doubt: Is transaction id loan id?
                                Cancelled = 0

                            };
                            db.ReceiptHeader.Add(receipt);
                            db.SaveChanges();

                            receipt.VoucherNo = accCashBook.ReceiptVoucherPrefix + receipt.ReceiptId + accCashBook.ReceiptVoucherSuffix;
                            db.Entry(receipt).State = EntityState.Modified;
                            db.SaveChanges();

                            var receiptDetails = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan amount received",
                                DbAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            db.ReceiptDetails.Add(receiptDetails);
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
                                VoucherTotal = sdtoLoanInfo.LoanAmount, //Doubt: Voucher total should be loan amount or loan amount + additional value from user
                                TransType = ReceiptType.Receipt,
                                FinYear = 1,
                                FromModule = 1,  //Doubt: 0 for "From Accounts", 1 for "From Posting"
                                Transaction = TransactionType.LoanEntry, //Doubt: //0 for Cash Receipt, 1 for Cash Payment, 2 for "Loan Entry", 3 for "Loan repayment"
                                TransId = sdtoLoanInfo.LoanId, //Doubt: Is transaction id loan id?
                                Cancelled = 0

                            };
                            db.ReceiptHeader.Add(receipt);
                            db.SaveChanges();

                            receipt.VoucherNo = accBankBook.ReceiptVoucherPrefix + receipt.ReceiptId + accBankBook.ReceiptVoucherSuffix;
                            db.Entry(receipt).State = EntityState.Modified;
                            db.SaveChanges();

                            var receiptDetails = new sdtoReceiptDetails()
                            {
                                ReceiptId = receipt.ReceiptId,
                                AccountId = accHead.AccountHeadId,
                                Narration = "Loan dispatched",
                                CrAmount = sdtoLoanInfo.LoanAmount, //Doubt: Is it debit or credit
                                Display = 1
                            };

                            db.ReceiptDetails.Add(receiptDetails);
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            var listUsers = db.User.Where(x => x.UserType == UserType.Member);
            var users = listUsers.Select(x => new SelectListItem() { Value = x.UserID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            users.Insert(0, new SelectListItem() { Value = "0", Text = "Select a Member" });
            ViewBag.UserList = new SelectList(users, "Value", "Text");
            return View(sdtoLoanInfo);
        }

        // GET: Loan/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            db.sdtoLoanInfoes.Remove(sdtoLoanInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LoanCancellation(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanRepayment repay = db.sdtoLoanRepayments.Where(x => x.LoanId == id).FirstOrDefault();
            ViewData["PaidAmount"] = repay.RepaymentAmount;
            ViewData["BalaceAmount"] = repay.PendingPrincipalAmount;

            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanCancellation(sdtoLoanInfo objLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo sdtoloanInfo = db.sdtoLoanInfoes.Find(objLoanInfo.LoanId);
                sdtoloanInfo.Status = LoanStatus.Cancelled;
                sdtoloanInfo.Notes = objLoanInfo.Notes;
                db.Entry(sdtoloanInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoanRecall(long? id)
        {
            sdtoLoanRepayment repay = db.sdtoLoanRepayments.Where(x => x.LoanId == id).FirstOrDefault();
            ViewData["PaidAmount"] = repay.RepaymentAmount;
            ViewData["BalaceAmount"] = repay.PendingPrincipalAmount;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoLoanInfo sdtoLoanInfo = db.sdtoLoanInfoes.Find(id);
            if (sdtoLoanInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoLoanInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanRecall(sdtoLoanInfo objLoanInfo)
        {
            if (ModelState.IsValid)
            {
                sdtoLoanInfo sdtoloanInfo = db.sdtoLoanInfoes.Find(objLoanInfo.LoanId);
                sdtoloanInfo.Status = LoanStatus.Recalled;
                sdtoloanInfo.Notes = objLoanInfo.Notes;
                db.Entry(sdtoloanInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
