using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Di.Common.Logging;
using Di.Common.Utils;
using Di.Common.Utils.Context;
using Di.Common.Utils.Url;
using Di.Common.WebRequests;
using Di.ServicePlus.Utils;
using Di.Subscription.Logic.Campaign;
using Di.Subscription.Logic.Campaign.Types;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Newtonsoft.Json;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Campaign;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.Detection;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Package;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Utils.Replace;
using Pren.Web.Business.Utils.Translate;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    [NoCache]
    public class CampaignPageIframeController : PageControllerBase<CampaignPageIframe>
    {
        #region Fields

        private readonly ILogger _logService;
        private readonly IDetectionHandler _detection;
        private readonly IContentRepository _contentRepository;
        private readonly ISiteSettings _siteSettings;
        private readonly ICampaignHandler _campaignHandler;
        private readonly IDataAccess _dataAccess;
        private readonly IContentLoader _contentLoader;
        private readonly IAntiForgeryValidator _antiForgeryValidator;
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly IUrlHelper _urlHelper;
        private readonly IPackageRelationManager _packageRelationManager;

        #endregion

        #region Constructor

        public CampaignPageIframeController(
            ILogger logService,
            IDetectionHandler detection,
            IContentRepository contentRepository,
            ISiteSettings siteSettings,
            ICampaignHandler campaignHandler, 
            IDataAccess dataAccess, 
            IContentLoader contentLoader, 
            IPackageRelationManager packageRelationManager,
            IAntiForgeryValidator antiForgeryValidator,
            IServicePlusFacade servicePlusFacade, 
            IUrlHelper urlHelper)
        {
            _logService = logService;
            _detection = detection;
            _contentRepository = contentRepository;
            _siteSettings = siteSettings;
            _campaignHandler = campaignHandler;
            _dataAccess = dataAccess;
            _contentLoader = contentLoader;
            _antiForgeryValidator = antiForgeryValidator;
            _packageRelationManager = packageRelationManager;
            _servicePlusFacade = servicePlusFacade;
            _urlHelper = urlHelper;
        }

        #endregion

        #region actions

        public ActionResult Index(CampaignPageIframe currentPage, string tg, string callback, string offerorigin, string sc, string appid)
        {
            var model = new CampaignPageOrderViewModel(currentPage)
            {
                IsMobileDevice = _detection.IsMobileDevice(),
                TargetGroup = GetTargetGroup(currentPage, tg),
                Callback = callback,
                OfferOrigin = offerorigin,
                SalesChannel = sc,
                AppId = appid
            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Order(CampaignPageIframe currentPage, CampaignPageOrderFormModel postedForm)
        {
            // GET reqeusts should redirect user to Index
            if (_detection.IsHttpMethod(RequestMethod.GET))
            {
                return RedirectToAction("Index");
            }

            try
            {
                _antiForgeryValidator.Validate();

                var selectedCampaign = GetSelectedCampaignBlock(postedForm.CampaignContentId);

                var parameters = new Dictionary<string, string>
                {
                    {"appId", string.IsNullOrEmpty(postedForm.AppId) ? _siteSettings.ServicePlusAppId : postedForm.AppId}, 
                    {"lc","sv"},
                    {"ts", DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now).ToString(CultureInfo.InvariantCulture)},
                    {"callback", postedForm.Callback ?? GetDefaultCallbackUrl()}, 
                    {"emptyoffer_message", UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/ErrMessDenySameSubType"))},
                    {"terms_and_agreement", UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/DisclaimerText"))}
                };

                AddCampaignUrlToDictionary(parameters, currentPage);

                AddOriginToDictionary(parameters, selectedCampaign, postedForm.OfferOrigin);

                AddStudentToDictionary(parameters, selectedCampaign);

                AddTrackingUrlToDictionary(parameters, currentPage);

                AddSalesChannelToDictionary(parameters, currentPage, postedForm.SalesChannel);

                if (currentPage.UseLatestIframeFlow)
                {
                    //Use the new s+ iframe with support for paper subscriptions
                    parameters.Add("periodsPrivate", GetCampagnPeriodsAsJson(selectedCampaign, postedForm.TargetGroup, false));
                    parameters.Add("periodsBusiness", GetCampagnPeriodsAsJson(selectedCampaign, postedForm.TargetGroup, true));
                }
                else
                {
                    PopulateDictionaryWithCampaignData(parameters, selectedCampaign, postedForm.TargetGroup);
                }

                // Add signature to parameters
                var timeStamp = DateTimeUtils.GetUnixTimeStampInMilliseconds(DateTime.Now);
                var signature = ServicePlusSignatureUtils.Sign(_siteSettings.ServicePlusSecretKey, timeStamp, parameters);
                parameters.Add("s", signature);

                var model = new CampaignPageOrderViewModel(currentPage)
                {
                    Parameters = parameters,
                    IframeUrl = _siteSettings.ServicePlusLoginPageUrl + "subscribe",
                    SelectedCampaign = selectedCampaign,
                    TargetGroup = postedForm.TargetGroup
                };

                // Populate model with usp-text
                if (selectedCampaign.UspProduct <= 0) return View(model);

                var uspTexts = _dataAccess.UspHandler.GetUspTexts(selectedCampaign.UspProduct).ToList();
                if (uspTexts.Any())
                {
                    model.UspTexts = uspTexts.Select(uspText => ReplaceUtil.ReplacePlaceholderWithImage(uspText.Text)).ToList();
                }

                return View(model);
            }
            catch (HttpAntiForgeryException ex)
            {
                ex.Data.Add("loginfo", new UserContext().ToLogString());
                _logService.Log(ex, "CampaignIframe.Order - Antiforgery validation failed", LogLevel.Error, typeof(CampaignPageIframeController));
            }
            catch (Exception ex)
            {
                _logService.Log(ex, "CampaignIframe.Order - failed", LogLevel.Error, typeof(CampaignPageIframeController));
            }

            return GetErrorView(new CampaignPageOrderViewModel(currentPage), CampaignConstants.GeneralErrorMessage);
        }

        public ActionResult Script(CampaignPageIframe currentPage, string id, string orderNumber, string orderAmount)
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

        #endregion

        #region Private methods

        private StartPage GetStartPage()
        {
            return _contentLoader.Get<StartPage>(ContentReference.StartPage);
        }

        private void AddTrackingUrlToDictionary(IDictionary<string, string> parameters, CampaignPageIframe currentPage)
        {
            var startPage = GetStartPage();

            if (startPage.SendTrackingUrlParameter)
            {
                parameters.Add("tracking-script-url", _urlHelper.GetContentUrlWithHost(currentPage.ContentLink) + UrlConstants.ScriptAction);
            }
        }

        private void AddStudentToDictionary(IDictionary<string, string> parameters, CampaignBlock selectedCampaign)
        {
            var startPage = GetStartPage();

            if (startPage.SendStudentParameter)
            {
                parameters.Add("student", selectedCampaign.IsStudent.ToString().ToLower());
            }
        }

        private void AddCampaignUrlToDictionary(IDictionary<string, string> parameters, CampaignPage currentPage)
        {
            var startPage = GetStartPage();

            if (startPage.SendCampaignUrlParameter)
            {
                parameters.Add("campaign-url", _urlHelper.GetContentUrlWithHost(currentPage.ContentLink).ToLower());
            }
        }

        private void AddSalesChannelToDictionary(IDictionary<string, string> parameters, CampaignPage currentPage, string salesChannel)
        {
            var startPage = GetStartPage();
            var salesChannelParameter = string.IsNullOrEmpty(salesChannel) ? currentPage.SalesChannel : salesChannel;

            if (startPage.SendSalesChannelParameter && !string.IsNullOrEmpty(salesChannelParameter))
            {                
                parameters.Add("salesChannel", salesChannelParameter);
            }
        }

        private void AddOriginToDictionary(IDictionary<string, string> parameters, CampaignBlock selectedCampaign, string offerOrigin)
        {
            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
            
            // If featureflag is off - add no origin param
            if (!startPage.SendOriginParameter)
            {
                return;
            }

            // Not a PayWall campaign - set default origin
            if (!selectedCampaign.IsPayWall)
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

        private string GetDefaultCallbackUrl()
        {
            var startPage = GetStartPage();

            return startPage.DefaultCallbackUrl != null ? startPage.DefaultCallbackUrl.ToString() : string.Empty;
        }

        private void PopulateDictionaryWithCampaignData(IDictionary<string, string> dictionary, CampaignBlock campaignBlock, string targetGroup)
        {
            var campaignPeriods = new List<CampaignPeriodBlock>
            {
                campaignBlock.FirstCampaignPeriod,
                campaignBlock.SecondCampaignPeriod,
                campaignBlock.ThirdCampaignPeriod
            };

            var i = 0;
            foreach (var campaignPeriod in campaignPeriods)
            {
                if (campaignPeriod == null || string.IsNullOrEmpty(campaignPeriod.CampaignCardAndInvoice)) continue;

                var campaignIdArray = campaignPeriod.CampaignCardAndInvoice.Split('|'); //P15030000E|2001

                var campaign = campaignIdArray.Length > 1
                    ? _campaignHandler.GetCampaign(long.Parse(campaignIdArray[1]))
                    : _campaignHandler.GetCampaign(campaignIdArray[0]);

                var priceGroup = GetPriceGroup(campaignPeriod, campaign.PriceGroup);
                var priceInOre = GetStartPage().UsePartPayment ? (campaign.PriceForCustomerToPay * 100).ToString(CultureInfo.InvariantCulture) : (campaign.TotalPriceIncludningVat * 100).ToString(CultureInfo.InvariantCulture);
                var applyPriceGroupCheck = campaignPeriod.IsTrial || campaignPeriod.IsTrialFree;
                var productSummaryJson = GetProductSummary(campaignPeriod).ConvertToJson();

                dictionary.Add("mustHave_" + i, campaignPeriod.IsUpgradeCampaign ? _packageRelationManager.GetParameters(PackageRelationTypeEnum.MustHave, campaign.PackageId) : string.Empty);
                dictionary.Add("mustNotHave_" + i, _packageRelationManager.GetParameters(PackageRelationTypeEnum.MustNotHave, campaign.PackageId));
                dictionary.Add("productId_" + i, GetProductId(campaign));
                dictionary.Add("productDesc_" + i, campaignPeriod.PeriodHeading ?? campaignBlock.Heading);
                dictionary.Add("productExtraDesc_" + i, productSummaryJson);
                dictionary.Add("price_sth_" + i, priceInOre);
                dictionary.Add("payment_methods_" + i, GetPaymentMethods(campaignPeriod));
                dictionary.Add("kayak_lCampNo_" + i, campaign.CampaignNumber.ToString(CultureInfo.InvariantCulture));
                dictionary.Add("kayak_lPricelistno_" + i, priceGroup);
                dictionary.Add("kayak_sTargetGroup_" + i, targetGroup);
                
                dictionary.Add("check_pricegroup_" + i, applyPriceGroupCheck.ToString().ToLower());
                dictionary.Add("check_pricegroup_months_" + i, "3");
                dictionary.Add("check_pricegroup_errormessage_" + i, UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/ErrMessDenyShortSub")));

                var startPage = GetStartPage();
                if (startPage.SendIsDigitalParameter)
                {
                    dictionary.Add("is_digital_" + i, campaignBlock.IsDigital.ToString().ToLower());
                }

                var subsKind = startPage.SendSubsKindParameter && campaignPeriod.SubsKind != null && campaignPeriod.SubsKind != "0"
                    ? campaignPeriod.SubsKind
                    : campaign.SubsKind;

                dictionary.Add("subskind_" + i, subsKind);

                i++;
            }
        }

        private string GetCampagnPeriodsAsJson(CampaignBlock campaignBlock, string targetGroup, bool exclVat)
        {
            var periodList = new List<Period>();
            var campaignPeriods = new List<CampaignPeriodBlock>
            {
                campaignBlock.FirstCampaignPeriod,
                campaignBlock.SecondCampaignPeriod,
                campaignBlock.ThirdCampaignPeriod
            };

            var startPage = GetStartPage();

            foreach (var campaignPeriod in campaignPeriods)
            {
                if (campaignPeriod == null || string.IsNullOrEmpty(campaignPeriod.CampaignCardAndInvoice)) continue;

                var campaignIdArray = campaignPeriod.CampaignCardAndInvoice.Split('|'); //P15030000E|2001

                var campaign = campaignIdArray.Length > 1
                    ? _campaignHandler.GetCampaign(long.Parse(campaignIdArray[1]))
                    : _campaignHandler.GetCampaign(campaignIdArray[0]);

                var priceInOre = startPage.UsePartPayment
                    ? (campaign.PriceForCustomerToPay * 100).ToString(CultureInfo.InvariantCulture)
                    : (campaign.TotalPriceIncludningVat * 100).ToString(CultureInfo.InvariantCulture);

                var priceDescription = ""; 

                if (startPage.UsePriceFromKayak)
                {
                    priceDescription = (exclVat ? campaign.PriceExludingVatForCustomerToPay : campaign.PriceForCustomerToPay).ToString() + " kr";
                }
                else
                {
                    //Fallback to totalprice if no totalPriceExVat is set on block
                    var priceExclVat = string.IsNullOrEmpty(campaignPeriod.TotalPriceExVat) ? campaignPeriod.TotalPrice : campaignPeriod.TotalPriceExVat;
                    priceDescription = exclVat ? priceExclVat : campaignPeriod.TotalPrice;
                }

                var period = new Period
                {
                    MustHave =
                        campaignPeriod.IsUpgradeCampaign
                            ? _packageRelationManager.GetParameters(PackageRelationTypeEnum.MustHave,
                                campaign.PackageId)
                            : string.Empty,
                    MustNotHave =
                        _packageRelationManager.GetParameters(PackageRelationTypeEnum.MustNotHave,
                            campaign.PackageId),
                    Name = campaignBlock.Heading,
                    Description = priceDescription,
                    ProductId = GetProductId(campaign),
                    ProductDesc = campaignPeriod.PeriodHeading ?? campaignBlock.Heading,
                    PriceSth = priceInOre,
                    PaymentMethods = GetPaymentMethods(campaignPeriod),
                    KayaklCampNo = campaign.CampaignNumber.ToString(CultureInfo.InvariantCulture),
                    KayaklPricelistno = GetPriceGroup(campaignPeriod, campaign.PriceGroup),
                    KayaksTargetGroup = targetGroup,
                    CheckPricegroup = campaignPeriod.IsTrial || campaignPeriod.IsTrialFree,
                    CheckPricegroupMonths = 3,
                    CheckPricegroupErrormessage =
                        UrlUtils.UrlEncode(TranslateUtil.GetLocalizedText("/feedback/ErrMessDenyShortSub")),
                };

                if (startPage.SendIsDigitalParameter)
                {
                    period.IsDigital = campaignBlock.IsDigital;
                }

                var subsKind = startPage.SendSubsKindParameter && campaignPeriod.SubsKind != null && campaignPeriod.SubsKind != "0"
                    ? campaignPeriod.SubsKind
                    : campaign.SubsKind;

                period.SubsKind = subsKind;

                periodList.Add(period);
            }

            return periodList.ConvertToJson();
        }

        private string GetProductId(Campaign campaign)
        {
            var startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);

            if (startPage.PackageIdAsProductId)
            {
                return campaign.PackageId;
            }

            return campaign.PaperCode + "-" + campaign.ProductNumber;
        }

        private List<SummaryItem> GetProductSummary(CampaignPeriodBlock campaignPeriod)
        {
            var addIfNotNull = new Action<string, string, List<SummaryItem>>((name, value, summaryList) =>
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    summaryList.Add(new SummaryItem(name, value));
                }
            });

            var summaryItems = new List<SummaryItem>();

            addIfNotNull(campaignPeriod.FirstSummaryText, campaignPeriod.FirstSummaryPrice, summaryItems);
            addIfNotNull(campaignPeriod.SecondSummaryText, campaignPeriod.SecondSummaryPrice, summaryItems);
            addIfNotNull(campaignPeriod.ThirdSummaryText, campaignPeriod.ThirdSummaryPrice, summaryItems);

            return summaryItems;
        }

        private string GetPaymentMethods(CampaignPeriodBlock campaignPeriod)
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

        private string GetPriceGroup(CampaignPeriodBlock campaignPeriod, string defaultPriceGroup)
        {
            if (campaignPeriod.IsTrial)
                return _siteSettings.PriceGroupTrial;

            return campaignPeriod.IsTrialFree ? _siteSettings.PriceGroupTrialFree : defaultPriceGroup ?? "00";
        }

        private string GetTargetGroup(CampaignPage currentPage, string targetGroupFromUrl)
        {
            if (!string.IsNullOrEmpty(targetGroupFromUrl))
                return targetGroupFromUrl;

            return _detection.IsMobileDevice() ? currentPage.TargetGroupMobile : currentPage.TargetGroup;
        }


        private CampaignBlock GetSelectedCampaignBlock(int campaignContentId)
        {
            return (campaignContentId > 0)
                ? _contentRepository.Get<CampaignBlock>(new ContentReference(campaignContentId))
                : null;
        }

        private ViewResult GetErrorView(CampaignPageOrderViewModel model, string errorMessage)
        {
            model.ErrorMessage = errorMessage;
            return View("Error", model);
        }

        #endregion
    }

    public class SummaryItem
    {
        public SummaryItem(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public SummaryItem()
        {

        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}