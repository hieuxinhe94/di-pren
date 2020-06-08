using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

using DIClassLib.BonnierDigital;
using DIClassLib.Campaign;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.Subscriptions.DiPlus;
using DIClassLib.Kayak;

namespace DIClassLib.DbHandlers
{
    public static class SubscriptionController
    {
        public enum AvailableHandlers
        {
            Cirix,
            Kayak
        };

        public static AvailableHandlers ActiveHandler
        {
			//cirix will never happen again...
			get { return AvailableHandlers.Kayak; }
            //get { return Settings.UseCirixHandler ? AvailableHandlers.Cirix : AvailableHandlers.Kayak; }
        }

        private static ISubscriptionHandler _subsHandler;

        private static ISubscriptionHandler SubsHandler
        {
            get {
                if (_subsHandler != null)
                {
                    return _subsHandler;
                }
                switch (ActiveHandler)
                {
                    case AvailableHandlers.Kayak:
                        _subsHandler = new KayakHandler();
                        break;
					//case AvailableHandlers.Cirix:
					//	_subsHandler=new CirixDbHandlerAdapter();
					//	break;
                }
                return _subsHandler;
            }
        }

        public static long GetCustomerByEcusno(long eCusno)
        {
            return SubsHandler.GetCustomerByEcusno(eCusno);
        }

        public static long GetEcusnoByCustomer(long cusno)
        {
            return SubsHandler.GetEcusnoByCustomer(cusno);
        }

        public static string TestOracleConnection()
        {
            return SubsHandler.TestOracleConnection();
        }

        public static HashSet<string> GetTargetGroups(string paperCode)
        {
            return SubsHandler.GetTargetGroups(paperCode);
        }

        public static List<StringPair> GetAllTargetGroups()
        {
            return SubsHandler.GetAllTargetGroups();
        }

        public static DataSet GetCampaign(long campNo)
        {
            return SubsHandler.GetCampaign(campNo);
        }

        public static DataSet GetCampaign(string campId)
        {
            return SubsHandler.GetCampaign(campId);
        }

        public static long GetCampno(string campId)
        {
            return SubsHandler.GetCampno(campId);
        }

        public static string GetProductName(string paperCode, string productNo)
        {
            return SubsHandler.GetProductName(paperCode, productNo);
        }
        
        public static List<CampaignInfo> GetActiveFreeCampaigns(string paperCode, string productNo)
        {
            return SubsHandler.GetActiveFreeCampaigns(paperCode, productNo);
        }

        public static string GetCommuneCode(string zipCode)
        {
            return SubsHandler.GetCommuneCode(zipCode);
        }

        //TODO: Remove this as it is only used for renewal methods, and they doesn't require pricelistno as parameter in Kayak as in Cirix.
        public static long GetPriceListNo(string paperCode, string productNo, DateTime invStartDate, string communeCode, string priceGr, string campId)
        {
            return SubsHandler.GetPriceListNo(paperCode, productNo, invStartDate, communeCode, priceGr, campId);
        }
        
        public static string UpdateCustomerInformation(long lCusNo, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sSalesDen,
            string sOfferdenDir, string sOfferdenSal, string sOfferdenEmail, string sDenySmsMark, string sAccnoBank,
            string sAccnoAcc, string sNotes, long lEcusno, string sOtherCusno, string sWWWUserID, string sExpday,
            double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string companyId)
        {
            return SubsHandler.UpdateCustomerInformation(lCusNo, sEmailAddress, sHPhone, sWPhone, sOPhone, sSalesDen,
                sOfferdenDir, sOfferdenSal, sOfferdenEmail, sDenySmsMark, sAccnoBank,
                sAccnoAcc, sNotes, lEcusno, sOtherCusno, sWWWUserID, sExpday,
                dDiscPercent, sTerms, sSocialSecNo, sCategory, lMasterCusno, companyId);
        }

        public static string GetWwwPassword(long cusNo)
        {
            return SubsHandler.GetWwwPassword(cusNo);
        }

