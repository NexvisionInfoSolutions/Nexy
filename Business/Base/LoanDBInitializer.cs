﻿using Data.Models.Accounts;
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
            sdtoUrlInfo urlUserGroupHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "User Groups", ParentId = 4, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlUserGroupHead = context.UrlInfoCollection.Add(urlUserGroupHead);

            //6
            sdtoUrlInfo urlGeneralSettingsHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "General Settings", ParentId = 4, IsMenu = true, MenuOrder = 2 };// urlSettings.ParentId };
            urlGeneralSettingsHead = context.UrlInfoCollection.Add(urlGeneralSettingsHead);

            //7
            sdtoUrlInfo urlAccountSchedulesHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Account Schedules", ParentId = 4, IsMenu = true, MenuOrder = 3 };//urlSettings.ParentId };
            urlAccountSchedulesHead = context.UrlInfoCollection.Add(urlAccountSchedulesHead);

            //8
            sdtoUrlInfo urlAccountHeadsHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Account Heads", ParentId = 4, IsMenu = true, MenuOrder = 4 };//urlSettings.ParentId };
            urlAccountHeadsHead = context.UrlInfoCollection.Add(urlAccountHeadsHead);

            //9
            sdtoUrlInfo urlAccountBooksHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Account Books", ParentId = 4, IsMenu = true, MenuOrder = 5 };//urlSettings.ParentId };
            urlAccountBooksHead = context.UrlInfoCollection.Add(urlAccountBooksHead);

            //10
            sdtoUrlInfo urlUserGroup = new sdtoUrlInfo() { Url = "/UserGroup/Index", CreatedOn = DateTime.Now, UrlText = "User Groups", ParentId = 5, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlUserGroup = context.UrlInfoCollection.Add(urlUserGroup);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/UserGroup/Create", CreatedOn = DateTime.Now, UrlText = "Add User Group", ParentId = 5, IsMenu = true, MenuOrder = 2 });//urlUserGroup.ParentId });                       

            //12
            sdtoUrlInfo urlGeneralSettings = new sdtoUrlInfo() { Url = "/Settings/Index", CreatedOn = DateTime.Now, UrlText = "General Settings", ParentId = 6, IsMenu = true, MenuOrder = 1 };// urlSettings.ParentId };
            urlGeneralSettings = context.UrlInfoCollection.Add(urlGeneralSettings);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Settings/Create", CreatedOn = DateTime.Now, UrlText = "Add General Settings", ParentId = 6, IsMenu = true, MenuOrder = 2 });//urlGeneralSettings.ParentId });

            //14
            sdtoUrlInfo urlAccountSchedules = new sdtoUrlInfo() { Url = "/ScheduleSettings/Index", CreatedOn = DateTime.Now, UrlText = "Account Schedules", ParentId = 7, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountSchedules = context.UrlInfoCollection.Add(urlAccountSchedules);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/ScheduleSettings/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Schedule", ParentId = 7, IsMenu = true, MenuOrder = 2 });//urlAccountSchedules.ParentId });

            //sdtoUrlInfo urlAccountTypes = new sdtoUrlInfo() { Url = "/AccountTypes/Index", CreatedOn = DateTime.Now, UrlText = "Account Types", ParentId = 4 };//urlSettings.ParentId };
            //urlAccountTypes = context.UrlInfoCollection.Add(urlAccountTypes);
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountTypes/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Type", ParentId = 11 });//urlAccountTypes.ParentId });

            //16
            sdtoUrlInfo urlAccountHeads = new sdtoUrlInfo() { Url = "/AccountHeads/Index", CreatedOn = DateTime.Now, UrlText = "Account Heads", ParentId = 8, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountHeads = context.UrlInfoCollection.Add(urlAccountHeads);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountTypes/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Head", ParentId = 8, IsMenu = true, MenuOrder = 2 });//urlAccountHeads.ParentId });

            //sdtoUrlInfo urlAccountBookTypes = new sdtoUrlInfo() { Url = "/AccountBookTypes/Index", CreatedOn = DateTime.Now, UrlText = "Account Book Types", ParentId = 4 };//urlSettings.ParentId };
            //urlAccountBookTypes = context.UrlInfoCollection.Add(urlAccountBookTypes);
            //context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountBookTypes/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Book Type", ParentId = 15 });//urlAccountBookTypes.ParentId });

            //18
            sdtoUrlInfo urlAccountBooks = new sdtoUrlInfo() { Url = "/AccountBooks/Index", CreatedOn = DateTime.Now, UrlText = "Account Books", ParentId = 9, IsMenu = true, MenuOrder = 1 };//urlSettings.ParentId };
            urlAccountBooks = context.UrlInfoCollection.Add(urlAccountBooks);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/AccountBooks/Create", CreatedOn = DateTime.Now, UrlText = "Add Account Book", ParentId = 9, IsMenu = true, MenuOrder = 2 });//urlAccountBooks.ParentId });

            //20
            sdtoUrlInfo urlUserManagement = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Manage Users", IsMenu = true, MenuOrder = 1 };
            urlUserManagement = context.UrlInfoCollection.Add(urlUserManagement);

            //21
            sdtoUrlInfo urlUsersHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Users", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlUsersHead = context.UrlInfoCollection.Add(urlUsersHead);

            //22
            sdtoUrlInfo urlMembersHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Members", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlMembersHead = context.UrlInfoCollection.Add(urlMembersHead);

            //23
            sdtoUrlInfo urlExecutivesHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Executives", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlExecutivesHead = context.UrlInfoCollection.Add(urlExecutivesHead);

            sdtoUrlInfo urlUsers = new sdtoUrlInfo() { Url = "/User/Index", CreatedOn = DateTime.Now, UrlText = "Users", ParentId = 21, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlUsers = context.UrlInfoCollection.Add(urlUsers);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/User/Create", CreatedOn = DateTime.Now, UrlText = "Add User", ParentId = 21, IsMenu = true, MenuOrder = 2 });//urlUsers.ParentId });

            sdtoUrlInfo urlMembers = new sdtoUrlInfo() { Url = "/Member/Index", CreatedOn = DateTime.Now, UrlText = "Members", ParentId = 22, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlMembers = context.UrlInfoCollection.Add(urlMembers);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Member/Create", CreatedOn = DateTime.Now, UrlText = "Add Member", ParentId = 22, IsMenu = true, MenuOrder = 2 }); ;// urlMembers.ParentId });

            sdtoUrlInfo urlExecutives = new sdtoUrlInfo() { Url = "/Executive/Index", CreatedOn = DateTime.Now, UrlText = "Executives", ParentId = 23, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlExecutives = context.UrlInfoCollection.Add(urlExecutives);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Executive/Create", CreatedOn = DateTime.Now, UrlText = "Add Executive", ParentId = 23, IsMenu = true, MenuOrder = 2 });//urlExecutives.ParentId });

            //30
            sdtoUrlInfo urlAccounts = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Manage Accounts", IsMenu = true, MenuOrder = 2 };
            urlAccounts = context.UrlInfoCollection.Add(urlAccounts);

            //31
            sdtoUrlInfo urlLoansHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Loans", ParentId = 30, IsMenu = true, MenuOrder = 1 };//urlAccounts.ParentId };
            urlLoansHead = context.UrlInfoCollection.Add(urlLoansHead);

            sdtoUrlInfo urlLoans = new sdtoUrlInfo() { Url = "/Loan/Index", CreatedOn = DateTime.Now, UrlText = "Loans", ParentId = 31, IsMenu = true, MenuOrder = 1 };//urlAccounts.ParentId };
            urlLoans = context.UrlInfoCollection.Add(urlLoans);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Loan/Create", CreatedOn = DateTime.Now, UrlText = "Add Loan", ParentId = 31, IsMenu = true, MenuOrder = 2 });//urlLoans.ParentId });

            //34
            sdtoUrlInfo urlCompanyHead = new sdtoUrlInfo() { CreatedOn = DateTime.Now, UrlText = "Company", ParentId = 20, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlCompanyHead = context.UrlInfoCollection.Add(urlCompanyHead);

            sdtoUrlInfo urlCompanies = new sdtoUrlInfo() { Url = "/Company/Index", CreatedOn = DateTime.Now, UrlText = "Company details", ParentId = 34, IsMenu = true, MenuOrder = 1 };//urlUserManagement.ParentId };
            urlCompanies = context.UrlInfoCollection.Add(urlCompanies);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/Company/Create", CreatedOn = DateTime.Now, UrlText = "Add Company", ParentId = 34, IsMenu = true, MenuOrder = 2 });//urlCompanies.ParentId });

            sdtoUrlInfo urlRepaymentLoans = new sdtoUrlInfo() { Url = "/LoanRepayments/Index", CreatedOn = DateTime.Now, UrlText = "Loan Repayments", ParentId = 31, IsMenu = true, MenuOrder = 1 };//urlAccounts.ParentId };
            urlRepaymentLoans = context.UrlInfoCollection.Add(urlRepaymentLoans);
            context.UrlInfoCollection.Add(new sdtoUrlInfo() { Url = "/LoanRepayments/Create", CreatedOn = DateTime.Now, UrlText = "Add Loan Repayment", ParentId = 31, IsMenu = true, MenuOrder = 2 });//urlLoans.ParentId });

            /*******************************/
            /** Account Types **/

            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Cash", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "CASH" });
            context.AccountTypes.Add(new sdtoAccountType() { AccountType = "Bank", Status = Data.Models.Enumerations.AccountTypeStatus.Active, UniqueName = "Bank" });

            /*******************************/
            /** Account Book Types **/

            context.AccountBookTypes.Add(new sdtoAccountBookType() { AccountBookType = "Cash", Status = Data.Models.Enumerations.AccountBookTypeStatus.Active, UniqueName = "CASH" });
            context.AccountBookTypes.Add(new sdtoAccountBookType() { AccountBookType = "Bank", Status = Data.Models.Enumerations.AccountBookTypeStatus.Active, UniqueName = "BANK" });

            base.Seed(context);
        }
    }
}