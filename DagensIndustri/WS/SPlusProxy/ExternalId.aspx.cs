using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WS.SPlusProxy
{
    public partial class ExternalId : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var token = Request.QueryString["access_token"];

            var url = "https://api.bonnier.se/v1/entitlements/external-ids?access_token=" + token;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/json";

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string json = sr.ReadToEnd();

            LitResponse.Text = json;
        }

        //private static ExternalIdsOutput GetExternalIds(string token)
        //{
        //    ExternalIdsOutput ret = new ExternalIdsOutput();
        //    string url = ConfigurationManager.AppSettings["BonDigUrlExternalIds"] + "?access_token=" + token;

        //    try
        //    {
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Method = "GET";
        //        req.ContentType = "application/json";

        //        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        //        StreamReader sr = new StreamReader(resp.GetResponseStream());
        //        string json = sr.ReadToEnd();
        //        sr.Close();

        //        //make it a list (if only 1 item)
        //        if (!json.Contains("\"externalIds\":["))
        //        {
        //            json = json.Replace("\"externalIds\":", "\"externalIds\":[");
        //            json = json.Insert(json.Length - 1, "]");
        //        }

        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        ret = js.Deserialize<ExternalIdsOutput>(CleanUpJson(json));

        //        return ret;
        //    }
        //    catch (Exception ex)
        //    {
        //        string exStr = ex.ToString();

        //        //404 not interesting (thrown when no externalIds are found)
        //        if (!exStr.Contains("(404) Not Found") && !exStr.Contains("(401) Unauthorized"))
        //            new Logger("GetExternalIds() failed for url: " + url, exStr);

        //        ret = new ExternalIdsOutput()
        //        {
        //            httpResponseCode = "404",
        //            requestId = "",
        //            totalItems = "0",
        //            externalIds = new List<ExternalIds>()
        //        };
        //        return ret;
        //    }
        //}
    }
}