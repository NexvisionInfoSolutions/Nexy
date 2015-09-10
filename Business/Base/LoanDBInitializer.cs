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
            sdtoUrlInfo urlSettings = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Settings", IsMenu = true, MenuOrder = 4 };
            urlSettings = context.UrlInfoCollection.Add(urlSettings);

            //5
            sdtoUrlInfo urlUserGroupHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "User Groups", ParentId = 4, IsMenu = false, MenuOrder = 1 };//urlSettings.ParentId };
            urlUserGroupHead = context.UrlInfoCollection.Add(urlUserGroupHead);

            //6
            sdtoUrlInfo urlGeneralSettingsHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "General Settings", ParentId = 4, IsMenu = false, MenuOrder = 2 };// urlSettings.ParentId };
            urlGeneralSettingsHead = context.UrlInfoCollection.Add(urlGeneralSettingsHead);

            //7
            sdtoUrlInfo urlAccountSchedulesHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Account Schedules", ParentId = 4, IsMenu = false, MenuOrder = 3 };//urlSettings.ParentId };
            urlAccountSchedulesHead = context.UrlInfoCollection.Add(urlAccountSchedulesHead);

            //8
            sdtoUrlInfo urlAccountHeadsHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Account Heads", ParentId = 4, IsMenu = false, MenuOrder = 4 };//urlSettings.ParentId };
            urlAccountHeadsHead = context.UrlInfoCollection.Add(urlAccountHeadsHead);

            //9
            sdtoUrlInfo urlAccountBooksHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Account Books", ParentId = 4, IsMenu = false, MenuOrder = 5 };//urlSettings.ParentId };
            urlAccountBooksHead = context.UrlInfoCollection.Add(urlAccountBooksHead);

            //10
            sdtoUrlInfo urlUserGroup = new sdtoUrlInfo() { Url = "/UserGroup/Index", CreatedOn = DateTime.Now, UrlText = "List User Groups", ParentId = 4, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlUserGroup = context.UrlInfoCollection.Add(urlUserGroup);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/UserGroup/Create", CreatedOn = DateTime.Now, UrlText = "Add User Group", ParentId = 5, IsMenu = false, MenuOrder = 2 });//urlUserGroup.ParentId });                       

            //12
            sdtoUrlInfo urlGeneralSettings = new sdtoUrlInfo() { Url = "/Settings/Edit/1", CreatedOn = DateTime.Now, UrlText = "General Settings", ParentId = 4, IsMenu = true, MenuOrder = 1 };// urlSettings.ParentId };
            urlGeneralSettings = context.UrlInfoCollection.Add(urlGeneralSettings);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/Edit/1", CreatedOn = DateTime.Now, UrlText = "General Settings", ParentId = 6, IsMenu = false, MenuOrder = 2 });//urlGeneralSettings.ParentId });

            //14
            sdtoUrlInfo urlAccountSchedules = new sdtoUrlInfo() { Url = "/ScheduleSettings/Index", CreatedOn = DateTime.Now, UrlText = "View Account Schedules", ParentId = 4, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountSchedules = context.UrlInfoCollection.Add(urlAccountSchedules);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/ScheduleSettings/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Schedule", ParentId = 7, IsMenu = false, MenuOrder = 2 });//urlAccountSchedules.ParentId });

            //sdtoUrlInfo urlAccountTypes = new sdtoUrlInfo() { Url = "/AccountTypes/Index", CreatedOn = DateTime.Now, UrlText = "Account Types", ParentId = 4 };//urlSettings.ParentId };
            //urlAccountTypes = context.UrlInfoCollection.Add(urlAccountTypes);
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountTypes/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Type", ParentId = 11 });//urlAccountTypes.ParentId });

            //16
            sdtoUrlInfo urlAccountHeads = new sdtoUrlInfo() { Url = "/AccountHeads/Index", CreatedOn = DateTime.Now, UrlText = "List Account Heads", ParentId = 4, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountHeads = context.UrlInfoCollection.Add(urlAccountHeads);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountHeads/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Head", ParentId = 8, IsMenu = false, MenuOrder = 2 });//urlAccountHeads.ParentId });

            //sdtoUrlInfo urlAccountBookTypes = new sdtoUrlInfo() { Url = "/AccountBookTypes/Index", CreatedOn = DateTime.Now, UrlText = "Account Book Types", ParentId = 4 };//urlSettings.ParentId };
            //urlAccountBookTypes = context.UrlInfoCollection.Add(urlAccountBookTypes);
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountBookTypes/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Book Type", ParentId = 15 });//urlAccountBookTypes.ParentId });

            //18
            sdtoUrlInfo urlAccountBooks = new sdtoUrlInfo() { Url = "/AccountBooks/Index", CreatedOn = DateTime.Now, UrlText = "List Account Books", ParentId = 4, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountBooks = context.UrlInfoCollection.Add(urlAccountBooks);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountBooks/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Book", ParentId = 9, IsMenu = false, MenuOrder = 2 });//urlAccountBooks.ParentId });

            //20
            sdtoUrlInfo urlUserManagement = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Manage Users", IsMenu = true, MenuOrder = 1 };
            urlUserManagement = context.UrlInfoCollection.Add(urlUserManagement);

            //21
            sdtoUrlInfo urlUsersHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Users", ParentId = 20, IsMenu = false, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlUsersHead = context.UrlInfoCollection.Add(urlUsersHead);

            //22
            sdtoUrlInfo urlMembersHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Members", ParentId = 20, IsMenu = false, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlMembersHead = context.UrlInfoCollection.Add(urlMembersHead);

            //23
            sdtoUrlInfo urlExecutivesHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Executives", ParentId = 20, IsMenu = false, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlExecutivesHead = context.UrlInfoCollection.Add(urlExecutivesHead);

            sdtoUrlInfo urlUsers = new sdtoUrlInfo() { Url = "/User/Index", CreatedOn = DateTime.Now, UrlText = "List of Users", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlUsers = context.UrlInfoCollection.Add(urlUsers);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/User/Create", CreatedOn = DateTime.Now, UrlText = "Add User", ParentId = 21, IsMenu = false, MenuOrder = 2 });//urlUsers.ParentId });

            sdtoUrlInfo urlMembers = new sdtoUrlInfo() { Url = "/Member/Index", CreatedOn = DateTime.Now, UrlText = "List of Members", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlMembers = context.UrlInfoCollection.Add(urlMembers);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Member/Create", CreatedOn = DateTime.Now, UrlText = "Add Member", ParentId = 22, IsMenu = false, MenuOrder = 2 }); ;// urlMembers.ParentId });

            sdtoUrlInfo urlExecutives = new sdtoUrlInfo() { Url = "/Executive/Index", CreatedOn = DateTime.Now, UrlText = "List of Executives", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlExecutives = context.UrlInfoCollection.Add(urlExecutives);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Executive/Create", CreatedOn = DateTime.Now, UrlText = "Add Executive", ParentId = 23, IsMenu = false, MenuOrder = 2 });//urlExecutives.ParentId });

            //30
            sdtoUrlInfo urlAccounts = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Manage Accounts", IsMenu = true, MenuOrder = 2 };
            urlAccounts = context.UrlInfoCollection.Add(urlAccounts);

            //31
            sdtoUrlInfo urlLoansHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Loans", ParentId = 30, IsMenu = false, MenuOrder = 1 };//urlAccounts.ParentId };
            urlLoansHead = context.UrlInfoCollection.Add(urlLoansHead);

            sdtoUrlInfo urlLoans = new sdtoUrlInfo() { Url = "/Loan/Index", CreatedOn = DateTime.Now, UrlText = "List Loans", ParentId = 30, IsMenu = true, MenuOrder = 1 };//urlAccounts.ParentId };
            urlLoans = context.UrlInfoCollection.Add(urlLoans);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Loan/Create", CreatedOn = DateTime.Now, UrlText = "Add Loan", ParentId = 31, IsMenu = false, MenuOrder = 2 });//urlLoans.ParentId });

            //34
            sdtoUrlInfo urlCompanyHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Company", ParentId = 20, IsMenu = false, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlCompanyHead = context.UrlInfoCollection.Add(urlCompanyHead);

            //35
            sdtoUrlInfo urlCompanies = new sdtoUrlInfo() { Url = "/Company/Index", CreatedOn = DateTime.Now, UrlText = "View Company details", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlCompanies = context.UrlInfoCollection.Add(urlCompanies);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Company/Create", CreatedOn = DateTime.Now, UrlText = "Add Company", ParentId = 34, IsMenu = false, MenuOrder = 2 });//urlCompanies.ParentId });

            //37
            sdtoUrlInfo urlRepaymentLoans = new sdtoUrlInfo() { Url = "/LoanRepayments/Index", CreatedOn = DateTime.Now, UrlText = "View Loan Repayments", ParentId = 30, IsMenu = true, MenuOrder = 3 };//urlAccounts.ParentId };
            urlRepaymentLoans = context.UrlInfoCollection.Add(urlRepaymentLoans);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/LoanRepayments/Create", CreatedOn = DateTime.Now, UrlText = "Add Loan Repayment", ParentId = 31, IsMenu = false, MenuOrder = 4 });//urlLoans.ParentId });

            //39
            sdtoUrlInfo urlDepositHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Deposits", ParentId = 30, IsMenu = false, MenuOrder = 1 };//urlAccounts.ParentId };
            urlDepositHead = context.UrlInfoCollection.Add(urlDepositHead);

            //40
            sdtoUrlInfo urlDeposits = new sdtoUrlInfo() { Url = "/Deposit/Index", CreatedOn = DateTime.Now, UrlText = "View Deposits", ParentId = 30, IsMenu = true, MenuOrder = 1 };//urlAccounts.ParentId };
            urlDeposits = context.UrlInfoCollection.Add(urlDeposits);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Deposit/Create", CreatedOn = DateTime.Now, UrlText = "Add Deposit Account", ParentId = 39, IsMenu = false, MenuOrder = 2 });//urlDeposits.ParentId });

            //42
            sdtoUrlInfo urlReports = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Reports", IsMenu = true, MenuOrder = 5 };
            urlReports = context.UrlInfoCollection.Add(urlReports);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/LoanSummary", CreatedOn = DateTime.Now, UrlText = "Loan Summary", ParentId = 42, IsMenu = true, MenuOrder = 1 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Reports/DepositSummary", CreatedOn = DateTime.Now, UrlText = "Deposit Summary", ParentId = 42, IsMenu = true, MenuOrder = 2 });//urlDeposits.ParentId });

            //45
            sdtoUrlInfo exportPalm = new sdtoUrlInfo() { Url = "/Loan/ExportView", CreatedOn = DateTime.Now, UrlText = "Export", ParentId = 30, IsMenu = true, MenuOrder = 5 };//urlAccounts.ParentId };
            exportPalm = context.UrlInfoCollection.Add(exportPalm);

            //46
            sdtoUrlInfo importPalm = new sdtoUrlInfo() { Url = "/Loan/ImportView", CreatedOn = DateTime.Now, UrlText = "Import", ParentId = 30, IsMenu = true, MenuOrder = 6 };//urlAccounts.ParentId };
            importPalm = context.UrlInfoCollection.Add(importPalm);

            //47
            sdtoUrlInfo urlTransacctionHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Transactions", ParentId = 30, IsMenu = true, MenuOrder = 1 };//urlAccounts.ParentId };
            urlTransacctionHead = context.UrlInfoCollection.Add(urlTransacctionHead);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/OpeningBalances", CreatedOn = DateTime.Now, UrlText = "Opening Balance", ParentId = 47, IsMenu = true, MenuOrder = 1 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/DepositWithdrawal", CreatedOn = DateTime.Now, UrlText = "Deposit Withdrawal", ParentId = 47, IsMenu = true, MenuOrder = 2 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/CashReceiptPayment", CreatedOn = DateTime.Now, UrlText = "Cash Receipt Payment", ParentId = 47, IsMenu = true, MenuOrder = 3 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/ExpenseEntry", CreatedOn = DateTime.Now, UrlText = "Expense Entry", ParentId = 47, IsMenu = true, MenuOrder = 4 });//urlDeposits.ParentId });
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Accounting/JournalEntry", CreatedOn = DateTime.Now, UrlText = "Journal Entry", ParentId = 47, IsMenu = true, MenuOrder = 5 });//urlDeposits.ParentId });

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
