using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Reports;
using System.Xml;
using System.IO;
using DIClassLib.Misc;


namespace DagensIndustri.Tools.Reports
{
    [GuiPlugIn(
    Area = PlugInArea.ReportMenu,
    Description = "Site Statistics Report",
    Category = "Di Reports", DisplayName = "DI Site Statistics",
    Url = "~/Tools/Reports/SiteStatistics.aspx")]
    public partial class SiteStatistics : EPiServer.TemplatePage
    {
        //public DateTime toDate =;
        //public DateTime fromDate = ;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.MasterPageFile = ResolveUrlFromUI("MasterPages/EPiServerUI.master");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                GetResults(DateTime.Now.AddDays(-7),  DateTime.Now);

            GetEvents();
            GetEntries();
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            GetResults(Convert.ToDateTime(FromDateTextBox.Text), Convert.ToDateTime(ToDateTextBox.Text));
        }

        protected void GetResults(DateTime fromDate, DateTime toDate)
        {                                                                                       //OLD (not valid login)
            string profileID = MiscFunctions.GetAppsettingsValue("googleAnalyticsProfileId");   //"ga:43937104";
            string username = MiscFunctions.GetAppsettingsValue("googleAnalyticsUser");         //"karl.nystrom@di.se";
            string password = MiscFunctions.GetAppsettingsValue("googleAnalyticsPass");         //"Dagensindustrired";

            string visits = "ga:visits";
            string visitors = "ga:visitors";
            string pageviews = "ga:pageviews";

            string token = GAnalytics.GetAuthorizationToken(username, password);

            //Visitors
            DagensindustriVisitorLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/", fromDate, toDate);
            DiGoldVisitorLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/diguld/", fromDate, toDate);
            ContactsVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/diguld/Tjanster/Affarskontakter/", fromDate, toDate);
            MeetRoomVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/diguld/Tjanster/Motesrum/", fromDate, toDate);
            WineClubVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/diguld/vinklubb/", fromDate, toDate);
            ReadOnlineVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/Vara-Tidningar/Las-Dagens-industri-online/", fromDate, toDate);
            ArchiveVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/System/Archive/", fromDate, toDate);
            GasellVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/DI-Gasell/", fromDate, toDate);
            ConferenceVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/konferens/", fromDate, toDate);

            //Visits
            //DagensindustriVisitorLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/", fromDate, toDate);
            //DiGoldVisitorLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath=~/diguld/", fromDate, toDate);
            ContactsUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/diguld/Tjanster/Affarskontakter/", fromDate, toDate);
            MeetRoomUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/diguld/Tjanster/Motesrum/", fromDate, toDate);
            WineClubUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/diguld/vinklubb/", fromDate, toDate);
            ReadOnlineUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/Vara-Tidningar/Las-Dagens-industri-online/", fromDate, toDate);
            ArchiveUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/System/Archive/", fromDate, toDate);
            GasellUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath=~/DI-Gasell/", fromDate, toDate);
            ConferenceUniqueVisitorsLiteral.Text = GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath=~/konferens/", fromDate, toDate);

            //PageViews
            ContactsPageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/diguld/Tjanster/Affarskontakter/", fromDate, toDate);
            MeetRoomPageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/diguld/Tjanster/Motesrum/", fromDate, toDate);
            WineClubPageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/diguld/vinklubb/", fromDate, toDate);
            ReadOnlinePageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/Vara-Tidningar/Las-Dagens-industri-online/", fromDate, toDate);
            ArchivePageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/System/Archive/", fromDate, toDate);
            GasellPageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath=~/DI-Gasell/", fromDate, toDate);
            ConferencePageviewsLiteral.Text = GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath=~/konferens/", fromDate, toDate);
        }

        protected void GetEvents()
        {
            TempAddressLiteral.Text = MsSqlHandler.GetEventsEntriesById(1).ToString();
            HolidayStopLiteral.Text = MsSqlHandler.GetEventsEntriesById(2).ToString();
            PermanentAddressLiteral.Text = MsSqlHandler.GetEventsEntriesById(3).ToString();
            FreeDiLiteral.Text = MsSqlHandler.GetEventsEntriesById(4).ToString();
            CreatedURLLiteral.Text = MsSqlHandler.GetEventsEntriesById(5).ToString();
            CompanySearchesLiteral.Text = MsSqlHandler.GetEventsEntriesById(6).ToString();
            WineClubMailLiteral.Text = MsSqlHandler.GetEmailSentToAddress("leads@antipodeswines.com").ToString();
            UnitedSpacesLiteral.Text = MsSqlHandler.GetEmailSentToAddress("reception@unitedspaces.se").ToString();

        }

        protected void GetEntries()
        {
            BreakfastOneLiteral.Text = MsSqlHandler.GetSignUpEntriesById(494).ToString();
            BreakfastTwoLiteral.Text = MsSqlHandler.GetSignUpEntriesById(497).ToString(); ;
            BreakfastThreeLiteral.Text = MsSqlHandler.GetSignUpEntriesById(498).ToString(); ;
            TennisClinicLiteral.Text = MsSqlHandler.GetSimpleFormEntriesById(491).ToString();
        }
    }
}