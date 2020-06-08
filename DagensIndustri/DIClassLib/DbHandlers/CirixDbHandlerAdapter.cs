using System;
using System.Collections.Generic;
using System.Data;
using DIClassLib.Campaign;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;

namespace DIClassLib.DbHandlers
{
    // This adapter created as CirixDbhandler is static class and therefore can't implement interface ISubscriptionHandler
    public class CirixDbHandlerAdapter : ISubscriptionHandler
    {
        private const string CirixUserId = "WEBCIRIX";

        public HashSet<string> GetTargetGroups(string paperCode)
        {
            return CirixDbHandler.GetTargetGroups(paperCode);
        }

        public List<StringPair> GetAllTargetGroups()
        {
            return CirixDbHandler.GetAllTargetGroups();
        }

        public DataSet GetCampaign(long campNo)
        {
            return CirixDbHandler.GetCampaign(campNo);
        }

        public DataSet GetCampaign(string campId)
        {
            return CirixDbHandler.GetCampaign(campId);
        }

        public long GetCampno(string campId)
        {
            return CirixDbHandler.GetCampno(campId);
        }

        public string GetProductName(string paperCode, string productNo)
        {
            return CirixDbHandler.GetProductName(paperCode, productNo);
        }
        
        public List<Campaign.CampaignInfo> GetActiveFreeCampaigns(string paperCode, string productNo)
        {
            return CirixDbHandler.GetActiveFreeCampaigns(paperCode, productNo);
        }

        public string GetCommuneCode(string zipCode)
        {
            return CirixDbHandler.GetCommuneCode(zipCode);
        }

        public long GetPriceListNo(string paperCode, string productNo, System.DateTime invStartDate, string communeCode, string priceGr, string campId)
        {
            return CirixDbHandler.GetPriceListNo(paperCode, productNo, invStartDate, communeCode, priceGr, campId);
        }
        
        public string UpdateCustomerInformation(long lCusNo, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sSalesDen, string sOfferdenDir, string sOfferdenSal, string sOfferdenEmail, string sDenySmsMark, string sAccnoBank, string sAccnoAcc, string sNotes, long lEcusno, string sOtherCusno, string sWwwUserId, string sExpday, double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string companyId)
        {
            return CirixDbHandler.UpdateCustomerInformation(lCusNo, sEmailAddress, sHPhone, sWPhone, sOPhone, sSalesDen, sOfferdenDir, sOfferdenSal, sOfferdenEmail, sDenySmsMark, sAccnoBank, sAccnoAcc, sNotes, lEcusno, sOtherCusno, sWwwUserId, sExpday, dDiscPercent, sTerms, sSocialSecNo, sCategory, lMasterCusno, companyId);
        }

        public string GetWwwPassword(long cusNo)
        {
            return CirixDbHandler.GetWWWPassword(cusNo);
        }

        public string GetWwwUserId(long cusno)
        {
            return CirixDbHandler.GetWWWUserId(cusno);
        }

        public DataSet GetCustomer(long cusNo)
        {
            return CirixDbHandler.GetCustomer(cusNo);
        }

        public Subscriptions.Person GetCustomerInfo(long cusNo)
        {
            return CirixDbHandler.GetCustomerInfo(cusNo);
        }

        public string GetEmailAddress(long cusno)
        {
            return CirixDbHandler.GetEmailAddress(cusno);
        }
        
        public int InsertCustomerProperty(long cusno, string propCode, string propValue)
        {
            return CirixDbHandler.InsertCustomerProperty(cusno, propCode, propValue);
        }

        public DataSet GetSubscriptions(long cusNo, bool showPassiveIfNoActive)
        {
            return CirixDbHandler.GetSubscriptions(cusNo, showPassiveIfNoActive, CirixUserId);
        }

        public List<EPiJobs.Apsis.ApsisCustomer> CirixGetNewCusts()
        {
            return CirixDbHandler.CirixGetNewCusts(CirixUserId);
        }

        public List<EPiJobs.Apsis.ApsisCustomer> CirixGetCustsManuallyFromList(IEnumerable<string> customerIds)
        {
            return CirixDbHandler.CirixGetCustsManuallyFromList(customerIds, CirixUserId);
        }
        
