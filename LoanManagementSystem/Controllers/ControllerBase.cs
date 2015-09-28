using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LoanManagementSystem.Controllers
{
    public class ControllerBase : Controller
    {
      protected sdtoUserSession CurrentUserSession{get;set;}

        public ControllerBase():base (){
            var sessionUser = UtilityHelper.UserSession.GetSession(UtilityHelper.UserSession.LoggedInUser);
            if (sessionUser != null)
                CurrentUserSession = sessionUser.UserSession;
        }
        
        protected void SetDisplayMessage(string message)
        {
            TempData["ShowHeaderInfo"] = true;
            TempData["ViewMessage"] = message;
        }
    }
}