        public static string GetWwwUserId(long cusno)
        {
            return SubsHandler.GetWwwUserId(cusno);
        }

        public static DataSet GetCustomer(long cusNo)
        {
            return SubsHandler.GetCustomer(cusNo);
        }

        public static Person GetCustomerInfo(long cusNo)
        {
            return SubsHandler.GetCustomerInfo(cusNo);
        }

        public static string GetEmailAddress(long cusno)
        {
            return SubsHandler.GetEmailAddress(cusno);
        }

        public static int InsertCustomerProperty(long cusno, string propCode, string propValue)
        {
            return SubsHandler.InsertCustomerProperty(cusno, propCode, propValue);
        }

        public static DataSet GetSubscriptions(long cusNo, bool showPassiveIfNoActive)
        {
            return SubsHandler.GetSubscriptions(cusNo, showPassiveIfNoActive);
        }

        public static List<ApsisCustomer> CirixGetNewCusts(DateTime? takeFrom = null)
        {
            return SubsHandler.CirixGetNewCusts(takeFrom);
        }

        public static List<ApsisCustomer> CirixGetCustsManuallyFromList(IEnumerable<string> customerIds)
        {
            return SubsHandler.CirixGetCustsManuallyFromList(customerIds);
        }
        
        //public static List<long> GetUpdatedCusnosInDateInterval(DateTime dateMin, DateTime dateMax)
        //{
        //    return SubsHandler.GetUpdatedCusnosInDateInterval(dateMin, dateMax);
        //}

        public static void FlagCustsInLetter(List<ApsisCustomer> custs, string flag)
        {
            SubsHandler.FlagCustsInLetter(custs, flag);
        }

        //public static void FlagCustsInExpCustomer(HashSet<int> cusnos, string flag)
        //{
        //    SubsHandler.FlagCustsInExpCustomer(cusnos, flag);
        //}

        public static void UpdateEmailInCirix(int cusno, string email)
        {
            SubsHandler.UpdateCustomerEmail(cusno, email);
        }

        public static ApsisCustomer CirixGetCustomer(int customerId, int subsNo, bool isExtCustomer)
        {
            return SubsHandler.CirixGetCustomer(customerId, subsNo, isExtCustomer);
        }

        public static void UpdateLetterInCirix(ApsisCustomer c, int subsno)
        {
            SubsHandler.UpdateLetterInCirix(c, subsno);
        }

        public static bool CustomerIsActive(int cusno, string paperCode, string productNo)
        {
            return SubsHandler.CustomerIsActive(cusno, paperCode, productNo);
        }

        public static List<ApsisCustomer> GetUpdatedCustomers(DateTime startDate, DateTime endDate)
        {
            return SubsHandler.GetUpdatedCustomers(startDate, endDate);
        }
        
        public static List<long> GetCusnosByEmail(string email)
        {
            return SubsHandler.GetCusnosByEmail(email);
        }

        public static List<string> GetCustomerXtraFields(long cusno)
        {
            return SubsHandler.GetCustomerXtraFields(cusno);
        }

        public static List<DateTime> GetPublDays(string sPaperCode, string sProductNo, DateTime dteFirstDate,DateTime dteLastDate)
        {
            return SubsHandler.GetPublDays(sPaperCode, sProductNo, dteFirstDate, dteLastDate);
        }

        public static List<DateTime> GetProductsIssueDatesInInterval(string paperCode, string productno, DateTime dateMin, DateTime dateMax)
        {
            return SubsHandler.GetProductsIssueDatesInInterval(paperCode, productno, dateMin, dateMax);
        }

        public static DateTime GetIssueDate(string paperCode, string productno, DateTime date, EnumIssue.Issue issue)
        {
            return SubsHandler.GetIssueDate(paperCode, productno, date, issue);
        }

        public static DateTime GetNextIssueDateIncDiRules(DateTime wantedDate, string paperCode, string productNo)
        {
            return SubsHandler.GetNextIssueDateIncDiRules(wantedDate, paperCode, productNo);
        }

