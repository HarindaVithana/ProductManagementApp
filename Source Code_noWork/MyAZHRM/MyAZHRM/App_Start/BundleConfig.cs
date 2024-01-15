using System.Web;
using System.Web.Optimization;

namespace MyAZHRM
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));



            bundles.Add(new ScriptBundle("~/CustomJS/jquery").Include(
                         "~/CustomJS/jquery-3.1.1.min.js",
                         "~/CustomJS/popper.min.js",
                         "~/CustomJS/bootstrap.js",
                         "~/CustomJS/jquery.metisMenu.js",
                         "~/CustomJS/jquery.slimscroll.min.js",
                         "~/CustomJS/jquery.flot.js",
                         "~/CustomJS/jquery.flot.tooltip.min.js",
                         "~/CustomJS/jquery.flot.spline.js",
                         "~/CustomJS/jquery.flot.resize.js",
                         "~/CustomJS/jquery.flot.pie.js",
                         "~/CustomJS/jquery.peity.min.js",
                         "~/CustomJS/peity-demo.js",
                         "~/CustomJS/inspinia.js",
                         "~/CustomJS/pace.min.js",
                         "~/CustomJS/jquery-ui.min.js",
                         "~/CustomJS/jquery.gritter.min.js", 
                         "~/CustomJS/jquery.sparkline.min.js",
                         "~/CustomJS/sparkline-demo.js",
                         "~/CustomJS/Chart.min.js",
                         "~/CustomJS/toastr.min.js"));

            //bundles.Add(new StyleBundle("~/CustomCSS/css").Include(
            //             "~/CustomCSS/bootstrap.min.css",
            //             "~/CustomCSS/font-awesome.css",
            //             "~/CustomCSS/toastr.min.css",
            //             "~/CustomCSS/jquery.gritter.css",
            //             "~/CustomCSS/animate.css",
            //             "~/CustomCSS/style.css"));

        }
    }
}