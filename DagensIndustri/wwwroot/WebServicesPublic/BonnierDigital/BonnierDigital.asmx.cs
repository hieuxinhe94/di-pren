using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using System.Data;
using DIClassLib.Misc;


namespace DagensIndustri.WebServicesPublic.BonnierDigital
{
    /// <summary>
    /// Summary description for BonnierDigital
    /// </summary>
    [WebService(Namespace = "http://dagensindustri.se/")]
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

    }


    /// <summary>
    /// Light weight customer class used in Bonnier Digital Services webservice.
    /// </summary>
    public class PlusCustomer
    {
        public long CustomerId = -1;
        public bool IsPlusSubscriber = false;
        public bool IsActivePlusSubscriber = false;
        public DateTime DateSubsStart = DateTime.MinValue;
        public DateTime DateSubsEnd = DateTime.MinValue;


        public PlusCustomer() { }


        public PlusCustomer(long customerId)
        {
            TrySetMembers(customerId);
        }


        private void TrySetMembers(long customerId)
        {
            CustomerId = customerId;
            var ds = SubscriptionController.GetSubscriptions(CustomerId, false);

            if (ds == null || ds.Tables[0].Rows == null)
            {
                return;
            }
            //todo: support multiple IPAD custs? return cust with latest subs if multiple?
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["PAPERCODE"].ToString().ToUpper() == "IPAD")
                {
                    IsPlusSubscriber = true;
                    IsActivePlusSubscriber = true;
                    DateSubsStart = SetDateFromFieldName(dr, "SUBSSTARTDATE");   //DateTime.Now
                    DateSubsEnd = SetDateFromFieldName(dr, "SUBSENDDATE");
                    var susp = SetDateFromFieldName(dr, "SUSPENDDATE");

                    //official DateSubsEnd is SUSPEND-DATE if SUSPEND-DATE is set
                    if (MiscFunctions.DateHasBeenSet(susp) && susp < DateSubsEnd)
                        DateSubsEnd = susp;

                    if (DateTime.Now.Date > DateSubsEnd.Date)
                        IsActivePlusSubscriber = false;

                    return;
                }
            }
        }


        private DateTime SetDateFromFieldName(DataRow dr, string fieldName)
        {
            DateTime dt;
            if (DateTime.TryParse(dr[fieldName].ToString(), out dt))
                return dt;

            return DateTime.MinValue;
        }
    }
}