        public static DateTime GetClosesIssueDateInTheory(string paperCode, string productNo)
        {
            return SubsHandler.GetClosesIssueDateInTheory(paperCode, productNo);
        }

        public static DateTime GetNextIssuedate(string papercode, string productno, DateTime minDate)
        {
            return SubsHandler.GetNextIssuedate(papercode, productno, minDate);
        }

        public static string CreateRenewal_DI(long lSubscusno, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate,
            DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty, string sSalesno, long lPaycusno, string sPackageId, string sPaperCode, string sProductno,
            string sReceiveType, string sTargetGroup, string sPriceAtStart, string sOtherSubsno, string sOrderId, string sAutogiro, string sPriceGroup, string sInvMode)
        {
            var result = SubsHandler.CreateRenewal_DI(lSubscusno, lSubsno, iExtno, lPricelistno, lCampno, iSubslenMons, iSubslenDays, dateSubsStartdate,
                dateSubsEnddate, sSubskind, dblTotalPrice, dblItemPrice, iItemqty, sSalesno, lPaycusno, sPackageId, sPaperCode, sProductno,
                sReceiveType, sTargetGroup, sPriceAtStart, sOtherSubsno, sOrderId, sAutogiro, sPriceGroup, sInvMode);
            //Kayak: If operation is successful returns subscription and package subscription number, like "1234567;0" otherwise "FAILED"
            return (result.ToUpper().StartsWith("FAILED")) ? "FAILED" : "OK";
        }

        public static string DefinitiveAddressChange(long cusno, string street, string houseNo, string stairCase, string apartment, string careOf, string zip, DateTime startDate)
        {
            return SubsHandler.DefinitiveAddressChange(cusno, street, houseNo, stairCase, apartment, careOf, zip, startDate);
        }

        public static string DefinitiveAddressChangeRemove(long cusno, DateTime startDate)
        {
            return SubsHandler.DefinitiveAddressChangeRemove(cusno, startDate);
        }
        
        //TODO: Obsolete!
        public static List<StringPair> GetNumSubsForPayingCust(string customerRowText1SearchStr)
        {
            return SubsHandler.GetNumSubsForPayingCust(customerRowText1SearchStr);
        }

        //TODO: Obsolete!
        public static int GetNumSubsForPayingCustOnline(string customerRowText1SearchStr)
        {
            return SubsHandler.GetNumSubsForPayingCustOnline(customerRowText1SearchStr);
        }

        //TODO: Remove? As this was last used 2011-12-20 if look at last made page of template DiGoldFreePlusSubs.aspx that is the only place using this method!!!
        public static bool AddSubsIpadSummer(long cusno)
        {
            return SubsHandler.AddSubsIpadSummer(cusno);
        }

        // TODO: Obsolete - Use AddCustAndSubHandler.TryAddCustAndSub() instead?
        //public static string TryInsertSubscription2(Subscription sub) //, DiPlusSubscription plusSub
        //{
        //    return SubsHandler.TryInsertSubscription2(sub);
        //}

        public static List<Person> FindCustomerByPerson(Person searchCriterias, bool includeEmailAsSearchCriteria)
        {
            return SubsHandler.FindCustomerByPerson(searchCriterias, includeEmailAsSearchCriteria);
        }

        public static long TryAddCustomer2(Subscription sub, bool isSubscriber)
        {
            return SubsHandler.TryAddCustomer2(sub, isSubscriber);
        }

        public static string AddNewSubs2(Subscription sub, long cusNoPay, DiPlusSubscription plusSub)
        {
            return SubsHandler.AddNewSubs2(sub, cusNoPay, plusSub);
        }

        public static List<Subscription> GetSubscriptions2(long cusno)
        {
            return SubsHandler.GetSubscriptions2(cusno);
        }

        public static void ChangeCusInvMode(long cusno, string newInvoiceMode, string oldInvoiceMode)
        {
            SubsHandler.ChangeCusInvMode(cusno, newInvoiceMode, oldInvoiceMode);
        }

