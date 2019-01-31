using System.Web;
using System.Web.Optimization;

namespace ProERP.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/app.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/fontAwesome/css/font-awesome-{version}.css",
                      "~/Content/ionicons/css/ionicons-{version}.css",
                      "~/Content/AdminLTE/AdminLTE.min.css",
                      "~/Content/AdminLTE/skins/skin-blue.min.css",
                      "~/Content/AdminLTE/skins/skin-black.min.css",
                      "~/Content/AdminLTE/skins/skin-purple.min.css",
                      "~/Content/AdminLTE/skins/skin-yellow.min.css",
                      "~/Content/AdminLTE/skins/skin-red.min.css",
                      "~/Content/AdminLTE/skins/skin-green.min.css",
                      "~/Content/Site.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqx").Include(
                "~/Scripts/angular.min.js",
                "~/jqwidgets/globalization/globalize.js",
                "~/jqwidgets/jqxcore.js",
                 "~/jqwidgets/jqxbuttons.js",
                "~/jqwidgets/jqxdata.js",
                "~/jqwidgets/jqx-all.js",
                "~/jqwidgets/jqxchart.core.js",
                "~/jqwidgets/jqxchart.api.js",
                "~/jqwidgets/jqxchart.js",
                "~/jqwidgets/jqxangular.js",
                "~/Scripts/jsUtilities.js",
                "~/js/BaseController.js"
               ));

            bundles.Add(new StyleBundle("~/jqwidgets/styles/css")
                .Include("~/jqwidgets/styles/jqx.base.css"
                , "~/jqwidgets/styles/jqx.web.css"
                , "~/jqwidgets/styles/jqx.android.css"
                , "~/jqwidgets/styles/jqx.arctic.css"
                , "~/jqwidgets/styles/jqx.black.css"
                , "~/jqwidgets/styles/jqx.blackberry.css"
                , "~/jqwidgets/styles/jqx.bootstrap.css"
                , "~/jqwidgets/styles/jqx.classic.css"
                , "~/jqwidgets/styles/jqx.darkblue.css"
                , "~/jqwidgets/styles/jqx.energyblue.css"
                , "~/jqwidgets/styles/jqx.fresh.css"
                , "~/jqwidgets/styles/jqx.highcontrast.css"
                , "~/jqwidgets/styles/jqx.metro.css"
                , "~/jqwidgets/styles/jqx.mobile.css"
                , "~/jqwidgets/styles/jqx.office.css"
                , "~/jqwidgets/styles/jqx.orange.css"
                , "~/jqwidgets/styles/jqx.shinyblack.css"
                , "~/jqwidgets/styles/jqx.summer.css"
                , "~/jqwidgets/styles/jqx.ui-redmond.css"
                , "~/jqwidgets/styles/jqx.ui-sunny.css"));
        }
    }
}
