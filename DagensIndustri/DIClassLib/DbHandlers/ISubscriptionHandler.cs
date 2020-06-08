using System;
using System.Collections.Generic;
using System.Data;

using DIClassLib.Campaign;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Kayak;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.Subscriptions.DiPlus;

namespace DIClassLib.DbHandlers
{
    public interface ISubscriptionHandler
    {
        string TestOracleConnection();

        HashSet<string> GetTargetGroups(string paperCode);

        List<StringPair> GetAllTargetGroups();

        DataSet GetCampaign(long campNo);

        DataSet GetCampaign(string campId);

        long GetCampno(string campId);

        string GetProductName(string paperCode, string productNo);

        List<CampaignInfo> GetActiveFreeCampaigns(string paperCode, string productNo);

        string GetCommuneCode(string zipCode);

        long GetPriceListNo(string paperCode, string productNo, DateTime invStartDate, string communeCode, string priceGr, string campId);
        
        string UpdateCustomerInformation(long lCusNo, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sSalesDen,
            string sOfferdenDir, string sOfferdenSal, string sOfferdenEmail, string sDenySmsMark, string sAccnoBank,
            string sAccnoAcc, string sNotes, long lEcusno, string sOtherCusno, string sWwwUserId, string sExpday,
            double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string companyId);

        string GetWwwPassword(long cusNo);

        string GetWwwUserId(long cusno);

        DataSet GetCustomer(long cusNo);

        Person GetCustomerInfo(long cusNo);

        string GetEmailAddress(long cusno);
        
        int InsertCustomerProperty(long cusno, string propCode, string propValue);

        DataSet GetSubscriptions(long cusNo, bool showPassiveIfNoActive);

        List<ApsisCustomer> CirixGetNewCusts(DateTime? takeFrom = null);

        List<ApsisCustomer> CirixGetCustsManuallyFromList(IEnumerable<string> customerIds);

        List<long> GetUpdatedCusnosInDateInterval(DateTime dateMin, DateTime dateMax);

        void FlagCustsInLetter(List<ApsisCustomer> custs, string flag);

        void FlagCustsInExpCustomer(HashSet<int> cusnos, string flag);

        void UpdateCustomerEmail(int cusno, string email);

        ApsisCustomer CirixGetCustomer(int customerId, int subsNo, bool isExtCustomer);

        void UpdateLetterInCirix(ApsisCustomer c, int subsno);

        bool CustomerIsActive(int cusno, string paperCode, string productNo);

        List<ApsisCustomer> GetUpdatedCustomers(DateTime startDate, DateTime endDate);
        
        List<long> GetCusnosByEmail(string email);

        List<string> GetCustomerXtraFields(long cusno);

        List<DateTime> GetPublDays(string sPaperCode, string sProductNo, DateTime dteFirstDate, DateTime dteLastDate);

        List<DateTime> GetProductsIssueDatesInInterval(string paperCode, string productno, DateTime dateMin, DateTime dateMax);

        DateTime GetIssueDate(string paperCode, string productno, DateTime date, EnumIssue.Issue issue);

        DateTime GetNextIssueDateIncDiRules(DateTime wantedDate, string paperCode, string productNo);

        DateTime GetClosesIssueDateInTheory(string paperCode, string productNo);

        DateTime GetNextIssuedate(string papercode, string productno, DateTime minDate);

        string CreateRenewal_DI(long lSubscusno, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate,
            DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty, string sSalesno, long lPaycusno, string sPackageId, string sPaperCode, string sProductno,
            string sReceiveType, string sTargetGroup, string sPriceAtStart, string sOtherSubsno, string sOrderId, string sAutogiro, string sPriceGroup, string sInvMode);

        string DefinitiveAddressChange(long cusno, string street, string houseNo, string stairCase, string apartment, string careOf, string zip, DateTime startDate);

        string DefinitiveAddressChangeRemove(long cusno, DateTime startDate);

        List<StringPair> GetNumSubsForPayingCust(string customerRowText1SearchStr);

        int GetNumSubsForPayingCustOnline(string customerRowText1SearchStr);

        bool AddSubsIpadSummer(long cusno);

        List<Person> FindCustomerByPerson(Person searchCriterias, bool includeEmailAsSearchCriteria);

        long TryAddCustomer2(Subscription sub, bool isSubscriber);

        string AddNewSubs2(Subscription sub, long cusNoPay, DiPlusSubscription plusSub);

        List<Subscription> GetSubscriptions2(long cusno);

        void ChangeCusInvMode(long cusno, string newInvoiceMode, string oldInvoiceMode);

        string TryGetDefaultCusInvMode(long cusno);

        void AddNewCusInvmode(long cusno, string invMode, bool isInvDefault);

        long GetNextInvno();

        string BuildRefno2(long lInvno, string sInvType, string sPaperCode);

        long CreateImmediateInvoice(Subscription subscription, int iExtno, int iItemno, long lInvno, string sRefno);

        string InsertElectronicPayment(long lCusno, long lInvno, string sRefno, double dAmount);

        long CreateNewInvoice(Subscription subscription);

        DataSet GetOpenInvoices(long lCusno);

        DataSet GetInvArgItems(long subsno, int extno);

        DataSet FindCustomers(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sEmail,
            string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname);

        DataSet GetCustomerProperties(long lCusno);

        string CleanCustomerProperties(long lCusno);

        string UpdateCustomerExtraInfo(long lCusno, string sExtra1, string sExtra2, string sExtra3);

        DataSet GetSubsSleeps(long subscriptionNumber);

        DataSet GetCurrentAndPendingAddressChanges(long cusno, long subsno);

        DataSet GetCusTempAddresses(long customerNumber, string sOnlyThisCountry, string sOnlyValid);

        long CreateSubssleep_CII(long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sCreditType, string sAllowWebPaper, string sSleepReason, string sReceiveType, string sSleepLimit);

        string CreateHolidayStop(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sAllowWebpaper, string sCreditType);

        string DeleteHolidayStop(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate);

        string TemporaryAddressChange(string sUserId, long lCusno, long lSubsno, int iExtno, AddressMap addressMap, DateTime dAddrStartDate, DateTime dAddrEndDate, string sNewName1, string sNewName2, string sInvToTempAddress);

        //string TemporaryAddressChangeNewAddress(long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, DateTime dateStartdate, DateTime dateEnddate, string sInvToTempAddress);

        string TemporaryAddressChangeRemove(string sUserId, long lCusno, long lSubsno, int iExtno, int iAddrno, DateTime dOriginalStartDate);

        long IdentifyCustomer(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserId);

        string UpdateWwwPassword(long lCusno, string sNewPassword1, string sNewPassword2, bool bCheckOldPassword, string sOldPassword);

        bool IsHybridSubscriber(long cusno);

        bool IsHybridCampaign(long campNo);

        /// <summary>
        /// NOT CACHED, USE WITH CARE.
        /// </summary>
        IEnumerable<DataSet> GetActiveCampaigns();

        string GetPostName(string zipCode, string countryCode = "SE");

        List<PackageProduct> GetPackageProducts(string packageId);

        long GetCustomerByEcusno(long ecusno);
        long GetEcusnoByCustomer(long cusno);
    }
}