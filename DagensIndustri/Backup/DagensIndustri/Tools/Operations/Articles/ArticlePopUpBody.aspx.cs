using System;
using System.IO;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Web.UI;
using DIClassLib.DbHelpers;

namespace EPiServer.Functions.Articles
{
    /// <summary>
    ///     
    /// </summary>
    public partial class ArticlePopUpBody : Page
    {
        protected string p_Title = string.Empty;
        protected string p_Headline = string.Empty;
        protected string p_PublishDate = string.Empty;
        protected string p_Authors = string.Empty;
        protected string p_ArticleText = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {                
                string p_artlink = Request.QueryString["art"];

                //do nothing if length is smaller than 11
                //this to prevent substring function to cause exception if user manipulates the querystring
                if (string.IsNullOrEmpty(p_artlink) || p_artlink.Length < 11)
                    return;

                string p_Datepath = p_artlink.Substring(3, 4) + "/" + p_artlink.Substring(7, 2) + "/" + p_artlink.Substring(9, 2);
                p_PublishDate = p_Datepath.Replace("/", "-");

                processXML(p_artlink, p_Datepath);
            }
        }

        private void processXML(string p_artlink, string p_Datepath)
        {            
            string p_XMLPath = ConfigurationManager.AppSettings["PDFPaperPath"] + "/" + p_Datepath + "/" + p_artlink + ".xml";

            try
            {
                if (!File.Exists(p_XMLPath))
                    return;

                XmlDocument XMLDoc = new XmlDocument();
                XMLDoc.Load(p_XMLPath);

                p_Headline = XMLDoc.GetElementsByTagName("Headline")[0].InnerText;
                p_Title = p_Headline + " [" + p_PublishDate + "]";
                p_ArticleText = XMLDoc.GetElementsByTagName("Body")[0].InnerXml;                

                XmlNodeList oAuthorNodes = XMLDoc.GetElementsByTagName("Author");

                foreach (XmlNode oAuthorNode in oAuthorNodes)
                {
                    for (int i = 0; i <= oAuthorNode.ChildNodes.Count - 1; i++)
                        p_Authors += oAuthorNode.ChildNodes.Item(i).InnerText + " ";                    
                }
            }
            catch (Exception ex)
            {
                new Logger("processXML() - failed", ex.ToString());
            }
        }

    }
}