        public List<long> GetUpdatedCusnosInDateInterval(System.DateTime dateMin, System.DateTime dateMax)
        {
            return CirixDbHandler.GetUpdatedCusnosInDateInterval(dateMin, dateMax);
        }

        public void FlagCustsInLetter(List<EPiJobs.Apsis.ApsisCustomer> custs, string flag)
        {
            CirixDbHandler.FlagCustsInLetter(custs, flag);
        }

        public void FlagCustsInExpCustomer(HashSet<int> cusnos, string flag)
        {
            CirixDbHandler.FlagCustsInExpCustomer(cusnos, flag);
        }

        public void UpdateCustomerEmail(int cusno, string email)
        {
            CirixDbHandler.UpdateEmailInCirix(cusno, email);
        }

        public EPiJobs.Apsis.ApsisCustomer CirixGetCustomer(int customerId, int subsNo, bool isExtCustomer)
        {
            return CirixDbHandler.CirixGetCustomer(customerId, subsNo, isExtCustomer);
        }

        public void UpdateLetterInCirix(EPiJobs.Apsis.ApsisCustomer c, int subsno)
        {
            CirixDbHandler.UpdateLetterInCirix(c, subsno);
        }

        public bool CustomerIsActive(int cusno, string paperCode, string productNo)
        {
            return CirixDbHandler.CustomerIsActive(cusno, paperCode, productNo);
        }

        public List<EPiJobs.Apsis.ApsisCustomer> GetUpdatedCustomers()
        {
            return CirixDbHandler.GetUpdatedCustomers();
        }
        
        public List<long> GetCusnosByEmail(string email)
        {
            return CirixDbHandler.GetCusnosByEmail(email);
        }
        
        public List<string> GetCustomerXtraFields(long cusno)
        {
            return CirixDbHandler.GetCustomerXtraFields(cusno);
        }

        public List<DateTime> GetPublDays(string sPaperCode, string sProductNo, DateTime dteFirstDate, DateTime dteLastDate)
        {
            throw new NotImplementedException();
        }

        public List<DateTime> GetProductsIssueDatesInInterval(string paperCode, string productno, DateTime dateMin, DateTime dateMax)
        {
            return CirixDbHandler.GetProductsIssueDatesInInterval(paperCode, productno, dateMin, dateMax);
        }

        public DateTime GetIssueDate(string paperCode, string productno, DateTime date, EnumIssue.Issue issue)
        {
            return CirixDbHandler.GetIssueDate(paperCode, productno, date, issue);
        }

        public DateTime GetNextIssueDateIncDiRules(DateTime wantedDate, string paperCode, string productNo)
        {
            return CirixDbHandler.GetNextIssueDateIncDiRules(wantedDate, paperCode, productNo);
        }

        public DateTime GetClosesIssueDateInTheory(string paperCode, string productNo)
        {
            return CirixDbHandler.GetClosesIssueDateInTheory(paperCode, productNo);
        }

        public DateTime GetNextIssuedate(string papercode, string productno, DateTime minDate)
        {
            return CirixDbHandler.GetNextIssuedate(papercode, productno, minDate);
        }

        public string CreateRenewal_DI(long lSubscusno, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays, System.DateTime dateSubsStartdate, System.DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty, string sSalesno, long lPaycusno, string sPackageId, string sPaperCode, string sProductno, string sReceiveType, string sTargetGroup, string sPriceAtStart, string sOtherSubsno, string sOrderID, string sAutogiro, string sPriceGroup, string sInvMode)
        {
            return CirixDbHandler.CreateRenewal_DI(CirixUserId, lSubsno, iExtno, lPricelistno, lCampno, iSubslenMons, iSubslenDays, dateSubsStartdate, dateSubsEnddate, sSubskind, dblTotalPrice, dblItemPrice, iItemqty, sSalesno, lPaycusno, sProductno, sReceiveType, sTargetGroup, sPriceAtStart, sOtherSubsno, sOrderID, sAutogiro, sPriceGroup, sInvMode);
        }

        public string DefinitiveAddressChange(long cusno, string street, string houseNo, string stairCase, string apartment, string careOf, string zip, System.DateTime startDate)
        {
            return CirixDbHandler.DefinitiveAddressChange(cusno, street, houseNo, stairCase, apartment, careOf, zip, startDate);
        }

