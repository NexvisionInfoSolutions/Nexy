using Business.Reports;
using CrystalDecisions.CrystalReports.Engine;
using Data.Models.Accounts;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers.Reports
{
    public class ReportsController : Controller
    {
        LoanDBContext dbContext = new LoanDBContext();
        // GET: Reports
        public ActionResult LoanSummary()
        {
            return View();
        }

        public ActionResult DepositSummary()
        {
            return View();
        }

        public JsonResult LoanSummaryInfo(LoanManagementSystem.Models.sdtoViewReportFilter Filter)
        {
            Filter.DepositIds = GetList(Filter.DepositIds.FirstOrDefault());
            Filter.AccountIds = GetList(Filter.AccountIds.FirstOrDefault());
            Filter.LoanIds = GetList(Filter.LoanIds.FirstOrDefault());
            Filter.MemberIds = GetList(Filter.MemberIds.FirstOrDefault());
            Filter.StatusIds = GetList(Filter.StatusIds.FirstOrDefault());

            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
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

            foreach (string Id in Filter.MemberIds)
            {
                DataRow row = dtRptParams.NewRow();
                row.ItemArray = new object[] { Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "U" };
                dtRptParams.Rows.Add(row);
            }

            foreach (string Id in Filter.LoanIds)
            {
                DataRow row = dtRptParams.NewRow();
                row.ItemArray = new object[] { Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "L" };
                dtRptParams.Rows.Add(row);
            }

            foreach (string Id in Filter.StatusIds)
            {
                DataRow row = dtRptParams.NewRow();
                row.ItemArray = new object[] { Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "S" };
                dtRptParams.Rows.Add(row);
            }

            DataRow rowDate = dtRptParams.NewRow();
            rowDate.ItemArray = new object[] { 0, Filter.StartDate, Filter.EndDate, DBNull.Value, DBNull.Value, "D" };
            dtRptParams.Rows.Add(rowDate);

            DataRow rowMisc = dtRptParams.NewRow();
            rowMisc.ItemArray = new object[] { 0, DBNull.Value, DBNull.Value, DBNull.Value, Filter.MiscFilter, "M" };
            dtRptParams.Rows.Add(rowMisc);


            bfReport objReport = new bfReport(null);
            return Json(objReport.GetRptLoanSummary(CompanyId, dtRptParams), JsonRequestBehavior.AllowGet);
        }

        private List<string> GetList(string commaArray)
        {
            if (string.IsNullOrWhiteSpace(commaArray))
                return new List<string>();
            else
                return commaArray.Trim(", ".ToCharArray()).Split(", ".ToCharArray()).ToList();
        }

        public JsonResult DepositSummaryInfo(LoanManagementSystem.Models.sdtoViewReportFilter Filter)
        {
            Filter.DepositIds = GetList(Filter.DepositIds.FirstOrDefault());
            Filter.AccountIds = GetList(Filter.AccountIds.FirstOrDefault());
            Filter.LoanIds = GetList(Filter.LoanIds.FirstOrDefault());
            Filter.MemberIds = GetList(Filter.MemberIds.FirstOrDefault());
            Filter.StatusIds = GetList(Filter.StatusIds.FirstOrDefault());

            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
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

            foreach (string Id in Filter.MemberIds)
            {
                DataRow row = dtRptParams.NewRow();
                row.ItemArray = new object[] { Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "U" };
                dtRptParams.Rows.Add(row);
            }

            foreach (string Id in Filter.DepositIds)
            {
                DataRow row = dtRptParams.NewRow();
                row.ItemArray = new object[] { Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "L" };
                dtRptParams.Rows.Add(row);
            }

            foreach (string Id in Filter.StatusIds)
            {
                DataRow row = dtRptParams.NewRow();
                row.ItemArray = new object[] { Id, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, "S" };
                dtRptParams.Rows.Add(row);
            }

            DataRow rowDate = dtRptParams.NewRow();
            rowDate.ItemArray = new object[] { 0, Filter.StartDate, Filter.EndDate, DBNull.Value, DBNull.Value, "D" };
            dtRptParams.Rows.Add(rowDate);

            DataRow rowMisc = dtRptParams.NewRow();
            rowMisc.ItemArray = new object[] { 0, DBNull.Value, DBNull.Value, DBNull.Value, Filter.MiscFilter, "M" };
            dtRptParams.Rows.Add(rowMisc);

            bfReport objReport = new bfReport(null);
            return Json(objReport.GetRptDepositSummary(CompanyId, dtRptParams), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LedgerReport()
        {
            sdtoViewReportFilter filter = new sdtoViewReportFilter() { };

            //setup properties
            var modelAccountHeads = new MultipleSelectionModel();
            var selectedAccounts = new List<MutipleSelectionItem>();
            //setup a view model
            modelAccountHeads.Items = dbContext.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            // model.SelectedFruits = selectedFruits;            
            filter.Accounts = modelAccountHeads;

            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            long cCompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                cCompanyId = sessionUser.CompanyId.Value;
            var cmpList = dbContext.Companies.Where(x => x.IsDeleted == false).Select(x => new { CompanyId = x.CompanyId.ToString(), CompanyName = x.CompanyName.ToString() }).ToList();
            cmpList.Insert(0, new { CompanyId = "0", CompanyName = "Select a company" });
            filter.Companies = new SelectList(cmpList, "CompanyId", "CompanyName", cCompanyId);

            return View(filter);
        }

        private MultipleSelectionModel GetMutipleSelectionModel(PostedMutipleSelectionItems postedItems)
        {
            // setup properties
            var model = new MultipleSelectionModel();
            var selectedItems = new List<MutipleSelectionItem>();
            var postedItemsIds = new string[0];
            if (postedItems == null) postedItems = new PostedMutipleSelectionItems();

            // if a view model array of posted fruits ids exists
            // and is not empty,save selected ids
            if (postedItems.Ids != null && postedItems.Ids.Any())
            {
                postedItemsIds = postedItems.Ids;
            }
            var allItems = dbContext.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName });
            // if there are any selected ids saved, create a list of fruits
            if (postedItemsIds.Any())
            {
                selectedItems = allItems
                 .Where(x => postedItemsIds.Any(s => x.Value.Equals(s)))
                 .ToList();
            }

            //setup a view model
            model.Items = allItems.ToList();
            model.SelectedItems = selectedItems;
            model.PostedItems = postedItems;
            return model;
        }

        [HttpPost]
        public ActionResult LedgerReport(sdtoViewReportFilter filter)
        {
            var filterModel = GetMutipleSelectionModel(filter.Accounts.PostedItems);

            // ledger report -> account id
            // cash book -> account id (book type cash only)
            // bank book -> account (book type bank only)
            // trial balance

            long CompanyId = 0;
            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            if (sessionUser != null)
                CompanyId = sessionUser.CompanyId.Value;

            List<int> l = new List<int>();
            string.Join(",", l.Select(x => x));

            bfReport objReport = new bfReport(null);
            List<sdtoLedgerReport> lst = new List<sdtoLedgerReport>();
            lst = objReport.GetRptLedgerReport(Convert.ToInt64(filter.CompanyId), filter.OperationId, filter.StartDate, filter.EndDate, filter.Accounts != null && filter.Accounts.PostedItems != null && filter.Accounts.PostedItems.Ids != null ? string.Join(",", filter.Accounts.PostedItems.Ids.Select(x => x)) : "");

            //setup properties
            var modelAccountHeads = new MultipleSelectionModel();
            var selectedAccounts = new List<MutipleSelectionItem>();
            //setup a view model
            modelAccountHeads.Items = dbContext.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            // model.SelectedFruits = selectedFruits;            
            filter.Accounts = modelAccountHeads;

            long cCompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                cCompanyId = sessionUser.CompanyId.Value;
            var cmpList = dbContext.Companies.Where(x => x.IsDeleted == false).ToList();
            cmpList.Insert(0, new sdtoCompany() { CompanyId = 0, CompanyName = "Select a company" });
            filter.Companies = new SelectList(cmpList, "CompanyId", "CompanyName", cCompanyId);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "LedgerReport.rpt"));
            //rd.SetParameterValue("Heading", "Ledger Report");
            // rd.SetDatabaseLogon("sa", "saat1234", @".\SQLEXPRESS", "LoanManagement");
            string strConnection = ConfigurationManager.ConnectionStrings["LoanDBContext"].ConnectionString;
            System.Data.SqlClient.SqlConnection obj = new System.Data.SqlClient.SqlConnection(strConnection);
            string uName = ConfigurationManager.AppSettings["DbUserName"];
            string uPassword = ConfigurationManager.AppSettings["DbPassword"];
            rd.SetDatabaseLogon(uName, uPassword, obj.DataSource, obj.Database);
            rd.SetDataSource(lst);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //System.IO.MemoryStream mem = (System.IO.MemoryStream)rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.BinaryWrite(mem.ToArray());

                Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                str.Seek(0, SeekOrigin.Begin);
                return File(str, "application/pdf", "ReportLedger.pdf");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

            return View(filter);
        }


        public ActionResult TrialBalance()
        {
            sdtoViewReportFilter filter = new sdtoViewReportFilter() { };

            //setup properties
            var modelAccountHeads = new MultipleSelectionModel();
            var selectedAccounts = new List<MutipleSelectionItem>();
            //setup a view model
            modelAccountHeads.Items = dbContext.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            // model.SelectedFruits = selectedFruits;            
            filter.Accounts = modelAccountHeads;

            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            long cCompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                cCompanyId = sessionUser.CompanyId.Value;
            var cmpList = dbContext.Companies.Where(x => x.IsDeleted == false).Select(x => new { CompanyId = x.CompanyId.ToString(), CompanyName = x.CompanyName.ToString() }).ToList();
            cmpList.Insert(0, new { CompanyId = "0", CompanyName = "Select a company" });
            filter.Companies = new SelectList(cmpList, "CompanyId", "CompanyName", cCompanyId);

            return View(filter);
        }

        [HttpPost]
        public ActionResult TrialBalance(sdtoViewReportFilter filter)
        {
            //sdtoLedgerReport sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoLedgerReport;
            //long CompanyId = 0;

            //bfReport objReport = new bfReport(null);
            //List<sdtoLedgerReport> lst = new List<sdtoLedgerReport>();
            //lst = objReport.GetRptLedgerReport(CompanyId, 1, DateTime.Now.Date.AddDays(-20), DateTime.Now, "1,2,3,4,5,6,7,8,9,10").ToList();

            //ReportDocument rd = new ReportDocument();
            //rd.Load(Path.Combine(Server.MapPath("~/Report"), "TrailBalance.rpt"));
            ////rd.SetParameterValue("Heading", "Ledger Report");
            //rd.SetDatabaseLogon("sa", "TechnoCrunchLabs", @"DELL-PC\SQLEXPRESS", "LoanManagement");
            //rd.SetDataSource(lst);
            //Response.Buffer = false;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //try
            //{
            //    System.IO.MemoryStream mem = (System.IO.MemoryStream)rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/pdf";
            //    Response.BinaryWrite(mem.ToArray());

            //    Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //    str.Seek(0, SeekOrigin.Begin);
            //    return File(str, "application/pdf", "TrialBalance.pdf");
            //}
            //catch (Exception e)
            //{
            //    throw e.InnerException;
            //}

            var filterModel = GetMutipleSelectionModel(filter.Accounts.PostedItems);

            // ledger report -> account id
            // cash book -> account id (book type cash only)
            // bank book -> account (book type bank only)
            // trial balance

            long CompanyId = 0;
            sdtoUser sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser) as sdtoUser;
            if (sessionUser != null)
                CompanyId = sessionUser.CompanyId.Value;

            List<int> l = new List<int>();
            string.Join(",", l.Select(x => x));

            bfReport objReport = new bfReport(null);
            List<sdtoLedgerReport> lst = new List<sdtoLedgerReport>();
            lst = objReport.GetRptLedgerReport(Convert.ToInt64(filter.CompanyId), filter.OperationId, filter.StartDate, filter.EndDate, filter.Accounts != null && filter.Accounts.PostedItems != null && filter.Accounts.PostedItems.Ids != null ? string.Join(",", filter.Accounts.PostedItems.Ids.Select(x => x)) : "");

            //setup properties
            var modelAccountHeads = new MultipleSelectionModel();
            var selectedAccounts = new List<MutipleSelectionItem>();
            //setup a view model
            modelAccountHeads.Items = dbContext.AccountHeads.Select(x => new MutipleSelectionItem() { Value = x.AccountHeadId.ToString(), Text = x.AccountName }).ToList();
            // model.SelectedFruits = selectedFruits;            
            filter.Accounts = modelAccountHeads;

            long cCompanyId = 0;
            if (sessionUser != null && sessionUser.CompanyId != null)
                cCompanyId = sessionUser.CompanyId.Value;
            var cmpList = dbContext.Companies.Where(x => x.IsDeleted == false).ToList();
            cmpList.Insert(0, new sdtoCompany() { CompanyId = 0, CompanyName = "Select a company" });
            filter.Companies = new SelectList(cmpList, "CompanyId", "CompanyName", cCompanyId);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "TrailBalance.rpt"));
            //rd.SetParameterValue("Heading", "Ledger Report");
            string strConn = ConfigurationManager.ConnectionStrings["LoanDBContext"].ConnectionString;
            System.Data.SqlClient.SqlConnection obj = new System.Data.SqlClient.SqlConnection(strConn);
            string uName = ConfigurationManager.AppSettings["DbUserName"];
            string uPassword = ConfigurationManager.AppSettings["DbPassword"];
            rd.SetDatabaseLogon(uName, uPassword, obj.DataSource, obj.Database);

            rd.SetDataSource(lst);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                //System.IO.MemoryStream mem = (System.IO.MemoryStream)rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.BinaryWrite(mem.ToArray());

                Stream str = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                str.Seek(0, SeekOrigin.Begin);
                return File(str, "application/pdf", "TrialBalance.pdf");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

            return View(filter);
        }
    }
}