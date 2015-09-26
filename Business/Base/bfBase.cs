using LoanManagementSystem.Models;
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
        protected sdtoUser CurrentUser = new sdtoUser();
        public bfBase(DbContext Db)
        {
            if (Db == null)
                AppDb = new LoanManagementSystem.Models.LoanDBContext();
            else
                AppDb = Db as LoanManagementSystem.Models.LoanDBContext;
            sdtoUser user = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (user != null && user.UserSession != null)
            {
                CurrentUser = user;
            }
        }
    }
}
