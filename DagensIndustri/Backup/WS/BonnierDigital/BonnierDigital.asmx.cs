using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;


namespace WS.BonnierDigital
{
    /// <summary>
    /// Summary description for BonnierDigital
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class BonnierDigital : System.Web.Services.WebService
    {

        /// <summary>
        /// Get PlusCustomer object by providing a customerId.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>PlusCustomer object</returns>
        [WebMethod]
        public PlusCustomer GetPlusCustomer(long customerId)
        {
            try
            {
                return new PlusCustomer(customerId);
            }
            catch (Exception ex)
            {
                new Logger("GetPlusCustomer() failed for customerId: " + customerId.ToString(), ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Get customer numbers for customers that have been updated in Cirix. 
        /// Update entitlements in S+ for cusnos in list [NB!] IF THEY EXIST AS EXTERNAL SUBSCRIBER IDs IN S+.
        /// </summary>
        /// <param name="dateMin">string: YYYY-MM-DD HH:MM:SS (eg. 2012-09-07 22:05:08)</param>
        /// <param name="dateMax">string: YYYY-MM-DD HH:MM:SS (eg. 2012-09-07 23:05:08)</param>
        /// <returns>Success: list of customer numbers [1,2,3] or empty list []. Fail: exception [-1]. Fail: convert dateMin to date [-2]. Fail: convert dateMax to date [-3]</returns>
        [WebMethod]
        public List<long> GetUpdatedCusnosInDateInterval(string dateMin, string dateMax)
        {
            DateTime m = DateTime.MinValue;
            DateTime min = m;
            DateTime max = m;
            
            DateTime.TryParse(dateMin, out min);
            DateTime.TryParse(dateMax, out max);
            
            if (min == m || max == m)
            {
                new Logger("GetUpdatedCusnosInDateInterval() failed for dateMin:" + dateMin + ", dateMax:" + dateMax, "Failed converting string to date");
                List<long> tmp = new List<long>();
                if (min == m) tmp.Add(-2);
                if (max == m) tmp.Add(-3);
                return tmp;
            }
            
            return CirixDbHandler.GetUpdatedCusnosInDateInterval(min, max);
        }


        /// <summary>
        /// When customers entitlements have been updated in S+ call this method to flag updated customers in Cirix.
        /// </summary>
        /// <param name="updatedCusnos">List of updated cusnos</param>
        [WebMethod]
        public void FlagCusnosAsUpdated(List<long> updatedCusnos)
        {
            HashSet<int> cusnos = new HashSet<int>();
            foreach (long cn in updatedCusnos)
                cusnos.Add((int)cn);

            CirixDbHandler.FlagCustsInExpCustomer(cusnos, "Y");
        }

    }


    /// <summary>
    /// Light weight customer class used in Bonnier Digital Services webservice.
    /// </summary>
    public class PlusCustomer : IComparable<PlusCustomer>
    {
        public long CustomerId             = -1;
        public bool IsPlusSubscriber       = false;
        public bool IsActivePlusSubscriber = false;
        public DateTime DateSubsStart      = DateTime.MinValue;
        public DateTime DateSubsEnd        = DateTime.MinValue;


        //ws will fail without parameterless constructor (object cannot be serialized)
        public PlusCustomer() { }


        public PlusCustomer(long customerId)
        {
            CustomerId = customerId;
            
            PlusCustomer pc = GetPlusCustWithLatestExpDate();
            if (pc != null)
                SetProperties(pc.CustomerId, pc.IsPlusSubscriber, pc.IsActivePlusSubscriber, pc.DateSubsStart, pc.DateSubsEnd);
        }

        public PlusCustomer(long customerId, bool isPlusSubscriber, bool isActivePlusSubscriber, DateTime dateSubsStart, DateTime dateSubsEnd)
        {
            SetProperties(customerId, isPlusSubscriber, isActivePlusSubscriber, dateSubsStart, dateSubsEnd);
        }


        private void SetProperties(long customerId, bool isPlusSubscriber, bool isActivePlusSubscriber, DateTime dateSubsStart, DateTime dateSubsEnd)
        {
            CustomerId = customerId;
            IsPlusSubscriber = isPlusSubscriber;
            IsActivePlusSubscriber = isActivePlusSubscriber;
            DateSubsStart = dateSubsStart;
            DateSubsEnd = dateSubsEnd;
        }
        
        private PlusCustomer GetPlusCustWithLatestExpDate()
        {
            List<PlusCustomer> custs = new List<PlusCustomer>();

            try
            {
                DataSet ds = CirixDbHandler.GetSubscriptions(CustomerId, false);
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["PAPERCODE"].ToString().ToUpper() == Settings.PaperCode_IPAD)
                        {
                            bool isPlusSubscriber = true;
                            bool isActivePlusSubscriber = true;
                            DateTime dateSubsStart = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSSTARTDATE");
                            DateTime dateSubsEnd = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSENDDATE");
                            DateTime susp = DbHelpMethods.SetDateFromDbFieldName(dr, "SUSPENDDATE");

                            //official DateSubsEnd is SUSPEND-DATE if SUSPEND-DATE is set
                            if (MiscFunctions.DateHasBeenSet(susp) && susp < dateSubsEnd)
                                dateSubsEnd = susp;

                            if (DateTime.Now.Date > dateSubsEnd.Date)
                                isActivePlusSubscriber = false;

                            custs.Add(new PlusCustomer(CustomerId, isPlusSubscriber, isActivePlusSubscriber, dateSubsStart, dateSubsEnd));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetPlusCustWithLatestExpDate() failed", ex.ToString());
            }

            if (custs.Count > 0)
            {
                custs.Sort();
                return custs[0];
            }

            return null;
        }

        /// <summary>
        /// used when List<PlusCustomer>.Sort() is called
        /// </summary>
        public int CompareTo(PlusCustomer other)
        {
            //return DateSubsEnd.CompareTo(other.DateSubsEnd);  //ASC
            return -DateSubsEnd.CompareTo(other.DateSubsEnd);   //DESC
        }


        #region old code
        //public PlusCustomer() { }
        //private void TrySetMembersOLD()
        //{
        //    //bool useCirixTestWs = false;
        //    //bool.TryParse(Functions.GetAppsettingsValue("bonnierDigitalWsUsesCirixTestWs").ToLower(), out useCirixTestWs);

        //    DataSet ds = CirixDbHandler.GetSubscriptions(CustomerId, false);

        //    if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
        //    {
        //        //todo: support multiple IPAD custs? return cust with latest subs if multiple?
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            if (dr["PAPERCODE"].ToString().ToUpper() == "IPAD")
        //            {
        //                IsPlusSubscriber = true;
        //                IsActivePlusSubscriber = true;
        //                DateSubsStart = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSSTARTDATE");   //DateTime.Now
        //                DateSubsEnd = DbHelpMethods.SetDateFromDbFieldName(dr, "SUBSENDDATE");
        //                DateTime susp = DbHelpMethods.SetDateFromDbFieldName(dr, "SUSPENDDATE");

        //                //official DateSubsEnd is SUSPEND-DATE if SUSPEND-DATE is set
        //                if (MiscFunctions.DateHasBeenSet(susp) && susp < DateSubsEnd)
        //                    DateSubsEnd = susp;

        //                if (DateTime.Now.Date > DateSubsEnd.Date)
        //                    IsActivePlusSubscriber = false;

        //                //110224 - not a good check if customer bought subs starting in late future...
        //                //is active subscriber 5 days earlier then SUBSSTARTDATE
        //                //if (now < DateSubsStart.AddDays(-5) || now > DateSubsEnd)

        //                return;
        //            }
        //        }
        //    }
        //}
        #endregion

    }

}
