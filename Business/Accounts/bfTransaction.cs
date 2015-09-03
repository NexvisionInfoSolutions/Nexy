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
    public class bfTransaction : bfBase
    {
        public bfTransaction(DbContext dbConnection) : base(dbConnection) { }

        public void InitiateMemberAccounts(LoanManagementSystem.Models.sdtoUser Member)
        {
            try
            {
                sdtoSettings settings = AppDb.GeneralSettings.Include(x => x.AssetScheduleId).Where(x => x.SettingsId == 1).FirstOrDefault();
                sdtoAccountType accTypeDebiter = AppDb.AccountTypes.Where(x => x.UniqueName.Equals("Debiter", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                var accHead = new sdtoAccountHead()
                {
                    AccountCode = "ACH_" + Member.Code,
                    AccountName = "AC_" + Member.Code,
                    ScheduleId = settings.AssetScheduleId,
                    AccountTypeId = accTypeDebiter.AccountTypeId,
                    CreditLimit = 0,
                    CreditDays = 0,
                    TIN = string.Empty,
                    CST = string.Empty,
                    AddressId = Member.UserAddressId.Value,
                    ContactId = Member.UserContactId.Value,
                    CreatedBy = Member.UserID,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };
                AppDb.AccountHeads.Add(accHead);
                AppDb.SaveChanges();

                sdtoOpeningBalance memberOpeniningBalance = new sdtoOpeningBalance()
                {
                    AccountHeadId = accHead.AccountHeadId,
                    ClosingBalance = 0,
                    CreditOpeningBalance = 0,
                    DebitOpeningBalance = 0,
                    FinancialYearId = 1,
                    ScheduleId = accHead.ScheduleId,
                    IsDeleted = false,
                    CreatedBy = Member.UserID,
                    CreatedOn = DateTime.Now
                };

                AppDb.OpeningBalance.Add(memberOpeniningBalance);
                AppDb.SaveChanges();
            }
            catch (Exception)
            {

            }
            finally
            {

            }
        }
    }
}
