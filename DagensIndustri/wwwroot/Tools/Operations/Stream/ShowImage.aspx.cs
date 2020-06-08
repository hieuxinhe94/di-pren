using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Tools.Operations.Stream
{
    public partial class ShowImage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string imageStr = string.Empty;
            string pdfPaperPath = ConfigurationManager.AppSettings["PDFPaperPath"];
            string pdfArchivePath = ConfigurationManager.AppSettings["PDFArchivePath"];
            string imageArchivePath = ConfigurationManager.AppSettings["ImageArchivePath"];

            //check access on these, check if user is logged in
            List<string> checkAccessList = new List<string> { "dipaperarchive" };
            //stream these as thumbnails with possiblity to set width and height
            List<string> streamAsThumbnailList = new List<string> { "dimorgon", "weekend", "dimension", "dipaperarchive", "appendix", "ettan", "dipaper", "dihr", "diide" };

            string what = !string.IsNullOrEmpty(Request.QueryString["what"]) ? Request.QueryString["what"].ToString().ToLower() : string.Empty;

            switch (what)
            {
                case "dimorgon": //from Paper.aspx
                    DirectoryInfo dir = new DirectoryInfo(ConfigurationManager.AppSettings["PDFPaperPathTomorrow"]);
                    foreach (FileInfo file in dir.GetFiles("*_1.jpg"))
                        imageStr = ConfigurationManager.AppSettings["PDFPaperPathTomorrow"] + file.Name;
                    break;
                case "weekend": //from Paper.aspx, Login.aspx
                    imageStr = pdfPaperPath + "DIWeekend\\ettan.jpg";
                    break;
                case "dimension": //from Paper.aspx, Login.aspx
                    imageStr = pdfPaperPath + "Dimension\\ettan.jpg";
                    break;
                case "diide":
                    imageStr = pdfPaperPath + "DiIde\\ettan.jpg";
                    break;
                case "dipaperarchive": //from ArchivePaper.aspx
                case "appendix": //from Appendix.aspx
                    imageStr = pdfArchivePath + Request.QueryString["imgname"] + ".gif";
                    break;
                case "ettan": //from Login.aspx
                    //Per L changed 2013-08-19!
                    imageStr = GetCachedEttanGifpath(pdfArchivePath);
                    if (!File.Exists(imageStr))
                        imageStr = pdfArchivePath + "NoGif.gif";
                    break;
                case "dipaper": //from Paper.aspx
                    imageStr = pdfPaperPath + "Ettan\\" + Request.QueryString["imgname"] + "Ettan.jpg";
                    break;
                case "dihr":
                    imageStr = imageArchivePath + Request.QueryString["imgname"] + ".jpg";
                    break;
            }

            //stream image
            if (!string.IsNullOrEmpty(imageStr))
            {
                if (checkAccessList.Contains(what))
                    if (!HttpContext.Current.User.Identity.IsAuthenticated)
                        return;

                if (streamAsThumbnailList.Contains(what))
                    MiscFunctions.StreamThumbNailImage(imageStr);
                else
                    MiscFunctions.StreamImage(imageStr);

            }
        }

        /// <summary>
        /// Returns the path to Ettan.gif, taken from cache if exist, otherwise builds it from correct editiondate in CIRIX-DB
        /// </summary>
        /// <author>
        /// Per Lundkvist
        /// </author>
        /// <param name="pdfArchivePath">Path to archive folder where the GIF is supposed to reside</param>
        /// <returns></returns>
        private string GetCachedEttanGifpath(string pdfArchivePath)
        {
            const string paperCode = "DI";
            const string productNo = "01";
            string cacheKey = string.Format("Ettan_gif_path_{0}_{1}_{2}", paperCode, productNo, DateTime.Today.ToString("yyyyMMdd"));
            
            string data = (string)HttpRuntime.Cache.Get(cacheKey);
            if (!string.IsNullOrEmpty(data))
            {
                return data;
            }

            string returnPath = pdfArchivePath + SubscriptionController.GetIssueDate(paperCode, productNo, DateTime.Today, EnumIssue.Issue.InDateOrFirstBeforeInDate).ToString("yyyyMMdd") + ".gif";
            HttpRuntime.Cache.Insert(
                cacheKey,
                returnPath,
                null,
                DateTime.Now.AddSeconds(180),
                System.Web.Caching.Cache.NoSlidingExpiration);  //TODO: Take amount of seconds from a appsettings key or similar instead of hard code it
            return returnPath;
        }
    }
}