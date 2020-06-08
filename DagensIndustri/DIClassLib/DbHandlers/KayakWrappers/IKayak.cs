using System;
using System.Data;

namespace DIClassLib.DbHandlers.KayakWrappers
{
    public interface IKayak
    {
        string TestOracleConnection_();

        DataSet GetPublDays_(string sPaperCode, string sProductNo, DateTime dteFirstDate, DateTime dteLastDate);

        DataSet GetParameterValuesByGroup_(string sPapercode, string sCodegroupno);

        DataSet GetCampaign_(long lCampno);

        DataSet GetCampaignByCampOrGroupId_(string sCampId, string sGroupId);

        DataSet GetCampaignSubs_(long lCampno);

        DataSet GetActiveCampaigns_CII_(string sPackageId); // sPackageId = PAPERCODE_PRODUCTNO

        string GetCommuneCode_(string sZipcode, string sCountry);

        int GetPricelistno_CII_(string sPackageId, DateTime datePricestartDate, string sCommuneCode, string sCountryCode, string sZipcode, string sPriceGroup, string sCampNo);

        string CreateNewCustomer_CII_(
            string sUserId,
            string sName1,
            string sName2,
            string sName3,
            string sFirstName,
            string sLastName,
            string sStreetname,
            string sHouseno,
            string sStaircase,
            string sApartment,
            string sStreet2,
            string sStreet3,
            string sCountrycode,
            string sZipcode,
            string sHomePhone,
            string sWorkPhone,
            string sOtherPhone,
            string sEmailAddress,
            string sCurrency,
            string sAccnoBank,
            string sAccnoAcc,
            bool bCollectInv,
            string sCusType,
            string sWwwUserId,
            string sWwwPinCode,
            string sNotes,
            string sExpday,
            string sTerms,
            string sSocialSecNo,
            string sExtra1,
            string sExtra2,
            string sExtra3,
            string sCategory,
            long lMasterCusno,
            string sOtherCusno,
            bool bMarketingDnl,
            string sMarketingDnlRsn,
            DateTime dMarketingDnlDate,
            string sCompanyId,
            string sTitle,
            bool bProtectedIdentity);

        string UpdateCustomerInformation_CII_(long lCusno, string sEmailAddress, string sHPhone, string sWPhone, string sOPhone, string sAccnoBank, string sAccnoAcc, bool bCollectInv,
            string sNotes, long lEcusno, string sOtherCusno, string sWwwUserId, string sExpday, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno,
            string sMarketingDnl, string sMarketingDnlRsn, DateTime dMarketingDnlDate, string sCustype, string sTitle, bool bProtectedIdentity, string sCompanyId, string sExtra1,
            string sExtra2, string sExtra3);

        string UpdateCustomerEmail_CII_(long lCusno, string sEmailAddress);

        string GetWwwPassword_CII_(long lCusno);

        DataSet GetChangedCustomerSubs_CII_(DateTime dteStartdate, DateTime dteEnddate, long lNoOfChanges, string sPapercodes);

        DataSet GetCustomer_CII_(long lCusno);

        DataSet GetCusByEmail_(string sEmail);

        string CreateNewSubs_CII_(
            string sUserId,
            long lSubscusno,
            long lPaycusno,
            string sSubskind,
            string sPackageId,
            string sPricegroup,
            string sSubstype,
            long lCampno,
            int iSubslenMons,
            int iSubslenDays,
            DateTime dateSubsStartdate,
            DateTime dateInvStartdate,
            DateTime dateSubsEnddate,
            int iItemqty,
            string sSalesno,
            string sTargetGroup,
            string sReceiveType,
            double dblDiscAmount,
            double dblDiscountPercent,
            string sInvMode,
            string sOrderId,
            bool bGiftSubs,
            long lRecomCusno,
            string sCriter1,
            string sCriter2,
            string sCriter3,
            string sCriter4,
            string sCriter5,
            string sCriter6,
            string sFreesubsgrp,
            string sGranterno,
            string sDeptno,
            string sUnitno,
            string sExtraMode);

