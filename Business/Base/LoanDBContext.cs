using Data.Models.Accounts.Schedules;
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

        public DbSet<sdtoSchedule> Schedules { get; set; }
    }
}