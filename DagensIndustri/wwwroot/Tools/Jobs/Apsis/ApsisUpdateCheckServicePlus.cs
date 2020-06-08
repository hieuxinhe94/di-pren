using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;

using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Jobs.Apsis
{
    [ScheduledPlugIn(DisplayName = "Apsis check Service Plus", Description = "Har kund som fått välkomstemail nr 1 skapat Service Plus konto efteråt, uppdatera då info hos Apsis inför mail nr 2 som skickas ifrån Apsis")]
    public class ApsisUpdateCheckServicePlus
    {
        private static object ApsisCheckServicePlusLock = new object();
        private static int customerCounter;
        private static int counterFailedApsisUpdate;
        private static DateTime startTime;
        private static readonly ApsisWsHandler ApsisHandler = new ApsisWsHandler();
        private static readonly MailSenderDbHandler DataBasehandler = new MailSenderDbHandler();
   
        public static string Execute()
        {
            if (!System.Threading.Monitor.TryEnter(ApsisCheckServicePlusLock))
            {
                return "Job is still running. You might want to increase the time interval for this job.";
            }

            // Initialize start values
            startTime = DateTime.Now;
            customerCounter = 0;
            counterFailedApsisUpdate = 0;
            var checkUserErrors = new StringBuilder();
            try
            {
                //Running steps backwards as each step gets increased after check. That way it will not be immediately checked again in next step
                for (var step = 3; step >= 1; step--)
                {
                    var stepResult = RunStep(step);
                    if (!string.IsNullOrEmpty(stepResult)) checkUserErrors.Append(stepResult);
                }
                var timeForRun = DateTime.Now - startTime;
                if (checkUserErrors.Length > 0)
                {
                    return string.Format("Fail! Total {0} checked. Failed update = {1}. {2}. Seconds to run: {3}", customerCounter, counterFailedApsisUpdate, checkUserErrors, Math.Round(timeForRun.TotalSeconds,1));
                }
                return string.Format("Total {0} checked. Seconds to run: {1}", customerCounter, Math.Round(timeForRun.TotalSeconds, 1));
            }
            finally
            {
                System.Threading.Monitor.Exit(ApsisCheckServicePlusLock);
            }
        }

        private static string RunStep(int step)
        {
            var checkUserErrors = new StringBuilder();
            var customers = DataBasehandler.GetCustomersForApsisStep(step);
            foreach (var customer in customers)
            {
                customerCounter++;
                //First check that user exist at Apsis
                var apsisListSubscriber = ApsisHandler.GetSubscriberDetails(customer.Email);
                if (apsisListSubscriber == null || string.IsNullOrEmpty(apsisListSubscriber.SubscriberID))
                {
                    checkUserErrors.Append(string.Format("{0} finns ej hos Apsis! ", customer.Email));
                    counterFailedApsisUpdate++;
                    DataBasehandler.SetCustomerNotAvailableInApsis(customer.CustomerId);
                    continue;
                }
                if (!Settings.DoApsisUpdates) //For testmode!
                {
                    continue;
                }
                //new Logger(string.Format("ApsisUpdateCheckServicePlus Step:{0} start processing {1} customers", step, customers.Count));
                var asm = new ApsisSharedMethods();
                var result = asm.UpdateApsisCustomer(customer);
                if (string.IsNullOrEmpty(result))
                {
                    //Update customer in DB table, to the step value that currently ran
                    DataBasehandler.UpdateApsisCustomerStep(customer, step);
                }
                else
                {
                    checkUserErrors.Append(result);
                }
            }
            return checkUserErrors.ToString();
        }
    }
}