using System;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web;
using System.Configuration;
using DIClassLib.DbHelpers;

namespace EPiServer.Functions.Articles
{
    /// <summary>
    ///     
    /// </summary>
    public partial class ArticlePopUp : Page
    {
        protected string p_Title = string.Empty;
        protected string p_artlink = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {                                               
                try
                {
                    p_artlink = Request.QueryString["art"];

                    //do nothing if length is smaller than 11
                    //this to prevent substring function to cause exception if user manipulates the querystring
                    if (string.IsNullOrEmpty(p_artlink) || p_artlink.Length < 11)
                        return;

                    string p_Datepath = p_artlink.Substring(3, 4) + "/" + p_artlink.Substring(7, 2) + "/" + p_artlink.Substring(9, 2);
                    string p_PublishDate = p_Datepath.Replace("/", "-");
                    string XMLPath = ConfigurationManager.AppSettings["PDFPaperPath"] + "/" + p_Datepath + "/" + p_artlink + ".xml";

                    if (!File.Exists(XMLPath))
                        return;

                    XmlDocument XMLDoc = new XmlDocument();
                    XMLDoc.Load(XMLPath);
                    string p_Headline = XMLDoc.GetElementsByTagName("Headline")[0].InnerText;
                    p_Title = p_Headline + " [" + p_PublishDate + "]";                    
                }
                catch (Exception ex)
                {
                    new Logger("ArticlePopUp.OnLoad() - failed", ex.ToString());
                }
                
            }
        }



    }
}
