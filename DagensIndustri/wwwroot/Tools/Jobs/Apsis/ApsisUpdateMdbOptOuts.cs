using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
        private static int _countOfFileEmails;

        public static string Execute()
        {
            if (!Monitor.TryEnter(ApsisCheckLock))
            {
                return "Job is still running. You might want to increase the time interval for this job.";
            }
            try
            {

                var takefromSetting = ConfigurationManager.AppSettings.Get("OptOutTakeFromDate");
                DateTime? takeFrom = null;
                try
                {
                    takeFrom = DateTime.Parse(takefromSetting);
                }
                catch (Exception)
                {

                }

                DownloadOptOutFileFromFtp(takeFrom);

                if (Settings.DoApsisUpdates)
                {
                    _countOfTotal = 0;
                    _countOfFailed = 0;
                    _countOfNonExisting = 0;
                    _countOfFileEmails = 0;
                    DateTime fromDate;
                    DateTime.TryParse(ConfigurationManager.AppSettings["ApsisMdbOptOutFromDate"], out fromDate);
                    DateTime toDate;
                    DateTime.TryParse(ConfigurationManager.AppSettings["ApsisMdbOptOutToDateOptional"], out toDate);
                    int daysBack;
                    int.TryParse(ConfigurationManager.AppSettings["ApsisMdbOptOutDaysBack"], out daysBack);

                    var currentOptOutList = _apsisWsHandler.GetCurrentOptOutAllList();
                    var mdbOptOutList = MdbDbHandler.GetAllOptOuts(daysBack, fromDate, toDate);
                    var finalList = mdbOptOutList.Except(currentOptOutList).ToList();

                    //Complement finalList with email addresses from textfile exported from QlikView software
                    var optOutFilePathPattern = ConfigurationManager.AppSettings["ApsisOptOutFilePath"];



                    var qlikViewFileContent = ReadQlikViewFile(string.Format(optOutFilePathPattern, takeFrom ?? DateTime.Today));
                    _countOfFileEmails = qlikViewFileContent.Count();
                    finalList.AddRange(qlikViewFileContent);

                    ProcessOptOutList(finalList);
                }
            }
            finally
            {
                Monitor.Exit(ApsisCheckLock);
            }

            return Settings.DoApsisUpdates ? string.Format("Job done. {0} of {1} failed! {2} not exist in Apsis. {3} emails from QlikView file.", _countOfFailed, _countOfTotal, _countOfNonExisting, _countOfFileEmails) : "Job done in testmode without updating Apsis";
        }

        private static List<string> ReadQlikViewFile(string filePath)
        {
            var returnList = new List<string>();
            string line;
            try
            {
                using (var file = new StreamReader(filePath))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        //File have 1 emailaddress on each line. Parse each line to fetch email address and add it to returnList
                        if (line.Contains("@"))
                        {
                            returnList.Add(line.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("Scheduled job ApsisUpdateMdbOptOuts()", ex.Message);
            }
            return returnList;
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

        private static bool DownloadOptOutFileFromFtp(DateTime? takeFrom)
        {
            const string optOutFilePattern = "WebmailOptOuts_collect_{0}.csv";

            var optOutFtpUri = ConfigurationManager.AppSettings["ApsisOptOutFtpUri"];
            var optOutFtpUserName = ConfigurationManager.AppSettings["ApsisOptOutFtpUserName"];
            var optOutFtpPassword = ConfigurationManager.AppSettings["ApsisOptOutFtpPassword"];
            var optOutFilePath = ConfigurationManager.AppSettings["ApsisOptOutSaveFilePath"];

            var todaysFileName = String.Format(optOutFilePattern, takeFrom != null ? ((DateTime)takeFrom).ToString("yyy-MM-dd") : DateTime.Now.ToString("yyy-MM-dd"));

            var ftpRequestUri = optOutFtpUri + todaysFileName;

            Stream ftpResponseStream = null;

            try
            {
                ftpResponseStream = GetStreamFromFtpRequest(ftpRequestUri, optOutFtpUserName, optOutFtpPassword);

                if (ftpResponseStream == null)
                {
                    new Logger("GetStreamFromFtpRequest returned null for ftRequestUri " + ftpRequestUri, "Stream was null");
                    return false;
                }

                CreateFileFromStream(optOutFilePath + todaysFileName, ftpResponseStream);

                return true;
            }
            catch (Exception exception)
            {
                new Logger("DownloadOptOutFileFromFtp - failed. ftpRequestUri " + ftpRequestUri, exception.ToString());
                return false;
            }
            finally
            {
                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }
            }
        }

        private static Stream GetStreamFromFtpRequest(string ftpRequestUri, string ftpUserName, string ftpPassword)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpRequestUri);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

            var response = (FtpWebResponse)request.GetResponse();

            return response.GetResponseStream();
        }

        private static void CreateFileFromStream(string filePath, Stream stream)
        {
            var fileStream = new FileStream(filePath, FileMode.Create);

            var buffer = new byte[2048];

            while (true)
            {
                var bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                    break;

                fileStream.Write(buffer, 0, bytesRead);
            }

            fileStream.Close();
        }
    }
}