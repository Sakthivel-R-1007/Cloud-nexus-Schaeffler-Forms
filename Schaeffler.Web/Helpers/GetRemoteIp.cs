using System;
using System.Web;

namespace Schaeffler.Helpers
{
    public static class GetRemoteIp
    {
        public static String GetIPAddress(HttpContextBase HttpContext)
        {
            String ip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            return (string.IsNullOrEmpty(ip)) ? HttpContext.Request.ServerVariables["REMOTE_ADDR"] : ip.Split(',')[0];
        }
    }
}