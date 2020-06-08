using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Configuration;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;
using System.IO;
using DIClassLib.Membership;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Units.Placeable.ArchiveSearch
{
    public partial class DateArchiveSearch : EPiServer.UserControlBase
    {
        #region Properties

        public string PDFArchivePath = MiscFunctions.GetAppsettingsValue("PDFArchivePath");
        private bool IsIssue { get; set; }
        public string RequestedIssue 
        {
            get
            {
                return (string)ViewState["RequestedIssue"];
            }
            set
            {
                ViewState["RequestedIssue"] = value;
            }
        }
        public string PapersDate { get; set; }
        public string PDFLink { get; set; }

        private Pages.ArchiveSearch ArchiveSearchPage
        {
            get
            {
                return (Pages.ArchiveSearch)Page;
            }
        }
        
        #endregion


        #region Events
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RegisterScript();

            if (!IsPostBack)
            {
                SetIssueDate(false, null);

                if (Page.User.IsInRole(DiRoleHandler.RoleDiWeekend))
                    PlaceHolderWeekendSubscriberText.Visible = true;

            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchOnDateInput.Text))
                SetIssueDate(true, SearchOnDateInput.Text);
            else
                SetIssueDate(false, null);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            HideTexTalkBtnForOldPapers();
        }

        #endregion

        
        #region Methods
        /// <summary>
        /// Register clientscripts. When "Search" is clicked, the id of the selected tab is saved in hidden fields.        
        /// </summary>
        private void RegisterScript()
        {
            HiddenField SelectedTabHiddenField = ArchiveSearchPage.HiddenFieldSelectedTab;
            HyperLink DateSearchHyperLink = ArchiveSearchPage.HyperLinkDateSearch;

            // Create script for click on Send password where selected tab and section will be stored in hiddenfields
            string script = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('{2}');
                                                }})
                                            }});",
                                            SearchButton.ClientID,
                                            SelectedTabHiddenField.ClientID,
                                            DateSearchHyperLink.NavigateUrl
                                        );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "DateSearchButton_Click", script, true);
        }
        public void SetIssueDate(bool pdfSearch, string issuedate)
        { 
            if (pdfSearch)
            {
                RequestedIssue = issuedate; //EPiFunctions.TryParse("20" + issuedate).ToString();
            }
            else
            {
                if (string.IsNullOrEmpty(issuedate))
                {
                    string tmpDate = EPiFunctions.GetSwedishDateFormat(EPiFunctions.GetCurrentEditionDateDI());
                    RequestedIssue = tmpDate;
                }
                else
                {
                    RequestedIssue = issuedate;
                }
            }
            //Check that requested issue is newer then 20030101 and that the requested pdf exist
            IsIssue = EPiFunctions.TryParse(RequestedIssue.Replace("-", "")) > 20030101 && File.Exists(PDFArchivePath + RequestedIssue.Replace("-", "") + ".pdf");
        }

        public string GetPapersLink()
        {
            string href = string.Empty;

            href = ConfigurationManager.AppSettings["LinkToPaperUrl"];
            href = MiscFunctions.textalkGetLink(RequestedIssue.Replace("-", ""), "1", href);
            return href;
        }

        public string GetPapersImage()
        {
            const string streamImageLink = "/Tools/Operations/Stream/ShowImage.aspx?what=dipaperarchive&imgname={0}";

            string imgName = string.Empty;
            if (string.IsNullOrEmpty(RequestedIssue) || IsIssue == false)
            {
                if (RequestedIssue.Replace("-", "") == EPiFunctions.GetSwedishDateFormat(EPiFunctions.GetCurrentEditionDateDI()).Replace("-", ""))
                {
                    HiddenButton.Visible = false;
                    //return string.Format(streamImageLink, "NoPDF");
                    return "\\Templates\\Public\\Images\\NoPDF.png";
                }
                else
                {
                    HiddenButton.Visible = false;
                    return "\\Templates\\Public\\Images\\NoTidning.png";
                }
            }
            else
            {
                if (File.Exists(PDFArchivePath + RequestedIssue.Replace("-", "") + ".gif"))
                    imgName = RequestedIssue.Replace("-", "");
                else
                    imgName = "NoGIF";

                HiddenButton.Visible = true;

                if (HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiWeekend))
                {
                    List<DateTime> issueDates = new List<DateTime>();
                    DateTime dt;
                    if (DateTime.TryParse(RequestedIssue, out dt))
                        issueDates = CirixDbHandler.GetProductsIssueDatesInInterval(Settings.PaperCode_DI, Settings.ProductNo_Weekend, dt, dt);

                    if (issueDates.Count == 0)
                        HiddenButton.Visible = false;
                }
            }
           
            return string.Format(streamImageLink, imgName);
        }

        public string GetPapersDate()
        {
            DateTime papersDate = Convert.ToDateTime(RequestedIssue);

            string paperDateStr = papersDate.ToString("dddd d MMMM yyyy");
            if (!string.IsNullOrEmpty(paperDateStr))
                paperDateStr = string.Format("{0}{1}", paperDateStr[0].ToString().ToUpper(), paperDateStr.Substring(1));
            return paperDateStr;
        }

        public string GetDownloadLink()
        {
            return "javascript:downloadPDF(" + RequestedIssue.Replace("-", "") + ",false)";
        }

        //textalk only keep papers for 60 days
        private void HideTexTalkBtnForOldPapers()
        {
            DateTime dtSel = DateTime.Parse(RequestedIssue);
            DateTime dtMin60 = DateTime.Now.Date.AddDays(-60);
            PlaceHolderTexTalkBtn.Visible = (dtSel > dtMin60) ? true : false;
        }

        #endregion
    }
}