        public static string TryGetDefaultCusInvMode(long cusno)
        {
            return SubsHandler.TryGetDefaultCusInvMode(cusno);
        }

        public static void AddNewCusInvmode(long cusno, string invMode, bool isInvDefault)
        {
            SubsHandler.AddNewCusInvmode(cusno, invMode, isInvDefault);
        }

        public static long GetNextInvno()
        {
            return SubsHandler.GetNextInvno();
        }

        public static string BuildRefno2(long lInvno, string sInvType, string sPaperCode)
        {
            return SubsHandler.BuildRefno2(lInvno, sInvType, sPaperCode);
        }

        public static long CreateImmediateInvoice(Subscription subscription, int iExtno, int iItemno, long lInvno, string sRefno)
        {
            return SubsHandler.CreateImmediateInvoice(subscription, iExtno, iItemno, lInvno, sRefno);
        }

        public static string InsertElectronicPayment(long lCusno, long lInvno, string sRefno, double dAmount)
        {
            return SubsHandler.InsertElectronicPayment(lCusno, lInvno, sRefno, dAmount);
        }

        public static long CreateNewInvoice(Subscription subscription)
        {
            return SubsHandler.CreateNewInvoice(subscription);
        }

        public static DataSet GetOpenInvoices(long lCusno)
        {
            return SubsHandler.GetOpenInvoices(lCusno);
        }

        public static DataSet GetInvArgItems(long subsno, int extno)
        {
            return SubsHandler.GetInvArgItems(subsno, extno);
        }

        public static DataSet FindCustomers(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname)
        {
            return SubsHandler.FindCustomers(lCusno, lInvno, lSubsno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseNo, sStaircase, sApartment, sCountry, sZipcode, sUserid, sPostname);
        }
        
        public static DataSet GetCustomerProperties(long lCusno)
        {
            return SubsHandler.GetCustomerProperties(lCusno);
        }

        public static string CleanCustomerProperties(long lCusno)
        {
            return SubsHandler.CleanCustomerProperties(lCusno);
        }

        public static string UpdateCustomerExtraInfo(long lCusno, string sExtra1, string sExtra2, string sExtra3)
        {
            return SubsHandler.UpdateCustomerExtraInfo(lCusno, sExtra1, sExtra2, sExtra3);
        }

        public static DataSet GetSubsSleeps(long subscriptionNumber)
        {
            return SubsHandler.GetSubsSleeps(subscriptionNumber);
        }

        public static DataSet GetCurrentAndPendingAddressChanges(long cusno, long subsno)
        {
            return SubsHandler.GetCurrentAndPendingAddressChanges(cusno, subsno);
        }

        public static DataSet GetCusTempAddresses(long customerNumber, string sOnlyThisCountry, string sOnlyValid)
        {
            return SubsHandler.GetCusTempAddresses(customerNumber, sOnlyThisCountry, sOnlyValid);
        }

        public static long CreateSubssleep_CII(long lSubsno, DateTime dateSleepStartDate,
            DateTime dateSleepEndDate, string sSleepType, string sCreditType, string sAllowWebPaper, string sSleepReason,
            string sReceiveType, string sSleepLimit)
        {
            return SubsHandler.CreateSubssleep_CII(lSubsno, dateSleepStartDate, dateSleepEndDate, sSleepType, sCreditType, sAllowWebPaper, sSleepReason, sReceiveType, sSleepLimit);
        }

        public static string CreateHolidayStop(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sAllowWebpaper, string sCreditType)
        {
            return SubsHandler.CreateHolidayStop(lCusno, sCusName, lSubsno, dateSleepStartDate, dateSleepEndDate, sSleepType, sAllowWebpaper, sCreditType);
        }

        public static string DeleteHolidayStop(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate)
        {
            return SubsHandler.DeleteHolidayStop(lCusno, sCusName, lSubsno, dateSleepStartDate, dateSleepEndDate);
        }

