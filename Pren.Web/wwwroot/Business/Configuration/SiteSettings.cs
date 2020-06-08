using System;
using System.Collections.Generic;
using System.Linq;

namespace Pren.Web.Business.Configuration
{
    class SiteSettings : ISiteSettings
    {

        private readonly ISiteConfiguration _siteConfiguration;

        public SiteSettings(ISiteConfiguration siteConfiguration)
        {
            _siteConfiguration = siteConfiguration;
        }

        public bool IsDigitalSub(string packageId)
        {
            var digitalPackageIdArray = new[] { "AGENDA_01", "AGENDA_02", "AGENDA_03", "DISE_01", "IPAD_01", "IPAD_02" };

            return digitalPackageIdArray.Contains(packageId);
        }

        /// <summary>
        /// Tidsbestämnd, Settings.SubsKind_tidsbestamd
        /// </summary>
        public string SubsKindTimed { get { return "02"; } }

        [Obsolete("Use GetProductName(string packageId) instead. Exists for reference only.")]
        public string GetProductName(string paperCode, string productNumber)
        {
            if (paperCode == "DI")
            {
                if (productNumber == "01")
                    return "Di 6-dagars";

                if (productNumber == "05")
                    return "Di Weekend";

                if (productNumber == "IPAD")
                    return "Di Digital";
            }

            if (paperCode == "IPAD")
                return "Dagens industri i läsplatta";

            if (paperCode == "DISE")
                return "DISE";

            if (paperCode == "DIY")
                return "Dagens industri Y";

            if (paperCode == "AGENDA")
            {
                if (productNumber == "01")
                    return "Agenda Energi";

                if (productNumber == "02")
                    return "Agenda Vård och omsorg";

                if (productNumber == "03")
                    return "Agenda Import och export";
            }

            return string.Empty;  
        }

        public string GetProductName(string packageId)
        {
            switch (packageId)
            {
                case "IPAD_STUD":
                    return "Di Student";
                case "DI_PLUS":
                    return "Di Plus";
                case "FLEX":
                case "DI_05":
                    return "Di Helg";
                case "IPAD_02":
                    return "Di Max";
                case "DI_01":
                    return "Di Total";
                case "DI_02":
                    return "Di Taltidning";
                case "IPAD_01":
                    return "Di Digitalt";
                default:
                    return "Di";
            }            
        }

        #region PriceGroups

        public string PriceGroupTrial { get { return "42"; } }

        public string PriceGroupTrialFree { get { return "43"; } }

        public string PriceGroupDirectDebit { get { return "25"; } }

        public string PriceGroupRegular { get { return "00"; } }

        #endregion


        public int LogEventHolidayStop { get { return 2; } }

        public int LogEventPermAddressChange { get { return 3; } }

        public int LogEventTempAddressChange { get { return 1; } }

        public int PhoneMaxNoOfDigits { get { return 20; } }

        public string FaqApiBaseUrl
        {
            get { return _siteConfiguration.GetSetting(SettingConstants.FaqApiUrl); }
        }

        public string MailPrenAddress
        {
            get { return _siteConfiguration.GetSetting(SettingConstants.MailPrenDiSe); }
        }

        public string MailNoReplyAddress
        {
            get { return _siteConfiguration.GetSetting(SettingConstants.MailNoReplyAddress); }
        }

        /// <summary>
        /// 00=not active yet, 01=active, 02=break
        /// </summary>
        public List<string> SubsStateActiveValues
        {
            get
            {
                return new List<string> { "00", "01", "02" };      
            }
        }

        public string SubsStateRenewal
        {
            get { return "30"; }
        }

        public string OptimizelyDesktopScriptSrc {
            get { return _siteConfiguration.GetSetting(SettingConstants.OptimizelyDesktopScriptSrc); }
        }

        public string OptimizelyMobileScriptSrc {
            get { return _siteConfiguration.GetSetting(SettingConstants.OptimizelyMobileScriptSrc); }
        }

        public string HideSubsSleepsForAutogiroCust{
            get { return _siteConfiguration.GetSetting(SettingConstants.HideSubsSleepsForAutogiroCust); }
        }

        public string StudentVerificationUrl {
            get { return _siteConfiguration.GetSetting("StudentVerificationUrl"); }
        }

        public string StudentVerificationUserName
        {
            get { return _siteConfiguration.GetSetting("StudentVerificationUserName"); }
        }

        public string StudentVerificationPassword
        {
            get { return _siteConfiguration.GetSetting("StudentVerificationPassword"); }
        }

        public string CryptoKeyCodeListId { get { return "v0f84cf3Ht5Bg02VX2q49RBI14C80rre"; } }
        public string CryptoIvCodeListId { get { return "gKW3xe1HqIGtyyEH"; } }
        public string CryptoKeyUserId { get { return "D63895y47jw5J2HX832025F975ck6Ngr"; } }
        public string CryptoIvUserId { get { return "b6qAf8U7EVvFflJL"; } }
        public string CryptoKeyToken { get { return "N6jdI72Yib5ajgMcO09d7721v9r8uBht"; } }
        public string CryptoIvToken { get { return "YZVUkEDVDY4291KU"; } }

