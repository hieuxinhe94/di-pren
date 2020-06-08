using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WS.SPlusProxy
{
    public partial class CreateEntitlement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var token = Request.QueryString["access_token"];
            var q = Request.QueryString["q"];
            var query = "q=userId_s:" + q;

            var url = "https://api.bonnier.se/v1/entitlements";

            url = url + "?access_token=" + token + "&" + query;


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/xml";

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string json = sr.ReadToEnd();
            sr.Close();

            litOutput.Text = json;
        }
    }
}