        public static string TemporaryAddressChange(string sUserId, long lCusno, long lSubsno, int iExtno, AddressMap addressMap, DateTime dAddrStartDate, DateTime dAddrEndDate, string sNewName1, string sNewName2, string sInvToTempAddress)
        {
            return SubsHandler.TemporaryAddressChange(sUserId, lCusno, lSubsno, iExtno, addressMap, dAddrStartDate, dAddrEndDate, sNewName1, sNewName2, sInvToTempAddress);
        }

		//public static string TemporaryAddressChangeNewAddress(long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, System.DateTime dateStartdate, System.DateTime dateEnddate, string sInvToTempAddress)
		//{
		//	return SubsHandler.TemporaryAddressChangeNewAddress(lCusno, lSubsno, sStreetname, sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, sNewName1, sNewName2, dateStartdate, dateEnddate, sInvToTempAddress);
		//}

        public static string TemporaryAddressChangeRemove(string sUserId, long lCusno, long lSubsno, int iExtno, int iAddrno, DateTime dOriginalStartDate)
        {
            return SubsHandler.TemporaryAddressChangeRemove(sUserId, lCusno, lSubsno, iExtno, iAddrno, dOriginalStartDate);
        }

        public static long IdentifyCustomer(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserId)
        {
            return SubsHandler.IdentifyCustomer(lCusno, lSubsno, lInvno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseno, sStaircase, sApartment, sCountry, sZipcode, sPostname, sUserId);
        }

        public static string UpdateWwwPassword(long lCusno, string sNewPassword1, string sNewPassword2, bool bCheckOldPassword, string sOldPassword)
        {
            return SubsHandler.UpdateWwwPassword(lCusno, sNewPassword1, sNewPassword2, bCheckOldPassword, sOldPassword);
        }

        public static bool IsHybridSubscriber(long cusno)
        {
            return SubsHandler.IsHybridSubscriber(cusno);
        }

        public static bool IsHybridCampaign(long campNo)
        {
            return SubsHandler.IsHybridCampaign(campNo);
        }

        public static string TryAddUserToBonDig(Subscription sub, string digitalUsername, string digitalPassword, bool addSubToExistingDiAccount)
        {
            var contact = " Var god kontakta kundtjäst.";

            // Return: 
            // 1=new passwd saved to S+, 
            // 2=existing passwd not changed in S+, 
            // -1=S+ productId not found, 
            // -2=S+ username does not exist, 
            // -3=S+ username not available, 
            // -4=save user to S+ failed, 
            // -5=S+ userId missing, 
            // -6=failed to create S+ import
            var ret = BonDigHandler.TryAddCustAndSubToBonDig(sub.PaperCode, sub.ProductNo, sub.Subscriber.Cusno, sub.SubsNo, digitalUsername.Trim(), sub.Subscriber.FirstName,
                sub.Subscriber.LastName, sub.Subscriber.MobilePhone, digitalPassword.Trim(), addSubToExistingDiAccount, IsHybridCampaign(sub.CampNo));

            if (ret == -1)
                return "Tekniskt fel: efterfrågad produkt hittades inte." + contact;

            if (ret == -2)
                return "Angivet användarnamn hittades tyvärr inte. Var god kontrollera stavningen och försök igen ELLER klicka på länken " +
                       "'Jag har inget Di-konto' för att skapa ett nytt konto.";

            if (ret == -3)
                return "Angivet användarnamn är tyvärr upptaget. Var god försök med ett annat användarnamn ELLER koppla prenumerationen till angivet " +
                       "användarnamn genom att klicka på länken 'Jag har redan ett Di-konto'.";

            if (ret == -4)
                return "Tekniskt fel: kunduppgifterna kunde inte sparas." + contact;

            if (ret == -5)
                return "Tekniskt fel: giltigt användar-ID saknas." + contact;

            if (ret == -6)
                return "Tekniskt fel: Prenumerationen kunde inte sparas på kundbilden." + contact;

            return string.Empty;
        }

