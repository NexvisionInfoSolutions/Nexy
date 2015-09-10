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
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            long cCompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                cCompanyId = sessionUser.CompanyId.Value;
            var cmpList = db.Companies.Where(x => x.IsDeleted == false).Select(x => new { CompanyId = x.CompanyId.ToString(), CompanyName = x.CompanyName.ToString() }).ToList();
            cmpList.Insert(0, new { CompanyId = "0", CompanyName = "Select a company" });
            sdtoViewReportFilter filter = new sdtoViewReportFilter() { CompanyId = cCompanyId.ToString() };

            //setup properties
            var modelAccountHeads = new MutipleSelectionModel();
            var selectedAccounts = new List<MutipleSelectionItem>();
            //setup a view model
            modelAccountHeads.Items = db.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            // model.SelectedFruits = selectedFruits;            
            filter.Accounts = modelAccountHeads;

            filter.Companies = new SelectList(cmpList, "CompanyId", "CompanyName", cCompanyId);
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