        long CreateCustomerAndSubsWithoutPayment_CII_(string sUserId, long lCusno, string sFindcustomer, string sUpdatecustomer, string sOrderId, string sFirstName, string sLastName,
            string sRowtext2, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sCountrycode, string sZipcode, string sCityname, string sHomePhone,
            string sWorkPhone, string sEmailAddress, string sSocialSecNo, string sCompanyId, string sCustype, DateTime dateSubsStartdate, DateTime dateSubsEnddate, string sInvmode,
            string sInvmodeInfo1, string sInvmodeInfo2, string sInvmodeInfo3, string sSubskind, string sPricegroup, int iItemqty, double dblTotalprice, string sPackageId,
            long lCampno, string sReceiveType, string sPassword, string sSubsnotes, string sCancelreason, bool bSendemail, bool bCheckactivesubs);
        
        string InsertCustomerProperty_CII_(long lCusno, string sPropcode, string sValue, string sProfileSourceGroup, string sProfileSourceSpecification);

        DataSet GetSubscriptions_(long lCusno, bool sShowPassiveIfNoActive, string sUserid, string sPaperCode);

        string GetNextIssuedate_CII_(string sPapercode, string sProductNo, System.DateTime dteMinDate);

        string CreateRenewal_CII_(string sUserId, long lSubscusno, long lPaycusno, long lOrigSubsNo, int iOrigExtNo, string sSubskind, string sPackageId, string sPricegroup,
            string sSubstype, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate, DateTime dateInvStartdate, DateTime dateSubsEnddate,
            int iItemqty, string sSalesno, DateTime dateSalesdate, string sTargetGroup, string sReceiveType, string sInvMode, string sOrderId, bool bGiftSubs,
            long lRecomCusno, string sCriter1, string sCriter2, string sCriter3, string sCriter4, string sCriter5, string sCriter6, DateTime dateExpdate, string sFreesubsgrp, string sGranterno,
            string sDeptno, string sUnitno, string sExtraMode);

        string AddNewBasicAddress_CII_(string sUserId, long lCusno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, System.DateTime dteStartdate, string sTempname1, string sTempname2, string sReceiveType, bool bChangeImmediately);

        string RemoveWaitingBasicAddress_CII_(string sUserId, long lCusno, DateTime dteStartDate);

        DataSet FindCustomers_CII_(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sSocialsecno, string sEmail,
            string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname, string sOtherCusno,
            string sOrderId, string sPaperCode, string sReferenceno);

        DataSet GetCampaignCoProducts_(string sCampid);

        string CreateExtraExpenseItem_CII_(string sUserId, long lSubsCusno, long lPayCusno, string sPaperCode, string sProductcode, double dblProductPrice, DateTime dteExpStartdate,
            DateTime dteExpEnddate, string sExpenseCategory, double dUnits, bool bSeparately, string sFreeText1, string sFreeText2, string sFreeText3, string sFreeText4,
            long lSendToCusno, DateTime dteExpDate, bool bStPrinted, DateTime dtePrinted, bool bExistingItem, long lSubsno, int iExtno,
            int iExpitemno, long lInvno, double dOpenAmount, double dDiscountPercent);

        string ChangeCusInvMode_CII_(long lCusno, string sOldInvmode, string sOldPapercode, string sNewInvmode, bool bNewInvDefault, string sNewInfo, string sNewInfo2, string sNewInfo3, string sNewPapercode, bool bNewSendInvoice);

        DataSet GetCusInvModes_CII_(long lCusno);

        string AddNewCusInvMode_CII_(long lCusno, string sInvmode, bool bInvDefault, string sInfo, string sInfo2, string sInfo3, string sPapercode, bool bSendInvoice, string sExtraMode);

        long GetNextInvno_();

