using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public class bfBase
    {
        protected LoanManagementSystem.Models.LoanDBContext AppDb = null;
        public bfBase(DbContext Db)
        {
            if (Db == null)
            {
                AppDb = new LoanManagementSystem.Models.LoanDBContext();
            }
            else
                AppDb = Db as LoanManagementSystem.Models.LoanDBContext;
        }
    }
}
