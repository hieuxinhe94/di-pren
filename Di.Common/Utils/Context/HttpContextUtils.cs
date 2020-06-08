using System;
using System.Text;
using System.Web;
using Di.Common.WebRequests;

namespace Di.Common.Utils.Context
{
    public class HttpContextUtils
    {
        public static void Stream(string charset, Encoding encoding, string contentType, string fileName, string content, HttpContext context = null)
        {
            var httpContext = context ?? HttpContext.Current;

            if (httpContext.Equals(null)) return;

            httpContext.Response.Clear();
            httpContext.Response.Charset = charset;
            httpContext.Response.ContentEncoding = encoding;
            httpContext.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName.Replace(" ", "_")));
            httpContext.Response.ContentType = contentType;
            httpContext.Response.Write(content);
            httpContext.Response.End();   
        }

        public static RequestMethod GetHttpMethod(HttpContext context = null)
        {
            var httpMethod = GetHttpContextInformation(httpContext => httpContext.Request.HttpMethod, context);

            switch (httpMethod)
            {
                case "GET":
                    return RequestMethod.GET;
                case "POST":
                    return RequestMethod.POST;
                case "DELETE":
                    return RequestMethod.DELETE;
                case "PUT":
                    return RequestMethod.PUT;
                default:
                    return RequestMethod.GET;
            }
        }

        public static bool IsMobileDevice(HttpContext context = null)
        {
            return GetHttpContextInformation(httpContext => httpContext.Request.Browser.IsMobileDevice, context);
        }

        public static string GetUserAgent(HttpContext context = null)
        {
            return GetHttpContextInformation(httpContext => httpContext.Request.UserAgent, context);
        }

        public static string GetUserBrowser(HttpContext context = null)
        {
            return GetHttpContextInformation(httpContext => httpContext.Request.Browser.Browser, context);
        }

        public static string GetUserBrowserVersion(HttpContext context = null)
        {
            return GetHttpContextInformation(httpContext => httpContext.Request.Browser.Version, context);
        }

        public static string GetUserIp(HttpContext context = null)
        {
            return GetHttpContextInformation(httpContext => httpContext.Request.UserHostAddress, context);
        }

        private static T GetHttpContextInformation<T>(Func<HttpContext, T> getHttpContextInformation, HttpContext context)
        {
            var httpContext = context ?? HttpContext.Current;
            return httpContext != null ? getHttpContextInformation(httpContext) : default(T);
        }
    }
}
