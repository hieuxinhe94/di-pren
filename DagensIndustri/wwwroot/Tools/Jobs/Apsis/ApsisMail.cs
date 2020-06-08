using System;
using System.Linq;
using EPiServer.PlugIn;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;


namespace DagensIndustri.Tools.Jobs.Apsis
{
    [ScheduledPlugIn(DisplayName = "Apsis mail", Description = "Skickar mail")]
    public class MyJob
    {
        public static string Execute()
        {
            if (MiscFunctions.GetAppsettingsValue("apsisJobDeactivated") == "true")
            {
                return "Jobbet kördes inte (ej exekverat från skarpa webbservern)";
            }
            var db = new MailSenderDbHandler();
            var batch = new ApsisBatch(); //new batch each time the program executes

            if (batch.BatchId == 0)
            {
                return "Kunde ej skapa ny batch. MSSQL-databas förmodligen ej tillgänglig. Se logg.";
            }
            var numNewBounces = db.AddBouncesFromApsis();
            var custsRetry = db.GetRetryCusts();

            var takefromSetting = ConfigurationManager.AppSettings.Get("ApsisMailTakeFromDate");
            DateTime? takeFrom = null;
            try
            {
                takeFrom = DateTime.Parse(takefromSetting);
            }
            catch (Exception)
            {
              
            }

            var custsNew = SubscriptionController.CirixGetNewCusts(takeFrom);
            db.AddNewCusts(custsNew);
            if (SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Cirix)
            {
                SubscriptionController.FlagCustsInLetter(custsNew, "P");

                var custsSendSuccess = db.GetNewSendSuccessCusts(); //get custs that has not bounced for x days
                db.SetIsSendSuccess(custsSendSuccess, true); //set isSendSuccess=1
                SubscriptionController.FlagCustsInLetter(custsSendSuccess, "Y");
            }
            else
            {
                SubscriptionController.FlagCustsInLetter(custsNew, "Y"); //For Kayak SubsIsConfirmed_CII() is called to not get same customers in next call to SubscriptionController.CirixGetNewCusts()
            }

            TrySendEmails(custsRetry, batch);
            TrySendEmails(custsNew, batch);

            //send 1 bounce info mail to staff at 10 and 15, mon-fri (if bounces exists)
            HandleStaffMail(batch.DateBatch);

            //log message displayed in EPi
            return BuildLogMessage(batch, numNewBounces, custsRetry.Count, custsNew.Count);
        }

        private static void TrySendEmails(List<ApsisCustomer> custs, ApsisBatch batch)
        {
            var db = new MailSenderDbHandler();
            var ws = new ApsisWsHandler();

            // Get Company Portal campaign number from setting
            var companyPortalCampNoSetting = ConfigurationManager.AppSettings.Get("CompanyPortalCampNo");
            int companyPortalCampNo;

            int.TryParse(companyPortalCampNoSetting, out companyPortalCampNo);

            foreach (var c in custs)
            {
                //apsis does not send messages to badly formated addresses, so fake bounce
                if (!MiscFunctions.IsValidEmail(c.Email))
                {
                    db.FlagCustAsBounce(c.CustomerId);
                    continue;
                }

                // If customer has bought Company Portal Campaign we don't want them to get the Apis welcome mail
                if (companyPortalCampNo > 0 && c.CampNo != null && c.CampNo == companyPortalCampNo)
                {
                    new Logger("Customer with email " + c.Email + " not send to Apsis becuase CampNo " + c.CampNo + " is a Company Portal campaign");
                    continue;
                }

                //avoid same cust in several batches if ApsisSendEmail() reply is slow
                db.FlagCustAsSent(c.CustomerId); //isBounce=0, forceRetry=0

                var identifier = Guid.NewGuid().ToString();
                var mailId = ws.ApsisSendEmail(identifier, c); //test mode: no mail sent, mailId -10 to -1000

                if (MiscFunctions.ApsisMailerIsInTestMode)
                    db.InsertCustomerInBatch(identifier, c, batch, mailId);
                else
                {
                    if (mailId > 0)
                        db.InsertCustomerInBatch(identifier, c, batch, mailId);
                    else
                    {
                        try
                        {
                            db.SetForceRetry(c.CustomerId, true, false);
                        } //send failure: include in next batch (forceRetry=1) 
                        catch
                        {
                            //silent EX - log is written in SetForceRetry() method
                        }
                    }
                }
            }
        }

