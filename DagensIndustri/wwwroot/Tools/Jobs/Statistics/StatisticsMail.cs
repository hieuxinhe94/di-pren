using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using DIClassLib.Misc;
using DagensIndustri.Tools.Reports;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Tools.Jobs.Statistics
{
    [ScheduledPlugIn(DisplayName = "Send Mail Statistics", Description = "Send mail with statistics from Google Analytics and other site statistics")]
    public class StatisticsMail
    {
        public static string Execute()
        {
            string status = "NOT OK";

            try
            {
                string mailFrom = "no-reply@di.se";

                string[] mailCollection = EPiFunctions.SettingsPageSetting(EPiFunctions.StartPage(), "StatisticsMailCollection").ToString().Split(';');

                if (mailCollection.Length > 0)
                {
                    string mailBody = GetResults(DateTime.Now.AddDays(-8), DateTime.Now.AddDays(-1));

                    foreach (string mailTo in mailCollection)
                    {
                        try
                        {
                            MailMessage statisticsMessage = new MailMessage(mailFrom, mailTo);
                            statisticsMessage.IsBodyHtml = false;
                            statisticsMessage.Subject = "Statistik från DagensIndustri.se och Di Guld";
                            statisticsMessage.Body = mailBody;

                            MiscFunctions.SendMail(statisticsMessage);
                        }
                        catch (Exception ex)
                        {
                            new DIClassLib.DbHelpers.Logger("SendStatisticsMailTo() " + mailTo.ToString() + "- failed", ex.ToString());
                        }
                    }

                    status = "OK";
                }
                else
                {
                    status = "You have to set the StatisticsMailCollection property in settings page";
                }

            }
            catch (Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("SendStatisticsMail() - failed", ex.ToString());
                status = "Send statistics mail failed";
            }
            
            return status;
        }

        protected static string GetResults(DateTime fromDate, DateTime toDate)
        {
            string result = string.Empty;

            try
            {
                string profileID = MiscFunctions.GetAppsettingsValue("googleAnalyticsProfileId");   //"ga:43937104";
                string username = MiscFunctions.GetAppsettingsValue("googleAnalyticsUser");         //"petter@di.se";
                string password = MiscFunctions.GetAppsettingsValue("googleAnalyticsPass");         //"di@google";

                string visits = "ga:visits";
                string visitors = "ga:visitors";
                string pageviews = "ga:pageviews";

                string token = GAnalytics.GetAuthorizationToken(username, password);

                StringBuilder sb = new StringBuilder();

                sb.Append("Statistik från ");
                sb.Append(fromDate.ToString("yyyy-MM-dd"));
                sb.Append(" till ");
                sb.Append(toDate.ToString("yyyy-MM-dd"));
                sb.Append("\n");
                sb.Append("-----------------------------------------");
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Antal kunder--");
                sb.Append("\n");
                sb.Append("Regular: ");
                sb.Append(MsSqlHandler.GetNumCustsInRole(1).ToString());
                sb.Append("\n");
                sb.Append("Di Guld: ");
                sb.Append(MsSqlHandler.GetNumCustsInRole(2).ToString());
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Unika besökare--");
                sb.Append("\n");
                sb.Append("Dagensindustri.se: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Di Guld: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/diguld/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Affärskontakter--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append( GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/diguld/Tjanster/Affarskontakter/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/diguld/Tjanster/Affarskontakter/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/diguld/Tjanster/Affarskontakter/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Mötesrum--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/diguld/Tjanster/Motesrum/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/diguld/Tjanster/Motesrum/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/diguld/Tjanster/Motesrum/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Vinklubben--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/diguld/vinklubb/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/diguld/vinklubb/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/diguld/vinklubb/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Läs online--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/Vara-Tidningar/Las-Dagens-industri-online/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/Vara-Tidningar/Las-Dagens-industri-online/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/Vara-Tidningar/Las-Dagens-industri-online/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Arkivet--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath==/System/Archive/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath==/System/Archive/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath==/System/Archive/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Di Gasell--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/DI-Gasell/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath=~/DI-Gasell/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath=~/DI-Gasell/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Di Konferens--");
                sb.Append("\n");
                sb.Append("Antal Besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visitors, "ga:pagePath=~/konferens/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal unika besökare: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, visits, "ga:pagePath=~/konferens/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("Antal sidvisnigar: ");
                sb.Append(GAnalytics.GetMetrics(token, profileID, pageviews, "ga:pagePath=~/konferens/", fromDate, toDate));
                sb.Append("\n");
                sb.Append("\n");
                sb.Append("--Events Log--");
                sb.Append("\n");
                sb.Append("Temporary address change: ");
                sb.Append(MsSqlHandler.GetEventsEntriesById(1).ToString());
                sb.Append("\n");
                sb.Append("Holiday stop: ");
                sb.Append(MsSqlHandler.GetEventsEntriesById(2).ToString());
                sb.Append("\n");
                sb.Append("Permanent address change: ");
                sb.Append(MsSqlHandler.GetEventsEntriesById(3).ToString());
                sb.Append("\n");
                sb.Append("Free DI + Summer 2011: ");
                sb.Append(MsSqlHandler.GetEventsEntriesById(4).ToString());
                sb.Append("\n");
                sb.Append("Antal skapade URL (Affärskalender): ");
                sb.Append(MsSqlHandler.GetEventsEntriesById(5).ToString());
                sb.Append("\n");
                sb.Append("Antal sök (Affärskontakter): ");
                sb.Append(MsSqlHandler.GetEventsEntriesById(6).ToString());
                sb.Append("\n");
                sb.Append("Intresseanmälan (Vinklubb): ");
                sb.Append(MsSqlHandler.GetEmailSentToAddress("leads@antipodeswines.com").ToString());
                sb.Append("\n");
                sb.Append("Antal skickade mail (Mötesrum): ");
                sb.Append(MsSqlHandler.GetEmailSentToAddress("reception@unitedspaces.se").ToString());
                sb.Append("\n");

                result = sb.ToString();
            }
            catch(Exception ex)
            {
                new DIClassLib.DbHelpers.Logger("GetResults() - failed", ex.ToString());
            }

            return result;
        }
    }
}