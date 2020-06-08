using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Utils;
using Di.Common.Utils.Url;
using Di.ServicePlus.Utils;
using Di.Subscription.Logic.Campaign;
using Di.Subscription.Logic.Campaign.Types;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Newtonsoft.Json;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Package;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Utils.Translate;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;
using Pren.Web.Models.ViewModels;
using Pren.Web.Models.ViewModels.OrderFlow;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class OrderFlowCampaignPageController : PageControllerBase<OrderFlowCampaignPage>
    {
        private readonly ISiteSettings _siteSettings;
        private readonly IUrlHelper _urlHelper;
        private readonly ICampaignHandler _campaignHandler;
        private readonly IPackageRelationManager _packageRelationManager;
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly StartPage _startPage;

        public OrderFlowCampaignPageController(
            IContentLoader contentLoader,
            ISiteSettings siteSettings,
            IUrlHelper urlHelper,
            ICampaignHandler campaignHandler,
            IPackageRelationManager packageRelationManager,
            IServicePlusFacade servicePlusFacade)
        {
            _siteSettings = siteSettings;
            _urlHelper = urlHelper;
            _campaignHandler = campaignHandler;
            _packageRelationManager = packageRelationManager;
            _servicePlusFacade = servicePlusFacade;

            _startPage = contentLoader.Get<StartPage>(ContentReference.StartPage);
        }

        public ActionResult Index(OrderFlowCampaignPage currentPage, string tg, string callback, string offerOrigin, string sc, string appId)
        {
            var packages = GetPackages(currentPage);

            // since it's nothing to choose from return order view 
            if (packages.Count == 1)
            {
                var orderModel = GetOrderViewModel(currentPage, packages.First(), tg, callback, offerOrigin, sc, appId);
                orderModel.HideChangePackageBtn = true;

                return View("Order", orderModel);
            }

            var model = new OrderFlowViewModel(currentPage)
            {
                Packages = GetPackages(currentPage),
                QueryString = UrlUtils.AddAllExistingQuerystrings(string.Empty),
                PageUrl = _urlHelper.GetContentUrlWithHost(currentPage.ContentLink, true)
            };

            return View(model);
        }

        public ActionResult Order(OrderFlowCampaignPage currentPage, string packageId, string tg, string callback, string offerOrigin, string sc, string appId)
        {
            var package = GetPackage(currentPage, packageId);

            if (package == null)
            {
                return RedirectToAction("Index");
            }

            var model = GetOrderViewModel(currentPage, package, tg, callback, offerOrigin, sc, appId);

            return View(model);
        }

        public ActionResult Script(OrderFlowCampaignPage currentPage, string id, string orderNumber, string orderAmount)
        {
            var user = _servicePlusFacade.GetUserById(id);

            var model = new CampaignScriptViewModel
            {
                Title = currentPage.PageName,
                ScriptBody = currentPage.ScriptThankyou,
                ScriptHead = currentPage.ScriptThankyouInHeader,
                Email = user != null ? user.Email : string.Empty,
                OrderNumber = orderNumber,
                OrderAmount = orderAmount
            };

            return View("Script", model);
        }

        private OrderFlowOrderViewModel GetOrderViewModel(OrderFlowCampaignPage currentPage, PackageModel package, string tg, string callback, string offerOrigin, string sc, string appId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"appId", string.IsNullOrEmpty(appId) ? _siteSettings.ServicePlusAppId : appId},
                {"lc","sv"},
                {"ts", DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now).ToString(CultureInfo.InvariantCulture)},
                {"callback", callback ?? (_startPage.DefaultCallbackUrl?.ToString() ?? string.Empty)},
                {"emptyoffer_message", UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/ErrMessDenySameSubType"))},
                {"terms_and_agreement", UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/DisclaimerText"))}
            };

            // if no targetgroup in url, use default from package
            var targetGroup = string.IsNullOrEmpty(tg) ? package.TargetGroup : tg;

            AddCampaignUrlToDictionary(parameters, currentPage);

            AddOriginToDictionary(parameters, package, offerOrigin);

            AddStudentToDictionary(parameters, package);

            AddTrackingUrlToDictionary(parameters, currentPage);

            AddSalesChannelToDictionary(parameters, currentPage, sc);

            parameters.Add("periodsPrivate", GetCampagnPeriodsAsJson(package.PeriodsPrivate, targetGroup, package));
            parameters.Add("periodsBusiness", GetCampagnPeriodsAsJson(package.PeriodsBusiness, targetGroup, package));

            // Add signature to parameters
            var signature = ServicePlusSignatureUtils.Sign(_siteSettings.ServicePlusSecretKey, DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now), parameters);
            parameters.Add("s", signature);

            var model = new OrderFlowOrderViewModel(currentPage)
            {
                Parameters = parameters,
                IframeUrl = _siteSettings.ServicePlusLoginPageUrl + "subscribe",
                Package = package,
                QueryString = UrlUtils.AddAllExistingQuerystrings(string.Empty),
                TargetGroup = targetGroup
            };

            return model;
        }

        private PackageModel GetPackage(OrderFlowCampaignPage currentPage, string packageId)
        {
            if (string.IsNullOrEmpty(packageId)) return null;

            var packages = GetPackages(currentPage);

            return packages.FirstOrDefault(t => t.Id.Equals(packageId));
        }

        private List<PackageModel> GetPackages(OrderFlowCampaignPage currentPage)
        {
            var packages = currentPage.Packages;

            return string.IsNullOrEmpty(packages) ? new List<PackageModel>() : packages.ConvertToObject<List<PackageModel>>();
        }

        private void AddCampaignUrlToDictionary(IDictionary<string, string> parameters, IContent currentPage)
        {
            if (_startPage.SendCampaignUrlParameter)
            {
                parameters.Add("campaign-url", _urlHelper.GetContentUrlWithHost(currentPage.ContentLink).ToLower());
            }
        }

        private void AddOriginToDictionary(IDictionary<string, string> parameters, PackageModel package, string offerOrigin)
        {
            // If featureflag is off - add no origin param
            if (!_startPage.SendOriginParameter)
            {
                return;
            }

            // Not a PayWall campaign - set default origin
            if (!package.IsPayWall)
            {
                parameters.Add("origin", "di-pren");
                return;
            }

            // PayWall campaign without provided offerorigin - set plusflow unknown site as origin
            if (string.IsNullOrEmpty(offerOrigin))
            {
                parameters.Add("origin", "di-pren-plusflow-unknown-site");
                return;
            }

            // PayWall campaign with provided offerorigin - set provided offerorigin as origin
            parameters.Add("origin", offerOrigin);
        }

        private void AddStudentToDictionary(IDictionary<string, string> parameters, PackageModel package)
        {
            if (_startPage.SendStudentParameter)
            {
                parameters.Add("student", package.IsStudent.ToString().ToLower());
            }
        }

        private void AddTrackingUrlToDictionary(IDictionary<string, string> parameters, IContent currentPage)
        {
            if (_startPage.SendTrackingUrlParameter)
            {
                parameters.Add("tracking-script-url", _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + UrlConstants.ScriptAction);
            }
        }

        private void AddSalesChannelToDictionary(IDictionary<string, string> parameters, OrderFlowCampaignPage currentPage, string salesChannel)
        {
            var salesChannelParameter = string.IsNullOrEmpty(salesChannel) ? currentPage.SalesChannel : salesChannel;

            if (_startPage.SendSalesChannelParameter && !string.IsNullOrEmpty(salesChannelParameter))
            {
                parameters.Add("salesChannel", salesChannelParameter);
            }
        }

        private string GetPriceGroup(PackagePeriod period, string defaultPriceGroup)
        {
            if (period.IsTrial)
                return _siteSettings.PriceGroupTrial;

            return period.IsTrialFree ? _siteSettings.PriceGroupTrialFree : defaultPriceGroup ?? "00";
        }

        private string GetProductId(Campaign campaign)
        {
            if (_startPage.PackageIdAsProductId)
            {
                return campaign.PackageId;
            }

            return campaign.PaperCode + "-" + campaign.ProductNumber;
        }

        private string GetPaymentMethods(PackagePeriod campaignPeriod)
        {
            var availablePriceGroups = "KLARNA2";

            if (!campaignPeriod.HideCardPayment)
            {
                availablePriceGroups += ",CREDIT_CARD";
            }

            if (!campaignPeriod.HideInvoicePayment)
            {
                availablePriceGroups += ",INVOICE";
            }

            return availablePriceGroups;
        }

        private string GetCampagnPeriodsAsJson(List<PackagePeriod> periods, string targetGroup, PackageModel package)
        {
            var periodList = new List<Period>();

            if (periods == null) return periodList.ConvertToJson();

            foreach (var period in periods)
            {
                var campaign = _campaignHandler.GetCampaign(period.CampaignNumber);

                var priceInOre = _startPage.UsePartPayment
                    ? (campaign.PriceForCustomerToPay * 100).ToString(CultureInfo.InvariantCulture)
                    : (campaign.TotalPriceIncludningVat * 100).ToString(CultureInfo.InvariantCulture);

                var campaignPeriod = new Period
                {
                    MustHave =
                        period.IsUpgradeCampaign
                            ? _packageRelationManager.GetParameters(PackageRelationTypeEnum.MustHave,
                                campaign.PackageId)
                            : string.Empty,
                    MustNotHave =
                        _packageRelationManager.GetParameters(PackageRelationTypeEnum.MustNotHave,
                            campaign.PackageId),
                    Name = period.Name,
                    Description = period.Description,
                    ProductId = GetProductId(campaign),
                    ProductDesc = period.ProductDescription,
                    ProductExtraDesc = period.SummaryRows,
                    PriceSth = priceInOre,
                    PaymentMethods = GetPaymentMethods(period),
                    KayaklCampNo = campaign.CampaignNumber.ToString(CultureInfo.InvariantCulture),
                    KayaklPricelistno = GetPriceGroup(period, campaign.PriceGroup),
                    KayaksTargetGroup = targetGroup,
                    CheckPricegroup = period.IsTrial || period.IsTrialFree,
                    CheckPricegroupMonths = 3,
                    CheckPricegroupErrormessage =
                        UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/ErrMessDenyShortSub")),
                };

                if (_startPage.SendIsDigitalParameter)
                {
                    campaignPeriod.IsDigital = period.IsDigital;
                }

                if (_startPage.SendProductExplParameter)
                {
                    campaignPeriod.ProductExpl = package.Title;
                }

                var subsKind = _startPage.SendSubsKindParameter && period.SubsKind != null && period.SubsKind != "0"
                    ? period.SubsKind
                    : campaign.SubsKind;

                campaignPeriod.SubsKind = subsKind;

                periodList.Add(campaignPeriod);
            }

            return periodList.ConvertToJson();
        }
    }

    public class Period
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("productExpl")]
        public string ProductExpl { get; set; }

        [JsonProperty("mustHave")]
        public string MustHave { get; set; }

        [JsonProperty("mustNotHave")]
        public string MustNotHave { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productDesc")]
        public string ProductDesc { get; set; }

        [JsonProperty("productExtraDesc")]
        public List<SummaryRow> ProductExtraDesc { get; set; }

        [JsonProperty("priceSth")]
        public string PriceSth { get; set; }

        [JsonProperty("paymentMethods")]
        public string PaymentMethods { get; set; }

        [JsonProperty("kayaklCampNo")]
        public string KayaklCampNo { get; set; }

        [JsonProperty("kayaklPricelistno")]
        public string KayaklPricelistno { get; set; }

        [JsonProperty("kayaksTargetGroup")]
        public string KayaksTargetGroup { get; set; }

        [JsonProperty("checkPricegroup")]
        public bool CheckPricegroup { get; set; }

        [JsonProperty("checkPricegroupMonths")]
        public int CheckPricegroupMonths { get; set; }

        [JsonProperty("checkPricegroupErrormessage")]
        public string CheckPricegroupErrormessage { get; set; }

        [JsonProperty("isDigital")]
        public bool IsDigital { get; set; }

        [JsonProperty("subsKind")]
        public string SubsKind { get; set; }
    }

}