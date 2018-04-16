using System.Web;
using System.Web.Optimization;

namespace Property_Management {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(
                new ScriptBundle("~/bundles/js/jquery_bootstrap").Include(
                    "~/Plugins/jquery/jquery-{version}.min.js",
                    "~/Plugins/bootstrap/js/bootstrap.min.js"
                ));

            bundles.Add(
                new StyleBundle("~/bundles/css/bootstrap").Include(
                    "~/Plugins/bootstrap/css/bootstrap.min.css"
                ));
        }
}