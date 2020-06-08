using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Di.Common.Utils.Url
{
    public class UrlUtils
    {

        public static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        public static string AddQueryString(string url, string queryStringName, string queryStringValue, bool urlEncode = false)
        {
            if (string.IsNullOrEmpty(queryStringName) || string.IsNullOrEmpty(queryStringValue))
            {
                return url;
            }
            return AddToQueryString(url, queryStringName, urlEncode ? HttpUtility.UrlEncode(queryStringValue) : queryStringValue);
        }

        public static string AddAllExistingQuerystrings(string url)
        {
            return GetAllQueryStringsKeys().Aggregate(url, (current, key) => AddQueryString(current, key, GetQueryStringValue(key)));            
        }

        public static string AddDictionaryToQueryString(string url, Dictionary<string, string> queryStringDictionary)
        {
            return queryStringDictionary.Aggregate(url, (current, queryStringValuePair) => AddQueryString(current, queryStringValuePair.Key, queryStringValuePair.Value, true));
        }

        public static string GetQueryStringValue(string key, HttpContext context = null)
        {
            var httpContext = context ?? HttpContext.Current;

            return httpContext != null ? httpContext.Request.QueryString[key] : string.Empty;
        }

        public static string GetQueryStringValue(string key, string url)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query)[key];
            return query;
        }

        public static IEnumerable<string> GetAllQueryStringsKeys(HttpContext context = null)
        {
            var httpContext = context ?? HttpContext.Current;

            return httpContext != null ? httpContext.Request.QueryString.AllKeys : new string[]{};
        }

        public static string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            var pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }

        /// <summary>
        /// Add query string parameter to given url
        /// 
        /// </summary>
        /// <param name="url">The original url (may include query string parameters)</param><param name="name">Name of the query string parameter (for example "status")</param><param name="val">The value of the query string parameter</param>
        /// <returns>
        /// The new url with the query string added.
        /// </returns>
        /// 
        /// <remarks>
        /// This method will replace the value of an existing query string with the same name.
        /// 
        /// </remarks>
        private static string AddToQueryString(string url, string name, string val)
        {
            if (name.StartsWith("?") || name.StartsWith("&"))
                name = name.Remove(0, 1);
            if (!name.EndsWith("="))
                name = name + "=";
            int num = url.IndexOf('?');
            if (num < 0)
                return url + "?" + name + val;
            string[] strArray = url.Substring(num + 1).Split(new char[1] { '&' });
            for (int index1 = 0; index1 < strArray.Length; ++index1)
            {
                if (strArray[index1].StartsWith(name))
                {
                    strArray[index1] = name + val;
                    string str = string.Empty;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                        str = index2 != 0 ? str + "&" + strArray[index2] : str + strArray[index2];
                    return url.Substring(0, num + 1) + str;
                }
            }
            return url + "&" + name + val;
        }


    }
}
