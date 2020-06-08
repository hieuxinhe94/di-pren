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
    public partial class VerifyEntitlement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var token = Request.QueryString["access_token"];
            var externalResourceId = Request.QueryString["externalResourceId"];


            string url = "https://api.bonnier.se/v1/resources/verify-entitlement" + "?externalResourceId=" + externalResourceId + "&access_token=" + token;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "application/xml";

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string json = sr.ReadToEnd();
            sr.Close();

            litOutPut.Text = json;
        }
    }
}