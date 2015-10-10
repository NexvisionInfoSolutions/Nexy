using Business.Base;
using Data.Models.Accounts;
using LoanManagementSystem.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Reports
{
    public class bfReport : bfBase
    {
        public bfReport(DbContext dbConnection) : base(dbConnection) { }

        public List<sdtoBankDepositWithdrawal> GetBankDepositWithdrawal(long CompanyId, string FilterValue)
        {
            List<sdtoBankDepositWithdrawal> rptCollection = new List<sdtoBankDepositWithdrawal>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@vFilterValue", SqlDbType.VarChar);
                pm.Value = FilterValue;
                DbRawSqlQuery<sdtoBankDepositWithdrawal> result = AppDb.Database.SqlQuery<sdtoBankDepositWithdrawal>("usp_GetBankDepositWithdrawal @vFilterValue, @vCompanyId",
                    pm,
                    new SqlParameter("@vCompanyId", CompanyId));

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public List<sdtoCashReceiptPayment> GetCashReceiptPayments(long CompanyId, string FilterValue)
        {
            List<sdtoCashReceiptPayment> rptCollection = new List<sdtoCashReceiptPayment>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@vFilterValue", SqlDbType.VarChar);
                pm.Value = FilterValue;
                DbRawSqlQuery<sdtoCashReceiptPayment> result = AppDb.Database.SqlQuery<sdtoCashReceiptPayment>("usp_GetCashReceiptPayment @vFilterValue, @vCompanyId",
                    pm,
                    new SqlParameter("@vCompanyId", CompanyId));

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public List<sdtoJournalEntries> GetJournalEntries(long CompanyId, string FilterValue)
        {
            List<sdtoJournalEntries> rptCollection = new List<sdtoJournalEntries>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@vFilterValue", SqlDbType.VarChar);
                pm.Value = FilterValue;
                DbRawSqlQuery<sdtoJournalEntries> result = AppDb.Database.SqlQuery<sdtoJournalEntries>("usp_GetJournalEntries @vFilterValue, @vCompanyId",
                    pm,
                    new SqlParameter("@vCompanyId", CompanyId));

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public List<sdtoRptLoanSummary> GetRptLoanSummary(long CompanyId, DataTable dtParameters)
        {
            List<sdtoRptLoanSummary> rptCollection = new List<sdtoRptLoanSummary>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@RptParameters", SqlDbType.Structured);
                pm.Value = dtParameters;
                pm.TypeName = "dbo.RptParameter";
                DbRawSqlQuery<sdtoRptLoanSummary> result = AppDb.Database.SqlQuery<sdtoRptLoanSummary>("usp_RptSummary @RptParameters, @vCompanyId",
                    pm,
                    new SqlParameter("@vCompanyId", CompanyId));

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public List<sdtoRptLoanSummary> GetRptDepositSummary(long CompanyId, DataTable dtParameters)
        {
            List<sdtoRptLoanSummary> rptCollection = new List<sdtoRptLoanSummary>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@RptParameters", SqlDbType.Structured);
                pm.Value = dtParameters;
                pm.TypeName = "dbo.RptParameter";
                DbRawSqlQuery<sdtoRptLoanSummary> result = AppDb.Database.SqlQuery<sdtoRptLoanSummary>("usp_RptDepositSummary @RptParameters, @vCompanyId",
                    pm,
                    new SqlParameter("@vCompanyId", CompanyId));

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public List<sdtoLedgerReport> GetRptLedgerReport(long CompanyId, int opId, DateTime fromDate, DateTime toDate, string accountIds)//, DataTable dtParameters
        {
            List<sdtoLedgerReport> rptCollection = new List<sdtoLedgerReport>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@OperationId", SqlDbType.BigInt);
                pm.Value = opId;

                SqlParameter fromdate = new SqlParameter("@FromDate", SqlDbType.Text);
                fromdate.Value = fromDate.ToString("yyyy-MM-dd hh:mm:ss");

                SqlParameter todate = new SqlParameter("@ToDate", SqlDbType.Text);
                todate.Value = toDate.ToString("yyyy-MM-dd hh:mm:ss");

                SqlParameter accountids = new SqlParameter("@AccountsIds", SqlDbType.Text);
                accountids.Value = accountIds;

                //pm.TypeName = "dbo.RptParameter";
                DbRawSqlQuery<sdtoLedgerReport> result = AppDb.Database.SqlQuery<sdtoLedgerReport>("sp_Acc_rptLedger @OperationId,@FromDate,@ToDate,@AccountsIds",
                   pm, fromdate, todate, accountids);

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public List<sdtoViewLoanDefaulterDetails> GetLoanDefaulters(long Interval)
        {
            List<sdtoViewLoanDefaulterDetails> rptCollection = new List<sdtoViewLoanDefaulterDetails>();
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@vDefaultInterval", SqlDbType.Int);
                pm.Value = Interval;
                DbRawSqlQuery<sdtoViewLoanDefaulterDetails> result = AppDb.Database.SqlQuery<sdtoViewLoanDefaulterDetails>("usp_GetLoanDefaulters @vDefaultInterval, @vSessionKey",
                    pm,
                    new SqlParameter("@vSessionKey", CurrentUser.UserSession.SessionKey));

                if (result != null)
                    rptCollection = result.ToList();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return rptCollection;
        }

        public string GenerateCode(string tableName)
        {
            string code = string.Empty;
            try
            {
                AppDb.Database.Connection.Open();
                SqlParameter pm = new SqlParameter("@vTableName", SqlDbType.VarChar);
                pm.Value = tableName;
                DbRawSqlQuery<string> result = AppDb.Database.SqlQuery<string>("usp_GenerateCode @vTableName",
                    pm);

                if (result != null)
                    code = result.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            finally
            {
                AppDb.Database.Connection.Close();
            }
            return code;
        }
    }
}
