using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Static
{
    public partial class RssSlider : EPiServer.UserControlBase
    {
        protected class RSSItem
        {
            public int ID { get; set; }
            public string Title{ get; set; }
            public string Link { get; set; }
            public DateTime PublishedDate{ get; set; }
            public string FormattedPublishedDate
            {
                get
                {
                    string formattedPublished = string.Empty;
                    if (PublishedDate.ToShortDateString().Equals(DateTime.Today.ToShortDateString()))
                    {
                        formattedPublished = string.Format("Idag {0}", PublishedDate.ToString("HH:mm"));
                    }
                    else
                    {
                        formattedPublished = PublishedDate.ToString("yyyy-MM-dd HH:mm");
                    }

                    return formattedPublished;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                string rssUrl = EPiFunctions.SettingsPageSetting(CurrentPage, "RSSUrl") as string;
                int maxNoOfRSS = Convert.ToInt32(EPiFunctions.SettingsPageSetting(CurrentPage, "RSSMaxNo"));
                RepRSS.DataSource = GetRSSItems(rssUrl, maxNoOfRSS);
                PlaceHolderRssWrapper.DataBind();
                
                //RepRSS.DataBind();
            }
        }

        /// <summary>
        /// Get a list containing the RSS news items from a specified URL . 
        /// </summary>
        /// <param name="rssUrl"></param>
        /// <param name="maxNoOfRSS"></param>
        /// <returns></returns>
        private List<RSSItem> GetRSSItems(string rssUrl, int maxNoOfRSS)
        {           
            List<RSSItem> rssItems = new List<RSSItem>();

            if (!string.IsNullOrEmpty(rssUrl) && maxNoOfRSS > 0)
            {
                try
                {
                    XmlTextReader xmlReader = new XmlTextReader(rssUrl);
                
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);

                    XmlNodeList rssItemNodes = xmlDoc.SelectNodes("rss/channel/item");

                    int counter = 0;
                    while(rssItems.Count < maxNoOfRSS)
                    {
                        XmlNode xmlNode = rssItemNodes[counter];
                        counter++;
                        RSSItem item = new RSSItem
                        {
                            ID = counter,
                            Title = xmlNode.SelectSingleNode("title").InnerText,
                            Link = xmlNode.SelectSingleNode("link").InnerText,
                            PublishedDate = Convert.ToDateTime(xmlNode.SelectSingleNode("pubDate").InnerText),
                        };

                        rssItems.Add(item);
                    }
                }
                catch { }
            }

            return rssItems;
        }
    }
}