using System.Web.Optimization;

namespace SeguridadWebv2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            /*Javascript*/
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/moment-with-locales.js",
                     "~/Scripts/vendor/chart/dist/Chart.bundle.js",
                     "~/Scripts/vendor/chart/sample/utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/script-custom").Include(
                      "~/Scripts/vendor/jquery/jquery.min.js",
                      "~/Scripts/vendor/bootstrap/js/bootstrap.min.js",
                       "~/Scripts/typeahead.bundle.js",
                      "~/Scripts/handlebars.js",
                      "~/Scripts/jquery.easing.1.3.js",
                      "~/Scripts/vendor/scrollreveal/scrollreveal.min.js",
                      "~/Scripts/vendor/magnific-popup/jquery.magnific-popup.min.js",
                      "~/Scripts/SistemaHome.js"));


            bundles.Add(new ScriptBundle("~/bundles/adminPanelJs").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/jquery.dcjqaccordion.2.7.js",
                     "~/Scripts/knob/jquery.knob.js",
                     "~/Content/daterangepicker/daterangepicker.js",
                     "~/Content/datepicker/bootstrap-datepicker.js",
                     "~/Scripts/sparkline/jquery.sparkline.min.js",
                     "~/Scripts/chartjs/Chart.min.js",
                     "~/Scripts/morris/morris.min.js",
                     "~/Scripts/SeguridadDashboard.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/VideoConfJs").Include(
                   "~/Scripts/adapter.js",
                   "~/Scripts/knockout-2.2.1.js",
                   "~/Scripts/knockout.mapping-latest.js",
                   "~/Scripts/alertify.js",
                   "~/Scripts/alertify.min.js",
                   "~/Scripts/webrtcdemo/viewModel.js",
                   "~/Scripts/webrtcdemo/connectionManager.js",
                   "~/Scripts/webrtcdemo/app.js",
                   "~/Scripts/vendor/countdown/jquery.countdown.js",
                   "~/Scripts/dropzone/dropzone.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/NotificationsJs").Include(
                   "~/Scripts/adapter.js",
                   "~/Scripts/jquery.gritter.js",
                   "~/Scripts/knockout-2.2.1.js",
                   "~/Scripts/knockout.mapping-latest.js",
                   "~/Scripts/webrtcdemo/viewModel.js",
                   "~/Scripts/webrtcdemo/connectionManager.js",
                   "~/Scripts/webrtcdemo/app.js",
                   "~/Scripts/knockout-3.4.0.debug.js",
                   "~/Scripts/knockout.mapping-latest.js",
                   "~/Scripts/array.extend/array.extend.min.js",
                   "~/Scripts/notifications/notifications.min.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                    "~/Scripts/jquery.signalR-{version}.js"
            ));

            /*Estilos*/
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/bootstrap.css",
                      "~/Content/typeaheadjs.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/magnific-popup.css",
                      "~/Scripts/morris/morris.css",
                      "~/Content/datepicker/datepicker3.css",
                      "~/Content/daterangepicker/daterangepicker-bs3.css",
                      "~/Content/SeguridadStyle.css",
                      "~/Content/skins/_all-skins.min.css"));

            bundles.Add(new StyleBundle("~/Content/csshome").Include(
                      "~/Scripts/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/bootstrap.css",
                      "~/Content/typeaheadjs.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Scripts/vendor/magnific-popup/magnific-popup.css",
                      "~/Content/SistemaHome.css"));

            bundles.Add(new StyleBundle("~/Content/cssespecialista").Include(
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/bootstrap.css",
                      "~/Content/especialistasHome.css",
                      "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/adminPanel").Include(
                 "~/Content/font-awesome/css/font-awesome.css",
                 "~/Content/bootstrap.css",
                 "~/Content/SeguridadDashboard.css",
                 "~/Content/SeguridadResponsiveDash.css",
                 "~/Content/datepicker/datepicker3.css",
                 "~/Content/daterangepicker/daterangepicker-bs3.css"
                ));

            bundles.Add(new StyleBundle("~/Content/VideoConf").Include(
                 "~/Content/icomoon/style.css",
                 "~/Content/VideoConf.css",
                 "~/Content/bootstrap/css/bootstrap.min.css",
                 "~/Content/alertify/alertify.bootstrap.css",
                 "~/Content/alertify/alertify.core.css",
                 "~/Content/alertify/alertify.default.css",
                 "~/Scripts/dropzone/basic.css",
                 "~/Scripts/dropzone/dropzone.css"
                ));
        }
    }
}