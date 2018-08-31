using System.Web.Optimization;

namespace MassaNews.Portal
{
  public class BundleConfig
  {
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/modules").Include("~/Assets/scripts/modules/*.js"));
      bundles.Add(new ScriptBundle("~/bundles/application").Include(
        "~/Assets/scripts/vendor/*.js",
        "~/Assets/scripts/components/*.js",
        "~/Assets/scripts/application/*.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/videos").Include(
        "~/Assets/scripts/vendor/jquery.js",
        "~/Assets/scripts/components/modal.js",
        "~/Assets/scripts/videos/video.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/news").Include(
        "~/Assets/scripts/news/*.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/videos-category").Include(
        "~/Assets/scripts/videos/video-category.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/quiz-unifil").Include(
        "~/Assets/scripts/quiz-unifil/quiz-unifil.js"
      ));

      BundleTable.EnableOptimizations = true;
    }
  }
}