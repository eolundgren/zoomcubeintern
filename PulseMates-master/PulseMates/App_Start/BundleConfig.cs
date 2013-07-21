using System.Web;
using System.Web.Optimization;

namespace PulseMates
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.*"
            ));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/knockout/ko.*"
            ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //var less = new Bundle("~/Content/css")
            //    .Include("~/Content/PulseMates.less");

            //less.Transforms.Add(new PulseMates.Infrastructure.Optimization.DotlessTransform());
            //less.Transforms.Add(new CssMinify());

            //bundles.Add(less);

            bundles.Add(new ScriptBundle("~/bundles/participate").Include(
                "~/Scripts/viewModels/participate/index.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/participate-edit").Include(
                "~/Scripts/viewModels/participate/index.js",
                "~/Scripts/viewModels/participate/edit.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/participate-upload").Include(
                "~/Scripts/viewModels/participate/index.js",
                "~/Scripts/viewModels/participate/upload.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/participate-facebook").Include(
                "~/Scripts/viewModels/participate/index.js",
                "~/Scripts/viewModels/participate/facebook.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/featured").Include(
                "~/Scripts/viewModels/home/featured.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/demo/grid").Include(
                "~/Scripts/colorbox/jquery.colorbox-min.js",
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/demo/grid.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/demo/map").Include(
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/demo/map.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/demo/timeslider").Include(
                "~/Scripts/colorbox/jquery.colorbox-min.js",
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/demo/timeslider.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/demo/page").Include(
                "~/Scripts/colorbox/jquery.colorbox-min.js",
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/demo/page.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/demo/event").Include(
                "~/Scripts/colorbox/jquery.colorbox-min.js",
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/demo/event.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/home/index").Include(
                "~/Scripts/colorbox/jquery.colorbox-min.js",
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/home/index.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/demo/three").Include(
                "~/Scripts/three.min.js",
                "~/Scripts/d3.v3.min.js",
                "~/Scripts/viewModels/zoomcube.js",
                "~/Scripts/viewModels/demo/three.js"
            ));
        }
    }
}