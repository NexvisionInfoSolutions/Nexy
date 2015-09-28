using Data.Models.Accounts;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public class LoanDBInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LoanDBContext>
    {
        protected override void Seed(LoanDBContext context)
        {
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Home/Index", CreatedOn = DateTime.Now, UrlText = "Home", IsMenu = true, MenuOrder = 1 });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Home/AboutUs", CreatedOn = DateTime.Now, UrlText = "About Us", IsMenu = false });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Home/ContactUs", CreatedOn = DateTime.Now, UrlText = "Contact Us", IsMenu = false });

            //4
            sdtoUrlInfo urlAccounts = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Accounts", IsMenu = true, MenuOrder = 1 };
            urlAccounts = context.UrlInfoCollection.Add(urlAccounts);

            //5
            sdtoUrlInfo urlSettings = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Settings", IsMenu = true, MenuOrder = 4 };
            urlSettings = context.UrlInfoCollection.Add(urlSettings);

            //6
            sdtoUrlInfo urlTransactions = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Transactions", IsMenu = true, MenuOrder = 2 };
            urlTransactions = context.UrlInfoCollection.Add(urlTransactions);

            //7
            sdtoUrlInfo urlReports = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Reports", IsMenu = true, MenuOrder = 3 };
            urlReports = context.UrlInfoCollection.Add(urlReports);

            //8 
            sdtoUrlInfo urlGeneralSettings = new sdtoUrlInfo() { Url = "/Settings/Edit/1", CreatedOn = DateTime.Now, UrlText = "General Settings", ParentId = 5, IsMenu = true, MenuOrder = 4 };// urlSettings.ParentId };
            urlGeneralSettings = context.UrlInfoCollection.Add(urlGeneralSettings);

            //9
            sdtoUrlInfo urlMngUsers = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Manager Users", IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlMngUsers = context.UrlInfoCollection.Add(urlMngUsers);

            //10
            sdtoUrlInfo urlUserGroup = new sdtoUrlInfo() { Url = "/UserGroup/Index", CreatedOn = DateTime.Now, UrlText = "User Groups", ParentId = 9, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlUserGroup = context.UrlInfoCollection.Add(urlUserGroup);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/UserGroup/Create", CreatedOn = DateTime.Now, UrlText = "Add User Group", ParentId = 9, IsMenu = false, MenuOrder = 2 });//urlUserGroup.ParentId });                       

            //12
            sdtoUrlInfo urlAccountSchedules = new sdtoUrlInfo() { Url = "/ScheduleSettings/Index", CreatedOn = DateTime.Now, UrlText = "Schedule", ParentId = 5, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountSchedules = context.UrlInfoCollection.Add(urlAccountSchedules);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/ScheduleSettings/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Schedule", ParentId = 5, IsMenu = false, MenuOrder = 2 });//urlAccountSchedules.ParentId });

            //14
            sdtoUrlInfo urlAccountHeads = new sdtoUrlInfo() { Url = "/AccountHeads/Index", CreatedOn = DateTime.Now, UrlText = "Account Head", ParentId = 5, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountHeads = context.UrlInfoCollection.Add(urlAccountHeads);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountHeads/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Head", ParentId = 5, IsMenu = false, MenuOrder = 2 });//urlAccountHeads.ParentId });

            //16
            sdtoUrlInfo urlAccountBooks = new sdtoUrlInfo() { Url = "/AccountBooks/Index", CreatedOn = DateTime.Now, UrlText = "Account Book", ParentId = 5, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountBooks = context.UrlInfoCollection.Add(urlAccountBooks);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountBooks/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Book", ParentId = 5, IsMenu = false, MenuOrder = 2 });//urlAccountBooks.ParentId });

            //18
            sdtoUrlInfo urlUsers = new sdtoUrlInfo() { Url = "/User/Index", CreatedOn = DateTime.Now, UrlText = "List of Users", ParentId = 9, IsMenu = true, MenuOrder = 2 };//urlUserManagement.ParentId };
            urlUsers = context.UrlInfoCollection.Add(urlUsers);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/User/Create", CreatedOn = DateTime.Now, UrlText = "Add User", ParentId = 9, IsMenu = false, MenuOrder = 2 });//urlUsers.ParentId });

            //20
            sdtoUrlInfo urlMembers = new sdtoUrlInfo() { Url = "/Member/Index", CreatedOn = DateTime.Now, UrlText = "List of Members", ParentId = 9, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlMembers = context.UrlInfoCollection.Add(urlMembers);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Member/Create", CreatedOn = DateTime.Now, UrlText = "Add Member", ParentId = 9, IsMenu = false, MenuOrder = 2 }); ;// urlMembers.ParentId });

            //22
            sdtoUrlInfo urlExecutives = new sdtoUrlInfo() { Url = "/Executive/Index", CreatedOn = DateTime.Now, UrlText = "List of Executives", ParentId = 9, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlExecutives = context.UrlInfoCollection.Add(urlExecutives);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Executive/Create", CreatedOn = DateTime.Now, UrlText = "Add Executive", ParentId = 9, IsMenu = false, MenuOrder = 2 });//urlExecutives.ParentId });

            //24
            sdtoUrlInfo urlLoansHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Loans", ParentId = 4, IsMenu = false, MenuOrder = 5 };//urlAccounts.ParentId };
            urlLoansHead = context.UrlInfoCollection.Add(urlLoansHead);

            sdtoUrlInfo urlLoans = new sdtoUrlInfo() { Url = "/Loan/Index", CreatedOn = DateTime.Now, UrlText = "List Loans", ParentId = 4, IsMenu = true, MenuOrder = 5 };//urlAccounts.ParentId };
            urlLoans = context.UrlInfoCollection.Add(urlLoans);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Loan/Create", CreatedOn = DateTime.Now, UrlText = "Add Loan", ParentId = 4, IsMenu = false, MenuOrder = 5 });//urlLoans.ParentId });

            //27
            sdtoUrlInfo urlCompanies = new sdtoUrlInfo() { Url = "/Company/Index", CreatedOn = DateTime.Now, UrlText = "View Company details", ParentId = 9, IsMenu = true, MenuOrder = 5 };//urlUserManagement.ParentId };
            urlCompanies = context.UrlInfoCollection.Add(urlCompanies);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Company/Create", CreatedOn = DateTime.Now, UrlText = "Add Company", ParentId = 9, IsMenu = false, MenuOrder = 5 });//urlCompanies.ParentId });

            //29
            sdtoUrlInfo urlRepaymentLoans = new sdtoUrlInfo() { Url = "/LoanRepayments/Index", CreatedOn = DateTime.Now, UrlText = "View Loan Repayments", ParentId = 4, IsMenu = false, MenuOrder = 6 };//urlAccounts.ParentId };
            urlRepaymentLoans = context.UrlInfoCollection.Add(urlRepaymentLoans);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/LoanRepayments/Create", CreatedOn = DateTime.Now, UrlText = "Add Loan Repayment", ParentId = 4, IsMenu = false, MenuOrder = 6 });//urlLoans.ParentId });

            //31
            sdtoUrlInfo urlDepositHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Deposits", ParentId = 4, IsMenu = false, MenuOrder = 7 };//urlAccounts.ParentId };
            urlDepositHead = context.UrlInfoCollection.Add(urlDepositHead);
            sdtoUrlInfo urlDeposits = new sdtoUrlInfo() { Url = "/Deposit/Index", CreatedOn = DateTime.Now, UrlText = "View Deposits", ParentId = 4, IsMenu = true, MenuOrder = 8 };//urlAccounts.ParentId };
            urlDeposits = context.UrlInfoCollection.Add(urlDeposits);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Deposit/Create", CreatedOn = DateTime.Now, UrlText = "Add Deposit Account", ParentId = 4, IsMenu = false, MenuOrder = 8 });//urlDeposits.ParentId });

            //34
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/LoanSummary", CreatedOn = DateTime.Now, UrlText = "Loan Summary", ParentId = 7, IsMenu = true, MenuOrder = 1 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/DepositSummary", CreatedOn = DateTime.Now, UrlText = "Deposit Summary", ParentId = 7, IsMenu = true, MenuOrder = 2 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/LedgerReport", CreatedOn = DateTime.Now, UrlText = "Ledger Report", ParentId = 7, IsMenu = true, MenuOrder = 3 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/TrialBalance", CreatedOn = DateTime.Now, UrlText = "Trial Balance", ParentId = 7, IsMenu = true, MenuOrder = 4 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/CashBook", CreatedOn = DateTime.Now, UrlText = "Cash Book", ParentId = 7, IsMenu = true, MenuOrder = 5 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/BankBook", CreatedOn = DateTime.Now, UrlText = "Bank Book", ParentId = 7, IsMenu = true, MenuOrder = 6 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/DayBook", CreatedOn = DateTime.Now, UrlText = "Day Book", ParentId = 7, IsMenu = true, MenuOrder = 7 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/PLReport", CreatedOn = DateTime.Now, UrlText = "P&L", ParentId = 7, IsMenu = true, MenuOrder = 8 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/BalanceSheet", CreatedOn = DateTime.Now, UrlText = "BalanceSheet", ParentId = 7, IsMenu = true, MenuOrder = 9 });//urlDeposits.ParentId });


            //43
            sdtoUrlInfo exportPalm = new sdtoUrlInfo() { Url = "/Loan/ExportView", CreatedOn = DateTime.Now, UrlText = "Export", ParentId = 4, IsMenu = true, MenuOrder = 9 };//urlAccounts.ParentId };
            exportPalm = context.UrlInfoCollection.Add(exportPalm);

            //46
            sdtoUrlInfo importPalm = new sdtoUrlInfo() { Url = "/Loan/ImportView", CreatedOn = DateTime.Now, UrlText = "Import", ParentId = 4, IsMenu = true, MenuOrder = 10 };//urlAccounts.ParentId };
            importPalm = context.UrlInfoCollection.Add(importPalm);

            //47            
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/OpeningBalances", CreatedOn = DateTime.Now, UrlText = "Opening Balance", ParentId = 6, IsMenu = true, MenuOrder = 1 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/ListCashReceiptPayment", CreatedOn = DateTime.Now, UrlText = "Cash Receipts/Payment", ParentId = 6, IsMenu = true, MenuOrder = 2 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/ListBankDepositWithdrawal", CreatedOn = DateTime.Now, UrlText = "Bank Deposits/Withdrawals", ParentId = 6, IsMenu = true, MenuOrder = 3 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/ListJournalEntry", CreatedOn = DateTime.Now, UrlText = "Journal", ParentId = 6, IsMenu = true, MenuOrder = 4 });//urlDeposits.ParentId });
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/BankDeposit", CreatedOn = DateTime.Now, UrlText = "Withdrawal", ParentId = 6, IsMenu = true, MenuOrder = 2 });//urlDeposits.ParentId });
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/BankWithdrawal", CreatedOn = DateTime.Now, UrlText = "Deposit", ParentId = 6, IsMenu = true, MenuOrder = 3 });//urlDeposits.ParentId });
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/CashReceipt", CreatedOn = DateTime.Now, UrlText = "Cash Receipt Payment", ParentId = 6, IsMenu = true, MenuOrder = 4 });//urlDeposits.ParentId });
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/CashPayment", CreatedOn = DateTime.Now, UrlText = "Cash Receipt Payment", ParentId = 6, IsMenu = true, MenuOrder = 5 });//urlDeposits.ParentId });            
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/JournalEntry", CreatedOn = DateTime.Now, UrlText = "Journal Entry", ParentId = 6, IsMenu = true, MenuOrder = 6 });//urlDeposits.ParentId });
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/ExpenseEntry", CreatedOn = DateTime.Now, UrlText = "Expense Entry", ParentId = 6, IsMenu = true, MenuOrder = 7 });//urlDeposits.ParentId });

            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/DistrictList", CreatedOn = DateTime.Now, UrlText = "List of Districts", ParentId = 5, IsMenu = true, MenuOrder = 5 });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/TalukList", CreatedOn = DateTime.Now, UrlText = "List of Taluks", ParentId = 5, IsMenu = true, MenuOrder = 6 });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/VillageList", CreatedOn = DateTime.Now, UrlText = "List of Villages", ParentId = 5, IsMenu = true, MenuOrder = 7 });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/StateList", CreatedOn = DateTime.Now, UrlText = "List of States", ParentId = 5, IsMenu = true, MenuOrder = 8 });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/CountryList", CreatedOn = DateTime.Now, UrlText = "List of Country", ParentId = 5, IsMenu = true, MenuOrder = 9 });
            
            /*******************************/
            /** Schedules **/

            context.Schedules.Add(new Data.Models.Accounts.Schedules.sdtoSchedule() { ScheduleName = "Liability", ParentId = 0, ShortName = "LY" });
            context.Schedules.Add(new Data.Models.Accounts.Schedules.sdtoSchedule() { ScheduleName = "Assets", ParentId = 0, ShortName = "AS" });
            context.Schedules.Add(new Data.Models.Accounts.Schedules.sdtoSchedule() { ScheduleName = "Income", ParentId = 0, ShortName = "IC" });
            context.Schedules.Add(new Data.Models.Accounts.Schedules.sdtoSchedule() { ScheduleName = "Expenditure", ParentId = 0, ShortName = "EX" });

            /*******************************/
            /** Account Types **/

            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Cash", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "Cash" });
            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Bank", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "Bank" });
            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Debiter", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "Debiter" });
            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Crediter", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "Crediter" });
            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Other", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "Other" });

            /*******************************/
            /** Account Book Types **/

            context.AccountBookTypes.Add(new sdtoAccountBookType() { AccountBookType = "Cash", Status = Data.Models.Enumerations.AccountBookTypeStatus.Active, UniqueName = "CASH" });
            context.AccountBookTypes.Add(new sdtoAccountBookType() { AccountBookType = "Bank", Status = Data.Models.Enumerations.AccountBookTypeStatus.Active, UniqueName = "BANK" });
            context.AccountBookTypes.Add(new sdtoAccountBookType() { AccountBookType = "Journal", Status = Data.Models.Enumerations.AccountBookTypeStatus.Active, UniqueName = "Journal" });

            /*******************************/
            /** Country **/
            context.Countries.Add(new sdtoCountry() { CountryName = "India", CountryAbbr = "IN", CreatedOn = DateTime.Now });
            context.Countries.Add(new sdtoCountry() { CountryName = "United States of America", CountryAbbr = "USA", CreatedOn = DateTime.Now });
            context.Countries.Add(new sdtoCountry() { CountryName = "United Arab Emirates", CountryAbbr = "UAE", CreatedOn = DateTime.Now });

            /*******************************/
            /** State **/
            context.States.Add(new sdtoState() { CountryId = 1, StateAbbr = "KL", StateName = "Kerala", CreatedOn = DateTime.Now });
            context.States.Add(new sdtoState() { CountryId = 1, StateAbbr = "KA", StateName = "Karnataka", CreatedOn = DateTime.Now });
            context.States.Add(new sdtoState() { CountryId = 1, StateAbbr = "TN", StateName = "Tamil Nadu", CreatedOn = DateTime.Now });

            /*******************************/
            /** Financial Year **/
            context.FinancialPeriod.Add(new sdtoFinancialPeriod() { StartDate = new DateTime(2015, 4, 1), EndDate = new DateTime(2016, 3, 31), IsCurrentYear = true, PeriodCode = "FY/2015/2016", PeriodName = "FY/2015/2016", IsDeleted = false, CreatedOn = DateTime.Now });

            context.Usergroup.Add(new sdtoUserGroup() { Code = "GN", Description = "General", Name = "Owner", Status = Data.Models.Enumerations.UserGroupStatus.Active, IsDeleted = false, CreatedOn = DateTime.Now });

            sdtoAddress sdtoCompAddr = new sdtoAddress() { Address1 = "Address of Owner" };

            sdtoCompAddr = context.Address.Add(sdtoCompAddr);
            var sa = context.Address.FirstOrDefault();
            if (sa != null)
                sdtoCompAddr = sa;

            sdtoContact sdtoContactss = new sdtoContact() { ContactName = "Name of Owner" };
            sdtoContactss = context.Contacts.Add(sdtoContactss);
            var sc = context.Contacts.FirstOrDefault();
            if (sc != null)
                sdtoContactss = sc;

            sdtoCompAddr.AddressId = 1;
            sdtoContactss.ContactId = 1;

            sdtoCompany cmpObj = new sdtoCompany() { AddressId = sdtoCompAddr.AddressId, ContactId = sdtoContactss.ContactId, Code = "General", CompanyName = "Owner", IsDeleted = false, TIN = "1233", CreatedOn = DateTime.Now };
            cmpObj = context.Companies.Add(cmpObj);
            var scc = context.Companies.FirstOrDefault();
            if (scc != null)
                cmpObj = scc;

            cmpObj.CompanyId = 1;

            sdtoSettings sd = new sdtoSettings() { BankCharges = 10, BankInterest = 10, CompanyId = cmpObj.CompanyId, CreatedOn = DateTime.Now };
            context.GeneralSettings.Add(sd);

            base.Seed(context);
        }
    }
}