        private static string BuildLogMessage(ApsisBatch batch, int numNewBounces, int numCustsRetry, int numCustsNew)
        {
            var ret = new StringBuilder();
            ret.Append("BatchId: " + batch.BatchId + ", datum: " + batch.DateBatch);

            if (numNewBounces > 0)
                ret.Append("<br>Antal nya studsar hämtade från Apsis: " + numNewBounces);

            if (numCustsRetry > 0)
                ret.Append("<br>Antal återförsök: " + numCustsRetry);

            if (numCustsNew > 0)
                ret.Append("<br>Antal nya utskick: " + numCustsNew);

            return ret.ToString();
        }

        private static void HandleStaffMail(DateTime dtBatch)
        {
            if (!IsTimeToSendStaffMail(dtBatch))
            {
                return;
            }
            var body = GetStaffMailText();
            if (string.IsNullOrEmpty(body))
            {
                return;
            }
            var recievers = ConfigurationManager.AppSettings["apsisBounceInfoMailReceivers"].Split(';');
            foreach (var email in recievers)
            {
                var s = email.Trim();
                if (!MiscFunctions.IsValidEmail(s))
                {
                    continue;
                }
                try
                {
                    MiscFunctions.SendMail("no-reply@di.se", s, "Studsar att hantera i EPi", body, true);
                }
                catch (Exception ex)
                {
                    new Logger("HandleStaffMail() failed when sending mail to: " + s, ex.ToString());
                }
            }
        }

        private static bool IsTimeToSendStaffMail(DateTime dtBatch)
        {
            //monday to friday
            if (dtBatch.DayOfWeek == DayOfWeek.Saturday || dtBatch.DayOfWeek == DayOfWeek.Sunday)
                return false;

            //time between 10.00-10.10 and 15.00-15.10
            if (!((dtBatch.Hour == 10 || dtBatch.Hour == 15) && dtBatch.Minute <= 10))
                return false;

            //db call - send ONE mail at 10 and ONE mail at 15
            var db = new MailSenderDbHandler();
            return db.IsFirstBatchInHour(dtBatch);
        }

        private static string GetStaffMailText()
        {
            var sb = new StringBuilder();
            var db = new MailSenderDbHandler();

            try
            {
                List<ApsisCustomer> custs = db.GetBouncedCusts();
                if (custs.Count > 0)
                {
                    sb.Append("Det finns nu " + custs.Count.ToString() + " st studsar att hantera i EPi-servers studshantering.");
                }
            }
            catch (Exception ex)
            {
                new Logger("GetStaffMailText() failed", ex.ToString());
            }

            return sb.ToString();
        }

        //ONLY FOR TEST
        // 20140129 tested that correct welcome mail template was used for Agenda
        /*
        public static string ExecuteTest()
        {
            var db = new MailSenderDbHandler();
            var batch = new ApsisBatch(); //new batch each time the program executes

            if (batch.BatchId == 0) //mssql db connection probably down
                return "Kunde ej skapa ny batch. MSSQL-databas förmodligen ej tillgänglig. Se logg.";

            //var numNewBounces = db.AddBouncesFromApsis();
            //List<ApsisCustomer> custsRetry = db.GetRetryCusts();

            var custsNew = CirixDbHandler.CirixGetNewCustsTest();
            db.AddNewCusts(custsNew);
            try
            {
                CirixDbHandler.FlagCustsInLetter(custsNew, "P");
            }
            catch
            {
                //updated=P, will NOT be deleted from cirix LETTER table  
            }

            //List<ApsisCustomer> custsSendSuccess = db.GetNewSendSuccessCusts();     //get custs that has not bounced for x days
            //db.SetIsSendSuccess(custsSendSuccess, true);                       //set isSendSuccess=1
            //try { CirixDbHandler.FlagCustsInLetter(custsSendSuccess, "Y"); }        //updated=Y, will be deleted from cirix LETTER table
            //catch { }

            //TrySendEmails(custsRetry, batch);
            var testableList = custsNew.Where(cust => cust.Email == "persyl@gmail.com").ToList();
            if (testableList.Count > 0)
            {
                TrySendEmails(testableList, batch);
            }

            //send 1 bounce info mail to staff at 10 and 15, mon-fri (if bounces exists)
            //HandleStaffMail(batch.DateBatch);

            var ret = new StringBuilder();
            ret.Append("BatchId: " + batch.BatchId.ToString() + ", datum: " + batch.DateBatch.ToString());
            if (testableList.Count > 0)
                ret.Append("<br>Antal nya utskick: " + testableList.Count.ToString());
            return ret.ToString();
        }
        */
    }
}