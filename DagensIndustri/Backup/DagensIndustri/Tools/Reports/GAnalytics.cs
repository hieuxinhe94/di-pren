using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DagensIndustri.Tools.Reports
{
    public class GAnalytics
    {
        #region GetToken (2 Methods)
        
        /// <summary>
        /// Get Token Method #1
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetAuthorizationToken(string email, string password)
        {
            using (var webClient = new WebClient())
            {
                var nameValueCollection = new NameValueCollection();
                nameValueCollection.Add("Email", email);
                nameValueCollection.Add("Passwd", password);
                nameValueCollection.Add("accountType", "GOOGLE");
                nameValueCollection.Add("source", "acountfeed-v1");
                nameValueCollection.Add("service", "analytics");

                var response = webClient.UploadValues("https://www.google.com/accounts/ClientLogin", nameValueCollection);
                var values = Encoding.Default.GetString(response).Split('\n');

                return values.First(value => value.StartsWith("Auth=")).Replace("Auth=", String.Empty);
            }
        }

        /// <summary>
        /// Get Token Method #2
        /// </summary>
        private static readonly string authUrlFormat = "accountType=GOOGLE&Email={0}&Passwd={1}&source=dagensindustri.se-v1&service=analytics";

        private static string GetToken(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Username, Password", "Username and/or password not set");
            }

            string authBody = string.Format(authUrlFormat, username, password);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com/accounts/ClientLogin");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            Stream stream = req.GetRequestStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(authBody);
            sw.Close();
            sw.Dispose();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string token = sr.ReadToEnd();
            string[] tokens = token.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in tokens)
            {
                if (item.StartsWith("Auth="))
                {
                    return item.Replace("Auth=", "");
                }
            }
            return string.Empty;
        }

        #endregion

        private static XmlDocument GetAccountFeed(string authorizationToken)
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("GData-Version", "2");
                webClient.Headers.Add("Authorization", String.Format("GoogleLogin Auth={0}", authorizationToken));

                var response = webClient.DownloadString("https://www.google.com/analytics/feeds/accounts/default");

                XmlDocument document = new XmlDocument();

                document.Load(XmlReader.Create(new StringReader(response)));

                return document;
            }
        }

        private static XmlDocument GetDataFeed(string authorizationToken, string profileId, DateTime startDate, DateTime endDate, string metrics, string filters, string dimensions, string sort, int? maxResults)
        {
            using (var webClient = new WebClient())
            {
                var query = HttpUtility.ParseQueryString(String.Empty);
                query.Add("ids", profileId);
                query.Add("start-date", startDate.ToString("yyyy-MM-dd"));
                query.Add("end-date", endDate.ToString("yyyy-MM-dd"));
                query.Add("metrics", metrics);
                query.Add("filters", filters); //"ga:pagePath=~/");

                if (!String.IsNullOrEmpty(dimensions))
                {
                    query.Add("dimensions", dimensions);
                }

                if (!String.IsNullOrEmpty(sort))
                {
                    query.Add("sort", sort);
                }

                if (maxResults.HasValue)
                {
                    query.Add("max-results", maxResults.Value.ToString());
                }

                var uri = new UriBuilder("https://www.google.com/analytics/feeds/data");
                uri.Query = query.ToString();

                webClient.Headers.Add("GData-Version", "2");
                webClient.Headers.Add("Authorization", String.Format("GoogleLogin Auth={0}", authorizationToken));

                var response = webClient.DownloadString(uri.Uri.AbsoluteUri);

                XmlDocument document = new XmlDocument();

                document.Load(XmlReader.Create(new StringReader(response)));

                return document;
            }
        }

        public static string GetMetrics(string authToken, string profileID, string metrics, string filters, DateTime fromDate, DateTime toDate)
        {
            string metricValue = string.Empty;

            XmlDocument doc = GAnalytics.GetDataFeed(authToken, profileID, fromDate, toDate, metrics, filters, "", "", 50); 

            XmlNamespaceManager xnsmgr = new XmlNamespaceManager(doc.NameTable);

            xnsmgr.AddNamespace("default", "http://www.w3.org/2005/Atom");
            xnsmgr.AddNamespace("openSearch", "http://a9.com/-/spec/opensearchrss/1.0/");
            xnsmgr.AddNamespace("dxp", "http://schemas.google.com/analytics/2009");

            foreach (XmlNode entry in doc.SelectNodes("/default:feed/default:entry", xnsmgr))
            {
                foreach (XmlNode metric in entry.SelectNodes("dxp:metric", xnsmgr))
                {
                    metricValue = metric.Attributes["value"].Value;
                }
            }

            return metricValue;
        }


    }
}