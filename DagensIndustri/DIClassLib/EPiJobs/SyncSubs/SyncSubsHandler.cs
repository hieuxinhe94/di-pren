using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using System.Data;


namespace DIClassLib.EPiJobs.SyncSubs
{
    public static class SyncSubsHandler
    {

        /// <summary>
        /// will sync all customer data for customers flagged as 'N' in expcustomer
        /// to the login tables in mssql (customer and subscription).
        /// </summary>
        //public static string SyncChangedCusts()
        //{
        //    HashSet<int> cusnosFlagY = new HashSet<int>();
        //    HashSet<int> cusnosFlagP = new HashSet<int>();
        //    HashSet<int> changedCusnos = CirixDbHandler.GetChangedCusnosFromExpCustomer();

        //    int i = 0;
        //    foreach (int cusno in changedCusnos)
        //    {
        //        i++;
        //        if (i <= 200)
        //        {
        //            if (SyncCustToMssqlLoginTables(cusno) == 1)
        //                cusnosFlagY.Add(cusno);
        //            else
        //                cusnosFlagP.Add(cusno);
        //        }
        //    }

        //    CirixDbHandler.FlagCustsInExpCustomer(cusnosFlagY, "Y");
        //    CirixDbHandler.FlagCustsInExpCustomer(cusnosFlagP, "P");

        //    return "Antal förändrade cusnos hämtade från Cirix: " + changedCusnos.Count.ToString() + "<br>" +
        //           "Antal flaggade till updated='Y' i Cirix: " + cusnosFlagY.Count.ToString() + "<br>" + 
        //           "Antal flaggade till updated='P' i Cirix: " + cusnosFlagP.Count.ToString();
        //}


        /// <summary>
        /// Will sync selected customer to the login tables in mssql (customer and subscription).
        /// Returns: 1 on ok, -1 does not have active subs, -2 could not find customer facts in cirix.
        /// </summary>
        //public static int SyncCustToMssqlAndFlagInExpCust(int cusno)
        //{
        //    int status = SyncCustToMssqlLoginTables(cusno);
            
        //    if (status == 1)
        //        CirixDbHandler.FlagCustsInExpCustomer(new HashSet<int>() { cusno }, "Y");
        //    else
        //        CirixDbHandler.FlagCustsInExpCustomer(new HashSet<int>() { cusno }, "P");

        //    return status;
        //}


        /// <summary>
        /// returns: 1 on ok, -1 does not have active subs, -2 could not find customer facts in cirix
        /// </summary>
        public static int SyncCustToMssqlLoginTables(int cusno)
        {
            Customer c = GetCustomer(cusno);
            
            if (c != null && c.IsPopulated)
            {
                MsSqlHandler.DeleteCustomerAndSubs(cusno);

                if (c.ShouldBeSaved)
                {
                    InsertCustomerInLoginTables(c);
                    return 1;
                }
                else
                {
                    //new Logger("DoSync() - c.ShouldBeSaved=false - " + c.ToString());
                    return -1;
                }
            }

            return -2;
        }


        private static Customer GetCustomer(int cusno)
        {
            var ds = SubscriptionController.GetCustomer(cusno);

            if (ds != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                var dr = ds.Tables[0].Rows[0];
                //string userid   = dr["WWWUSERID"].ToString();
                //string passwd = SubscriptionController.GetWwwPassword(cusno);
                var email = dr["EMAILADDRESS"].ToString();
                var birthNo = MsSqlHandler.GetCustomerBirthNo(cusno);
                return new Customer(cusno, email, birthNo, GetSubs(cusno));
            }
            return null;
        }


        private static List<Subscription> GetSubs(int cusno)
        {
            var subs = new List<Subscription>();
            var ds = SubscriptionController.GetSubscriptions(cusno, false);
            if (!DbHelpMethods.DataSetHasRows(ds))
            {
                return subs;
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                try
                {
                    var subsno = int.Parse(dr["SUBSNO"].ToString());
                    var subsCusno = int.Parse(dr["SUBSCUSNO"].ToString());
                    var packageId = DbHelpMethods.ValueIfColumnExist<string>(ds.Tables[0], dr, "PACKAGEID", string.Empty); //dr["PACKAGEID"].ToString();
                    var productNo = dr["PRODUCTNO"].ToString();
                    var paperCode = dr["PAPERCODE"].ToString();
                    var expireDate = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSENDDATE");
                    var subsActive = Settings.SubsStateActiveValues.Contains(dr["SUBSSTATE"].ToString());
                    var dtSusp = DbHelpMethods.SetDateFromDbFieldName(dr, "SUSPENDDATE");

                    if (MiscFunctions.DateHasBeenSet(dtSusp) && dtSusp < expireDate)
                    {
                        expireDate = dtSusp;
                    }
                    var s = new Subscription(subsno, packageId, productNo, paperCode, expireDate, subsActive);
                    if (cusno == subsCusno && s.ShouldBeSaved)
                    {
                        subs.Add(s);
                    }
                }
                catch (Exception ex)
                {
                    new Logger("SyncSubsHandler.GetSubs() - failed for cusno: " + cusno, ex.ToString());
                }
            }
            return subs;
        }


        private static void InsertCustomerInLoginTables(Customer c)
        {
            MsSqlHandler.InsertCustomer(c.Cusno, c.Email, c.BirthNo);

            foreach (Subscription sub in c.Subs)
            {
                if(sub.ShouldBeSaved)
                    MsSqlHandler.InsertSubscription(sub.Subsno, c.Cusno, sub.ProductNo, sub.PaperCode, sub.ExpireDate, sub.SubsActive);
            }
        }

    }
}
