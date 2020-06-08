using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using DIClassLib.DbHelpers;
using System.Web;


namespace DIClassLib.SingleSignOn
{

    /// <summary>
    /// Stores session variables for S+ logged in user.
    /// </summary>
    public class SsoLoginHandler
    {
        /// <summary>
        /// value stored in session
        /// </summary>
        public bool IsLoggedInToBonDig
        {
            get
            {
                if (HttpContext.Current.Session["ssoIsLoggedIn"] != null)
                    return (bool)HttpContext.Current.Session["ssoIsLoggedIn"];

                return false;
            }
            set
            {
                HttpContext.Current.Session["ssoIsLoggedIn"] = value;
            }
        }

        /// <summary>
        /// value stored in session
        /// </summary>
        public string Token
        {
            get
            {
                if (HttpContext.Current.Session["ssoToken"] != null)
                    return HttpContext.Current.Session["ssoToken"].ToString();

                return null;
            }
            set
            {
                HttpContext.Current.Session["ssoToken"] = value;
            }
        }
        
        public SsoLoginHandler() { }                

        //public string Token { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Remembered { get; set; }
        //public string Callback { get; set; }
        //public DateTime DateUpdated { get; set; }
        //public string UrlCheckLogin 
        //{
        //    get
        //    { 
        //        // account.qa.newsplus.se/check-logged-in?appId=dagensindustri.se&lc=sv&callback=http://localhost
        //        string url = ConfigurationManager.AppSettings["BonDigUrlAccount"] +
        //                     ConfigurationManager.AppSettings["BonDigCheckLoginPage"] +
        //                     "?appId=" + ConfigurationManager.AppSettings["BonDigAppIdDagInd"] +
        //                     "&lc=sv" +
        //                     "&callback=" + Callback;

        //        return url;
        //    }
        //}

        


        //private XmlDocument TryGetRssFromDise()
        //{
        //    XmlDocument xmlDoc = null;

        //    try
        //    {
        //        string url = ConfigurationManager.AppSettings["DiIpadPathDiRss"];
        //        XmlTextReader reader = new XmlTextReader(url + "?latest=" + ConfigurationManager.AppSettings["DiIpadNumRssItems"] + "&text=1&comments=1");
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(reader);
        //        //return doc.SelectSingleNode("root/status").InnerText;
        //        //return _xmlDocRssLatest.InnerXml;  //string
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("TryGetRssFromDise() failed", ex.ToString());
        //        xmlDoc = null;
        //    }

        //    return xmlDoc;
        //}

    }
}
