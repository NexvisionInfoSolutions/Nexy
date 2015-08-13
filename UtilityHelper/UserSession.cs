using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UtilityHelper
{
    public class UserSession : UtilityBase
    {
        public const string LoggedInUser = "sessionKey_LoggedInUser";

        public static dynamic GetSession(string sessionKey)
        {
            return HttpContext.Current.Session[sessionKey];
        }

        public static void SetSession(string sessionKey, dynamic value)
        {
            HttpContext.Current.Session[sessionKey] = value;
        }

        public static void ClearSession() { HttpContext.Current.Session.Clear(); }
    }
}