        /// <summary>
        /// Prevent activation of campaign offer within 6 months of last one (if this is a campaign)
        /// </summary>
        public static bool DenyShortSub(Subscription sub, out string errorMessage)
        {
            var trialPriceGroup = MiscFunctions.GetAppsettingsValue("PriceGroupTrialPeriod");
            var trialPriceGroupFree = Settings.PriceGroupTrialFree;

            if (sub.Pricegroup == trialPriceGroup || sub.Pricegroup == trialPriceGroupFree)
            {
                var subscriberMatches = FindCustomerByPerson(sub.Subscriber, false);
                var activeSubscriberMatches = GetCustsWithActiveSubs(subscriberMatches);
                var nonActiveSubscriberMatches = GetNonActiveSubsCusts(subscriberMatches, activeSubscriberMatches);
                var mostRecentSubscriberMatch = GetMostRecentUsedCust(activeSubscriberMatches, nonActiveSubscriberMatches);
                if (CustHadTrialPeriodLast6Months(mostRecentSubscriberMatch, sub.Pricegroup))
                {
                    errorMessage = Subscriptions.AddCustAndSub.Message.ErrMess_DenyShortSub; //Settings.ErrMess_DenyShortSub;
                    MiscFunctions.SendStaffMailFailedSaveSubs(errorMessage, sub, null);
                    return true;
                }
            }
            errorMessage = string.Empty;
            return false;
        }

        public static List<PackageProduct> GetPackageProducts(string packageId)
        {
            return SubsHandler.GetPackageProducts(packageId);
        }

        public static double GetProductVat(string paperCode, string productNo)
        {
            return (paperCode == Settings.PaperCode_DI && productNo != Settings.ProductNo_IPAD)
                ? Settings.VatPaper
                : Settings.VatIpad;
        }

        private static Person GetMostRecentUsedCust(List<Person> active, List<Person> nonActive)
        {
            //try get latest cusno for active cust
            var p = GetPersonWithLatestActiveSubs(active);
            if (p != null)
                return p;

            //try get latest cusno for non active cust
            p = GetPersonWithLatestActiveSubs(nonActive);
            if (p != null)
                return p;

            return null;
        }

        private static Person GetPersonWithLatestActiveSubs(List<Person> custs)
        {
            Person ret = null;
            int i = 0;
            foreach (Person p in custs)
            {
                i++;
                if (i == 1)
                {
                    ret = p;
                    continue;
                }

                if (p.SubsHistory.Count > 0)
                {
                    DateTime tmp = ret.SubsHistory.Count > 0 ? ret.SubsHistory[0].SubsRealEndDate : DateTime.MinValue;
                    if (p.SubsHistory[0].SubsRealEndDate > tmp)
                        ret = p;
                }
            }

            return ret;
        }

        private static bool CustHadTrialPeriodLast6Months(Person p, string trialPriceGroup)
        {
            if (p != null)
            {
                if (p.SubsHistory.Count == 0)
                    return false;

                //Get all subscriptions with end date within the last 3 months
                var dtLimit = DateTime.Now.AddMonths(-3);
                var subs = p.SubsHistory.Where(x => x.SubsEndDate > dtLimit).ToList();

                foreach (Subscription sub in subs)
                {
                    if(sub.Pricegroup == trialPriceGroup)
                        return true;
                }
            }
            return false;
        }

        private static List<Person> GetCustsWithActiveSubs(List<Person> custs)
        {
            var ret = new HashSet<Person>();
            foreach (var p in from p in custs from sub in p.SubsHistory where Settings.SubsStateActiveValues.Contains(sub.SubsState) select p)
            {
                ret.Add(p);
            }
            return ret.ToList();
        }

        private static List<Person> GetNonActiveSubsCusts(List<Person> allCusts, List<Person> activeCusts)
        {
            var ret = new HashSet<Person>();
            foreach (var p in allCusts.Where(p => !(activeCusts.Any(p2 => p.Cusno == p2.Cusno))))
            {
                ret.Add(p);
            }
            return ret.ToList();
        }
    }
}