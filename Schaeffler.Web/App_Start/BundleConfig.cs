using System.Web;
using System.Web.Optimization;

namespace Schaeffler.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/login-indexjs").Include(
                                     "~/Contents/Admin/js/jqueryValidate/jquery.validate.js",
                                     "~/Contents/Admin/js/jqueryValidate/additional-methods.js",
                                     "~/Contents/Admin/js/jqueryValidate/jquery.alphanum.js",
                                      "~/Contents/Admin/js/Custom/login-index.js"
                                      ));
            bundles.Add(new ScriptBundle("~/login-forgetpasswordjs").Include(
             "~/Contents/Admin/js/jqueryValidate/jquery.validate.js",
                         "~/Contents/Admin/js/jqueryValidate/additional-methods.js",
                         "~/Contents/Admin/js/jqueryValidate/jquery.alphanum.js",
                          "~/Contents/Admin/js/Custom/login-forgetpassword.js"
            ));
            bundles.Add(new ScriptBundle("~/login-resetpasswordjs").Include(
             "~/Contents/Admin/js/jqueryValidate/jquery.validate.js",
                         "~/Contents/Admin/js/jqueryValidate/additional-methods.js",
                         "~/Contents/Admin/js/jqueryValidate/jquery.alphanum.js",
                          "~/Contents/Admin/js/Custom/login-resetpassword.js"
            ));

            bundles.Add(new ScriptBundle("~/User-changepasswordjs").Include(
         "~/Contents/Admin/js/jqueryValidate/jquery.validate.js",
                        "~/Contents/Admin/js/jqueryValidate/additional-methods.js",
                        "~/Contents/Admin/js/jqueryValidate/jquery.alphanum.js",
          "~/Contents/Admin/js/Custom/login-changepassword.js"

        ));
        }
    }
}
