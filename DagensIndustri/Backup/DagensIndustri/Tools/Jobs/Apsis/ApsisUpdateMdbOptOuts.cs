using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;
using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Jobs.Apsis
{
    [ScheduledPlugIn(DisplayName = "Apsis update OptOuts from MDB", Description = "Flyttar e-postadress till Apsis OptOutAll listan om kund finns i MDBs OptOut tabell")]
    public class ApsisUpdateMdbOptOuts
    {
        private static object ApsisCheckLock = new object();
        private static readonly ApsisWsHandler _apsisWsHandler = new ApsisWsHandler();
        private static int _countOfTotal;
        private static int _countOfFailed;
        private static int _countOfNonExisting;

        public static string Execute()
        {
            if (!Monitor.TryEnter(ApsisCheckLock))
            {
                return "Job is still running. You might want to increase the time interval for this job.";
            }
            try
            {
                if (Settings.DoApsisUpdates)
                {
                    _countOfTotal = 0;
                    _countOfFailed = 0;
                    _countOfNonExisting = 0;
                    DateTime fromDate;
                    DateTime.TryParse(ConfigurationManager.AppSettings["ApsisMdbOptOutFromDate"], out fromDate);
                    DateTime toDate;
                    DateTime.TryParse(ConfigurationManager.AppSettings["ApsisMdbOptOutToDateOptional"], out toDate);
                    int daysBack;
                    int.TryParse(ConfigurationManager.AppSettings["ApsisMdbOptOutDaysBack"], out daysBack);

                    var currentOptOutList = _apsisWsHandler.GetCurrentOptOutAllList();
                    var mdbOptOutList = MdbDbHandler.GetAllOptOuts(daysBack, fromDate, toDate);
                    var finalList = mdbOptOutList.Except(currentOptOutList).ToList();
                    ProcessOptOutList(finalList);
                }
            }
            finally
            {
                Monitor.Exit(ApsisCheckLock);
            }

            return Settings.DoApsisUpdates ? string.Format("Job done. {0} of {1} failed! {2} not exist in Apsis.", _countOfFailed, _countOfTotal, _countOfNonExisting) : "Job done in testmode without updating Apsis";
        }

        private static void ProcessOptOutList(IEnumerable<string> list)
        {
            foreach (var optOutEmail in list)
            {
                _countOfTotal++;
                MoveEmailToApsisOptOutList(optOutEmail);
            }
        }

        private static void MoveEmailToApsisOptOutList(string email)
        {
            var apsisListSubscriber = _apsisWsHandler.GetSubscriberDetails(email);
            if (apsisListSubscriber == null || string.IsNullOrEmpty(apsisListSubscriber.SubscriberID))
            {
                _countOfNonExisting++;
                return;
            }

            var apsisResult = _apsisWsHandler.MoveSubscriberToOptOutAll(email.ToString());
            if (apsisResult != 1)
            {
                _countOfFailed++;
                new Logger(string.Format("Scheduled job ApsisUpdateMdbOptOuts.MoveEmailToApsisOptOutList() failed to move {0} to Apsis optout list, errorcode: {1}", email, apsisResult));
            }
        }
    }
}