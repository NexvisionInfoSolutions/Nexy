using Data.Models.Accounts;
using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LoanManagementSystem.App_Start
{
    public class MenuLoaderActionFilter : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //filterContext.Controller.ViewBag.IsAuthenticated = MembershipService.IsAuthenticated;
            //filterContext.Controller.ViewBag.IsAdmin = MembershipService.IsAdmin;

            //var userProfile = MembershipService.GetCurrentUserProfile();
            //if (userProfile != null)
            //{
            //    filterContext.Controller.ViewBag.Avatar = userProfile.Picture;
            //}
            LoanDBContext db = new LoanDBContext();
            Business.SysBase.Tree.bfTree<sdtoUrlInfo> urls = new Business.SysBase.Tree.bfTree<sdtoUrlInfo>(db);
            filterContext.Controller.ViewBag.Menu = urls.GetData();
        }

    }
}
