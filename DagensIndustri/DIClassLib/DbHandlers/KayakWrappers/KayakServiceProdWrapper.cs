using System;
using System.Data;

using DIClassLib.Misc;

namespace DIClassLib.DbHandlers.KayakWrappers
{
    public class KayakServiceProdWrapper : KayakProd.KayakWebServiceSoapClient, IKayak
    {
        public DataSet GetParameterValuesByGroup_(string sPapercode, string sCodegroupno)
        {
            return this.GetParameterValuesByGroup(sPapercode, sCodegroupno);
        }

        public DataSet GetCampaign_(long lCampno)
        {
            return this.GetCampaign(lCampno);
        }

        public DataSet GetActiveCampaigns_CII_(string sPackageId)
        {
            return this.GetActiveCampaigns_CII(sPackageId);
        }

        public DataSet GetChangedCustomerSubs_CII_(DateTime dteStartdate, DateTime dteEnddate, long lNoOfChanges, string sPapercodes)
        {
            return this.GetChangedCustomerSubs_CII(dteStartdate, dteEnddate, lNoOfChanges, sPapercodes);
        }

        public string GetCommuneCode_(string sZipcode, string sCountry)
        {
            return this.GetCommuneCode(sZipcode, sCountry);
        }

        public int GetPricelistno_CII_(string sPackageId, DateTime datePricestartDate, string sCommuneCode, string sCountryCode, string sZipcode, string sPriceGroup, string sCampNo)
        {
            return this.GetPricelistno_CII(sPackageId, datePricestartDate, sCommuneCode, sCountryCode, sZipcode, sPriceGroup, sCampNo);
        }

        public string CreateNewCustomer_CII_(string sUserId, string sName1, string sName2, string sName3, string sFirstName, string sLastName, string sStreetname,
            string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sHomePhone, string sWorkPhone,
            string sOtherPhone, string sEmailAddress, string sCurrency, string sAccnoBank, string sAccnoAcc, bool bCollectInv, string sCusType, string sWwwUserId, string sWwwPinCode,
            string sNotes, string sExpday, string sTerms, string sSocialSecNo, string sExtra1, string sExtra2, string sExtra3, string sCategory, long lMasterCusno, string sOtherCusno,
            bool bMarketingDnl, string sMarketingDnlRsn, DateTime dMarketingDnlDate, string sCompanyId, string sTitle, bool bProtectedIdentity)
        {
            var sFloor = string.Empty;

            return this.CreateNewCustomer_CII(sUserId, sName1, sName2, sName3, sFirstName, sLastName, sStreetname,
                sHouseno, sStaircase, sFloor, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, sHomePhone, sWorkPhone,
                sOtherPhone, sEmailAddress, sCurrency, sAccnoBank, sAccnoAcc, bCollectInv, sCusType, sWwwUserId, sWwwPinCode,
                sNotes, sExpday, sTerms, sSocialSecNo, sExtra1, sExtra2, sExtra3, sCategory, lMasterCusno, sOtherCusno,
                bMarketingDnl, sMarketingDnlRsn, dMarketingDnlDate, sCompanyId, sTitle, bProtectedIdentity);
        }

        public string UpdateCustomerInformation_CII_(long lCusno, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sAccnoBank, string sAccnoAcc, bool bCollectInv,
            string sNotes, long lEcusno, string sOtherCusno, string sWwwUserId, string sExpday, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno,
            string sMarketingDnl, string sMarketingDnlRsn, DateTime dMarketingDnlDate, string sCustype, string sTitle, bool bProtectedIdentity, string sCompanyId, string sExtra1,
            string sExtra2, string sExtra3)
        {
            return this.UpdateCustomerInformation_CII(lCusno, sEmailAddress, sHPhone, sWPhone, sOPhone, sAccnoBank, sAccnoAcc, bCollectInv,
                sNotes, lEcusno, sOtherCusno, sWwwUserId, sExpday, sTerms, sSocialSecNo, sCategory, lMasterCusno,
                sMarketingDnl, sMarketingDnlRsn, dMarketingDnlDate, sCustype, sTitle, bProtectedIdentity, sCompanyId, sExtra1,
                sExtra2, sExtra3);
        }

