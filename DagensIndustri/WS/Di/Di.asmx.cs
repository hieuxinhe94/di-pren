using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using System.Text;
using DIClassLib.Misc;
using DIClassLib.EPiJobs.EpiDataForExternalUse;


namespace WS.Di
{
    /// <summary>
    /// Summary description for Di
    /// </summary>
    [WebService(Namespace = "http://ws.dagensindustri.se/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Di : System.Web.Services.WebService
    {
        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        
        /// <summary>
        /// Get code for connecting Cirix customer to Bonnier digital single sign-on account (Di-konto)
        /// </summary>
        /// <param name="cusno">Cirix cusno</param>
        /// <returns>Code: 12345-XXXX</returns>
        [WebMethod]
        public string GetSingleSignOnCodeByCusno(long cusno)
        {
            try
            {
                return MsSqlHandler.GetSsoCustomerCode(cusno);
            }
            catch (Exception ex)
            {
                new Logger("GetSingleSignOnCodeByCusno() failed for cusno: " + cusno.ToString(), ex.ToString());
                return string.Empty;
            }
        }


        /// <summary>
        /// Get future Di conferences
        /// </summary>
        /// <param name="numUpcomingConfrences"></param>
        /// <returns>List of Conference objects</returns>
        [WebMethod]
        public List<ConferenceEpiData> GetUpcomingConferences(int numUpcomingConfrences = 3)
        {
            return LocalCache.GetUpcomingConferences(numUpcomingConfrences);
        }


        
        /// <summary>
        /// Provide in-parameters. On success there will be no erros in the return object. 
        /// If data is missing/not valid OR if save to Cirix/Service+ failed there will be erros in the return object. 
        /// NB: if a cirix customer number is found in Service+ 'ExtSubscriberId' field, then provide that cusno as last parameter.
        /// </summary>
        /// <returns>SaveCustomerAndSubscription object</returns>
        //[WebMethod]
        //public SaveCustomerAndSubscriptionReturn SaveCustAndSubInvoice(string servicePlusUserId, string cirixCampId, string firstName, string lastName, string email, string phoneMobile, 
        //                                                               string companyName, string companyNumber, string careOf, string streetAddress, string streetNumber, string streetEntrance, string zipCode,
        //                                                               long cirixCusno = 0)
        //{
        //    var retObj = new SaveCustomerAndSubscriptionReturn()
        //    {
        //        ServicePlusUserId = servicePlusUserId,
        //        CirixCampId = cirixCampId,
        //        //CardPayTableUpdated = false,
        //        SavedEntitlementInServicePlus = false,
        //        //CardPayCustRefno = cardPayCustRefno
        //    };

        //    try
        //    {
        //        #region validation

        //        retObj.Errors.AddRange(GetStdValErrors(servicePlusUserId, cirixCampId, firstName, lastName, email, phoneMobile));

        //        if (string.IsNullOrEmpty(streetAddress))
        //            retObj.Errors.Add(new Error(100, "Ange gatuadress", "Street address is missing"));

        //        if (!string.IsNullOrEmpty(streetNumber) || !MiscFunctions.IsNumeric(streetNumber))
        //            retObj.Errors.Add(new Error(110, "Gatunummer måste vara ett tal", "Street number must be numeric"));

        //        if (!MiscFunctions.IsValidSweZipCode(zipCode))
        //            retObj.Errors.Add(new Error(120, "Postnumret är inte giltigt. Det måste bestå av 5 siffror.", "Zip code is not valid. It must consist of 5 integers."));

        //        if (!string.IsNullOrEmpty(companyNumber) || !MiscFunctions.IsNumeric(companyNumber) || companyNumber.Length != 10)
        //            retObj.Errors.Add(new Error(130, "Företagsnummer måste bestå av 10 siffror", "Company number must consist of 10 integers"));

        //        if (retObj.Errors.Count > 0)
        //            return retObj;

        //        #endregion

        //        //retObj.CardPayCustRefno = cardPayCustRefno;
        //        retObj.CirixCusno = 12345;
        //        retObj.CirixSubsnos.Add(23456);
        //        //retObj.CardPayTableUpdated = true;
        //        retObj.SavedEntitlementInServicePlus = true;

        //        return retObj;

        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("SaveCustAndSubInvoice() failed", ex.ToString());
        //        retObj.Errors.Add(new Error(-1, "Ett tekniskt fel uppstod", "Exception was thrown"));
        //        return retObj;
        //    }
        //}

        //private List<Error> GetStdValErrors(string servicePlusUserId, string cirixCampId, string firstName, string lastName, string email, string phoneMobile)
        //{
        //    List<Error> stdErrs = new List<Error>();

        //    if (string.IsNullOrEmpty(servicePlusUserId))
        //        stdErrs.Add(new Error(10, "Id för Di-konto saknas", "ServicePlus userId is missing"));

        //    if (string.IsNullOrEmpty(cirixCampId))
        //        stdErrs.Add(new Error(20, "CirixCampId saknas", "CirixCampId is missing"));

        //    if (string.IsNullOrEmpty(firstName))
        //        stdErrs.Add(new Error(30, "Förnamn saknas", "First name is missing"));

        //    if (string.IsNullOrEmpty(lastName))
        //        stdErrs.Add(new Error(40, "Efternamn saknas", "Last name is missing"));

        //    if (!MiscFunctions.IsValidEmail(email))
        //        stdErrs.Add(new Error(50, "Ogiltig e-postadress", "Email address is not valid"));

        //    //phoneMobile

        //    return stdErrs;
        //}


        /// <summary>
        /// Returns price for campaign in Cirix. NB! At the moment 99 is returned for all "cirixCampId:s".
        /// </summary>
        /// <param name="cirixCampId"></param>
        /// <returns>Price in SEK</returns>
        //[WebMethod]
        //public int GetCampaignPrice(string cirixCampId)
        //{
        //    return 99;
        //}

    }

    //public class SaveCustomerAndSubscriptionReturn
    //{
    //    public string ServicePlusUserId { get; set; }
    //    public string CirixCampId { get; set; }
    //    public long CirixCusno { get; set; }
    //    public List<long> CirixSubsnos { get; set; }
    //    //public long CardPayCustRefno { get; set; }
    //    //public bool CardPayTableUpdated { get; set; }
    //    public bool SavedEntitlementInServicePlus { get; set; }
    //    public List<Error> Errors { get; set; }

    //    public SaveCustomerAndSubscriptionReturn() {}
    //}

    
    //public class Error
    //{
    //    public int ErrorCode { get; set; }
    //    public string ErrorMessageSwe { get; set; }
    //    public string ErrorMessageEng { get; set; }

    //    public Error(){}

    //    public Error(int errCode, string errSwe, string errEng)
    //    {
    //        ErrorCode = errCode;
    //        ErrorMessageSwe = errSwe;
    //        ErrorMessageEng = errEng;
    //    }
    //}

    //public class Conference
    //{
    //    public string ConfName { get; set; }
    //    public string ConfCity { get; set; }
    //    public DateTime ConfDate { get; set; }

    //    public Conference() {}

    //    public Conference(string confName, string confCity, DateTime confDate)
    //    {
    //        ConfName = confName;
    //        ConfCity = confCity;
    //        ConfDate = confDate;
    //    }
    //}

    //public enum PayMethods
    //{
    //    Invioce,
    //    CreditCard
    //}

}
