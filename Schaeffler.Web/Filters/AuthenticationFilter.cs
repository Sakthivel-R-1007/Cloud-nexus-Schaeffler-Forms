using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Schaeffler.Filters
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        public bool Disable { get; set; }

        public string ControllerName { get; set; }

        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.ActionParameters.Count != 0 && filterContext.ActionParameters.Values.Last() != null)
            //{
            //    var sts = filterContext.ActionParameters.Values.Last().ToString();
            //    if (sts == "True")
            //    {
            //        Disable = Convert.ToBoolean(filterContext.ActionParameters.Values.Last());
            //    }
            //}
            if (Disable) return;
            base.OnActionExecuting(filterContext);

            bool authorized = filterContext.HttpContext.Session["UserAccount"] != null;

            //authorized = authorized ? (string.IsNullOrEmpty(Roles) || (!string.IsNullOrEmpty(Roles) && Roles.Split(',').Contains("1"))) : false;

            if (!authorized)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.Flush();
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult("Logout", null);
                }
            }
            else
            {
                if (!authorized)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = 401;
                        filterContext.HttpContext.Response.Flush();
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult("Logout", null);
                    }
                }

            }

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            dumpData(filterContext, "OnResultExecuted");
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            dumpData(filterContext, "OnResultExecuting");
        }

        void dumpData(ControllerContext context, string filter, string actionName = "Admin")
        {
        }
    }
}