        public string GetWwwPassword_CII_(long lCusno)
        {
            return this.GetWWWPassword_CII(lCusno);
        }

        public DataSet GetCustomer_CII_(long lCusno)
        {
            return this.GetCustomer_CII(lCusno);
        }

        public string CreateNewSubs_CII_(string sUserId, long lSubscusno, long lPaycusno, string sSubskind, string sPackageId, string sPricegroup, string sSubstype, long lCampno,
            int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate, DateTime dateInvStartdate, DateTime dateSubsEnddate, int iItemqty, string sSalesno, string sTargetGroup,
            string sReceiveType, double dblDiscAmount, double dblDiscountPercent, string sInvMode, string sOrderId, bool bGiftSubs, long lRecomCusno, string sCriter1, string sCriter2,
            string sCriter3, string sCriter4, string sCriter5, string sCriter6, string sFreesubsgrp, string sGranterno, string sDeptno, string sUnitno, string sExtraMode)
        {
            var sCampaignChnl = string.Empty;
            var sEditions = string.Empty;

            return this.CreateNewSubs_CII(sUserId, lSubscusno, lPaycusno, sSubskind, sPackageId, sPricegroup, sSubstype, lCampno,
                iSubslenMons, iSubslenDays, dateSubsStartdate, dateInvStartdate, dateSubsEnddate, iItemqty, sSalesno, sTargetGroup,
                sCampaignChnl, sReceiveType, dblDiscAmount, dblDiscountPercent, sInvMode, sOrderId, bGiftSubs, lRecomCusno, sCriter1, sCriter2,
                sCriter3, sCriter4, sCriter5, sCriter6, sFreesubsgrp, sGranterno, sDeptno, sUnitno, sExtraMode, sEditions);
        }

        public long CreateCustomerAndSubsWithoutPayment_CII_(string sUserId, long lCusno, string sFindcustomer, string sUpdatecustomer, string sOrderId, string sFirstName, string sLastName,
            string sRowtext2, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sCountrycode, string sZipcode, string sCityname, string sHomePhone,
            string sWorkPhone, string sEmailAddress, string sSocialSecNo, string sCompanyId, string sCustype, DateTime dateSubsStartdate, DateTime dateSubsEnddate, string sInvmode,
            string sInvmodeInfo1, string sInvmodeInfo2, string sInvmodeInfo3, string sSubskind, string sPricegroup, int iItemqty, double dblTotalprice, string sPackageId,
            long lCampno, string sReceiveType, string sPassword, string sSubsnotes, string sCancelreason, bool bSendemail, bool bCheckactivesubs)
        {
            var floor = string.Empty;
            var street2 = string.Empty;
            var campChnl = string.Empty;
            var targetGroup = string.Empty;

            return this.CreateCustomerAndSubsWithoutPayment_CII(sUserId, lCusno, sFindcustomer, sUpdatecustomer, sOrderId, sFirstName, sLastName,
                sRowtext2, sStreetname, street2, sHouseno, sStaircase, floor, sApartment, sCountrycode, sZipcode, sCityname, sHomePhone,
                sWorkPhone, sEmailAddress, sSocialSecNo, sCompanyId, sCustype, dateSubsStartdate, dateSubsEnddate, sInvmode,
                sInvmodeInfo1, sInvmodeInfo2, sInvmodeInfo3, sSubskind, sPricegroup, iItemqty, dblTotalprice, sPackageId,
                lCampno, campChnl, sReceiveType, sPassword, sSubsnotes, targetGroup, sCancelreason, bSendemail, bCheckactivesubs);
        }

