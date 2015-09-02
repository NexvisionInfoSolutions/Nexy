using Business.Reports;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers.Reports
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult LoanSummary()
        {
            return View();
        }

        public ActionResult DepositSummary()
        {
            return View();
        }

        public JsonResult LoanSummaryInfo()
        {
            sdtoUser sessionUser = Session["UserDetails"] as sdtoUser;
            long CompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId!=null)
                CompanyId = sessionUser.CompanyId.Value;

            DataTable dtRptParams = new DataTable();
            dtRptParams.Columns.Add(new DataColumn("EntityId", typeof(long)));
            dtRptParams.Columns.Add(new DataColumn("EntityStartDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityEndDate", typeof(DateTime)));
            dtRptParams.Columns.Add(new DataColumn("EntityIntVal", typeof(int)));
            dtRptParams.Columns.Add(new DataColumn("EntityStrVal", typeof(string)));
            dtRptParams.Columns.Add(new DataColumn("EntityType", typeof(string)));

            bfReport objReport = new bfReport(null);
            return Json(objReport.GetRptLoanSummary(CompanyId, dtRptParams), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DepositSummaryInfo()
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
            return Json(objReport.GetRptDepositSummary(CompanyId, dtRptParams), JsonRequestBehavior.AllowGet);
        }
    }
}