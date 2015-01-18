using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace govnocode
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
         var t=    bundles.ToList();
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/jquery-1.10.2.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js", 
                      "~/Scripts/markdown.js",
                      "~/Scripts/bootstrap-markdown.js"
                      , "~/Scripts/MyScripts.js",
                      "~/Scripts/google-code-prettify/lang*",
                      "~/Scripts/google-code-prettify/*prettify.js",
                      "~/Scripts/jquery-ui-1.11.2.js"
                      ,"~/Scripts/tag-it.js",
                      "~/Scripts/jquery.tagcanvas.js"
                      
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",  
                      "~/Content/site.css"
                      ,"~/Content/bootstrap-markdown.min.css" ,
                      "~/Content/prettify.css",
                      "~/Content/themes/base/all.css"  ,
                      "~/Content/jquery.tagit.css" ,
                      "~/Content/tagit.ui-zendesk.css"
                      
                      )
                      );
            bundles.Add(new StyleBundle("~/Content/css2").Include(
            "~/Content/bootstrap2.css",
            "~/Content/site.css"
            , "~/Content/bootstrap-markdown.min.css",
            "~/Content/prettify.css",
            "~/Content/themes/base/all.css",
            "~/Content/jquery.tagit.css",
            "~/Content/tagit.ui-zendesk.css"

            )
            );
        }
    }
}