        public string InsertCustomerProperty_CII_(long lCusno, string sPropcode, string sValue, string sProfileSourceGroup, string sProfileSourceSpecification)
        {
            return this.InsertCustomerProperty_CII(lCusno, sPropcode, sValue, sProfileSourceGroup, sProfileSourceSpecification);
        }

        // TODO: Not sure if we instead should use GetSubscriptions_CII(), but I can't get it to work
        public DataSet GetSubscriptions_(long lCusno, bool sShowPassiveIfNoActive, string sUserid, string sPaperCode)
        {
            return this.GetSubscriptions(lCusno, sShowPassiveIfNoActive.ToString(), sUserid, sPaperCode);
        }

        public string GetNextIssuedate_CII_(string sPapercode, string sProductNo, DateTime dteMinDate)
        {
            return this.GetNextIssuedate_CII(sPapercode, sProductNo, dteMinDate);
        }

        public string CreateRenewal_CII_(string sUserId, long lSubscusno, long lPaycusno, long lOrigSubsNo, int iOrigExtNo, string sSubskind, string sPackageId, string sPricegroup,
            string sSubstype, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate, DateTime dateInvStartdate, DateTime dateSubsEnddate,
            int iItemqty, string sSalesno, DateTime dateSalesdate, string sTargetGroup, string sReceiveType, string sInvMode, string sOrderId, bool bGiftSubs,
            long lRecomCusno, string sCriter1, string sCriter2, string sCriter3, string sCriter4, string sCriter5, string sCriter6, DateTime dateExpdate, string sFreesubsgrp, string sGranterno,
            string sDeptno, string sUnitno, string sExtraMode)
        {
            var sCampaignChnl = string.Empty;
            var sEditions = string.Empty;

            return this.CreateRenewal_CII(sUserId, lSubscusno, lPaycusno, lOrigSubsNo, iOrigExtNo, sSubskind, sPackageId, sPricegroup, sSubstype, lCampno, iSubslenMons, iSubslenDays, dateSubsStartdate, dateInvStartdate, dateSubsEnddate, iItemqty, sSalesno, dateSalesdate, sTargetGroup, sCampaignChnl, sReceiveType, sInvMode, sOrderId, bGiftSubs, lRecomCusno, sCriter1, sCriter2, sCriter3, sCriter4, sCriter5, sCriter6, dateExpdate, sFreesubsgrp, sGranterno, sDeptno, sUnitno, sExtraMode, sEditions);
        }

        public DataSet FindCustomers_CII_(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sSocialsecno, string sEmail,
            string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname, string sOtherCusno,
            string sOrderId, string sPaperCode, string sReferenceno)
        {
            var sFloor = string.Empty;

            return this.FindCustomers_CII(lCusno, lInvno, lSubsno, sName1, sName2, sName3, sPhone, sSocialsecno, sEmail,
                sStreet, sHouseNo, sStaircase, sFloor, sApartment, sCountry, sZipcode, sUserid, sPostname, sOtherCusno,
                sOrderId, sPaperCode, sReferenceno);
        }

        public DataSet GetCampaignCoProducts_(string sCampid)
        {
            return this.GetCampaignCoProducts(sCampid);
        }

        public string CreateExtraExpenseItem_CII_(string sUserId, long lSubsCusno, long lPayCusno, string sPaperCode, string sProductcode, double dblProductPrice, DateTime dteExpStartdate,
            DateTime dteExpEnddate, string sExpenseCategory, double dUnits, bool bSeparately, string sFreeText1, string sFreeText2, string sFreeText3, string sFreeText4,
            long lSendToCusno, DateTime dteExpDate, bool bStPrinted, DateTime dtePrinted, bool bExistingItem, long lSubsno, int iExtno, int iExpitemno,
            long lInvno, double dOpenAmount, double dDiscountPercent)
        {
            return this.CreateExtraExpenseItem_CII(sUserId, lSubsCusno, lPayCusno, sPaperCode, sProductcode, dblProductPrice, dteExpStartdate, dteExpEnddate, sExpenseCategory, dUnits,
                bSeparately, sFreeText1, sFreeText2, sFreeText3, sFreeText4, lSendToCusno, dteExpDate, bStPrinted, dtePrinted, bExistingItem, lSubsno, iExtno, iExpitemno, lInvno, dOpenAmount, dDiscountPercent);
        }

