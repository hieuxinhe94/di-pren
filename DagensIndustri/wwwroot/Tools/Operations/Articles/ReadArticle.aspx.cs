using System;
using System.Web;
using System.IO;
using System.Text;
using System.Xml;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;

namespace DagensIndustri.Tools.Operations.Articles
{
    public partial class ReadArticle : DiTemplatePage
    {
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ShowArticle();
        }

        private void ShowArticle()
        {
            try
            {
                string p_ArticleId = MiscFunctions.RC4EnDeCrypt(Server.UrlDecode(Request.QueryString["DIArtId"]), "pGEghLjDwkGDf8");                
                int aBodyStart = 0;

                StringBuilder sbArticle = new StringBuilder();

                if (p_ArticleId.StartsWith("NSDI"))
                {
                    p_ArticleId = p_ArticleId.Substring(2);
                    string datepath = p_ArticleId.Substring(3, 4) + "/" + p_ArticleId.Substring(7, 2) + "/" + p_ArticleId.Substring(9, 2);
                    string p_XMLPath = System.Configuration.ConfigurationManager.AppSettings["PDFPaperPath"] + "/" + datepath + "/" + p_ArticleId + ".xml";

                    XmlDocument XMLDoc = new XmlDocument();
                    XMLDoc.Load(p_XMLPath);
                    sbArticle.Append("<div class=\"MainColumnTable\"><h1>" + XMLDoc.GetElementsByTagName("Headline")[0].InnerText + "</h1>");
                    sbArticle.Append("<span class=\"Brodtext\">" + XMLDoc.GetElementsByTagName("Body")[0].InnerXml + "</span>");

                    string Authors = string.Empty;
                    XmlNodeList oAuthorNodes = XMLDoc.GetElementsByTagName("Author");

                    foreach (XmlNode oAuthorNode in oAuthorNodes)
                    {
                        for (int i = 0; i <= oAuthorNode.ChildNodes.Count - 1; i++)
                            Authors += oAuthorNode.ChildNodes.Item(i).InnerText + " ";
                    }
                    sbArticle.Append("<span class=\"Brodtext\">" + Authors + "</span></div>");
                    litContent.Text = sbArticle.ToString();
                }
                else
                {
                    sbArticle.Append(RenderStaticHTMLFile(System.Configuration.ConfigurationManager.AppSettings["PDFPaperPath"] + p_ArticleId.Substring(3, 4) + "\\" + p_ArticleId.Substring(7, 2) + "\\" + p_ArticleId.Substring(9, 2) + "\\public\\pages\\" + p_ArticleId.Substring(12, 5) + "\\articles\\" + p_ArticleId + "_body.html"));
                    aBodyStart = sbArticle.ToString().ToLower().IndexOf("<body>") + 6;
                    litContent.Text = sbArticle.ToString().Substring(aBodyStart, sbArticle.ToString().ToLower().IndexOf("</body>") - aBodyStart);
                }
            }
            catch
            {
                litContent.Text = Translate("/articlesearch/readarticle/error");
            }
        }

        public string RenderStaticHTMLFile(string FileToRead)
        {
            try
            {
                StreamReader StreamReader = File.OpenText(FileToRead);
                string HTML = StreamReader.ReadToEnd();
                StreamReader.Close();
                StreamReader = null;
                return HTML;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
