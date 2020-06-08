using System;
using System.Configuration;
using DIClassLib.Misc;
using EPiServer.PlugIn;
using EPiServer.BaseLibrary.Scheduling;

namespace DagensIndustri.Tools.Jobs
{
    [ScheduledPlugIn(DisplayName = "Send Scheduler Is Working Mail", SortIndex = -10, Description = "Ska vara inställt att skicka ett mail varje morgon så att vi vet att jobben fungerar")]
    public class SendSchedulerIsWorkingMail : JobBase
    {
        private bool _stopSignaled;

        public SendSchedulerIsWorkingMail()
        {
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(String.Format("Starting execution of {0}", this.GetType()));

            var emails = ConfigurationManager.AppSettings.Get("SendSchedulerIsWorkingMailToEmails");

            foreach (var email in emails.Split(';'))
            {
                MiscFunctions.SendMail("noreply@di.se", email, "Schemalagda jobben på dagensindustri.se fungerar " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), string.Empty, false); 
            }

            //For long running jobs periodically check if stop is signaled and if so stop execution 
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return "Mails send successfully to " + emails;
        }
    }
}
