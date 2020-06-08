using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using DIClassLib.EPiJobs.Apsis;
using System.Data.OracleClient;


namespace DIClassLib.DbHandlers.CirixWrappers
{
    
    public interface ICirix
    {
        
        //DataSet GetSubsChoices2_(string sPapercode, DateTime dateTargetDate, string sPageid);
        
        DataSet GetCampaign_(long lCampno);
        
        DataSet GetActiveCampaigns_(string sPapercode, DateTime dStartDate, string sProductno);
        
        string CreateNewSubs_DI_(string sUserID, long lSubscusno, long lPaycusno, string sSubskind, string sPapercode, string sPricegroup,
                                string sProductno, string sSubstype, string sAutogiro, long lPricelistno, long lCampno, int iSubslenMons,
                                int iSubslenDays, DateTime dateSubsStartdate, DateTime dateSubsEnddate, double dblTotalPrice, double dblGrossPrice,
                                double dblItemPrice, int iItemqty, string sSalesno, string sTargetGroup, string sReceiveType, double dblDiscAmount,
                                string sPriceAtStart, string sOtherSubsno, string sOrderID, string sInvMode);
       
        string InsertCustomerProperty_DI_(long lCusno, string sPropcode, string sValue);

        
        string UpdateCustomerInformation_(long lCusno, string sEmailAddress, string sH_Phone, string sW_Phone, string sO_Phone, string sSalesDen,
                                        string sOfferden_dir, string sOfferden_sal, string sOfferden_email, string sDenysmsmark, string sAccno_bank,
                                        string sAccno_acc, string sNotes, long lEcusno, string sOther_cusno, string sWWWUserID, string sExpday,
                                        double dDiscPercent, string sTerms, string sSocialSecNo, string sCategory, long lMasterCusno, string sCompanyId);

        
        string GetWWWPassword_(long lCusno);

        DataSet GetSubscriptions_(long lCusno, string sShowPassiveIfNoActive, string sUserid);

        string GetCommuneCode_(string sZipcode, string sCountry);

        long GetPricelistno_(string sPaperCode, string sProductno, DateTime dateInvstartDate, string sCommuneCode, string sPriceGroup, string sCampID);

        DataSet GetCustomer_(long lCusno);


        DataSet FindCustomers_(long lCusno, long lInvno, long lSubsno, string sName1, string sName2, string sName3, string sPhone, string sEmail,
                            string sStreet, string sHouseNo, string sStaircase, string sApartment, string sCountry, string sZipcode, string sUserid, string sPostname);


        long CreateNewCustomer_(string sUserID, string sName1, string sName2, string sName3, string sFirstName, string sLastName, string sStreetname,
                                string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode,
                                string sZipcode, string sHomePhone, string sWorkPhone, string sOtherPhone, string sEmailAddress, string sOfferden_Dir,
                                string sOfferden_Sal, string sOfferden_Email, string sSalesDen, string sOfferden_SMS, string sCurrency, string sInvMode,
                                string sAccnoBank, string sAccnoAcc, string sCollectInv, string sCusType, string sWWWUserID, string sWWWPinCode, string sNotes,
                                string sExpday, double dDiscPercent, string sTerms, string sSocialSecNo, string sExtra1, string sExtra2, string sExtra3,
                                string sCategory, long lMasterCusno, string sGenUidAndPin, string sOtherCusno, string sCompanyId);

        DataSet GetCustomerProperties_(long lCusno);

        DataSet GetCampaignCoProducts_(string sCampid);

        string CreateExtraExpenseItem_(long lSubsCusno, long lPayCusno, long lSubsno, long lExtno, string sPapercode, string sProductno,
                                        string sExpCode, double dblUnits, DateTime dateStartdate, DateTime dateEnddate, string sSource,
                                        double dDiscountPercentage);

        string CleanCustomerProperties_(long lCusno);

        DataSet GetParameterValuesByGroup_(string sPapercode, string sCodegroupno);



        DataSet GetSubsSleeps_(long SubscriptionNumber);

        DataSet GetPendingAddressChanges_(long CustomerNumber, long SubscriptionNumber);

        DataSet GetCusTempAddresses_(long CustomerNumber, string sOnlyThisCountry, string sOnlyValid);