        public string DefinitiveAddressChangeRemove(long cusno, DateTime startDate)
        {
            return CirixDbHandler.DefinitiveAddressChangeRemove(cusno, startDate);
        }

        public List<StringPair> GetNumSubsForPayingCust(string customerRowText1SearchStr)
        {
            return CirixDbHandler.GetNumSubsForPayingCust(customerRowText1SearchStr);
        }

        public int GetNumSubsForPayingCustOnline(string customerRowText1SearchStr)
        {
            return CirixDbHandler.GetNumSubsForPayingCustOnline(customerRowText1SearchStr);
        }

        public bool AddSubsIpadSummer(long cusno)
        {
            return CirixDbHandler.AddSubsIpadSummer(cusno, CirixUserId);
        }
        
        public List<Person> FindCustomerByPerson(Person searchCriterias, bool includeEmailAsSearchCriteria)
        {
            return CirixDbHandler.FindCustomerByPerson(searchCriterias, includeEmailAsSearchCriteria);
        }

        public long TryAddCustomer2(Subscription sub, bool isSubscriber)
        {
            return CirixDbHandler.TryAddCustomer2(sub, isSubscriber, CirixUserId);
        }

        public string AddNewSubs2(Subscription sub, long cusNoPay, Subscriptions.DiPlus.DiPlusSubscription plusSub)
        {
            return CirixDbHandler.AddNewSubs2(sub, cusNoPay, plusSub, CirixUserId);
        }

        public List<Subscription> GetSubscriptions2(long cusno)
        {
            return CirixDbHandler.GetSubscriptions2(cusno, CirixUserId);
        }

        public void ChangeCusInvMode(long cusno, string newInvoiceMode, string oldInvoiceMode)
        {
            CirixDbHandler.ChangeCusInvMode(cusno, newInvoiceMode, oldInvoiceMode);
        }

        public string TryGetDefaultCusInvMode(long cusno)
        {
            return CirixDbHandler.TryGetDefaultCusInvMode(cusno);
        }

        public void AddNewCusInvmode(long cusno, string invMode, bool isInvDefault)
        {
            CirixDbHandler.AddNewCusInvmode(cusno, invMode, isInvDefault);
        }

        public long GetNextInvno()
        {
            return CirixDbHandler.GetNextInvno();
        }

        public string BuildRefno2(long lInvno, string sInvType, string sPaperCode)
        {
            return CirixDbHandler.BuildRefno2(lInvno, sInvType, sPaperCode);
        }

        public long CreateImmediateInvoice(Subscription subscription, int iExtno, int iItemno, long lInvno, string sRefno)
        {
            return CirixDbHandler.CreateImmediateInvoice(subscription, iExtno, iItemno, lInvno, sRefno);
        }

        public string InsertElectronicPayment(long lCusno, long lInvno, string sRefno, double dAmount)
        {
            return CirixDbHandler.InsertElectronicPayment(lCusno, lInvno, sRefno, dAmount);
        }

        public void CreateNewInvoice(Subscription subscription)
        {
            CirixDbHandler.CreateNewInvoice(subscription);
        }
        
        public DataSet GetOpenInvoices(long lCusno)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetOpenInvoices_(lCusno, CirixUserId);
        }


