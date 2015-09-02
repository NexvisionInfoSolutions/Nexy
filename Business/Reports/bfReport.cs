using Business.Base;
using Data.Models.Accounts;
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
    }
}
