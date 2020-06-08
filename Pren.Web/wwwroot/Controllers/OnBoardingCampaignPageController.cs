using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Di.Common.Utils;
using Di.ServicePlus.Utils;
using EPiServer.Web.Mvc;
using Newtonsoft.Json;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Helpers;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class OnBoardingCampaignPageController : PageController<OnBoardingCampaignPage>
    {
        private readonly ISiteSettings _siteSettings;
        private readonly IUrlHelper _urlHelper;

        public OnBoardingCampaignPageController(
            ISiteSettings siteSettings, 
            IUrlHelper urlHelper)
        {
            _siteSettings = siteSettings;
            _urlHelper = urlHelper;
        }

        public ActionResult Index(OnBoardingCampaignPage currentPage)
        {            
            var onboardingParameters = currentPage.Parameters.ConvertToObject<OnboardingParameters>();
            var timeStamp = DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now);

            var parameters = new Dictionary<string, string>
            {
                { "data", Base64Encode(onboardingParameters.ConvertToJson()) },
                { "ts", timeStamp.ToString() },
                { "s", CreateSignature(currentPage.CompanyId, timeStamp, onboardingParameters) }
            };

            var model = new OnBoardingViewModel(currentPage)
            {
                Package = string.IsNullOrEmpty(currentPage.Package) ? null : currentPage.Package.ConvertToObject<PackageModel>(),
                IframeUrl = $"{_siteSettings.ServicePlusLoginPageUrl}webpayment/large-company-sales/{currentPage.CompanyId}",
                CallbackUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink, true),
                BaseUrl = _siteSettings.ServicePlusLoginPageUrl,
                Parameters = parameters
            };
                        
            return View(model);
        }

        private string CreateSignature(string companyId, long timeStamp, OnboardingParameters parameters)
        {
            var signatureParameters = new Dictionary<string, string>
            {
                { "companyId", companyId },
                { "companyAgreementId", parameters.CompanyAgreementId },
                { "resourceId", parameters.ResourceId },
                { "appId", parameters.AppId },
                { "paymentProviderType", parameters.PaymentProviderType },
                { "emailDomainWhitelist", parameters.EmailDomainWhitelist },
                { "productId", parameters.ProductId },
                { "currency", parameters.Currency },
                { "kayak_lCampNo", parameters.KayakCampaign },
                { "kayak_lPricelistno", parameters.KayakPriceList },
                { "kayak_sTargetGroup", parameters.KayakTargetGroup },
                { "paymentWithoutLogin", parameters.PaymentWithoutLogin },
                { "price", parameters.Price },
                { "productDesc", parameters.ProductDescription },
                { "subskind", parameters.Subskind },
                { "taxRate", parameters.TaxRate },
                { "payerCustomerNumber", JsonConvert.SerializeObject(parameters.PayerCustomerNumber) }
            };

            var signature = ServicePlusSignatureUtils.Sign(_siteSettings.ServicePlusSecretKey, timeStamp, signatureParameters);

            return signature;
        }

        private string Base64Encode(string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }
    }


}