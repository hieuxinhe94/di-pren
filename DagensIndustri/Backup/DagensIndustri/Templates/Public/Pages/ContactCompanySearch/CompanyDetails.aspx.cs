using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.OneByOne;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Pages.ContactCompanySearch
{
    public partial class CompanyDetails : DiTemplatePage
    {
        #region Properties
        public DocumentFactoryService DocumentFactory { get; set; }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            EPiFunctions.SetAttributeOnControl(Page.Master, "Body", "class", "bizbook");
            DocumentFactory = EPiFunctions.SetUpOneByOneDocumentFactory();
            if (DocumentFactory == null)
            {
                ShowMessage("/common/errors/error", true, true);
            }
            else
            {
                WorksiteDetailsControl.DocumentFactory = DocumentFactory;
                DecisionMakersControl.DocumentFactory = DocumentFactory;
                BusinessControl.DocumentFactory = DocumentFactory;
                FinancialDetailsControl.DocumentFactory = DocumentFactory;

                DataBind();
            }            
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Create an url to contact and worksite search page, containing necessary search parameters from querystring.
        /// </summary>
        /// <returns></returns>
        protected string CreateBackToSearchResultUrl()
        {
            string backUrl = "#";
            StringBuilder urlBuilder = new StringBuilder();
            if (CurrentPage["ContactCompanySearchPage"] != null)
            {
                urlBuilder.Append(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage["ContactCompanySearchPage"] as PageReference));
                urlBuilder.AppendFormat("?who={0}", HttpUtility.UrlEncode(Request.QueryString["who"]));
                urlBuilder.AppendFormat("&what={0}", HttpUtility.UrlEncode(Request.QueryString["what"]));
                urlBuilder.AppendFormat("&where={0}", HttpUtility.UrlEncode(Request.QueryString["where"]));
                urlBuilder.AppendFormat("&index={0}", Request.QueryString["index"]);
                
                backUrl = urlBuilder.ToString();
            }

            return backUrl;
        }

        /// <summary>
        /// Create an url to update company contact details, containing necessary search parameters from querystring.
        /// </summary>
        /// <returns></returns>
        protected string CreateUpdateCompanyDetailsUrl()
        {
            string url = "#";
            if (!string.IsNullOrEmpty(Request.QueryString["worksiteid"]))
            {
                url = string.Format("{0}?worksiteid={1}",
                                        EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage["UpdateCompanyDetailsPage"] as PageReference),
                                        HttpUtility.UrlEncode(Request.QueryString["worksiteid"]));
            }
            return url;
        }
        #endregion
    }
}