        public DataSet GetInvArgItems(long subsno, int extno)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetInvArgItems_(subsno, extno);
        }


        public DataSet FindCustomers(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.FindCustomers_(lCusno, lInvno, lSubsno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseNo, sStaircase, sApartment, sCountry, sZipcode, sUserid, sPostname);
        }


        public DataSet GetCampaignCoProducts(string sCampid)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetCampaignCoProducts_(sCampid);
        }


        public string CreateExtraExpenseItem(long lSubsCusno, long lPayCusno, long lSubsno, long lExtno, string sPapercode, string sProductno, string sExpCode, double dblUnits, System.DateTime dateStartdate, System.DateTime dateEnddate, string sSource, double dDiscountPercentage)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.CreateExtraExpenseItem_(lSubsCusno, lPayCusno, lSubsno, lExtno, sPapercode, sProductno, sExpCode, dblUnits, dateStartdate, dateEnddate, sSource, dDiscountPercentage);
        }


        public DataSet GetCustomerProperties(long lCusno)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetCustomerProperties_(lCusno);
        }


        public string CleanCustomerProperties(long lCusno)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.CleanCustomerProperties_(lCusno);
        }


        public string UpdateCustomerExtraInfo(long lCusno, string sExtra1, string sExtra2, string sExtra3)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.UpdateCustomerExtraInfo_(lCusno, sExtra1, sExtra2, sExtra3);
        }


        public DataSet GetSubsSleeps(long subscriptionNumber)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetSubsSleeps_(subscriptionNumber);
        }


        public DataSet GetCurrentAndPendingAddressChanges(long cusno, long subsno)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetCurrentAndPendingAddressChanges_(cusno, subsno);
        }


        public DataSet GetCusTempAddresses(long customerNumber, string sOnlyThisCountry, string sOnlyValid)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.GetCusTempAddresses_(customerNumber, sOnlyThisCountry, sOnlyValid);
        }


        public string CreateHolidayStop(long lCusno, string sCusName, long lSubsno, System.DateTime dateSleepStartDate, System.DateTime dateSleepEndDate, string sSleepType, string sAllowWebpaper, string sCreditType)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.CreateHolidayStop_(lCusno, sCusName, lSubsno, dateSleepStartDate, dateSleepEndDate, sSleepType, sAllowWebpaper, sCreditType);
        }


        public string DeleteHolidayStop(long lCusno, string sCusName, long lSubsno, System.DateTime dateSleepStartDate, System.DateTime dateSleepEndDate)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.DeleteHolidayStop_(lCusno, sCusName, lSubsno, dateSleepStartDate, dateSleepEndDate);
        }


        public string TemporaryAddressChange(string sUserId, long lCusno, long lSubsno, AddressMap addressMap, System.DateTime dAddrStartDate, System.DateTime dAddrEndDate, string sNewName1, string sNewName2, string sInvToTempAddress)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.TemporaryAddressChange_(sUserId, lCusno, lSubsno, addressMap.Addrno, dAddrStartDate, dAddrEndDate, sNewName1, sNewName2, sInvToTempAddress);
        }


        public string TemporaryAddressChangeNewAddress(long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, System.DateTime dateStartdate, System.DateTime dateEnddate, string sInvToTempAddress)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.TemporaryAddressChangeNewAddress_(lCusno, lSubsno, sStreetname, sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, sNewName1, sNewName2, dateStartdate, dateEnddate, sInvToTempAddress);
        }


        public string TemporaryAddressChangeRemove(string sUserId, long lCusno, long lSubsno, int iAddrno, System.DateTime dOriginalStartDate)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.TemporaryAddressChangeRemove_(sUserId, lCusno, lSubsno, iAddrno, dOriginalStartDate);
        }


        public long IdentifyCustomer(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserId)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.IdentifyCustomer_(lCusno, lSubsno, lInvno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseno, sStaircase, sApartment, sCountry, sZipcode, sPostname, sUserId);
        }


        public string UpdateWwwPassword(long lCusno, string sNewPassword1, string sNewPassword2, bool bCheckOldPassword, string sOldPassword)
        {
            return CirixDbHandler.CirixWebServiceForAdapter.UpdateWWWPassword_(lCusno, sNewPassword1, sNewPassword2, bCheckOldPassword.ToString().ToUpper(), sOldPassword);
        }

        public bool IsHybridSubscriber(long cusno)
        {
            return CirixDbHandler.IsHybridSubscriber(cusno, CirixUserId);
        }

        public bool IsHybridCampaign(long campNo)
        {
            return CirixDbHandler.IsHybridCampaign(campNo);
        }

        public IEnumerable<DataSet> GetActiveCampaigns()
        {
            throw new NotImplementedException();
        }

        public string GetPostName(string zipCode, string countryCode = "SE")
        {
            throw new NotImplementedException();
        }

        public string TestOracleConnection()
        {
            return string.Empty;
        }

        public long CreateSubssleep_CII(long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sCreditType, string sAllowWebPaper, string sSleepReason, string sReceiveType, string sSleepLimit)
        {
            throw new NotImplementedException();
        }

        public List<Kayak.PackageProduct> GetPackageProducts(string packageId)
        {
            throw new NotImplementedException();
        }

        public long GetCustomerByEcusno(long ecusno)
        {
            throw new NotImplementedException();
        }

        public long GetEcusnoByCustomer(long cusno)
        {
            throw new NotImplementedException();
        }

    }
}