        string UpdateWWWPassword_(long lCusno, string sNewPassword1, string sNewPassword2, string sCheckOldPassword, string sOldPassword);

        string CreateHolidayStop_(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate, string sSleepType, string sALLOW_WEBPAPER, string sCreditType);

        string DeleteHolidayStop_(long lCusno, string sCusName, long lSubsno, DateTime dateSleepStartDate, DateTime dateSleepEndDate);

        string TemporaryAddressChange_(string sUserID, long lCusno, long lSubsno, int iAddrno, DateTime dAddrStartDate, DateTime dAddrEndDate, string sNewName1, string sNewName2, string sInvToTempAddress);

        string TemporaryAddressChangeRemove_(string sUserID, long lCusno, long lSubsno, int iAddrno, DateTime dOriginalStartDate);

        string TemporaryAddressChangeNewAddress_(long lCusno, long lSubsno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, string sNewName1, string sNewName2, DateTime dateStartdate, DateTime dateEnddate, string sInvToTempAddress);

        string EndAvailableTemporaryAddress_(long lCusno, int iAddrno, System.DateTime dOriginalStartDate);
        
        string DefinitiveAddressChange_(long lCusno, string sStreetname, string sHouseno, string sStaircase, string sApartment, string sStreet2, string sStreet3, string sCountrycode, string sZipcode, DateTime dateStartdate);

        string DefinitiveAddressChangeRemove_(long lCusno, DateTime dateOriginalStartdate);

        long IdentifyCustomer_(long lCusno, long lSubsno, long lInvno, string sName1, string sName2, string sName3, string sPhone, string sEmail, string sStreet, string sHouseno, string sStaircase, string sApartment, string sCountry, string sZipcode, string sPostname, string sUserID);

        DataSet GetOpenInvoices_(long lCusno, string sUserid);

        string UpdateCustomerExtraInfo_(long lCusno, string sExtra1, string sExtra2, string sExtra3);

        DataSet GetCusInvModes_(long lCusno);

        string ChangeCusInvMode_(long lCusno, string sOriginalInvMode, string sOriginalInvDefault, string sNewInvMode, string sNewInvDefault, string sInfo, string sInfo2, string sInfo3);

        string AddNewCusInvmode_(long lCusno, string sInvmode, string sInvDefault, string sInfo, string sInfo2, string sInfo3);

        DateTime GetNextIssueDate_(string sPaperCode, string sProductNo, DateTime dteMinDate);

        string ChangeSubscriptionEnddate_(long lSubsno, int iExtno, DateTime dteNewEnddate);
        
        string CancelSubscription_(string sUserID, long lSubsno, int iExtno, string sCancelReason);

        DateTime GetNextIssuedate_(string sPapercode, string sProductno, DateTime dteMinDate);

        //121003 - new fields - double dblGrossPrice, double dblDiscAmount,
        string CreateRenewal_DI_(string sUserID, long lSubsno, int iExtno, long lPricelistno, long lCampno, int iSubslenMons, int iSubslenDays, DateTime dateSubsStartdate,
                                 DateTime dateSubsEnddate, string sSubskind, double dblTotalPrice, double dblItemPrice, int iItemqty, string sSalesno, long lPaycusno, string sProductno,
                                 string sReceiveType, string sTargetGroup, string sPriceAtStart, double dblGrossPrice, double dblDiscAmount, string sOtherSubsno, string sOrderID, string sAutogiro, string sPriceGroup, string sInvMode);

        long GetNextInvno_();

        string BuildRefno2_(long lInvno, string sInvType, string sPaperCode);

        long CreateImmediateInvoice_(long lSubsno, int iExtno, int iItemno, long lInvno, string sRefno);

        string InsertElectronicPayment_(long lCusno, long lInvno, string sRefno, double dAmount);

        DataSet GetCurrentAndPendingAddressChanges_(long cusno, long subsno);

        DataSet GetInvArgItems_(long subsno, int extno);

        string TemporaryAddressChangePeriod_(string sUserID, long lCusno, long lSubsno, int iAddrno, DateTime dOriginalStartDate, DateTime dNewStartDate, DateTime dNewEndDate);

    }

}
