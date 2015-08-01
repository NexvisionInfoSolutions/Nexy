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

        public DbSet<UserGroup> Usergroup { get; set; }
        public DbSet<User> User { get; set; }



    }
}