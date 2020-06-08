using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DIClassLib.DbHandlers.CirixWrappers
{
    
    class CirixTestWrapper : CirixTest.CirixWebServiceSoapClient, ICirix     //CirixTest.CirixWebServiceSoapClient, ICirix
    {
        
        //public DataSet GetSubsChoices2_(string sPapercode, DateTime dateTargetDate, string sPageid)
        //{
        //    return this.GetSubsChoices2(sPapercode, dateTargetDate, sPageid);
        //}
        
        
        public DataSet GetCampaign_(long lCampno)
        {
            return this.GetCampaign(lCampno);
        }


        public DataSet GetActiveCampaigns_(string sPapercode, DateTime dStartDate, string sProductno)
        { 
            return this.GetActiveCampaigns(sPapercode, dStartDate, sProductno);
        }


        public string InsertCustomerProperty_DI_(long lCusno, string sPropcode, string sValue)
        {
            return InsertCustomerProperty_DI(lCusno, sPropcode, sValue);
        }


        public string CreateNewSubs_DI_(string sUserID, long lSubscusno, long lPaycusno, string sSubskind, string sPapercode, string sPricegroup,
                                string sProductno, string sSubstype, string sAutogiro, long lPricelistno, long lCampno, int iSubslenMons,
                                int iSubslenDays, DateTime dateSubsStartdate, DateTime dateSubsEnddate, double dblTotalPrice, double dblGrossPrice,
                                double dblItemPrice, int iItemqty, string sSalesno, string sTargetGroup, string sReceiveType, double dblDiscAmount,
                                string sPriceAtStart, string sOtherSubsno, string sOrderID, string sInvMode)
        { 
            return CreateNewSubs_DI(sUserID, lSubscusno, lPaycusno, sSubskind, sPapercode, sPricegroup,
                                sProductno, sSubstype, sAutogiro, lPricelistno, lCampno, iSubslenMons,
                                iSubslenDays, dateSubsStartdate, dateSubsEnddate, dblTotalPrice, dblGrossPrice,
                                dblItemPrice, iItemqty, sSalesno, sTargetGroup, sReceiveType, dblDiscAmount,
                                sPriceAtStart, sOtherSubsno, sOrderID, sInvMode);
        }


        public string UpdateCustomerInformation_(long lCusno, string sEmailAddress, string sH_Phone, string sW_Phone, string sO_Phone, string sSalesDen,
                                        string sOfferden_dir, string sOfferden_sal, string sOfferden_email, string sDenysmsmark, string sAccno_bank,
                                        string sAccno_acc, string sNotes, long lEcusno, string sOther_cusno, string sWWWUserID, string sExpday,
                                        double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string sCompanyId)
        {
            return UpdateCustomerInformation(lCusno, sEmailAddress, sH_Phone, sW_Phone, sO_Phone, sSalesDen,
                                                sOfferden_dir, sOfferden_sal, sOfferden_email, sDenysmsmark, sAccno_bank,
                                                sAccno_acc, sNotes, lEcusno, sOther_cusno, sWWWUserID, sExpday,
                                                dDiscPercent, sTerms, sSocialSecNo, sCategory, lMasterCusno, sCompanyId);
        }

        

        public string GetWWWPassword_(long lCusno)
        {
            return GetWWWPassword(lCusno);
        }


        public DataSet GetSubscriptions_(long lCusno, string sShowPassiveIfNoActive, string sUserid)
        {
            return GetSubscriptions(lCusno, sShowPassiveIfNoActive, sUserid);
        }


        public string GetCommuneCode_(string sZipcode, string sCountry)
        {
            return GetCommuneCode(sZipcode, sCountry);
        }


        public long GetPricelistno_(string sPaperCode, string sProductno, DateTime dateInvstartDate, string sCommuneCode, string sPriceGroup, string sCampID)
        {
            return GetPricelistno(sPaperCode, sProductno, dateInvstartDate, sCommuneCode, sPriceGroup, sCampID);
        }


        public DataSet GetCustomer_(long lCusno)
        {
            return GetCustomer(lCusno);
        }


        public DataSet FindCustomers_(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sEmail,
                       string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname)
        {
            return FindCustomers(lCusno, lInvno, lSubsno, sName1, sName2, sName3, sPhone, sEmail,
                                 sStreet, sHouseNo, sStaircase, sApartment, sCountry, sZipcode, sUserid, sPostname);
        }


        public long CreateNewCustomer_(string sUserID, string sName1, string sName2, string sName3, string sFirstName, string sLastName, string sStreetname,
                                string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode,
                                string sZipcode, string sHomePhone, string sWorkPhone, string sOtherPhone, string sEmailAddress, string sOfferden_Dir,
                                string sOfferden_Sal, string sOfferden_Email, string sSalesDen, string sOfferden_SMS, string sCurrency, string sInvMode,
                                string sAccnoBank, string sAccnoAcc, string sCollectInv, string sCusType, string sWWWUserID, string sWWWPinCode, string sNotes,
                                string sExpday, double dDiscPercent, string sTerms, string sSocialSecNo, string sExtra1, string sExtra2, string sExtra3,
                                string sCategory, long lMasterCusno, string sGenUidAndPin, string sOtherCusno, string sCompanyId)
        {
            return CreateNewCustomer(sUserID, sName1, sName2, sName3, sFirstName, sLastName, sStreetname, 
                                sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, 
                                sZipcode, sHomePhone, sWorkPhone, sOtherPhone, sEmailAddress, sOfferden_Dir, 
                                sOfferden_Sal, sOfferden_Email, sSalesDen, sOfferden_SMS, sCurrency, sInvMode, 
                                sAccnoBank, sAccnoAcc, sCollectInv, sCusType, sWWWUserID, sWWWPinCode, sNotes, 
                                sExpday, dDiscPercent, sTerms, sSocialSecNo, sExtra1, sExtra2, sExtra3, 
                                sCategory, lMasterCusno, sGenUidAndPin, sOtherCusno, sCompanyId);
        }

        
        public DataSet GetCustomerProperties_(long lCusno)
        {
            return GetCustomerProperties(lCusno);
        }

        
        public DataSet GetCampaignCoProducts_(string sCampid)
        {
            return GetCampaignCoProducts(sCampid);
        }

        
        public string CreateExtraExpenseItem_(long lSubsCusno, long lPayCusno, long lSubsno, long lExtno, string sPapercode, string sProductno,
                                        string sExpCode, double dblUnits, DateTime dateStartdate, DateTime dateEnddate, string sSource,
                                        double dDiscountPercentage)
        {
            return CreateExtraExpenseItem(lSubsCusno, lPayCusno, lSubsno, lExtno, sPapercode, sProductno,
                                            sExpCode, dblUnits, dateStartdate, dateEnddate, sSource, dDiscountPercentage);
        }

        
        public string CleanCustomerProperties_(long lCusno)
        {
            return CleanCustomerProperties(lCusno);
        }


        public DataSet GetParameterValuesByGroup_(string sPapercode, string sCodegroupno)
        { 
            return GetParameterValuesByGroup(sPapercode, sCodegroupno);
        }


        public string TemporaryAddressChangeRemove_(string sUserID, long lCusno, long lSubsno, int iAddrno, DateTime dOriginalStartDate)
        { 
            return TemporaryAddressChangeRemove(sUserID, lCusno, lSubsno, iAddrno, dOriginalStartDate);
        }

        public string EndAvailableTemporaryAddress_(long lCusno, int iAddrno, System.DateTime dOriginalStartDate)
        {
            return EndAvailableTemporaryAddress(lCusno, iAddrno, dOriginalStartDate);
        }

        public DataSet GetSubsSleeps_(long SubscriptionNumber)
        {
            return GetSubsSleeps(SubscriptionNumber);
        }

        public DataSet GetPendingAddressChanges_(long CustomerNumber, long SubscriptionNumber)
        { 
            return GetPendingAddressChanges(CustomerNumber, SubscriptionNumber);       
        }

        public DataSet GetCusTempAddresses_(long CustomerNumber, string sOnlyThisCountry, string sOnlyValid)
        { 
            return GetCusTempAddresses(CustomerNumber, sOnlyThisCountry, sOnlyValid);
        }

        public string UpdateWWWPassword_(long lCusno, string sNewPassword1, string sNewPassword2, string sCheckOldPassword, string sOldPassword)
        { 
            return UpdateWWWPassword(lCusno, sNewPassword1, sNewPassword2, sCheckOldPassword, sOldPassword);
        }

        public string CreateHolidayStop_(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sALLOW_WEBPAPER, string sCreditType)
        {
            return CreateHolidayStop(lCusno, sCusName, lSubsno, dateSleepStartDate, dateSleepEndDate, sSleepType, sALLOW_WEBPAPER, sCreditType);
        }

        public string DeleteHolidayStop_(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate)
        {
            return DeleteHolidayStop(lCusno, sCusName, lSubsno, dateSleepStartDate, dateSleepEndDate);
        }

        public string TemporaryAddressChange_(string sUserID, long lCusno, long lSubsno, int iAddrno, DateTime dAddrStartDate, DateTime dAddrEndDate, string sNewName1, string sNewName2, string sInvToTempAddress)
        {
            return TemporaryAddressChange(sUserID, lCusno, lSubsno, iAddrno, dAddrStartDate, dAddrEndDate, sNewName1, sNewName2, sInvToTempAddress);
        }

        public string TemporaryAddressChangeNewAddress_(long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, DateTime dateStartdate, DateTime dateEnddate, string sInvToTempAddress)
        {
            return TemporaryAddressChangeNewAddress(lCusno, lSubsno, sStreetname, sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, sNewName1, sNewName2, dateStartdate, dateEnddate, sInvToTempAddress);
        }

        public string DefinitiveAddressChange_(long lCusno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, DateTime dateStartdate)
        {
            return DefinitiveAddressChange(lCusno, sStreetname, sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, dateStartdate);
        }

        public string DefinitiveAddressChangeRemove_(long lCusno, DateTime dateStartdate)
        {
            return DefinitiveAddressChangeRemove(lCusno, dateStartdate);
        }

        public long IdentifyCustomer_(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserID)
        {
            return IdentifyCustomer(lCusno, lSubsno, lInvno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseno, sStaircase, sApartment, sCountry, sZipcode, sPostname, sUserID);
        }

        public DataSet GetOpenInvoices_(long lCusno, string sUserid)
        {
            return GetOpenInvoices(lCusno, sUserid);
        }

        public string UpdateCustomerExtraInfo_(long lCusno, string sExtra1, string sExtra2, string sExtra3)
        {
            return UpdateCustomerExtraInfo(lCusno, sExtra1, sExtra2, sExtra3);
        }

        public DataSet GetCusInvModes_(long lCusno)
        {
            return GetCusInvModes(lCusno);
        }

        public string ChangeCusInvMode_(long lCusno, string sOriginalInvMode, string sOriginalInvDefault, string sNewInvMode, string sNewInvDefault, string sInfo, string sInfo2, string sInfo3)
        {
            return ChangeCusInvMode(lCusno, sOriginalInvMode, sOriginalInvDefault, sNewInvMode, sNewInvDefault, sInfo, sInfo2, sInfo3);
        }

        public string AddNewCusInvmode_(long lCusno, string sInvmode, string sInvDefault, string sInfo, string sInfo2, string sInfo3)
        {
            return AddNewCusInvMode(lCusno, sInvmode, sInvDefault, sInfo, sInfo2, sInfo3);
        }

        public DateTime GetNextIssueDate_(string sPaperCode, string sProductNo, DateTime dteMinDate)
        { 
            return GetNextIssuedate(sPaperCode, sProductNo, dteMinDate);
        }

        public string ChangeSubscriptionEnddate_(long lSubsno, int iExtno, DateTime dteNewEnddate)
        {
            return ChangeSubscriptionEnddate(lSubsno, iExtno, dteNewEnddate);
        }

        public string CancelSubscription_(string sUserID, long lSubsno, int iExtno, string sCancelReason)
        {
            return CancelSubscription(sUserID, lSubsno, iExtno, sCancelReason);
        }

        public DateTime GetNextIssuedate_(string sPapercode, string sProductno, DateTime dteMinDate)
        {
            return GetNextIssuedate(sPapercode, sProductno, dteMinDate);
        }

        public string CreateRenewal_DI_(string sUserID, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate,
                                        DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty, string sSalesno, long lPaycusno, 
                                        string sProductno, string sReceiveType, string sTargetGroup, string sPriceAtStart, double dblGrossPrice, double dblDiscAmount, string sOtherSubsno, 
                                        string sOrderID, string sAutogiro, string sPriceGroup, string sInvMode)
        {
            return CreateRenewal_DI(sUserID, lSubsno, iExtno, lPricelistno, lCampno, iSubslenMons, iSubslenDays, dateSubsStartdate,
                                    dateSubsEnddate, sSubskind, dblTotalPrice, dblItemPrice, iItemqty, sSalesno, lPaycusno, sProductno,
                                    sReceiveType, sTargetGroup, sPriceAtStart, dblGrossPrice, dblDiscAmount, sOtherSubsno, sOrderID, sAutogiro, sPriceGroup, sInvMode);
            //(sPriceAtStart), dblGrossPrice, dblDiscAmount, (sOtherSubsno)
        }

        public long GetNextInvno_()
        {
            return GetNextInvno();
        }

        public string BuildRefno2_(long lInvno, string sInvType, string sPaperCode)
        { 
            return BuildRefno2(lInvno, sInvType, sPaperCode);
        }

        public long CreateImmediateInvoice_(long lSubsno, int iExtno, int iItemno, long lInvno, string sRefno)
        { 
            return CreateImmediateInvoice(lSubsno, iExtno, iItemno, lInvno, sRefno);
        }

        public string InsertElectronicPayment_(long lCusno, long lInvno, string sRefno, double dAmount)
        { 
            return InsertElectronicPayment(lCusno, lInvno, sRefno, dAmount);
        }

        public DataSet GetCurrentAndPendingAddressChanges_(long lCusno, long lSubsno)
        {
            return GetCurrentAndPendingAddressChanges(lCusno, lSubsno);
        }

        public DataSet GetInvArgItems_(long lSubsno, int iExtno)
        {
            return GetInvArgItem(lSubsno, iExtno);
        }

        public string TemporaryAddressChangePeriod_(string sUserID, long lCusno, long lSubsno, int iAddrno, DateTime dOriginalStartDate, DateTime dNewStartDate, DateTime dNewEndDate)
        {
            return TemporaryAddressChangePeriod(sUserID, lCusno, lSubsno, iAddrno, dOriginalStartDate, dNewStartDate, dNewEndDate);
        }
    }
}
