using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Units.Placeable.ArchiveSearch
{
    public partial class TextArchiveSearch : UserControlBase
    {
        #region Properties
        private string SearchText
        {
            get
            {
                return (string)ViewState["SearchText"];
            }
            set
            {
                ViewState["SearchText"] = value;
            }
        }

        private Pages.ArchiveSearch ArchiveSearchPage
        {
            get
            {
                return (Pages.ArchiveSearch)Page;
            }
        }

        private DataSet SearchResultDatSet
        {
            get
            {
                return ViewState["SearchResultDatSet"] as DataSet;
            }
            set
            {
                ViewState["SearchResultDatSet"] = value;
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

            RegisterScript();

            if (IsPostBack)
            {
                PagingControl.CreatePagingControl();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            SearchText = SearchInput.Text;
            PagingControl.CurrentIndex = 1;

            //Search the archive
            Search();

            //Populate with the search result 
            Populate(0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Register clientscripts. When "Search" is clicked, the id of the selected tab is saved in hidden fields.        
        /// </summary>
        private void RegisterScript()
        {
            HiddenField SelectedTabHiddenField = ArchiveSearchPage.HiddenFieldSelectedTab;
            HyperLink TextSearchHyperLink = ArchiveSearchPage.HyperLinkTextSearch;

            // Create script for click on Send password where selected tab and section will be stored in hiddenfields
            string script = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('{2}');
                                                }})
                                            }});",
                                            SearchButton.ClientID,
                                            SelectedTabHiddenField.ClientID,
                                            TextSearchHyperLink.NavigateUrl
                                        );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "TextSearchButton_Click", script, true);
        }

        private void Search()
        {
            try
            {
                //if no search text, do nothing
                if (string.IsNullOrEmpty(SearchText) || SearchText.Length < 2)
                    return;

                //Remove non alphanumerc characters
                string searchText = Regex.Replace(SearchText.Trim(' '), @"[^\sa-öA-Ö0-9]", "");
                //Remove duplicate whitespace
                searchText = Regex.Replace(searchText.Trim(' '), @"\s+", " ");

                //TODO: Ugly as hell, but works for now                                            
                //Temporary replace keyword and
                searchText = searchText.ToLower().Replace(" and ", "#and#");
                //Replace remaining white space with keyword or
                searchText = searchText.Replace(" ", " or ");
                //Restore keyword and
                searchText = searchText.Replace("#and#", " and ");

                //Indexing server name
                string serverName = MiscFunctions.GetAppsettingsValue("MsidxsRemoteServerName");
                //Indexing catalog name
                string catalogName = MiscFunctions.GetAppsettingsValue("MsidxsRemoteCatalogName");

                //build query
                string strQuery = "SELECT DocTitle, vpath, path, filename, publishdate, prenlevel, Contents " +
                  "FROM " + serverName + "." + catalogName + "..Scope() WHERE ";

                strQuery += "prenlevel = '1' AND publishdate <= '" + DateTime.Now.Date + "' AND CONTAINS(Contents, '" + searchText + "') ORDER BY path DESC"; //TODO: Nazgol lägg tillbaka
                //strQuery += "CONTAINS(Contents, '" + searchText + "') ORDER BY path DESC"; 

                OleDbConnection conn = new OleDbConnection("Provider=MSIDXS.1;");
                conn.Open();
                OleDbDataAdapter cmd = new OleDbDataAdapter(strQuery, conn);
                DataSet dsSearchResult = new DataSet("IndexServerResults");
                
                cmd.Fill(dsSearchResult, 0, 150, "IndexServerResults");

                SearchResultDatSet = dsSearchResult;
            }
            catch (Exception ex)
            {
                new Logger("Search() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }
        }

        /// <summary>
        /// Calculate the specified number of product pages according to the paging position and 
        /// populate according to that. OBS! Do NOT change the method name or its modifier since
        /// it is used with reflection.
        /// </summary>
        public void Populate()
        {
            Populate(PagingControl.CurrentIndex - 1);
        }

        /// <summary>
        /// Get the specified number of hits according to the paging position and fill the search result repeater.
        /// </summary>
        /// <param name="offset"></param>
        protected void Populate(int pageIndex)
        {
            //Clear the paging area from old results
            PagingControl.Clear();

            try
            {
                //if no search result were found, do nothing
                if (SearchResultDatSet == null || SearchResultDatSet.Tables[0].Rows.Count == 0)                                
                    return;

                //show searcharea
                SearchResultPlaceHolder.Visible = true;

                //Total number of hits                
                PagingControl.TotalNumberOfHits = SearchResultDatSet.Tables[0].Rows.Count;
                if (PagingControl.TotalNumberOfHits > 0)
                {
                    //Create paging data source
                    PagedDataSource pagedDataSrc = new PagedDataSource();
                    pagedDataSrc.AllowPaging = true;
                    pagedDataSrc.DataSource = SearchResultDatSet.Tables[0].DefaultView;
                    pagedDataSrc.PageSize = PagingControl.NoOfHitsPerPage;
                    pagedDataSrc.CurrentPageIndex = pageIndex;

                    SearchResultRepeater.DataSource = pagedDataSrc;
                    SearchResultRepeater.DataBind();

                    int offset = CalculateOffset();
                    int startIndex = offset + 1;
                    
                    int endIndex = offset + PagingControl.NoOfHitsPerPage;
                    if (endIndex > PagingControl.TotalNumberOfHits)
                        endIndex = PagingControl.TotalNumberOfHits;

                    SearchResultLiteral.Text = string.Format(Translate("/archive/article/textsearch/showsearchresult"), startIndex.ToString(), endIndex.ToString(), PagingControl.TotalNumberOfHits);
    
                    //Create paging control if total number of hits exceeds the specified number of hits per page
                    if (PagingControl.TotalNumberOfHits > PagingControl.NoOfHitsPerPage)
                    {
                        PagingControl.CreatePagingControl();
                    }
                }
                else
                {
                    SearchResultLiteral.Text = String.Format(Translate("/archive/article/textsearch/nohits"), SearchText);
                }
            }
            catch (Exception ex)
            {
                new Logger("Populate(int offset) - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }
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
        /// Get javascript link to the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string GetLink(string fileName)
        {
            if (fileName.ToLower().StartsWith("di-")) //Xcago
                return "javascript:DiNatetOpenPopup('/PDFTidning/" + fileName.Substring(3, 4) + "/" + fileName.Substring(7, 2) + "/" + fileName.Substring(9, 2) + "/public/pages/" + fileName.Substring(12, 5) + "/articles/" + fileName.Replace("_body", "") + "')";
            else if (fileName.ToLower().StartsWith("nsdi-")) //Ninestars
                return "javascript:DiNatetOpenPopup('/Tools/Operations/Articles/ArticlePopUp.aspx?art=" + fileName.Substring(2, 20) + "')";

            return "#";
        }

        /// <summary>
        /// Get title of the document. Remove date if the title contains publish date.
        /// </summary>
        /// <param name="docTitle"></param>
        /// <returns></returns>
        protected string GetTitle(string docTitle)
        {
            string title = docTitle;
            if (!string.IsNullOrEmpty(title))
            {
                int index1 = title.IndexOf("[");
                int index2 = title.LastIndexOf("]");

                string tempStr = title.Substring(index1+1, index2-index1-1);
                DateTime dateTime = DateTime.MinValue;
                if (DateTime.TryParse(tempStr, out dateTime))
                    title = title.Replace("[" + tempStr + "]", "").Trim();
            }

            return title;
        }
        #endregion
    }
}