using System.Web.Mvc;

namespace Schaeffler.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            #region Login

            context.MapRoute(
                    name: "Login",
                    url: "Admin",
                    defaults: new { controller = "Login", action = "Index" }
            );

            context.MapRoute(
                   name: "Home",
                   url: "Admin/Home",
                   defaults: new { controller = "Home", action = "Index" }
           );
            context.MapRoute(
                   name: "Export",
                   url: "Admin/Home/Export",
                   defaults: new { controller = "Home", action = "Export" }
           );

            context.MapRoute(
                    name: "Logout",
                    url: "Admin/Logout",
                    defaults: new { controller = "Login", action = "Logout" }
            );

            context.MapRoute(
                    name: "AdminUserChangePassword",
                    url: "Admin/User/ChangePassword",
                    defaults: new { controller = "User", action = "ChangePassword" }
            );

            context.MapRoute(
                    name: "AdminUserForgotPassword",
                    url: "Admin/ForgotPassword",
                    defaults: new { controller = "Login", action = "ForgotPassword" }
            );

            context.MapRoute(
                  name: "ResetPassword",
                  url: "Admin/Login/ResetPassword/{EncDetail}",
                  defaults: new { controller = "Login", action = "ResetPassword" }
       );
            #endregion
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", controller = "Login", id = UrlParameter.Optional }
            );
        }
    }
}