        public string ChangeCusInvMode_CII_(long lCusno, string sOldInvmode, string sOldPapercode, string sNewInvmode, bool bNewInvDefault, string sNewInfo, string sNewInfo2, string sNewInfo3, string sNewPapercode, bool bNewSendInvoice)
        {
            var oldExtraMode = string.Empty;
            var newExtraMode = string.Empty;

            return this.ChangeCusInvMode_CII(lCusno, sOldInvmode, sOldPapercode, sNewInvmode, bNewInvDefault, sNewInfo, sNewInfo2, sNewInfo3, sNewPapercode, bNewSendInvoice, oldExtraMode, newExtraMode);
        }

        public DataSet GetCusInvModes_CII_(long lCusno)
        {
            return this.GetCusInvModes_CII(lCusno);
        }

        public string AddNewCusInvMode_CII_(long lCusno, string sInvmode, bool bInvDefault, string sInfo, string sInfo2, string sInfo3, string sPapercode, bool bSendInvoice, string sExtraMode)
        {
            return this.AddNewCusInvMode_CII(lCusno, sInvmode, bInvDefault, sInfo, sInfo2, sInfo3, sPapercode, bSendInvoice, sExtraMode);
        }

        public long GetNextInvno_()
        {
            return this.GetNextInvno();
        }

        public string BuildRefno2_(long lInvno, string sInvType, string sPaperCode)
        {
            return this.BuildRefno2(lInvno, sInvType, sPaperCode);
        }

        public string CreateImmediateInvoice_CII_(long lSubsno, int iExtno, int iInvArgItemno, int iInvExpItemno, string sPackageId, int iPackageSubsno, string sRefno, long lInvno, double dInvoiceFee)
        {
            return this.CreateImmediateInvoice_CII(lSubsno, iExtno, iInvArgItemno, iInvExpItemno, sPackageId, iPackageSubsno, sRefno, lInvno, dInvoiceFee);
        }

        public string CreatePaymentOnInvoice_CII_(long lInvno, DateTime dtePayDate, DateTime dteEntryDate, DateTime dteBookingDate, string sReceiptno, string sCashierno, string sOfficeno, double dCashAmount, string sSource)
        {
            return this.CreatePaymentOnInvoice_CII(lInvno, dtePayDate, dteEntryDate, dteBookingDate, sReceiptno, sCashierno, sOfficeno, dCashAmount, sSource);
        }

        public DataSet GetOpenInvoices_(long lCusno, string sUserid)
        {
            return this.GetOpenInvoices(lCusno, sUserid);
        }

        public DataSet GetInvArgItems_(long subsno, int extno)
        {
            return this.GetInvArgItem(subsno, extno);
        }

        public DataSet GetCustomerProperties_CII_(long lCusno)
        {
            return this.GetCustomerProperties_CII(lCusno);
        }

        public string CleanCustomerProperties_(long lCusno)
        {
            return this.CleanCustomerProperties(lCusno);
        }

        public string UpdateCustomerExtraInfo_(long lCusno, string sExtra1, string sExtra2, string sExtra3)
        {
            return this.UpdateCustomerExtraInfo(lCusno, sExtra1, sExtra2, sExtra3);
        }

        public DataSet GetSubsSleeps_CII_(string sUserId, long lSubsno, int iExtno)
        {
            return this.GetSubsSleeps_CII(sUserId, lSubsno, iExtno);
        }

