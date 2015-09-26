using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        LoanDBContext db = new LoanDBContext();
        public ActionResult NewDesign()
        {
            return View();
        }

        public ActionResult Index()
        {
            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            long cCompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                cCompanyId = sessionUser.CompanyId.Value;
            var cmpList = db.Companies.Where(x => x.IsDeleted == false).Select(x => new { CompanyId = x.CompanyId.ToString(), CompanyName = x.CompanyName.ToString() }).ToList();
            cmpList.Insert(0, new { CompanyId = "0", CompanyName = "Select a company" });
            sdtoViewReportFilter filter = new sdtoViewReportFilter() { CompanyId = cCompanyId.ToString() };

            //setup properties
            var modelAccountHeads = new MultipleSelectionModel();
            var selectedAccounts = new List<MutipleSelectionItem>();
            //setup a view model
            modelAccountHeads.Items = db.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            // model.SelectedFruits = selectedFruits;            
            filter.Accounts = modelAccountHeads;

            filter.Companies = new SelectList(cmpList, "CompanyId", "CompanyName", cCompanyId);

            var membersMultiModel = new MultipleSelectionModel();
            membersMultiModel.Items = db.User.Where(x => x.UserType == Data.Models.Enumerations.UserType.Member).Select(x => new MutipleSelectionItem() { Value = x.UserID.ToString(), Text = x.FirstName + " " + x.LastName }).ToList();
            filter.Members = membersMultiModel;

            var statusMultiModel = new MultipleSelectionModel();
            List<MutipleSelectionItem> statusItems = new List<MutipleSelectionItem>();
            statusItems.Add(new MutipleSelectionItem() { Text = Data.Models.Enumerations.LoanStatus.Active.ToString(), Value = Convert.ToInt16(Data.Models.Enumerations.LoanStatus.Active).ToString() });
            statusItems.Add(new MutipleSelectionItem() { Text = Data.Models.Enumerations.LoanStatus.Approved.ToString(), Value = Convert.ToInt16(Data.Models.Enumerations.LoanStatus.Approved).ToString() });
            statusItems.Add(new MutipleSelectionItem() { Text = Data.Models.Enumerations.LoanStatus.Cancelled.ToString(), Value = Convert.ToInt16(Data.Models.Enumerations.LoanStatus.Cancelled).ToString() });
            statusItems.Add(new MutipleSelectionItem() { Text = Data.Models.Enumerations.LoanStatus.Completed.ToString(), Value = Convert.ToInt16(Data.Models.Enumerations.LoanStatus.Completed).ToString() });
            statusItems.Add(new MutipleSelectionItem() { Text = Data.Models.Enumerations.LoanStatus.Inactive.ToString(), Value = Convert.ToInt16(Data.Models.Enumerations.LoanStatus.Inactive).ToString() });
            statusItems.Add(new MutipleSelectionItem() { Text = Data.Models.Enumerations.LoanStatus.Recalled.ToString(), Value = Convert.ToInt16(Data.Models.Enumerations.LoanStatus.Recalled).ToString() });
            statusMultiModel.Items = statusItems;
            filter.Status = statusMultiModel;

            var loansMultiModel = new MultipleSelectionModel();
            loansMultiModel.Items = db.sdtoLoanInfoes.Where(x => x.IsDeleted == false && x.Status == Data.Models.Enumerations.LoanStatus.Active).Select(x => new MutipleSelectionItem() { Value = x.LoanId.ToString(), Text = x.LoanId.ToString() }).ToList(); //+ " [" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            filter.Loans = loansMultiModel;

            var depositsMultiModel = new MultipleSelectionModel();
            depositsMultiModel.Items = db.sdtoDepositInfoes.Where(x => x.IsDeleted == false && x.Status == Data.Models.Enumerations.DepositStatus.Active).Select(x => new MutipleSelectionItem() { Value = x.DepositId.ToString(), Text = x.DepositId.ToString() }).ToList(); //+ " [" + x.Member.FirstName + " " + x.Member.LastName + "]" }).ToList();
            filter.Loans = depositsMultiModel;

            return View(filter);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}