        #region ServicePlus

        public string ServicePlusChangePasswordUrl
        {
            get { return _siteConfiguration.GetSetting(SettingConstants.BonDigUrlChangePassword); }
        }

        public string ServicePlusForgotPasswordUrl
        {
            get { return _siteConfiguration.GetSetting(SettingConstants.BonDigUrlForgotPassword); }
        }

        public string ServicePlusLoginPageUrl
        {
            get { return _siteConfiguration.GetSetting("ServicePlusLoginPageUrl"); }
        }

        public string ServicePlusAppId
        {
            get { return _siteConfiguration.GetSetting("BonDigAppIdDagInd"); }
        }

        public string ServicePlusSecretKey
        {
            get { return _siteConfiguration.GetSetting("BonDigSecretKey"); }
        }

        public string ServicePlusOrderProductId
        {
            get { return _siteConfiguration.GetSetting("BonDigProductIdOrderFlow"); }
        }

        public string ServicePlusBrandId
        {
            get { return _siteConfiguration.GetSetting("BonDigBrandId"); }
        }

        public string ServicePlusCreateOrUpdateOfferProductId
        {
            get { return _siteConfiguration.GetSetting("BonDigProductId1WeekFor29Sek"); }
        }

        public string ServicePlusIframeMustNotHaveProducts
        {
            get { return _siteConfiguration.GetSetting("ServicePlusMustNotHaveProducts", "*-IPAD_01-*,*-IPAD_02-*"); }
        }

        public string ServicePlusApiBaseUrl
        {
            get { return _siteConfiguration.GetSetting("ServicePlusApiBaseUrl"); }
        }

        public string ServicePlusClientId
        {
            get { return _siteConfiguration.GetSetting("ServicePlusClientId"); }
        }

        public string ServicePlusClientSecret
        {
            get { return _siteConfiguration.GetSetting("ServicePlusClientSecret"); }
        }

        public string GetServicePlusProductId(string paperCode, string productNo)
        {
            if (paperCode == "DI")
            {
                if (productNo == "01")
                    return _siteConfiguration.GetSetting("BonDigProductIdDi6Days");

                if (productNo == "05")
                    return _siteConfiguration.GetSetting("BonDigProductIdDiWeekend");

                if (productNo == "IPAD")
                    return _siteConfiguration.GetSetting("BonDigProductIdIpad");
        }

            if (paperCode == "IPAD")
                return _siteConfiguration.GetSetting("BonDigProductIdIpad");

            if (paperCode == "DISE")
                return _siteConfiguration.GetSetting("BonDigProductIdDise");

            if (paperCode == "AGENDA")
            {
                if (productNo == "01")
                    return _siteConfiguration.GetSetting("BonDigProductIdAgendaEnergy");

                if (productNo == "02")
                    return _siteConfiguration.GetSetting("BonDigProductIdAgendaHealthCare");

                if (productNo == "03")
                    return _siteConfiguration.GetSetting("BonDigProductIdAgendaImportExport");
        }

            return string.Empty;
        }

        #endregion

        #region NeoLane

        public string NeoLaneEndPointUrl
        {
            get { return _siteConfiguration.GetSetting("NeoLaneEndPointUrl"); }
        }

        public string NeoLaneEndPointUser
        {
            get { return _siteConfiguration.GetSetting("NeoLaneEndPointUser"); }
        }
        public string NeoLaneEndPointPassword
        {
            get { return _siteConfiguration.GetSetting("NeoLaneEndPointPassword"); }
        }

        #endregion

        #region Survey

        public string SurveyEndPointUrl
        {
            get { return _siteConfiguration.GetSetting("SurveyEndPointUrl"); }
        }

        public string SurveyEndPointCompany
        {
            get { return _siteConfiguration.GetSetting("SurveyEndPointCompany"); }
        }

        public string SurveyEndPointUser
        {
            get { return _siteConfiguration.GetSetting("SurveyEndPointUser"); }
        }

        public string SurveyEndPointPassword
        {
            get { return _siteConfiguration.GetSetting("SurveyEndPointPassword"); }
        }

        #endregion

        #region Stralfors

        public string InvoiceApiUrl
        {
            get { return _siteConfiguration.GetSetting("invoiceApiUrl"); }
        }

        public string InvoiceApiStreamUrl
        {
            get { return _siteConfiguration.GetSetting("invoiceApiStreamUrl"); }
        }

        public string InvoiceApiUserName
        {
            get { return _siteConfiguration.GetSetting("invoiceApiUserName"); }
        }

        public string InvoiceApiPassword
        {
            get { return _siteConfiguration.GetSetting("invoiceApiPassword"); }
        }

        public string InvoiceEncryptKey
        {
            get { return _siteConfiguration.GetSetting("invoiceEncryptKey"); }
        }

        public string InvoiceEncryptIv
        {
            get { return _siteConfiguration.GetSetting("invoiceEncryptIv"); }
        }

        #endregion
    }
}
