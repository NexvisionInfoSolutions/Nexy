using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class LoanDBContext : DbContext
    {
        public LoanDBContext()
            : base("name=LoanDBContext")
        {

        }

        public DbSet<sdtoUserGroup> Usergroup { get; set; }
        public DbSet<sdtoUser> User { get; set; }

        public DbSet<sdtoCompany> sdtoCompanies { get; set; }

        public DbSet<sdtoAddress> sdtoAddresses { get; set; }

        public DbSet<sdtoContact> sdtoContacts { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{ 
        //     modelBuilder.Entity<sdtoCompany>().MapToStoredProcedures
        //    (
        //      s => s.Insert(i => i.HasName("[dbo].[Company_Insert]"))
        //            .Update(u => u.HasName("[dbo].[Company_Update]"))
        //            .Delete(d => d.HasName("[dbo].[Company_Delete]")));
        //        base.OnModelCreating(modelBuilder);
        //}

    }
}