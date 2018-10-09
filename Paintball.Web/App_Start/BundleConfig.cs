namespace Paintball.Web
{
    using System.Web.Optimization;
    using Paintball.Web.Constants;

    public static class BundleConfig
    {
        /// <summary>
        /// For more information on bundling, visit <see cref="http://go.microsoft.com/fwlink/?LinkId=301862"/>.
        /// </summary>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Enable Optimizations
            // Set EnableOptimizations to false for debugging. For more information,
            // Web.config file system.web/compilation[debug=true]
            // OR
            // BundleTable.EnableOptimizations = true;

            // Enable CDN usage.
            // Note: that you can choose to remove the CDN if you are developing an intranet application.
            // Note: We are using Google's CDN where possible and then Microsoft if not available for better
            //       performance (Google is more likely to have been cached by the users browser).
            // Note: that protocol (http:) is omitted from the CDN URL on purpose to allow the browser to choose the protocol.
            bundles.UseCdn = true;

            AddCss(bundles);
            AddJavaScript(bundles);
        }

        private static void AddCss(BundleCollection bundles)
        {
            // Bootstrap - Twitter Bootstrap CSS (http://getbootstrap.com/).
            // Site - Your custom site CSS.
            // Note: No CDN support has been added here. Most likely you will want to customize your copy of bootstrap.
            bundles.Add(new StyleBundle(
                "~/Content/css")
                .Include("~/Content/loader.css")
                .Include("~/Content/bootstrap/site.css")
                .Include("~/Content/site.css")
                .Include("~/Content/bootstrap-theme.css")
                .Include("~/Content/angular-ui-notification.min.css"));

            // Font Awesome - Icons using font (http://fortawesome.github.io/Font-Awesome/).
            bundles.Add(new StyleBundle(
                "~/Content/fa",
                ContentDeliveryNetwork.MaxCdn.FontAwesomeUrl)
                .Include("~/Content/fontawesome/site.css"));
            
        }

        /// <summary>
        /// Creates and adds JavaScript bundles to the bundle collection. Content Delivery Network's (CDN) are used
        /// where available.
        ///
        /// Note: MVC's built in <see cref="System.Web.Optimization.Bundle.CdnFallbackExpression"/> is not used as
        /// using in-line scripts is not permitted under Content Security Policy (CSP) (See <see cref="FilterConfig"/>
        /// for more details).
        ///
        /// Instead, we create our own fail-over bundles. If a CDN is not reachable, the fail-over script loads the
        /// local bundles instead. The fail-over script is only a few lines of code and should have a minimal impact,
        /// although it does add an extra request (Two if the browser is IE8 or less). If you feel confident in the CDN
        /// availability and prefer better performance, you can delete these lines.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void AddJavaScript(BundleCollection bundles)
        {
            // jQuery - The JavaScript helper API (http://jquery.com/).
            Bundle jqueryBundle = new ScriptBundle("~/bundles/jquery", ContentDeliveryNetwork.Google.JQueryUrl)
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.cookie.js");
            bundles.Add(jqueryBundle);

            Bundle headroomBundle = new ScriptBundle("~/bundles/headroom", ContentDeliveryNetwork.Cdnjs.Headroom)
                .Include("~/Scripts/headroom.min.js");
            bundles.Add(headroomBundle);

            Bundle headroomJQueryBundle = new ScriptBundle("~/bundles/headroomJQuery", ContentDeliveryNetwork.Cdnjs.HeadroomJQuery)
                .Include("~/Scripts/jQuery.headroom.min.js");
            bundles.Add(headroomJQueryBundle);

            // Modernizr - Allows you to check if a particular API is available in the browser (http://modernizr.com).
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            // Note: The current version of Modernizr does not support Content Security Policy (CSP) (See FilterConfig).
            // See here for details: https://github.com/Modernizr/Modernizr/pull/1263 and
            // http://stackoverflow.com/questions/26532234/modernizr-causes-content-security-policy-csp-violation-errors
            Bundle modernizrBundle = new ScriptBundle(
                "~/bundles/modernizr",
                ContentDeliveryNetwork.Microsoft.ModernizrUrl)
                .Include("~/Scripts/modernizr-*");
            bundles.Add(modernizrBundle);

            // Bootstrap - Twitter Bootstrap JavaScript (http://getbootstrap.com/).
            Bundle bootstrapBundle = new ScriptBundle(
                "~/bundles/bootstrap",
                ContentDeliveryNetwork.Microsoft.BootstrapUrl)
                .Include("~/Scripts/bootstrap.js");
            bundles.Add(bootstrapBundle);

            // Script bundle for the site. The fall-back scripts are for when a CDN fails, in this case we load a local
            // copy of the script instead.
            Bundle failoverCoreBundle = new ScriptBundle("~/bundles/site")
                .Include("~/Scripts/Fallback/styles.js")
                .Include("~/Scripts/Fallback/scripts.js")
                .Include("~/Scripts/site.js");
            bundles.Add(failoverCoreBundle);

            Bundle angularBundle = new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular/angular.min.js")
                .Include("~/Scripts/angular/angular-locale_ru.js")
                .Include("~/Scripts/angular/angular-route.min.js")
                .Include("~/Scripts/angular/angular-resource.min.js")
                .Include("~/Scripts/angular/angular-animate.min.js")
                .Include("~/Scripts/angular/textAngular-rangy.min.js")
                .Include("~/Scripts/angular/textAngular-sanitize.min.js")
                .Include("~/Scripts/angular/textAngular.min.js")
                .Include("~/Scripts/angular/ui-bootstrap-2.5.0.min.js")
                .Include("~/Scripts/angular/ui-bootstrap-tpls.min.js")
                .Include("~/Scripts/angular/datetimepicker.js")
                .Include("~/Scripts/angular/angular-cookies.min.js")
                .Include("~/Scripts/angular/angular-ui-notification.min.js");
            bundles.Add(angularBundle);

            Bundle appBundle = new ScriptBundle("~/bundles/app")
                .IncludeDirectory("~/Scripts/app/modules", "*.js")
                .Include("~/Scripts/app/modules/ng-file-upload-shim.min.js")
                .Include("~/Scripts/app/modules/ng-file-upload.min.js")
                .IncludeDirectory("~/Scripts/app/services", "*.js", true)
                .IncludeDirectory("~/Scripts/app/resources", "*.js", true)
                .IncludeDirectory("~/Scripts/app/filters", "*.js", true)
                .IncludeDirectory("~/Scripts/app/components", "*.js", true)
                .Include("~/Scripts/app/app.module.js")
                .Include("~/Scripts/app/app.config.js")
                .Include("~/Scripts/app/app.run.js");
            bundles.Add(appBundle);
            
        }
    }
}
