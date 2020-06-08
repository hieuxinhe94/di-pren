using System.Collections.Generic;

namespace Pren.Web.Business.Configuration
{
    public interface ISiteSettings
    {
        string PriceGroupTrial { get; }
        string PriceGroupTrialFree { get; }
        string PriceGroupDirectDebit { get; }
        string PriceGroupRegular { get; }

        string GetProductName(string paperCode, string productNumber);
        string GetProductName(string packageId);
        bool IsDigitalSub(string packageId);

        string SubsKindTimed { get; }

        int LogEventHolidayStop { get; }   
        int LogEventPermAddressChange { get; }
        int LogEventTempAddressChange { get; }
        int PhoneMaxNoOfDigits { get; }

        string FaqApiBaseUrl { get; }        
        string ServicePlusChangePasswordUrl { get; }
        string ServicePlusForgotPasswordUrl { get; }
        string ServicePlusApiBaseUrl { get; }
        string ServicePlusClientId { get; }
        string ServicePlusClientSecret { get; }
        string ServicePlusLoginPageUrl { get; }

        string MailPrenAddress { get; }
        string MailNoReplyAddress { get; }

        string ServicePlusAppId { get; }
        string ServicePlusSecretKey { get; }
        string ServicePlusOrderProductId { get; }

        string ServicePlusBrandId { get; }
        string ServicePlusCreateOrUpdateOfferProductId { get; }
        string ServicePlusIframeMustNotHaveProducts { get; }
        string GetServicePlusProductId(string paperCode, string productNo);

        List<string> SubsStateActiveValues { get; }
        string SubsStateRenewal { get; }

        string OptimizelyMobileScriptSrc { get; }
        string OptimizelyDesktopScriptSrc { get; }

        string HideSubsSleepsForAutogiroCust { get; }

        string StudentVerificationUrl { get; }
        string StudentVerificationUserName { get; }
        string StudentVerificationPassword { get; }

        string CryptoKeyCodeListId { get; }
        string CryptoIvCodeListId { get; }

        string CryptoKeyUserId { get; }
        string CryptoIvUserId { get; }

        string CryptoKeyToken { get; }
        string CryptoIvToken { get; }

        string NeoLaneEndPointUrl { get; }
        string NeoLaneEndPointUser { get; }
        string NeoLaneEndPointPassword { get; }

        string SurveyEndPointUrl { get; }
        string SurveyEndPointCompany { get; }
        string SurveyEndPointUser { get; }
        string SurveyEndPointPassword { get; }

        string InvoiceApiUrl { get; }

        string InvoiceApiStreamUrl { get; }
        string InvoiceApiUserName { get; }

        string InvoiceApiPassword { get; }

        string InvoiceEncryptKey { get; }

        string InvoiceEncryptIv { get; }
    }
}