        public DataSet GetCurrentAndPendingAddressChanges_(long cusno, long subsno)
        {
            return this.GetCurrentAndPendingAddressChanges(cusno, subsno);
        }

        public DataSet GetCusTempAddresses_(long customerNumber, string sOnlyThisCountry, string sOnlyValid)
        {
            return this.GetCusTempAddresses(customerNumber, sOnlyThisCountry, sOnlyValid);
        }

        public long CreateSubssleep_CII_(string sUserId, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sCreditType, string sAllowWebPaper, string sSleepReason, string sReceiveType, string sSleepLimit)
        {
            return this.CreateSubssleep_CII(sUserId, lSubsno, dateSleepStartDate, dateSleepEndDate, sSleepType, sCreditType, sAllowWebPaper, sSleepReason, sReceiveType, sSleepLimit);
        }

        public string CreateHolidayStop_CII_(string sUserId, long lSubsno, int iExtno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepTYpe, string sCreditType, string sAllowWebPaper, bool bRenewSubs, string sSleepReason, string sReceiveType, string sSleepLimit)
        {
            return this.CreateHolidayStop_CII(sUserId, lSubsno, iExtno, dateSleepStartDate, dateSleepEndDate, sSleepTYpe, sCreditType, sAllowWebPaper, bRenewSubs, sSleepReason, sReceiveType, sSleepLimit);
        }

        public string DeleteHolidayStop_CII_(string sUserId, long lSubsno, int iExtno, DateTime dateSleepStartDate)
        {
            return this.DeleteHolidayStop_CII(sUserId, lSubsno, iExtno, dateSleepStartDate);
        }

        public string AddNewTemporaryAddress_CII_(string sUserId, long lCusno, long lSubsno, int iExtno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2,
            string sStreet3, string sCountrycode, string sZipcode, DateTime dteStartdate, DateTime dteEnddate, string sInvToTempAddress, string sName1,
            string sName2, string sPaperCode, string sReceiveType, bool bSaveAllPackageSubs)
        {
            var sFloor = string.Empty;

            return this.AddNewTemporaryAddress_CII(sUserId, lCusno, lSubsno, iExtno, sStreetname, sHouseno, sStaircase, sFloor, sApartment, sStreet2,
                sStreet3, sCountrycode, sZipcode, dteStartdate, dteEnddate, sInvToTempAddress, sName1, sName2, sPaperCode, sReceiveType, bSaveAllPackageSubs);
        }

        public string TemporaryAddressChangeNewAddress_(string sUserId, long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, DateTime dateStartdate, DateTime dateEnddate, string sInvToTempAddress)
        {
            return AddNewTemporaryAddress_CII_(sUserId, lCusno, lSubsno, 0, sStreetname, sHouseno, sStaircase, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, dateStartdate, dateEnddate, sInvToTempAddress, sNewName1, sNewName2, Settings.PaperCode_DI, Settings.sReceiveType, false);
        }

        public string RemoveWaitingTemporaryAddress_CII_(string sUserId, long lCusno, long lSubsno, int iExtno, DateTime dteStartDate)
        {
            return this.RemoveWaitingTemporaryAddress_CII(sUserId, lCusno, lSubsno, iExtno, dteStartDate);
        }

        public long IdentifyCustomer_(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserId)
        {
            return this.IdentifyCustomer(lCusno, lSubsno, lInvno, sName1, sName2, sName3, sPhone, sEmail, sStreet, sHouseno, sStaircase, sApartment, sCountry, sZipcode, sPostname, sUserId);
        }

        public string UpdateWwwPassword_CII_(long lCusno, string sNewPassword1, string sNewPassword2, bool bCheckOldPassword, string sOldPassword)
        {
            return this.UpdateWWWPassword_CII(lCusno, sNewPassword1, sNewPassword2, bCheckOldPassword, sOldPassword);
        }

