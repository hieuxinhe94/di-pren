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


namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class DateArchiveSearch : EPiServer.UserControlBase
    {
        #region Properties

        public string PDFArchivePath = MiscFunctions.GetAppsettingsValue("PDFArchivePath");
        private bool IsIssue { get; set; }
        public string RequestedIssue { get; set; }
        public string PapersDate { get; set; }
        public string PDFLink { get; set; }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                SetIssueDate(false, null);
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchOnDateInput.Text))
                SetIssueDate(true, SearchOnDateInput.Text);
            else
                SetIssueDate(false, null);
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
                {
                    imgName = RequestedIssue.Replace("-", "");
                }
                else
                {
                    imgName = "NoGIF";
                }
                HiddenButton.Visible = true;
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
    }
}