using Data.Models.Accounts.Schedules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanDBContext : DbContext
    {
        public LoanDBContext()
            : base("name=LoanDBContext")
        {
            base.Database.CommandTimeout = 180;
        }

        public DbSet<sdtoUserGroup> Usergroup { get; set; }
        public DbSet<sdtoUser> User { get; set; }
        public DbSet<sdtoSchedule> Schedules { get; set; }
        public DbSet<sdtoCompany> Companies { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoAccountType> AccountTypes { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoAccountHead> AccountHeads { get; set; }
        public System.Data.Entity.DbSet<LoanManagementSystem.Models.sdtoAddress> Address { get; set; }
        public System.Data.Entity.DbSet<LoanManagementSystem.Models.sdtoContact> Contacts { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoAccountBookType> AccountBookTypes { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoSettings> GeneralSettings { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoAccountBook> AccountBooks { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoUrlInfo> UrlInfoCollection { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Data.Models.Accounts.sdtoSettings>().HasRequired(t => t.AssetSchedule).WithMany().HasForeignKey(x => x.AssetScheduleId);

            //modelBuilder
            //.Entity<sdtoUser>()
            //.HasOptional<sdtoAddress>(u => u.UserAddress)
            //.WithOptionalPrincipal();

            //modelBuilder
            //.Entity<sdtoUser>()
            //.HasOptional<sdtoContact>(u => u.Contacts)
            //.WithOptionalPrincipal();

            //   modelBuilder
            //  .Entity<sdtoUser>()
            //  .HasOptional<sdtoAddress>(u => u.PermanentAddress)
            //  .WithOptionalPrincipal();

            //   modelBuilder
            // .Entity<sdtoUser>()
            // .HasOptional<sdtoAddress>(u => u.GuaranterAddress)
            // .WithOptionalPrincipal();

            //   modelBuilder
            // .Entity<sdtoUser>()
            // .HasOptional<sdtoContact>(u => u.PermanentContacts)
            // .WithOptionalPrincipal();

            //   modelBuilder
            //.Entity<sdtoUser>()
            //.HasOptional<sdtoContact>(u => u.GuaranterContacts)
            //.WithOptionalPrincipal();
        }

        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoLoanInfo> sdtoLoanInfoes { get; set; }

        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoLoanRepayment> sdtoLoanRepayments { get; set; }

        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoDepositInfo> sdtoDepositInfoes { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoWithdrawalInfo> DepositWithdrawals { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoBankDepositDetails> BankDepositDetails { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoBankDepositHeader> BankDepositHeader { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoJournalDetails> JournalDetails { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoJournalHeader> JournalHeader { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoReceiptDetails> ReceiptDetails { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoReceiptHeader> ReceiptHeader { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoOpeningBalance> OpeningBalance { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoDayBook> DayBook { get; set; }
        public System.Data.Entity.DbSet<Data.Models.Accounts.sdtoFinancialPeriod> FinancialPeriod { get; set; }
        public System.Data.Entity.DbSet<sdtoCountry> Countries { get; set; }
        public System.Data.Entity.DbSet<sdtoState> States { get; set; }
        public System.Data.Entity.DbSet<sdtoVillage> Villages { get; set; }
        public System.Data.Entity.DbSet<sdtoTaluk> Taluks { get; set; }
        public System.Data.Entity.DbSet<sdtoDistrict> Districts { get; set; }
        public System.Data.Entity.DbSet<sdtoUserSession> UserSessions { get; set; }       
        public System.Data.Entity.DbSet<sdtoSysCode> SysCodes { get; set; }
    }
}