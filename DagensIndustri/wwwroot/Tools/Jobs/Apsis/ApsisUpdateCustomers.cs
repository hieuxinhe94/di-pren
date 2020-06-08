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
    [ScheduledPlugIn(DisplayName = "Apsis update customer", Description = "Uppdaterar Apsis data för kunder som existerar i customertabellen")]
    public class ApsisUpdateCustomers
    {
        private static object ApsisCheckLock = new object();
        private static readonly ApsisWsHandler ApsisHandler = new ApsisWsHandler();
        private static readonly MailSenderDbHandler DataBasehandler = new MailSenderDbHandler();
        private static DateTime LastCheckInSubsSystem { get; set; }
        private static List<int> CustomersAlreadyProcessed { get; set; }
        private const int JobId = 1;

        public static string Execute()
        {
            var startTime = DateTime.Now;
            var returnString = string.Empty;
            if (!System.Threading.Monitor.TryEnter(ApsisCheckLock))
            {
                return "Job is still running. You might want to increase the time interval for this job.";
            }
            try
            {
                if (CustomersAlreadyProcessed == null)
                {
                    CustomersAlreadyProcessed = new List<int>();
                }
                var tsDifference = DateTime.Now - LastCheckInSubsSystem;
                if (tsDifference.TotalDays >= 1)
                {
                    CustomersAlreadyProcessed.Clear(); // New day, so clear list of all checked customers
                }
                LastCheckInSubsSystem = DateTime.Now;

                var appSettingNumberOfHoursToAdd = ConfigurationManager.AppSettings.Get("ApsisUpdateCustomerNumberOfHoursAddedToFromDate");
                int numberOfHoursToAdd;
                if(!int.TryParse(appSettingNumberOfHoursToAdd, out numberOfHoursToAdd))
                {
                    numberOfHoursToAdd = 1;
                }

                var getUpdatedCustomersFrom = DataBasehandler.GetScheduledJobRunLastGetDate(JobId);
                var getUpdatedCustomersTo = getUpdatedCustomersFrom.AddHours(numberOfHoursToAdd);

                returnString += "Searched Updated Customers between " + getUpdatedCustomersFrom + " - " +
                                getUpdatedCustomersTo + "<br />"; 

                //Get customers that have been edited in subscription system and mark those if exist in customer table
                var customersFromSubsSystem = UpdateAndGetCustomersFromSubsSystem(getUpdatedCustomersFrom, getUpdatedCustomersTo);

                returnString += "Found " + customersFromSubsSystem.Count + " updated Customers in Subs System" + "<br />"; 

                //Get all customers that are marked in customer table for update
                var customersFromCustomerTable = DataBasehandler.GetCustomersForApsisUpdateJob();

                returnString += "Found " + customersFromCustomerTable.Count + " updated Customers in Table" + "<br />"; 

                //Make sure both list are merged without duplicates so not missing any customer
                var customersToCheck = customersFromCustomerTable.Union(customersFromSubsSystem).ToList();
                //Put customers in "processed" list
                foreach (var cus in customersToCheck)
                {
                    CustomersAlreadyProcessed.Add(cus.CustomerId);
                }
                var numberOfFails = CheckCustomers(customersToCheck);
                returnString += "Number of failed updates " + numberOfFails + "<br />"; 

                // Update LatestScheduledJobRun in db
                DataBasehandler.UpdateScheduledJobRunLastGetDate(JobId, getUpdatedCustomersTo);
            }
            finally
            {
                System.Threading.Monitor.Exit(ApsisCheckLock);                
            }
            
            var endTime = DateTime.Now;
            var timeSpan = endTime - startTime;

            returnString += "The job started at " + startTime + " and ended at " + endTime + ". It took " + timeSpan.Minutes + " minutes and " + timeSpan.Seconds + " seconds. <br />";

            returnString += Settings.DoApsisUpdates ? "Job succeeded" : "Job succeeded in testmode without updating Apsis";

            return returnString;
        }

        private static List<ApsisCustomer> UpdateAndGetCustomersFromSubsSystem(DateTime getUpdatedCustomersFrom, DateTime getUpdatedCustomersTo)
        {
            
            var customersForUpdate = SubscriptionController.GetUpdatedCustomers(getUpdatedCustomersFrom, getUpdatedCustomersTo);
            //Clean list from already checked customers
            foreach (var doneCustomerId in CustomersAlreadyProcessed)
            {
                customersForUpdate.RemoveAll(c => c.CustomerId == doneCustomerId);
            }
            if (!customersForUpdate.Any())
            {
                CustomersAlreadyProcessed.Clear(); //If no customers left to check, clear list of checked customers to allow re-check of any customer
                return customersForUpdate;
            }
            DataBasehandler.MarkCustomerForApsisUpdateJob(customersForUpdate);
            return customersForUpdate;
        }

        private static int CheckCustomers(List<ApsisCustomer> customers)
        {
            var failedCount = 0;
            var asm = new ApsisSharedMethods();
            foreach (var customer in customers)
            {
                //Only do check if user already exist in Apsis
                var apsisListSubscriber = ApsisHandler.GetSubscriberDetails(customer.Email);
                if (apsisListSubscriber == null || string.IsNullOrEmpty(apsisListSubscriber.SubscriberID))
                {
                    DataBasehandler.SetCustomerNotAvailableInApsis(customer.CustomerId);
                    continue;
                }
                if (Settings.DoApsisUpdates)
                {
                    var errors = asm.UpdateApsisCustomer(customer);
                    if (!string.IsNullOrEmpty(errors))
                    {
                        failedCount++;
                        new Logger(
                            string.Format("UpdateApsisCustomer failed for customer with email '{0}', cusno '{1}'",
                                customer.Email, customer.CustomerId), errors);
                    }
                }
            }

            return failedCount;
        }
    }
}