        string BuildRefno2_(long lInvno, string sInvType, string sPaperCode);

        string CreateImmediateInvoice_CII_(long lSubsno, int iExtno, int iInvArgItemno, int iInvExpItemno, string sPackageId, int iPackageSubsno, string sRefno, long lInvno, double dInvoiceFee);

        string CreatePaymentOnInvoice_CII_(long lInvno, DateTime dtePayDate, DateTime dteEntryDate, DateTime dteBookingDate, string sReceiptno, string sCashierno, string sOfficeno, double dCashAmount, string sSource);

        DataSet GetOpenInvoices_(long lCusno, string sUserid);

        DataSet GetInvArgItems_(long subsno, int extno);

        DataSet GetCustomerProperties_CII_(long lCusno);

        string CleanCustomerProperties_(long lCusno);

        string UpdateCustomerExtraInfo_(long lCusno, string sExtra1, string sExtra2, string sExtra3);

        DataSet GetSubsSleeps_CII_(string sUserID, long lSubsno, int iExtno);

        DataSet GetCurrentAndPendingAddressChanges_(long cusno, long subsno);

        DataSet GetCusTempAddresses_(long customerNumber, string sOnlyThisCountry, string sOnlyValid);

        long CreateSubssleep_CII_(string sUserId, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sCreditType, string sAllowWebPaper, string sSleepReason, string sReceiveType, string sSleepLimit);

        string CreateHolidayStop_CII_(string sUserId, long lSubsno, int iExtno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepTYpe, string sCreditType, string sAllowWebPaper, bool bRenewSubs, string sSleepReason, string sReceiveType, string sSleepLimit);
        
        string DeleteHolidayStop_CII_(string sUserId, long lSubsno, int iExtno, DateTime dateSleepStartDate);

        string StoreCustomerPaperInformation_(long lCusno, string sPapercode, string sDirectMarkAllowed, string sSalesManMarkAllowed, string sEmailMarkAllowed, string sSmsMarkAllowed, string sCusMarkAllowed, DateTime dteDirectMarkChangeDate, string sDirectMarkChangeSource, DateTime dteSalesmanMarkChangeDate, string sSalesmanMarkChangeSource, DateTime dteEmailMarkChangeDate, string sEmailMarkChangeSource, DateTime dteSmsMarkChangeDate, string sSmsMarkChangeSource, DateTime dteCustomerSalesMarkChangeDate, string sCustomerMarkChangeSource);

        string AddNewTemporaryAddress_CII_(string sUserId, long lCusno, long lSubsno, int iExtno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2,
            string sStreet3, string sCountrycode, string sZipcode, DateTime dteStartdate, DateTime dteEnddate, string sInvToTempAddress, string sName1,
            string sName2, string sPaperCode, string sReceiveType, bool bSaveAllPackageSubs);

        string TemporaryAddressChangeNewAddress_(string sUserId, long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, DateTime dateStartdate, DateTime dateEnddate, string sInvToTempAddress);

        string RemoveWaitingTemporaryAddress_CII_(string sUserId, long lCusno, long lSubsno, int iExtno, DateTime dteStartDate);

        long IdentifyCustomer_(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserId);

        string UpdateWwwPassword_CII_(long lCusno, string sNewPassword1, string sNewPassword2, bool bCheckOldPassword, string sOldPassword);

        DataSet GetPapersAndProducts_CII_(string sUserid);

        string GetParameterValue_(string sPapercode, string sCodegroupno, string sCodeno);

        DataSet GetSubsForConfirm_CII_(string sPackageList, string sSubskindList, System.DateTime dteSalesdate, bool bEmailonly);

        string SubsIsConfirmed_CII_(string sUserId, string sSubsList);

        string GetPostName_CII(string sCountryCode, string sZipCode);

        long GetCustomerByEcusno_CII(long ecusno);

        long GetEcusnoByCustomer_CII(long cusno);
    }
}