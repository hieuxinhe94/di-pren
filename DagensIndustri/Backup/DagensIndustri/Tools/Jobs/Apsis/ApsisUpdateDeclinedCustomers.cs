using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Mdb;
using DIClassLib.Misc;

using EPiServer.PlugIn;

namespace DagensIndustri.Tools.Jobs.Apsis
{
    [ScheduledPlugIn(DisplayName = "Apsis update declined info on customers", Description = "Uppdaterar Apsis data för kunder som tackat nej via S2 efter prova-på.")]
    public class ApsisUpdateDeclinedCustomers
    {
        private static object ApsisCheckLock = new object();
        private static readonly string[] EnabledOutcomes = {"nej", "order"};

        public static string Execute()
        {
            if (!System.Threading.Monitor.TryEnter(ApsisCheckLock))
            {
                return "Job is still running. You might want to increase the time interval for this job.";
            }
            string result;
            try
            {
                var declineInfoList = MdbDbHandler.GetCustomerDeclines(DateTime.Today.AddDays(-1));
                result = ProcessCustomers(declineInfoList);
            }
            finally
            {
                System.Threading.Monitor.Exit(ApsisCheckLock);
            }
            return string.IsNullOrEmpty(result) ? Settings.DoApsisUpdates ? "Job succeeded" : "Job succeeded in testmode without updating Apsis" : result;
        }

        private static string ProcessCustomers(List<CustomerDecline> declineInfoList)
        {
            var messages = new StringBuilder();
            foreach (var declineInfo in declineInfoList)
            {
                if (!EnabledOutcomes.Contains(declineInfo.Outcome.ToLower()))
                {
                    continue;
                }
                var customer = CirixDbHandler.GetCustomerInfo(declineInfo.TpId);
                if (string.IsNullOrEmpty(customer.Email))
                {
                    messages.Append("No email found in Cirix, Cusno/TpId:" + declineInfo.TpId + ". ");
                    continue;
                }
                if (!Settings.DoApsisUpdates) //For testmode
                {
                    continue;
                }
                var data = new ApsisFields() {OutcomeS2 = declineInfo.Outcome.ToLower() == "order" ? "Y" : "N", OutcomeS2Date = declineInfo.DateTime.ToString("yyyy-MM-dd")};
                var asm = new ApsisSharedMethods();
                asm.UpdateDeclineInfoOnCustomer(customer.Email, data);
            }
            return messages.ToString();
        }
    }
}