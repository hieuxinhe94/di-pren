using System;
using System.Collections.Generic;
using System.Linq;
using DIClassLib.DbHandlers;
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
        private static DateTime LastCheckInCirix { get; set; }
        private static List<int> CustomersAlreadyProcessed { get; set; }

        public static string Execute()
        {
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
                var tsDifference = DateTime.Now - LastCheckInCirix;
                if (tsDifference.TotalDays >= 1)
                {
                    CustomersAlreadyProcessed.Clear(); // New day, so clear list of all checked customers
                }
                LastCheckInCirix = DateTime.Now;

                //Get customers that have been edited in Cirix and mark those if exist in customer table
                var customersFromCirix = UpdateAndGetCustomersFromCirix();
                //Get all customers that are marked in customer table for update
                var customersFromCustomerTable = DataBasehandler.GetCustomersForApsisUpdateJob();
                //Make sure both list are merged without duplicates so not missing any customer
                var customersToCheck = customersFromCustomerTable.Union(customersFromCirix).ToList();
                //Put customers in "processed" list
                foreach (var cus in customersToCheck)
                {
                    CustomersAlreadyProcessed.Add(cus.CustomerId);
                }
                CheckCustomers(customersToCheck);
            }
            finally
            {
                System.Threading.Monitor.Exit(ApsisCheckLock);
            }
            return Settings.DoApsisUpdates ? "Job succeeded" : "Job succeeded in testmode without updating Apsis";
        }

        private static List<ApsisCustomer> UpdateAndGetCustomersFromCirix()
        {
            var customersForUpdate = CirixDbHandler.GetUpdatedCustomers();
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

        private static void CheckCustomers(List<ApsisCustomer> customers)
        {
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
                    asm.UpdateApsisCustomer(customer);
                }
            }
        }
    }
}