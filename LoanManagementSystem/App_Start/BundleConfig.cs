using System.Web;
using System.Web.Optimization;

namespace LoanManagementSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                      "~/Scripts/layout2.js",
                      "~/Scripts/jquery.metisMenu.js"));

            bundles.Add(new ScriptBundle("~/bundles/grid").Include(
                    "~/Scripts/gridmvc.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqwidgets").Include(
                "~/Scripts/jqx-all.js",
                "~/Scripts/jqxcore.js"
                //"~/Scripts/jqxdata.js",
                //"~/Scripts/jqxgrid.js",
                //"~/Scripts/jqxgrid.selection.js",
                //"~/Scripts/jqxgrid.pager.js",
                //"~/Scripts/jqxtreegrid.js",
                //"~/Scripts/jqxtree.js",

            //"~/Scripts/jqxdatetimeinput.js",

            //"~/Scripts/jqxbuttons.js",
                //"~/Scripts/jqxslider.js",
                //"~/Scripts/jqxscrollbar.js",
                //"~/Scripts/jqxdropdownlist.js",
                //"~/Scripts/jqxlistbox.js",

            //"~/Scripts/jqxdatatable.js",
                //"~/Scripts/jqxmenu.js",
                //"~/Scripts/jqxlistmenu.js",
                //"~/Scripts/jqxcalendar.js",
                //"~/Scripts/jqxgrid.sort.js",
                //"~/Scripts/jqxgrid.filter.js",
                //"~/Scripts/jqxdatetimeinput.js",
                //"~/Scripts/jqxdropdownlist.js",
                //"~/Scripts/jqxslider.js",
                //"~/Scripts/globalize.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/Gridmvc.css",
                      "~/Content/jqx.base.css",
                      "~/Content/jqx.web.css",
                      "~/Content/jqx.ui-lightness.css",
                      "~/Content/Layout2.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/skin").Include(
                      "~/Content/skins/skin-green.css"));

            bundles.Add(new StyleBundle("~/Content/noskin").Include(
                      "~/Content/skins/skin-no.css"));
        }
    }
}
