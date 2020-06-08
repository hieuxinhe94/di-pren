using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Membership;
using EPiServer;
using EPiServer.Core;
using System.Web.UI.HtmlControls;
using EPiServer.Core.Html;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;
using System.Web.Security;

namespace DagensIndustri.Templates.Public.Units.Static
{
    public partial class Header : EPiServer.UserControlBase
    {
        public string cssUrl { get; set; }

        public string Cusno
        {
            get
            {
                if (Session["SessionLoggedInCusno"] != null)
                    return Session["SessionLoggedInCusno"].ToString();

                if (MembershipFunctions.UserIsLoggedInWithProvider(MembershipSettings.DiMembershipProviderName))
                {
                    int cusno = DIClassLib.DbHandlers.MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name);
                    if (cusno > 100)
                    {
                        Session["SessionLoggedInCusno"] = cusno.ToString();
                        return cusno.ToString();
                    }
                }
            
                return "false";
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            cssUrl = CurrentPage["MasterPageSelector"] != null ? CurrentPage["MasterPageSelector"].ToString() : "/Templates/Public/Styles/dagensindustri.css";

            string separator = string.Empty;

            if (CurrentPage["PageTitle"] != null)
                separator = " -";

            if (CurrentPage["MasterPageSelector"] != null)
            {
                if (CurrentPage["MasterPageSelector"].ToString() != "/Templates/Public/Styles/dagensindustri.css")
                {
                    //PageTitleLiteral.Text = string.Format("{0}{1} Di Guld", CurrentPage["PageTitle"] ?? "" , separator);
                    PageTitleLiteral.Text = string.Format("{0}{1} Di Guld", CurrentPage.PageName ?? "", separator);
                }
                else
                {
                    //PageTitleLiteral.Text = string.Format("{0}{1} Dagensindustri", CurrentPage["PageTitle"] ?? "", separator);
                    PageTitleLiteral.Text = string.Format("{0}{1} Dagensindustri", CurrentPage.PageName ?? "", separator);
                }
            }   

            PageData diGoldStartPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "DIGoldStartPage") as PageReference);

            if (CurrentPage.PageLink == PageReference.StartPage || CurrentPage.PageLink == diGoldStartPage.PageLink)
            {
                ModsLiteral.Text = "<link media=\"all\" rel=\"stylesheet\" type=\"text/css\" href=\"/Templates/Public/Styles/startpagemods.css\" />";
            }

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "CampaignPageType"))
            {
                CampaignCssLiteral.Text = "<link media=\"all\" rel=\"stylesheet\" type=\"text/css\" href=\"/Templates/Public/Styles/campaign-tool.css\" />";
                RegisterClientScriptFile("/Templates/Public/js/campaign-tool.js");
            }

            CreateMetaData();

            if (IsValue("PageExternalURL"))
            {
                var canonicalLink = new HtmlGenericControl("link");
                canonicalLink.Attributes.Add("rel", "canonical");
                canonicalLink.Attributes.Add("href", MiscFunctions.RemoveWwwFromUrl(EPiServer.Configuration.Settings.Instance.SiteUrl.ToString()) + CurrentPage["PageExternalURL"].ToString());

                this.SimpleAddressPlaceHolder.Controls.Add(canonicalLink);
            }
        }

        /// <summary>
        /// Creates the metadata tags for the website
        /// </summary>
        private void CreateMetaData()
        {
            // Description - use MetaDescription property if it exists, otherwise use contents 
            // from MainIntro property. Do not display the meta tag if none of these options return 
            // values.
            string metaDescription = GetPropertyString("MetaDescription", CurrentPage);
            if (metaDescription.Equals(string.Empty))
            {
                metaDescription = GetPropertyString("MainIntro", CurrentPage);
                if (!metaDescription.Equals(string.Empty))
                {
                    metaDescription = TextIndexer.StripHtml(metaDescription, 0);
                    if (metaDescription.Length > 255)
                        metaDescription = metaDescription.Substring(0, 252) + "...";
                }
            }
            if (!metaDescription.Equals(string.Empty))
            {
                CreateMetaTag("description", metaDescription, false);
            }

            // Keywords
            CreateMetaTag("keywords", "MetaKeywords", CurrentPage, false);

            // Author
            CreateMetaTag("author", "MetaAuthor", CurrentPage, false);

            // Rating
            CreateMetaTag("rating", "General", false);

            // Revisit each month
            CreateMetaTag("revisit-after", "4 weeks", false);

            // Generator
            CreateMetaTag("generator", "EPiServer", false);

            // Robots
            string robots = "all";

            if (DagensIndustri.Tools.Classes.EPiFunctions.SettingsPageSetting(CurrentPage, "ExcludeFromRobots") != null)
            {
                string[] pagetypesID = DagensIndustri.Tools.Classes.EPiFunctions.SettingsPageSetting(CurrentPage, "ExcludeFromRobots").ToString().Split(',');

                if (pagetypesID.Contains(CurrentPage.PageTypeID.ToString()))
                    robots = "noindex,nofollow";
            }
            
            CreateMetaTag("robots", robots, false);

            // Charset
            CreateMetaTag("Content-Type", string.Format("text/html; charset={0}", "UTF-8"), true);

            // Created - GMT format
            if (CurrentPage.Created != DateTime.MinValue)
            {
                CreateMetaTag("creation_date", CurrentPage.Created.ToString("R"), false);
            }
            // Last modified data, in GMT format - Note, same as revised
            if (CurrentPage.Changed != DateTime.MinValue)
            {
                CreateMetaTag("last-modified", CurrentPage.Changed.ToString("R"), false);
            }
            // Revised - GMT format
            if (CurrentPage.Changed != DateTime.MinValue)
            {
                CreateMetaTag("revised", CurrentPage.Changed.ToString("R"), false);
            }
            CreateMetaTag("Content-Language", CurrentPage.LanguageBranch, true);
        }

        /// <summary>
        /// Adds a meta tag control to the page header
        /// </summary>
        /// <param name="name">The name of the meta tag</param>
        /// <param name="content">The content of the meta tag</param>
        /// <param name="httpEquiv">True if the meta tag should be HTTP-EQUIV</param>
        private void CreateMetaTag(string name, string content, bool httpEquiv)
        {
            HtmlMeta tag = new HtmlMeta();
            if (httpEquiv)
            {
                tag.HttpEquiv = name;
            }
            else
            {
                tag.Name = name;
            }
            tag.Content = content;
            MetaDataAreaPlaceHolder.Controls.Add(tag);
        }

        /// <summary>
        /// Adds a meta tag control to the page header
        /// </summary>
        /// <param name="name">The name of the meta tag</param>
        /// <param name="propertyName">The name of the me tag page property</param>
        /// <param name="pageData">The page from where to get the property</param>
        /// <param name="httpEquiv">True if the meta tag should be HTTP-EQUIV</param>
        private void CreateMetaTag(string name, string propertyName, PageData pageData, bool httpEquiv)
        {
            string property = pageData[propertyName] as string;
            if (property != null)
            {
                CreateMetaTag(name, property, httpEquiv);
            }
        }

        /// <summary>
        /// Gets a page property as a string
        /// </summary>
        /// <param name="PropertyName">The name of the property to get</param>
        /// <param name="pageData">The page from where to get the property</param>
        /// <returns>A string representation of the page property</returns>
        private static string GetPropertyString(string PropertyName, PageData pageData)
        {
            return pageData[PropertyName] as string ?? String.Empty;
        }
    }
}