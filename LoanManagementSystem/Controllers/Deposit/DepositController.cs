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

namespace LoanManagementSystem.Controllers.Deposit
{
    public class DepositController : Controller
    {
        private LoanDBContext db = new LoanDBContext();

        // GET: sdtoDepositInfoes
        public ActionResult Index()
        {
            var sdtoDepositInfoes = db.sdtoDepositInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser);
            return View(sdtoDepositInfoes.ToList());
        }

        // GET: LoanRepayments
        public ActionResult Withdrawals(long? DepositId)
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

            sdtoWithdrawalInfo withdrawal = new sdtoWithdrawalInfo();
            var itemsDeposit = db.sdtoDepositInfoes.Include(x => x.Member).Where(x => x.Status == DepositStatus.Active).ToList();
            var itemsDeposits = itemsDeposit.Select(x => new SelectListItem() { Value = x.DepositId.ToString(), Text = x.DepositId + " - " + x.DepositAmount + " [" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsDeposits.Insert(0, new SelectListItem() { Value = "0", Text = "Select a deposit" });
            ViewBag.DepositList = new SelectList(itemsDeposits, "Value", "Text");

            ViewBag.DepositDetails = db.sdtoDepositInfoes.Where(x => x.DepositId == DepositId).FirstOrDefault();

            var sdtoWidthdrawals = db.DepositWithdrawals.Where(x => x.DepositId == DepositId).Include(s => s.Deposit);
            if (sdtoWidthdrawals != null && sdtoWidthdrawals.Count() > 0)
            {
                ViewBag.TotalWithdrawan = sdtoWidthdrawals.Sum(y => y.WithdrawalAmount);
                ViewBag.TotalBalance = sdtoWidthdrawals.OrderByDescending(x => x.WithdrawalId).FirstOrDefault().BalanceDepositAmount;
                ViewBag.NewInterestRate = sdtoWidthdrawals.OrderByDescending(x => x.WithdrawalId).FirstOrDefault().NewInterestRate;
            }
            return View(sdtoWidthdrawals.ToList());
        }

        [HttpGet]
        public ActionResult WithdrawAmount(int? DepositId)
        {
            sdtoWithdrawalInfo withdraw = new sdtoWithdrawalInfo();
            var itemsDeposit = db.sdtoDepositInfoes.Include(x => x.Member).Where(x => x.Status == DepositStatus.Active).ToList();
            var itemsDeposits = itemsDeposit.Select(x => new SelectListItem() { Value = x.DepositId.ToString(), Text = x.DepositId + " - " + x.DepositAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsDeposits.Insert(0, new SelectListItem() { Value = "0", Text = "Select a deposit" });
            ViewBag.DepositList = new SelectList(itemsDeposits, "Value", "Text");

            if (DepositId != null && DepositId.Value > 0)
            {
                var depositDetails = db.sdtoDepositInfoes.Find(DepositId);

                var depositBalanceAmt = depositDetails.DepositAmount;
                var depositInterest = depositDetails.InteresRate;
                //var loanPendingInstallments = depositDetails.TotalInstallments;

                var withdrawalInterest = depositInterest;
                int days = (DateTime.Now - depositDetails.CreatedOn.Value).Days;
                var withdrawalInterestAmt = ((depositDetails.DepositAmount * Convert.ToDecimal(withdrawalInterest / 100) * (days < depositDetails.Duration ? days : depositDetails.Duration)) / 365);

                var depositWithdrawals = db.DepositWithdrawals.Where(x => x.DepositId == DepositId).OrderByDescending(x => x.WithdrawalId).FirstOrDefault();

                if (depositWithdrawals != null && depositWithdrawals.WithdrawalId > 0)
                {
                    depositBalanceAmt = depositWithdrawals.BalanceDepositAmount;
                    //loanPendingInstallments = loanRepayment.PendingInstallments;
                    withdrawalInterest = depositWithdrawals.NewInterestRate;

                    withdrawalInterestAmt = ((depositBalanceAmt * Convert.ToDecimal(withdrawalInterest / 100) * (days < depositDetails.Duration ? days : depositDetails.Duration)) / 365);
                    withdraw.WithdrawalCode = depositWithdrawals.WithdrawalCode;
                }

                withdraw.InterestRate = withdrawalInterest;
                withdraw.NewInterestRate = withdrawalInterest;
                withdraw.InterestAmount = Math.Round(withdrawalInterestAmt, 2);
                withdraw.BalanceDepositAmount = depositBalanceAmt;
                //withdraw.PendingInstallments = loanPendingInstallments;
                //withdraw.BalanceDepositAmount = loandetails.LoanAmount;

                withdraw.Status = WithdrawalStatus.Paid;
                withdraw.PaymentMode = ModeOfPayment.Cash;

                withdraw.WithdrawalAmount = withdraw.BalanceDepositAmount + withdraw.InterestAmount;
            }

            return View(withdraw);
        }

        // POST: LoanRepayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WithdrawAmount(//[Bind(Include = "LoanRepaymentId,LoanId,RepaymentCode,PrincipalAmount,InterestAmount,InterestRate,RepaymentAmount,PendingPrincipalAmount,Status,PaymentMode,ChequeDetails,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] 
            sdtoWithdrawalInfo depositWithdrawal)
        {
            if (ModelState.IsValid)
            {
                if (depositWithdrawal.DepositId > 0)
                {
                    var depositDetails = db.sdtoDepositInfoes.Find(depositWithdrawal.DepositId);

                    var depositBalanceAmt = depositDetails.DepositAmount;
                    var depositInterest = depositDetails.InteresRate;
                    //var loanPendingInstallments = depositDetails.TotalInstallments;
                    
                    int days = (DateTime.Now - depositDetails.CreatedOn.Value).Days;

                    var withdrawalInterest = depositInterest;
                    var withdrawalInterestAmt = ((depositDetails.DepositAmount * Convert.ToDecimal(withdrawalInterest / 100) * (days < depositDetails.Duration ? days : depositDetails.Duration)) / 365);

                    var depositWithdrawalsPrev = db.DepositWithdrawals.Where(x => x.DepositId == depositWithdrawal.DepositId).OrderByDescending(x => x.WithdrawalId).FirstOrDefault();

                    if (depositWithdrawalsPrev != null && depositWithdrawalsPrev.WithdrawalId > 0)
                    {
                        depositBalanceAmt = depositWithdrawalsPrev.BalanceDepositAmount;
                        //loanPendingInstallments = loanRepayment.PendingInstallments;
                        withdrawalInterest = depositWithdrawalsPrev.NewInterestRate;
                    }

                    withdrawalInterestAmt = ((depositWithdrawal.WithdrawalAmount * Convert.ToDecimal(withdrawalInterest / 100) * (days < depositDetails.Duration ? days : depositDetails.Duration)) / 365);

                    depositWithdrawal.InterestAmount = Math.Round(withdrawalInterestAmt, 2);
                    depositWithdrawal.BalanceDepositAmount = depositBalanceAmt - depositWithdrawal.WithdrawalAmount;
                    //sdtoLoanRepayment.PendingInstallments -= Convert.ToInt32(Math.Floor(sdtoLoanRepayment.PendingPrincipalAmount / loandetails.InstallmentAmount));
                    //sdtoLoanRepayment.PrincipalAmount = loandetails.LoanAmount;

                    depositWithdrawal.Status = Data.Models.Enumerations.WithdrawalStatus.Paid;
                }

                db.DepositWithdrawals.Add(depositWithdrawal);
                db.SaveChanges();
                return RedirectToAction("Withdrawals", new { DepositId = depositWithdrawal.DepositId });
            }

            var itemsDeposit = db.sdtoDepositInfoes.Include(x => x.Member).Where(x => x.Status == DepositStatus.Active).ToList();
            var itemsDeposits = itemsDeposit.Select(x => new SelectListItem() { Value = x.DepositId.ToString(), Text = x.DepositId + " - " + x.DepositAmount + "[" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            itemsDeposits.Insert(0, new SelectListItem() { Value = "0", Text = "Select a deposit" });
            ViewBag.DepositList = new SelectList(itemsDeposits, "Value", "Text");


            return View(depositWithdrawal);
        }

        public JsonResult DepositInfo()
        {
            var dbResult = db.sdtoDepositInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser).ToList();
            var Deposits = (from Deposit in dbResult
                            select new
                            {
                                Deposit.DepositId,
                                Deposit.RecurringDepositDate,
                                Deposit.DepositCode,
                                Deposit.DepositAmount,
                                Deposit.InstallmentAmount,
                                Deposit.InteresRate,
                                DepositInfo = Deposit.DepositId,
                                Member = Deposit.Member.FirstName + " " + Deposit.Member.LastName
                            });
            return Json(Deposits, JsonRequestBehavior.AllowGet);
        }


        // GET: sdtoDepositInfoes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            if (sdtoDepositInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoDepositInfo);
        }

        // GET: sdtoDepositInfoes/Create
        public ActionResult Create(long? UserId)
        {

            var deposit = new sdtoDepositInfo();
            deposit.DepositType = Data.Models.Enumerations.DepositType.Fixed;
            deposit.Status = Data.Models.Enumerations.DepositStatus.Active;
            deposit.InteresRate = db.GeneralSettings.FirstOrDefault().BankInterest;
            deposit.Duration = 30;
            deposit.IsDeleted = false;
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            deposit.CreatedBy = sessionUser != null ? sessionUser.UserID : 0;
            deposit.CreatedOn = DateTime.Now;

            var listUsers = db.User.Where(x => x.UserType == UserType.Member && (UserId == null || UserId.Value == 0 || x.UserID == UserId));

            if (listUsers == null || listUsers.Count(x => x.UserID > 0) == 0)
                return RedirectToAction("Create", "Member");

            ViewBag.UserList = new SelectList(listUsers.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(deposit);
        }

        // POST: sdtoDepositInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(//[Bind(Include = "DepositId,UserId,Duration,DepositType,MaturityDate,TotalInstallments,DepositAmount,MatureAmount,InstallmentAmount,ClosedDate,RecurringDepositDate,Status,ChequeDetails,InteresRate,ApprovedDate,ApprovedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] 
            sdtoDepositInfo depositInfo)
        {
            depositInfo.DepositType = Data.Models.Enumerations.DepositType.Fixed;
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            depositInfo.IsDeleted = false;
            depositInfo.CreatedBy = sessionUser != null ? sessionUser.UserID : 0;
            depositInfo.CreatedOn = DateTime.Now;
            depositInfo.MatureAmount = depositInfo.DepositAmount + ((depositInfo.DepositAmount * Convert.ToDecimal(depositInfo.InteresRate / 100) * depositInfo.Duration) / 365);
            depositInfo.MaturityDate = depositInfo.CreatedOn.Value.AddDays(depositInfo.Duration);
            if (ModelState.IsValid)
            {
                db.sdtoDepositInfoes.Add(depositInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var listUsers = db.User.Where(x => x.UserType == UserType.Member);
            ViewBag.UserList = new SelectList(listUsers.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(depositInfo);
        }

        // GET: sdtoDepositInfoes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            if (sdtoDepositInfo == null)
            {
                return HttpNotFound();
            }
            var listUsers = db.User.Where(x => x.UserType == UserType.Member);
            ViewBag.UserList = new SelectList(listUsers.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(sdtoDepositInfo);
        }

        // POST: sdtoDepositInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(//[Bind(Include = "DepositId,UserId,Duration,DepositType,MaturityDate,TotalInstallments,DepositAmount,MatureAmount,InstallmentAmount,ClosedDate,RecurringDepositDate,Status,ChequeDetails,InteresRate,ApprovedDate,ApprovedBy,Notes,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,IsDeleted,DeletedBy,DeletedOn")] 
            sdtoDepositInfo depositInfo)
        {
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            depositInfo.ModifiedBy = sessionUser != null ? sessionUser.UserID : 0;
            depositInfo.ModifiedOn = DateTime.Now;
            depositInfo.MatureAmount = depositInfo.DepositAmount + ((depositInfo.DepositAmount * Convert.ToDecimal(depositInfo.InteresRate / 100) * depositInfo.Duration) / 365);
            depositInfo.MaturityDate = depositInfo.CreatedOn.Value.AddDays(depositInfo.Duration);
            if (ModelState.IsValid)
            {
                db.Entry(depositInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var listUsers = db.User.Where(x => x.UserType == UserType.Member);
            ViewBag.UserList = new SelectList(listUsers.Select(x => new { UserID = x.UserID, Name = x.FirstName + " " + x.LastName }), "UserID", "Name");
            return View(depositInfo);
        }

        // GET: sdtoDepositInfoes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            if (sdtoDepositInfo == null)
            {
                return HttpNotFound();
            }
            return View(sdtoDepositInfo);
        }

        // POST: sdtoDepositInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sdtoDepositInfo sdtoDepositInfo = db.sdtoDepositInfoes.Find(id);
            db.sdtoDepositInfoes.Remove(sdtoDepositInfo);
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
