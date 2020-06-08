using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DIClassLib.Misc
{
    public static class GoogleAnalyticsHelper
    {
        public enum GoogleTrackParams
        {
            utm_source,
            utm_medium,
            utm_campaign,
            utm_term,
            utm_content
        }

        public const string CookieName = "GoogleTrack";

        public static string BuildVirtualPageString(IEnumerable<string> parts)
        {
            var returnString = new StringBuilder();
            foreach (var part in parts)
            {
                returnString.Append(string.Format("{0}/", part.Trim('/')));
            }
            return returnString.ToString();
        }

        public static void SetCookie(HttpContext context, string cookieName, IEnumerable<KeyValuePair<string, string>> cookieValues, int expiresInMinutes)
        {
            var myCookie = new HttpCookie(cookieName);
            foreach (var keyValue in cookieValues)
            {
                myCookie[keyValue.Key] = keyValue.Value;
            }
            myCookie.Expires = DateTime.Now.AddMinutes(expiresInMinutes);
            context.Response.Cookies.Add(myCookie);
        }

        public static IEnumerable<KeyValuePair<string, string>> ReadCookie(HttpContext context, string cookieName)
        {
            var cookieKeyValues = new List<KeyValuePair<string, string>>();
            var myCookie = new HttpCookie(cookieName);
            myCookie = context.Request.Cookies[cookieName];
            if (myCookie == null)
            {
                return cookieKeyValues;
            }
            cookieKeyValues.AddRange(myCookie.Values.AllKeys.Select(cookieKey => new KeyValuePair<string, string>(cookieKey, myCookie[cookieKey])));
            return cookieKeyValues;
        }
    }
}
