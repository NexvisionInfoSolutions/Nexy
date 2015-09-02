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
        [ValidateAntiForgeryToken]
        public ActionResult ImportView(HttpPostedFileBase DeviceInput)
        {
            List<sdtoLoanRepayment> lstData = new List<sdtoLoanRepayment>();
            sdtoLoanRepayment objLoanRepayment;  

            if (DeviceInput != null)
            {
                string arrayofdata = string.Empty;
                string read = string.Empty;// new StreamReader(DeviceInput.InputStream).ReadLine();
                 
                StreamReader csvreader = new StreamReader(DeviceInput.InputStream);
                while (!csvreader.EndOfStream)
                {
                    var line = csvreader.ReadLine();
                    var values = line.Split(',');
                    objLoanRepayment = new sdtoLoanRepayment();
                    objLoanRepayment.LoanId = Convert.ToInt64(values[2]);
                    var loandetails = db.sdtoLoanInfoes.Find(objLoanRepayment.LoanId);
                    
                    if (loandetails!=null)       
                    {
                        objLoanRepayment.RepaymentAmount = Convert.ToInt64(values[3]);
                        objLoanRepayment.RepaymentDate = Convert.ToDateTime(values[8]);
                        var loanPendingAmt = loandetails.LoanAmount;
                        var loanInterest = loandetails.InteresRate;
                        var loanPendingInstallments = loandetails.TotalInstallments;

                        var loanRepayment = db.sdtoLoanRepayments.Where(x => x.LoanId == objLoanRepayment.LoanId).OrderByDescending(x => x.LoanRepaymentId).FirstOrDefault();
                        var repaymentInterest = loanInterest;
                        var repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;

                        if (loanRepayment != null && loanRepayment.LoanRepaymentId > 0)
                        {
                            loanPendingAmt = loanRepayment.PendingPrincipalAmount;
                            loanPendingInstallments = loanRepayment.PendingInstallments;
                            repaymentInterest = loanRepayment.InterestRate;

                            repaymentInterestAmt = (loanPendingAmt * Convert.ToDecimal(repaymentInterest / 100)) / 365;
                            objLoanRepayment.RepaymentCode = loanRepayment.RepaymentCode;
                        }

                        objLoanRepayment.InterestAmount = Math.Round(repaymentInterestAmt, 2);
                        objLoanRepayment.PendingPrincipalAmount = loanPendingAmt - (objLoanRepayment.RepaymentAmount - repaymentInterestAmt);
                        objLoanRepayment.PendingInstallments -= Convert.ToInt32(Math.Floor(objLoanRepayment.PendingPrincipalAmount / loandetails.InstallmentAmount));
                        objLoanRepayment.PrincipalAmount = loandetails.LoanAmount;

                        objLoanRepayment.Status = Data.Models.Enumerations.RepaymentStatus.Paid;
                        objLoanRepayment.CreatedOn = DateTime.Now;

                        db.sdtoLoanRepayments.Add(objLoanRepayment);
                        db.SaveChanges(); 
                    }
                    
                }
                 
            }

            return View(lstData);
        }

        public FileStreamResult Export()
        {
            var sdtoLoanInfoes = db.sdtoLoanInfoes.Include(s => s.CreatedByUser).Include(s => s.DeletedByUser).Include(s => s.Member).Include(s => s.ModifiedByUser).Where(x => x.IsDeleted == false && x.Status == LoanStatus.Active);
            string HeaderData = "";
            string Data = "";
            HeaderData = "Member ID" + "  " + "Name of customer" + "  " + "STOL" + "  " + "Amount" + "  " + "Interest" + Environment.NewLine;
            Data = HeaderData;
            foreach (sdtoLoanInfo Linfo in sdtoLoanInfoes)
            {
                Data += Linfo.Member.UserID.ToString() + " " + Linfo.Member.FirstName + " " + "STOL" + " " + Linfo.LoanAmount.ToString() + " " + Linfo.InteresRate.ToString() + Environment.NewLine;
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
