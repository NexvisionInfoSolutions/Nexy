using Business.Reports;
using Data.Models.Accounts;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

        public ActionResult LedgerReport()
        {
            // ledger report -> account id
            // cash book -> account id (book type cash only)
            // bank book -> account (book type bank only)
            // trial balance
            sdtoLedgerReport sessionUser = Session["UserDetails"] as sdtoLedgerReport;
            long CompanyId = 0;

            bfReport objReport = new bfReport(null);
            List<sdtoLedgerReport> lst = new List<sdtoLedgerReport>();
            lst = objReport.GetRptLedgerReport(CompanyId, 0, DateTime.Now.Date.AddDays(-20), DateTime.Now, "1,2,3,4,5,6,7,8,9,10").ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "LedgerReport.rpt"));
            //rd.SetParameterValue("Heading", "Ledger Report");
            rd.SetDatabaseLogon("sa", "TechnoCrunchLabs", @"DELL-PC\SQLEXPRESS", "LoanManagement");
            rd.SetDataSource(lst);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                System.IO.MemoryStream mem = (System.IO.MemoryStream)rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(mem.ToArray());

                Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                str.Seek(0, SeekOrigin.Begin);
                return File(str, "application/pdf", "ReportLedger.pdf");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public ActionResult TrialBalance()
        {
            sdtoLedgerReport sessionUser = Session["UserDetails"] as sdtoLedgerReport;
            long CompanyId = 0;

            bfReport objReport = new bfReport(null);
            List<sdtoLedgerReport> lst = new List<sdtoLedgerReport>();
            lst = objReport.GetRptLedgerReport(CompanyId, 1, DateTime.Now.Date.AddDays(-20), DateTime.Now, "1,2,3,4,5,6,7,8,9,10").ToList();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "TrailBalance.rpt"));
            //rd.SetParameterValue("Heading", "Ledger Report");
            rd.SetDatabaseLogon("sa", "TechnoCrunchLabs", @"DELL-PC\SQLEXPRESS", "LoanManagement");
            rd.SetDataSource(lst);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                System.IO.MemoryStream mem = (System.IO.MemoryStream)rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(mem.ToArray());

                Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                str.Seek(0, SeekOrigin.Begin);
                return File(str, "application/pdf", "TrialBalance.pdf");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}