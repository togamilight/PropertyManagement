using System.Web;
using System.Web.Optimization;

namespace Property_Management {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(
                new ScriptBundle("~/bundles/js/jquery_bootstrap").Include(
                    "~/Plugins/jquery/jquery-1.12.4.min.js",
                    "~/Plugins/bootstrap/js/bootstrap.min.js"
                ));

            bundles.Add(
                new StyleBundle("~/bundles/css/bootstrap").Include(
                    "~/Plugins/bootstrap/css/bootstrap.min.css",
                    "~/Plugins/bootstrap/css/bootstrap-theme.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/datepicker").Include(
                    "~/Plugins/bootstrap/js/bootstrap-datepicker.min.js",
                    "~/Plugins/bootstrap/js/bootstrap-datepicker.zh-CN.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css/datepicker").Include(
                    "~/Plugins/bootstrap/css/bootstrap-datepicker3.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js/datatables").Include(
                    "~/Plugins/datatables/js/jquery.dataTables.min.js",
                    "~/Plugins/datatables/js/dataTables.bootstrap.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css/datatables").Include(
                    "~/Plugins/datatables/css/jquery.dataTables.min.css",
                    "~/Plugins/datatables/css/dataTables.bootstrap.min.css"
                ));
        }
    }
}