        public DataSet GetPapersAndProducts_CII_(string sUserid)
        {
            return this.GetPapersAndProducts_CII(sUserid);
        }

        public string UpdateCustomerEmail_CII_(long lCusno, string sEmailAddress)
        {
            return this.UpdateCustomerEmail_CII(lCusno, sEmailAddress);
        }

        public DataSet GetCampaignSubs_(long lCampno)
        {
            return this.GetCampaignSubs(lCampno);
        }

        public DataSet GetCusByEmail_(string sEmail)
        {
            return this.GetCusByEmail(sEmail);
        }

        public string GetParameterValue_(string sPapercode, string sCodegroupno, string sCodeno)
        {
            return this.GetParameterValue(sPapercode, sCodegroupno, sCodeno);
        }

        public string StoreCustomerPaperInformation_(long lCusno, string sPapercode, string sDirectMarkAllowed, string sSalesManMarkAllowed, string sEmailMarkAllowed, string sSmsMarkAllowed,
            string sCusMarkAllowed, DateTime dteDirectMarkChangeDate, string sDirectMarkChangeSource, DateTime dteSalesmanMarkChangeDate, string sSalesmanMarkChangeSource,
            DateTime dteEmailMarkChangeDate, string sEmailMarkChangeSource, DateTime dteSmsMarkChangeDate, string sSmsMarkChangeSource, DateTime dteCustomerSalesMarkChangeDate,
            string sCustomerMarkChangeSource)
        {
            return this.StoreCustomerPaperInformation(lCusno, sPapercode, sDirectMarkAllowed, sSalesManMarkAllowed, sEmailMarkAllowed, sSmsMarkAllowed, sCusMarkAllowed,
                dteDirectMarkChangeDate, sDirectMarkChangeSource, dteSalesmanMarkChangeDate, sSalesmanMarkChangeSource, dteEmailMarkChangeDate, sEmailMarkChangeSource,
                dteSmsMarkChangeDate, sSmsMarkChangeSource, dteCustomerSalesMarkChangeDate, sCustomerMarkChangeSource);
        }

        public DataSet GetSubsForConfirm_CII_(string sPackageList, string sSubskindList, DateTime dteSalesdate, bool bEmailonly)
        {
            return this.GetSubsForConfirm_CII(sPackageList, sSubskindList, dteSalesdate, bEmailonly);
        }

        public string SubsIsConfirmed_CII_(string sUserId, string sSubsList)
        {
            return this.SubsIsConfirmed_CII(sUserId, sSubsList);
        }

        public string AddNewBasicAddress_CII_(string sUserId, long lCusno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3,
            string sCountrycode, string sZipcode, DateTime dteStartdate, string sTempname1, string sTempname2, string sReceiveType, bool bChangeImmediately)
        {
            var sFloor = string.Empty;

            return this.AddNewBasicAddress_CII(sUserId, lCusno, sStreetname, sHouseno, sStaircase, sFloor, sApartment, sStreet2, sStreet3, sCountrycode, sZipcode, dteStartdate, sTempname1,
                sTempname2, sReceiveType, bChangeImmediately);
        }

        public string RemoveWaitingBasicAddress_CII_(string sUserId, long lCusno, DateTime dteStartDate)
        {
            return this.RemoveWaitingBasicAddress_CII(sUserId, lCusno, dteStartDate);
        }

        public DataSet GetCampaignByCampOrGroupId_(string sCampId, string sGroupId)
        {
            return this.GetCampaignByCampOrGroupID(sCampId, sGroupId, DateTime.Now);
        }

        public DataSet GetPublDays_(string sPaperCode, string sProductNo, DateTime dteFirstDate, DateTime dteLastDate)
        {
            return this.GetPublDays(sPaperCode, sProductNo, dteFirstDate, dteLastDate);
        }

        public string TestOracleConnection_()
        {
            return this.TestOracleConnection();
        }



    }
}