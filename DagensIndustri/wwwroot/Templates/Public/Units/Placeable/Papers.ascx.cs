using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;

using Image = System.Web.UI.WebControls.Image;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class Papers : UserControlBase
    {
        #region Constants
        private const int EARLIEST_NEXT_EDITION_HOUR = 4;
        #endregion

        #region Properties
        protected string EditionsWrapperStartElement
        {
            get { return MobileView ? @"<div class=""editions-mobile"">" : @"<ul class=""previous-editions"">"; }
        }

        protected string EditionsWrapperEndElement
        {
            get { return MobileView ? @"</div><br clear=""all"" />" : "</ul>"; }
        }

        protected string EditionsItemStartElement
        {
            get { return MobileView ? "<div>" : "<li>"; }
        }

        protected string EditionsItemEndElement
        {
            get { return MobileView ? "</div>" : "</li>"; }
        }

        public bool MobileView { get; set; }

        private DateTime EttanPaperDate { get; set; }

        protected DateTime NowDateTime { get; set; }
        /*
        private DateTime CurrentEditonDateWeekend
        {
            get 
            {
                try
                {
                    return MsSqlHandler.GetCurrentEditonDateWeekend();
                }
                catch (Exception ex)
                {
                    new Logger("CurrentEditonDateWeekend", ex.ToString());
                    return DateTime.Now;
                }
            }
        }

        private DateTime CurrentEditonDateDimension
        {
            get
            {
                try
                {
                    return MsSqlHandler.GetCurrentEditonDateDimension();
                }
                catch (Exception ex)
                {
                    new Logger("CurrentEditonDateDimension", ex.ToString());
                    return DateTime.Now;
                }
            }
        }
        */
        public string ArchiveURL { get; set; }

        private bool AllowUserToReadPdfByCusno
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return false;

                return Settings.CusnosAllowReadPdf.Contains(CusnoLoggedIn);
            }
        }

        public bool UserInRoleAllowedToReadPdfs
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                    return false;

                if (HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiGold) || HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiIp) || HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiIpad) ||
                    HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiPdf) || HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiRegular) || HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiSms24Hour) ||
                    HttpContext.Current.User.IsInRole("WebAdmins") || HttpContext.Current.User.IsInRole("WebEditors") || HttpContext.Current.User.IsInRole("PdfReaders") ||
                    HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiHybrid))
                    return true;

                return false;
            }
        }

        public bool PdfsOpenForAllVisitors
        {
            get
            {
                bool? bIsOpen = (bool?)EPiFunctions.SettingsPageSetting(null, "PdfsOpenForAllVisitors");
                if (bIsOpen.HasValue && bIsOpen.Value == true)
                {
                    return true;
                }
                //if (ConfigurationManager.AppSettings["PdfsOpenForAllVisitors"].ToLower() == "true")
                //return true;

                return false;
            }
        }

        private int CusnoLoggedIn
        {
            get
            {
                string id = "cusnoPaper";

                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    ViewState[id] = null;
                    return -1;
                }

                if (ViewState[id] == null)
                    ViewState[id] = MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name);

                return int.Parse(ViewState[id].ToString());
            }
        }

        private List<DateTime> _weekendIssueDatesInCloseRange;
        public List<DateTime> WeekendIssueDatesInCloseRange
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated || !HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiWeekend))
                    return new List<DateTime>();

                if (_weekendIssueDatesInCloseRange == null)
                    _weekendIssueDatesInCloseRange = SubscriptionController.GetProductsIssueDatesInInterval(Settings.PaperCode_DI, Settings.ProductNo_Weekend, DateTime.Now.AddDays(-20), DateTime.Now.AddDays(1));

                return _weekendIssueDatesInCloseRange;
            }
        }

        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page.Master!=null && MobileView)
            {
                var mobileCssFileTag = new Literal();
                mobileCssFileTag.Text = @"<link href=""/Templates/Public/Styles/papers-mobile.css"" rel=""stylesheet"" type=""text/css"" />";
                Page.Master.FindControl("head").Controls.Add(mobileCssFileTag);
                HideArchiveLinkPlaceHolder.Visible = false;
            }

            // For testing purpose make it possible to set date with query string parameter "now"
            //if (Request.Url.Host == "localhost" && !String.IsNullOrEmpty(Request.QueryString["now"]))
            if (!String.IsNullOrEmpty(Request.QueryString["now"]))
            {
                DateTime dt;
                if (DateTime.TryParse(Request.QueryString["now"], out dt))
                    NowDateTime = dt;
                else
                    NowDateTime = DateTime.Now;
            }
            else
            {
                NowDateTime = DateTime.Now;
            }

            if (!IsPostBack)
            {
                LoadPreviousPapers();
                LoadCurrentPapers();

                #region Open a certain paper in a new window
                if (Session["PaperUrl"] != null)
                {
                    string paperUrl = (string)Session["PaperUrl"];
                    string issue = (string)Session["PaperIssue"];
                    Session["PaperUrl"] = null;
                    Session["PaperIssue"] = null;

                    //Check first if user is authenticated. 
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        bool showPaper = false;
                        //If user is member of the sms group, only today's paper can be read.
                        if (HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiSms24Hour))
                        {
                            if (issue == Paper.ISSUE_DIPAPER)
                                showPaper = true;
                        }
                        else
                        {
                            //If paper issue is not Di Tomorrow, or user is a Di Gold member, let user read the paper.
                            //if (issue != Paper.ISSUE_TOMORROW || HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiGold) || AllowUserToReadPdfByCusno)
                            if (issue != Paper.ISSUE_TOMORROW || AllowUserToReadPdfByCusno)
                                showPaper = true;
                        }

                        if (showPaper && !string.IsNullOrEmpty(paperUrl))
                            Response.Write("<script>window.open(\"" + paperUrl + "\");</script>");
                    }
                }
                #endregion
            }
            else
            {
                //Store selected issue and paper date retrieved from hidden fields in session.
                if (!string.IsNullOrEmpty(SelectedIssueHiddenField.Value) && !string.IsNullOrEmpty(SelectedPaperDateHiddenField.Value))
                {
                    Session["PaperUrl"] = Paper.GetLinkHref(SelectedIssueHiddenField.Value, Convert.ToDateTime(SelectedPaperDateHiddenField.Value));
                    Session["PaperIssue"] = SelectedIssueHiddenField.Value;
                }
            }

            if (IsValue("ArchivePage") && !HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiSms24Hour) && (UserInRoleAllowedToReadPdfs || HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiWeekend)))
            {
                ArchiveURL = EPiServer.DataFactory.Instance.GetPage(CurrentPage["ArchivePage"] as PageReference).LinkURL;
            }
            else
            {
                HideArchiveLinkPlaceHolder.Visible = false;
            }
        }

        protected void PapersCurrentRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Paper paper = e.Item.DataItem as Paper;

                PlaceHolder paperPlaceHolder = e.Item.FindControl("PaperPlaceHolder") as PlaceHolder;
                PlaceHolder tomorrowsPaperPlaceHolder = e.Item.FindControl("TomorrowsPaperPlaceHolder") as PlaceHolder;

                paperPlaceHolder.Visible = paper.TypeOfPaper != Paper.PaperType.Tomorrow;
                tomorrowsPaperPlaceHolder.Visible = paper.TypeOfPaper == Paper.PaperType.Tomorrow;

                if (paper.TypeOfPaper != Paper.PaperType.Tomorrow)
                {
                    HyperLink readPaperHyperLink = e.Item.FindControl("PaperPlaceHolder$ReadPaperHyperLink") as HyperLink;

                    //if (HttpContext.Current.User.Identity.IsAuthenticated || PdfsOpenForAllVisitors)
                    if (UserInRoleAllowedToReadPdfs || PdfsOpenForAllVisitors || IsWeekendReaderAndWeekendEditon(paper.PaperDate))
                    {
                        if (!HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiSms24Hour) || paper.TypeOfPaper == Paper.PaperType.DiPaper)
                        {
                            if (readPaperHyperLink != null)
                            {
                                readPaperHyperLink.CssClass = "btn";
                                readPaperHyperLink.Target = paper.LinkTarget;
                                if (MobileView)
                                {
                                    readPaperHyperLink.Text = "<span>PDF</span>";
                                    readPaperHyperLink.Attributes.Remove("onclick");
                                    readPaperHyperLink.NavigateUrl = "/Tools/Operations/Stream/DownloadPDF.aspx?appendix=false&issueid=" + paper.PaperDate.ToString("yyyyMMdd");
                                }
                                else
                                {
                                    readPaperHyperLink.Text = "<span>Läs</span>";
                                    readPaperHyperLink.NavigateUrl = Paper.GetLinkHref(paper.Issue, paper.PaperDate);
                                }
                            }
                        }
                        else
                        {
                            readPaperHyperLink.Visible = false;
                        }
                    }
                    else
                    {
                        //RegisterScript((Control)readPaperHyperLink, paper);

                        if (!HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            readPaperHyperLink.NavigateUrl = !IsValue("ConCurrLoginUrl") ? EPiFunctions.GetLoginPageUrl(CurrentPage) : CurrentPage["ConCurrLoginUrl"] as string;
                        }
                        else
                        {
                            readPaperHyperLink.Visible = false;
                        }
                    }
                }
                #region tomorrow
                else  //is PaperType.Tomorrow
                {
                    PlaceHolder linkPlaceHolder = e.Item.FindControl("TomorrowsPaperPlaceHolder$LinkPlaceHolder") as PlaceHolder;
                    linkPlaceHolder.Visible = false;

                    bool timeToShowPaper = string.IsNullOrEmpty(paper.CountDown);

                    if (timeToShowPaper)
                    {
                        if (UserInRoleAllowedToReadPdfs)
                            linkPlaceHolder.Visible = true;

                        DayOfWeek dow = paper.PaperDate.DayOfWeek;
                        if (HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiWeekend) && (dow == DayOfWeek.Friday || dow == DayOfWeek.Saturday))
                        {
                            linkPlaceHolder.Visible = true;
                        }
                    }


                    //PlaceHolder linkPlaceHolder = e.Item.FindControl("TomorrowsPaperPlaceHolder$LinkPlaceHolder") as PlaceHolder;
                    //linkPlaceHolder.Visible = timeToShowPaper;

                    //if (linkPlaceHolder.Visible)
                    //{
                    //    if (!UserInRoleAllowedToReadPdfs && User.IsInRole(DiRoleHandler.RoleAgenda))
                    //        linkPlaceHolder.Visible = false;

                    //    if (User.IsInRole(DiRoleHandler.RoleDiWeekend))
                    //    {
                    //        linkPlaceHolder.Visible = false;
                    //        DayOfWeek dow = paper.PaperDate.DayOfWeek;
                    //        if (dow == DayOfWeek.Friday || dow == DayOfWeek.Saturday)
                    //            linkPlaceHolder.Visible = true;
                    //    }
                    //}


                    Image tomorrowsPaperImage = e.Item.FindControl("TomorrowsPaperPlaceHolder$TomorrowsPaperImage") as Image;
                    tomorrowsPaperImage.ImageUrl = string.IsNullOrEmpty(paper.CountDown) ? GetImageUrl(paper) : "/Templates/Public/Images/ills/tomorrow_110px.png";

                    Literal dateCountDownLiteral = e.Item.FindControl("TomorrowsPaperPlaceHolder$DateCountDownLiteral") as Literal;
                    dateCountDownLiteral.Text = string.IsNullOrEmpty(paper.CountDown) ? paper.GetPaperDate() : paper.CountDown;

                    if (linkPlaceHolder.Visible)
                    {
                        HyperLink readPaperHyperLink = e.Item.FindControl("TomorrowsPaperPlaceHolder$ReadTomorrowsPaperHyperLink") as HyperLink;
                        HtmlAnchor downloadPaperAnchor = e.Item.FindControl("TomorrowsPaperPlaceHolder$DownloadPaperAnchor") as HtmlAnchor;
                        Literal litBtnReadTomorrow = e.Item.FindControl("LinkPlaceHolder$LiteralReadTomorrowBtn") as Literal;
                        litBtnReadTomorrow.Text = "Logga in";


                        //if (User.Identity.IsAuthenticated && (User.IsInRole(DiRoleHandler.RoleDiGold) || AllowUserToReadPdfByCusno))
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            readPaperHyperLink.CssClass = "btn";
                            readPaperHyperLink.NavigateUrl = Paper.GetLinkHref(paper.Issue, paper.PaperDate);
                            readPaperHyperLink.Target = paper.LinkTarget;

                            downloadPaperAnchor.Attributes.Add("class", "more");
                            downloadPaperAnchor.Attributes.Add("onclick", GetDownloadPDFLink(paper.PaperDate.ToString("yyyyMMdd")));
                            downloadPaperAnchor.HRef = "javascript:void(0);";
                            downloadPaperAnchor.Visible = true;
                            litBtnReadTomorrow.Text = "Läs";
                        }
                        else
                        {
                            //if (User.Identity.IsAuthenticated && !User.IsInRole(DiRoleHandler.RoleDiGold) && !User.IsInRole(DiRoleHandler.RoleDiWeekend))
                            //    litBtnReadTomorrow.Text = "Gå med i Di Guld";

                            //readPaperHyperLink.CssClass = "btn ajax";
                            //readPaperHyperLink.NavigateUrl = "#membership-required";
                            //RegisterScript((Control)readPaperHyperLink, paper);

                            downloadPaperAnchor.Visible = false;
                            //downloadPaperAnchor.Attributes.Add("class", "more ajax");
                            //downloadPaperAnchor.Attributes.Remove("onclick");
                            //downloadPaperAnchor.HRef = "#membership-required";

                            litBtnReadTomorrow.Visible = false;
                        }
                    }
                }
                #endregion
            }
        }


        protected void PapersPreviousRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Paper paper = e.Item.DataItem as Paper;
                HyperLink readPaperHyperLink = e.Item.FindControl("PaperNotAccessiblePlaceHolder$ReadPaperHyperLink") as HyperLink;
                //RegisterScript((Control)readPaperHyperLink, paper);

                PlaceHolder phPaperAccessible = e.Item.FindControl("PaperAccessiblePlaceHolder") as PlaceHolder;
                PlaceHolder phPaperNotAccessible = e.Item.FindControl("PaperNotAccessiblePlaceHolder") as PlaceHolder;
                PlaceHolder phSMSGroup = e.Item.FindControl("SMSGroupPlaceHolder") as PlaceHolder;

                phPaperAccessible.Visible = false;
                phPaperNotAccessible.Visible = false;
                phSMSGroup.Visible = false;

                if (!HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiSms24Hour))
                    if (UserInRoleAllowedToReadPdfs || IsWeekendReaderAndWeekendEditon(paper.PaperDate))
                        phPaperAccessible.Visible = true;
                    else
                        phPaperNotAccessible.Visible = true;
                else
                    phSMSGroup.Visible = true;
            }
        }
        #endregion



        #region Methods
        /// <summary>
        /// Load current papers (Ettan, Weekend, Dimension). 
        /// Also load tomorrows paper between 22.00 and 04.00. If not availabe, show countdown
        /// </summary>
        private void LoadCurrentPapers()
        {
            //DateTime nextEditionDate = CirixDbHandler.GetNextIssuedate(Settings.PaperCode_DI, Settings.ProductNo_Regular, NowDateTime.Date);

            //DateTime nextEditionDate = NowDateTime.Date.AddDays(1);
            //DateTime nextEditionDate = CirixDbHandler.GetIssueDate(Settings.PaperCode_DI, Settings.ProductNo_Regular, NowDateTime, EnumIssue.Issue.FirstAfterInDate);
            //DateTime nextEditionDate = MiscFunctions.GetNextEditionDateDI();

            IssueUtil issueDI = new IssueUtil(HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiGold), Settings.PaperCode_DI, Settings.ProductNo_Regular, NowDateTime);
            IssueUtil issueWeekend = new IssueUtil(HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiGold), Settings.PaperCode_DI, Settings.ProductNo_Weekend, NowDateTime);

            DateTime currentEditionDate = issueDI.CurrentIssueDate;

            List<Paper> papers = new List<Paper>();

            //Add Ettan, Weekend, Dimension and NextIssue
            papers.Add(new Paper(Paper.PaperType.DiPaper, currentEditionDate));
            //papers.Add(new Paper(Paper.PaperType.Weekend, issueWeekend.CurrentIssueDate));
            papers.Add(new Paper(Paper.PaperType.Weekend, issueWeekend.CurrentIssueDateWeekend));
            papers.Add(new Paper(Paper.PaperType.Dimension, issueWeekend.CurrentIssueDateDimension));
            papers.Add(new Paper(Paper.PaperType.Idea, issueWeekend.CurrentIssueDateDiIdea));
            papers.Add(new Paper(Paper.PaperType.Tomorrow, issueDI.NextIssueDate, GetCountDownString(issueDI.TimeToNextIssueAccess), Translate("/onlinepapers/tomorrowspaper")));

            PapersCurrentRepeater.DataSource = papers;
            PapersCurrentRepeater.DataBind();
        }

        /// <summary>
        /// Load previous papers
        /// </summary>
        private void LoadPreviousPapers()
        {
            int noOfIssuesToShow = (int)CurrentPage["NoOfPapersToShow"];
            PapersPreviousRepeater.Visible = noOfIssuesToShow > 0;

            List<Paper> papers = new List<Paper>();
            int issue = 0;

            DateTime startPaperDate = NowDateTime;

            //If the current hour is earlier than 4 in the morning, then today's paper is still accessible by Di Gold members only. Therefore the papers from yesterday and earlier is 
            //allowed to be shown to regular subscribers.
            if (startPaperDate.Hour < EARLIEST_NEXT_EDITION_HOUR)
                startPaperDate = startPaperDate.AddDays(-1);

            for (int i = 0; i <= 30; i++) //Look a month back should do
            {
                DateTime paperDate = startPaperDate.AddDays(-i);

                string directory = string.Format("{0}{1}", ConfigurationManager.AppSettings["PDFPaperPath"], paperDate.ToString("yyyy-MM-dd").Replace("-", "\\"));
                if (Directory.Exists(directory))
                {
                    if (issue > 0 && issue <= noOfIssuesToShow)
                        papers.Add(new Paper(Paper.PaperType.DiPaper, paperDate));
                    else if (issue.Equals(0)) //first issue = Ettan
                        EttanPaperDate = paperDate;
                    else if (issue.Equals(noOfIssuesToShow)) //If one week, look no further
                        break;

                    issue++;
                }
            }

            PapersPreviousRepeater.DataSource = papers;
            PapersPreviousRepeater.DataBind();
        }

        /// <summary>
        /// Calculate how many days, hours and minutes remains until next edition
        /// </summary>
        /// <param name="nextEditionDate"></param>
        /// <returns></returns>
        //private string GetCountDown(DateTime nextEditionDate)
        //{
        //    string countDown = string.Empty;
        //    double hoursToNextEdition = Math.Floor(nextEditionDate.AddHours(-2).Subtract(NowDateTime).TotalHours);
        //    double minutesToNextEdition = Math.Ceiling(nextEditionDate.AddHours(-2 - hoursToNextEdition).Subtract(NowDateTime).TotalMinutes);
        //    double daysToNextEdition = Math.Floor(hoursToNextEdition / 24);

        //    if (daysToNextEdition > 0)
        //        countDown = string.Format(Translate("/onlinepapers/countdownday"), daysToNextEdition, (hoursToNextEdition - (daysToNextEdition * 24)), minutesToNextEdition);
        //    else
        //        countDown = string.Format(Translate("/onlinepapers/countdownhour"), hoursToNextEdition, minutesToNextEdition);

        //    return countDown;
        //}

        private string GetCountDownString(TimeSpan countdown)
        {
            String sCountdown = "";
            if (countdown.Days > 0)
                sCountdown = string.Format(Translate("/onlinepapers/countdownday"), countdown.Days, countdown.Hours, countdown.Minutes);
            else if (countdown.TotalMinutes > 0)
                sCountdown = string.Format(Translate("/onlinepapers/countdownhour"), countdown.Hours, countdown.Minutes);

            return sCountdown;
        }

        /// <summary>
        /// Get paper image url
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        protected string GetImageUrl(Paper paper)
        {
            return string.Format("{0}{1}", Request.Url.GetLeftPart(UriPartial.Authority), paper.GetImgSrc(paper.Width));
        }

        /// <summary>
        /// Get javascript script for downloading pdf for a certain date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string GetDownloadPDFLink(string date)
        {
            string fileName = string.Format("morgondagen-{0}", date);
            return "javascript:downloadTomorrowPDF('" + fileName + "',false,true)";
        }

        /// <summary>
        /// Register script for storing values in the hiddens field when a control is clicked
        /// </summary>
        /// <param name="control"></param>
        /// <param name="paper"></param>
        private void RegisterScript(Control control, Paper paper)
        {
            string script = string.Format(@"$(document).ready(function() {{
                                                        $('#{0}').click(function () {{
                                                        $('#{1}').val('{2}');
                                                        $('#{3}').val('{4}');
                                                    }})
                                                }});",
                                            control.ClientID,
                                            SelectedIssueHiddenField.ClientID,
                                            paper.Issue,
                                            SelectedPaperDateHiddenField.ClientID,
                                            paper.PaperDate.ToString()
                                        );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SetPaperDetails_" + control.ClientID, script, true);
        }

        private bool IsWeekendReaderAndWeekendEditon(DateTime paperDate)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated || !HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiWeekend))
                return false;

            return WeekendIssueDatesInCloseRange.Contains(paperDate.Date);
        }

        #endregion
    }



    /// <summary>
    /// Class for paper used as datasource for repeater
    /// </summary>
    public class Paper
    {
        #region Constants
        public const string ISSUE_DIPAPER = "dipaper";
        public const string ISSUE_WEEKEND = "Weekend";
        public const string ISSUE_DIMENSION = "Dimension";
        public const string ISSUE_TOMORROW = "dimorgon";
        public const string ISSUE_IDEA = "DiIde";

        //private const string CSS_DIPAPER = "dagensindustri";
        //private const string CSS_WEEKEND = "weekend";
        //private const string CSS_DIMENSION = "dimension";
        //private const string CSS_TOMORROW = "tomorrow";

        private const string NAME_DIPAPER = "Dagens industri";
        private const string NAME_WEEKEND = "Di Weekend";
        private const string NAME_DIMENSION = "Di Dimension";
        private const string NAME_IDEA = "Di Idé";
        #endregion

        #region Enumerations
        public enum PaperType
        {
            DiPaper = 0,
            Weekend,
            Dimension,
            Idea,
            Tomorrow
        }
        #endregion

        #region Properties
        public PaperType TypeOfPaper { get; set; }
        public string Issue { get; set; }
        public DateTime PaperDate { get; set; }
        public string LinkTarget { get; set; }
        //public string CSSClass { get; set; }
        public string Name { get; set; }
        public string CountDown { get; set; }
        public int Width { get; set; }
        #endregion

        #region Constructor
        public Paper(PaperType paperType, DateTime paperDate)
        {
            PaperDate = paperDate;
            TypeOfPaper = paperType;
            LinkTarget = "_blank";

            switch (TypeOfPaper)
            {
                case PaperType.DiPaper:
                    Name = NAME_DIPAPER;
                    Issue = ISSUE_DIPAPER;
                    //CSSClass = CSS_DIPAPER;
                    Width = 110; //140;
                    break;
                case PaperType.Weekend:
                    Name = NAME_WEEKEND;
                    Issue = ISSUE_WEEKEND;
                    //CSSClass = CSS_WEEKEND;
                    Width = 110; //135;
                    break;
                case PaperType.Dimension:
                    Name = NAME_DIMENSION;
                    Issue = ISSUE_DIMENSION;
                    //CSSClass = CSS_DIMENSION;
                    Width = 110; //95;
                    break;
                case PaperType.Idea:
                    Name = NAME_IDEA;
                    Issue = ISSUE_IDEA;
                    //CSSClass = CSS_IDEA;
                    Width = 110; //95;
                    break;
                case PaperType.Tomorrow:
                    Issue = ISSUE_TOMORROW;
                    //CSSClass = CSS_TOMORROW;
                    Width = 110; //140;
                    break;
                default:
                    break;
            }
        }

        public Paper(PaperType paperType, DateTime paperDate, string countDown, string name)
            : this(paperType, paperDate)
        {
            Name = name;
            CountDown = countDown;
        }

        #endregion

        #region Methods
        public static string GetLinkHref(string issue, DateTime paperDate)
        {
            string href = string.Empty;

            switch (issue)
            {
                case ISSUE_WEEKEND:     //"Weekend"
                    href = ConfigurationManager.AppSettings["LinkToPaperWeekendUrl"];
                    break;
                case ISSUE_DIMENSION:   //"Dimension"
                    href = ConfigurationManager.AppSettings["LinkToPaperDimensionUrl"];
                    break;
                case ISSUE_IDEA:
                    href = ConfigurationManager.AppSettings["LinkToPaperDiIdeaUrl"];
                    break;
                default:
                    href = ConfigurationManager.AppSettings["LinkToPaperUrl"];
                    break;
            }

            href = MiscFunctions.textalkGetLink(paperDate.ToString("yyyyMMdd").Replace("\\", ""), "1", href);
            return href;
        }

        /// <summary>
        /// Get the source of the image
        /// </summary>
        /// <returns></returns>
        private string GetImgSrc()
        {
            return string.Format("/Tools/Operations/Stream/ShowImage.aspx?what={0}&imgname={1}", Issue, PaperDate.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Get the source of the image with a certain width
        /// </summary>
        /// <returns></returns>
        public string GetImgSrc(int width)
        {
            return string.Format("{0}&w={1}", GetImgSrc(), width);
        }

        /// <summary>
        /// Get the paper date in certain format.
        /// </summary>
        /// <returns></returns>
        public string GetPaperDate()
        {
            //string paperDateStr = PaperDate.ToString("dddd d MMMM yyyy");
            string paperDateStr = PaperDate.ToString("dddd d MMM yyyy");
            if (!string.IsNullOrEmpty(paperDateStr))
                paperDateStr = string.Format("{0}{1}", paperDateStr[0].ToString().ToUpper(), paperDateStr.Substring(1));
            return paperDateStr;
        }

        #endregion
    }
}