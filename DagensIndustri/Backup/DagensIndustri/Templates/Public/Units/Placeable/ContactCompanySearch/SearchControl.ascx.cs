using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.OneByOne;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Templates.Public.Units.Placeable.ContactCompanySearch
{
    public partial class SearchControl : UserControlBase
    {
        #region Properties

        /// <summary>
        /// Gets a reference to the page instance that contains the control
        /// </summary>
        private Pages.ContactCompanySearch.Search SearchPage
        {
            get
            {
                return (Pages.ContactCompanySearch.Search)Page;
            }
        }      

        /// <summary>
        /// Gets whether the search is person or worksite search
        /// </summary>
        private bool IsPersonSearch
        {
            get
            {
                return !string.IsNullOrEmpty(NameInput.Text.Trim());
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PagingControl.ParentControl = this;
            int noOfHitsPerPage = Convert.ToInt32(CurrentPage["NoOfHitsPerPage"]);
            PagingControl.NoOfHitsPerPage = noOfHitsPerPage == 0 ? 10 : noOfHitsPerPage;

            if (!IsPostBack)
            {
                Initialize();
            }
            else
            {
                PagingControl.CreatePagingControl();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            PagingControl.CurrentIndex = 1;

            //Populate the search result 
            Populate(0);

            new Logger(Settings.LogEvent_CompanySearch,  MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name) , true);
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            NameInput.Text = HttpUtility.UrlDecode(Request.QueryString["who"]);
            CompanyInput.Text = HttpUtility.UrlDecode(Request.QueryString["what"]);
            AddressInput.Text = HttpUtility.UrlDecode(Request.QueryString["where"]);

            int index = -1;
            PagingControl.CurrentIndex = int.TryParse(Request.QueryString["index"], out index) ? index : 1;

            Populate(CalculateOffset());
        }

        /// <summary>
        /// Calculate the specified number of product pages according to the paging position and 
        /// populate according to that. OBS! Do NOT change the method name or its modifier since
        /// it is used with reflection.
        /// </summary>
        public void Populate()
        {
            Populate(CalculateOffset());
        }

        /// <summary>
        /// Get the specified number of hits according to the paging position and fill the search result repeater.
        /// </summary>
        /// <param name="offset"></param>
        private void Populate(int offset)
        {
            //If no search criteria has been given, do nothing.
            if (string.IsNullOrEmpty(NameInput.Text.Trim()) && string.IsNullOrEmpty(CompanyInput.Text.Trim()) && string.IsNullOrEmpty(AddressInput.Text.Trim()))
                return;

            //Hide main intro and main body
            SearchPage.HideControls();

            //Clear the paging area from old results
            PagingControl.Clear();

            try
            {
                //Perform search with appropriate parameters
                string xml = PerformSearch(offset);

                if (string.IsNullOrEmpty(xml))
                    return;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                XmlNodeList xmlNodeList = null;
                XmlNode searchResultXmlNode = null;
                if (IsPersonSearch)
                {
                    searchResultXmlNode = xmlDoc.SelectSingleNode("Bizbook-Search-C/SearchResult");
                    xmlNodeList = xmlDoc.SelectNodes("Bizbook-Search-C/SearchResult/Hit");
                }
                else
                {
                    searchResultXmlNode = xmlDoc.SelectSingleNode("Bizbook-Search-W/SearchResult");
                    xmlNodeList = xmlDoc.SelectNodes("Bizbook-Search-W/SearchResult/Hit");
                }

                int hitsReturned = 0;
                if (searchResultXmlNode != null)
                {
                    //Get the number of hits returned based on the given search criterias. 
                    int.TryParse(searchResultXmlNode.Attributes["hitsReturned"].Value, out hitsReturned);

                    //Get the total number of hits found based on the given search criterias. 
                    int totalNoOfHits = 0;
                    int.TryParse(searchResultXmlNode.Attributes["hitsTotal"].Value, out totalNoOfHits);
                    PagingControl.TotalNumberOfHits = totalNoOfHits;
                }

                SearchResultPlaceHolder.Visible = true;
                PagingControl.Visible = PagingControl.TotalNumberOfHits > 0;

                if (PagingControl.TotalNumberOfHits > 0)
                {
                    //Create paging data source
                    PagedDataSource pagedDataSrc = new PagedDataSource();
                    pagedDataSrc.AllowCustomPaging = true;
                    pagedDataSrc.AllowPaging = true;
                    pagedDataSrc.DataSource = xmlNodeList;
                    pagedDataSrc.PageSize = PagingControl.NoOfHitsPerPage;

                    //If person name was given as search criteria then it is a person search. Fill PersonSearchResultRepeater with search result.
                    //Otherwise it is worksite search. Fill WorkSiteSearchResultRepeater with search result.
                    if (IsPersonSearch)
                    {
                        FillPersonSearchRepeater(pagedDataSrc);
                    }
                    else
                    {
                        FillWorkSiteSearchRepeater(pagedDataSrc);
                    }

                    int startIndex = offset + 1;
                    int endIndex = offset + hitsReturned;
                    SearchResultLiteral.Text = string.Format(Translate("/contactcompanysearch/search/searchresult/showsearchresult"), startIndex.ToString(), endIndex.ToString(), PagingControl.TotalNumberOfHits);

                    //Create paging control if total number of hits exceeds the specified number of hits per page
                    if (PagingControl.TotalNumberOfHits > PagingControl.NoOfHitsPerPage)
                    {
                        PagingControl.CreatePagingControl();
                    }
                }
                else
                {
                    SearchResultLiteral.Text = Translate("/contactcompanysearch/search/searchresult/noreturn");
                    PersonSearchResultRepeater.Visible = false;
                    WorkSiteSearchResultRepeater.Visible = false;
                }
            }
            catch (Exception ex)
            {
                new Logger("Populate() - failed", ex.ToString());
                SearchPage.ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Perform search by calling the OBO web service with appropriate parameters
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private string PerformSearch(int offset)
        {
            DocumentFactoryService factory = EPiFunctions.SetUpOneByOneDocumentFactory();
            if (factory == null)
            {
                SearchPage.ShowMessage("/common/errors/error", true, true);
                return null;
            }
           
            RequestParameter[] parameters = new RequestParameter[5];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = new RequestParameter();
            }

            parameters[0].name = "who";
            parameters[0].value = NameInput.Text.Trim();

            parameters[1].name = "what";
            parameters[1].value = CompanyInput.Text.Trim();

            parameters[2].name = "where";
            parameters[2].value = AddressInput.Text.Trim();

            parameters[3].name = "maxSize";
            parameters[3].value = PagingControl.NoOfHitsPerPage.ToString();

            parameters[4].name = "offset";
            parameters[4].value = offset.ToString();

            string documentName = IsPersonSearch
                                ? ConfigurationManager.AppSettings["OBOSearchPerson"]
                                : ConfigurationManager.AppSettings["OBOSearchWorkSite"];

            return factory.getDocument(documentName, parameters);
        }        
        
        /// <summary>
        /// Create offset for the paging.
        /// </summary>
        /// <returns></returns>
        private int CalculateOffset()
        {
            return (PagingControl.CurrentIndex - 1) * PagingControl.NoOfHitsPerPage;
        }
        
        /// <summary>
        /// Fill person search repeater with data. 
        /// </summary>
        /// <param name="pagedDataSrc"></param>
        private void FillPersonSearchRepeater(PagedDataSource pagedDataSrc)
        {
            PersonSearchResultRepeater.DataSource = pagedDataSrc;
            PersonSearchResultRepeater.DataBind();

            //Hide work site repeater and show person repeater if data exists
            PersonSearchResultRepeater.Visible = pagedDataSrc != null && pagedDataSrc.Count > 0;
            WorkSiteSearchResultRepeater.Visible = false;
        }
        
        /// <summary>
        /// Fill work site search repeater with data. 
        /// </summary>
        /// <param name="pagedDataSrc"></param>
        private void FillWorkSiteSearchRepeater(PagedDataSource pagedDataSrc)
        {
            WorkSiteSearchResultRepeater.DataSource = pagedDataSrc;
            WorkSiteSearchResultRepeater.DataBind();

            //Hide person repeater and show work site repeater if data exists
            WorkSiteSearchResultRepeater.Visible = pagedDataSrc != null && pagedDataSrc.Count > 0;
            PersonSearchResultRepeater.Visible = false;
        }
        
        /// <summary>
        /// Get map url for the address
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        protected string GetMapsUrl(XmlNode xmlNode)
        {
            return EPiFunctions.GetMapsUrl(MiscFunctions.GetXmlNodeText(xmlNode, "Address"), MiscFunctions.GetXmlNodeText(xmlNode, "City"));
        }

        /// <summary>
        /// Get the url to worksite details page together with necessary values in the querystring.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        protected string GetWorksiteDetailUrl(XmlNode xmlNode)
        {
            if (CurrentPage["ContactCompanyDetailsPage"] == null)
                return string.Empty;

            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage["ContactCompanyDetailsPage"] as PageReference));
            if (xmlNode != null)
            {
                string worksiteId = IsPersonSearch
                                ? MiscFunctions.GetXmlNodeText(xmlNode, "WorksiteDbId")
                                : MiscFunctions.GetXmlAttributeText(xmlNode, "dbId");

                urlBuilder.AppendFormat("?worksiteid={0}", worksiteId);

                string companynumber = string.Empty;
                if (!IsPersonSearch)
                {
                    urlBuilder.AppendFormat("&companynumber={0}", MiscFunctions.GetXmlNodeText(xmlNode, "CompanyNumber"));
                }

                urlBuilder.AppendFormat("&who={0}&what={1}&where={2}&index={3}",
                                        HttpUtility.UrlEncode(NameInput.Text.Trim()), 
                                        HttpUtility.UrlEncode(CompanyInput.Text.Trim()), 
                                        HttpUtility.UrlEncode(AddressInput.Text.Trim()),
                                        PagingControl.CurrentIndex);
            }

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Add contact id to the url for worksite detail's page
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        protected string GetWorksiteContactDetailUrl(XmlNode xmlNode)
        {
            string url = GetWorksiteDetailUrl(xmlNode);
            if (!string.IsNullOrEmpty(url))
            {
                url = string.Format("{0}&contactid={1}", url, MiscFunctions.GetXmlAttributeText(xmlNode, "dbId"));
            }
            return url;
        }
        #endregion
    }
}