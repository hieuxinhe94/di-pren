using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bn.SelfService.ContentApi;
using Bn.SelfService.ContentApi.Models;
using Di.Common.Security.Encryption;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.EpiSelfServiceContentApi.Models.Blocks;
using Pren.Web.Business.EpiSelfServiceContentApi.Models.Pages;
using Pren.Web.Models.Pages;

namespace Pren.Web.Business.EpiSelfServiceContentApi
{
    public class EpiSelfServiceContentService : ISelfServiceContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ISiteSettings _siteSettings;
        private readonly ICryptographyService _cryptographyService;
        private readonly UrlHelper _urlHelper;

        public EpiSelfServiceContentService(
            IContentRepository contentRepository,
            ISiteSettings siteSettings,
            ICryptographyService cryptographyService, UrlHelper urlHelper)
        {
            _contentRepository = contentRepository;
            _siteSettings = siteSettings;
            _cryptographyService = cryptographyService;
            _urlHelper = urlHelper;
        }

        public IEnumerable<Alert> GetAlerts(string brand)
        {
            var selfServiceHomePage = GetSelfServiceHomePage(brand);

            var alertBlocks =
                selfServiceHomePage.AlertsContentArea?.Items.Select(block => block.GetContent())
                    .OfType<AlertBlock>()
                    .ToList() ?? new List<AlertBlock>();

            var alerts = new List<Alert>();

            foreach (var alertBlock in alertBlocks)
            {
                var now = DateTime.Now;

                if (now > alertBlock.VisibleFrom && now < alertBlock.VisibleTo)
                {
                    var alertId = (alertBlock as IContent).ContentLink.ID.ToString();
                    alerts.Add(new Alert(alertId, alertBlock.Title ?? "", alertBlock.Text?.ToString() ?? ""));
                }
            }

            return alerts;
        }

        public IEnumerable<Teaser> GetTeasers(string brand)
        {
            var selfServiceHomePage = GetSelfServiceHomePage(brand);

            var teaserBlocks =
                selfServiceHomePage.TeaserContentArea?.Items.Select(block => block.GetContent())
                    .OfType<TeaserBlock>()
                    .ToList() ?? new List<TeaserBlock>();

            var teasers = new List<Teaser>();

            foreach (var teaserBlock in teaserBlocks)
            {
                // If imageUrl prop is set, use it otherwise use Image prop
                var imageUrl = teaserBlock.ImageUrl?.ToString() ?? GetExternalUrl(teaserBlock.Image);

                teasers.Add(new Teaser(
                    teaserBlock.Title ?? "",
                    teaserBlock.Text?.ToString() ?? "",
                    imageUrl,
                    teaserBlock.ButtonText ?? "",
                    teaserBlock.ButtonLinkUrl?.ToString() ?? ""
                ));
            }

            return teasers;
        }

        public IEnumerable<Code> GetCodes(string brand)
        {
            var selfServiceHomePage = GetSelfServiceHomePage(brand);

            var codeBlocks =
                selfServiceHomePage.CodesContentArea?.Items.Select(block => block.GetContent())
                    .OfType<CodeBlock>()
                    .ToList() ?? new List<CodeBlock>();

            var codes = new List<Code>();

            foreach (var codeBlock in codeBlocks)
            {
                // If imageUrl prop is set, use it otherwise use Image prop
                var imageUrl = codeBlock.ImageUrl?.ToString() ?? GetExternalUrl(codeBlock.Image);

                codes.Add(new Code(
                    _cryptographyService.EncryptString(codeBlock.CodeListId, _siteSettings.CryptoKeyCodeListId, _siteSettings.CryptoIvCodeListId),
                    codeBlock.Heading ?? "",
                    imageUrl,
                    codeBlock.InstructionText?.ToString() ?? "",
                    codeBlock.ButtonText ?? "",
                    codeBlock.TermsText?.ToString() ?? ""
                ));
            }

            return codes;
        }

        public IEnumerable<RightColumnTeaser> GetRightColumnTeasers(string brand)
        {
            var selfServiceHomePage = GetSelfServiceHomePage(brand);

            var teaserBlocks =
                selfServiceHomePage.RightColumnTeasersContentArea?.Items.Select(block => block.GetContent())
                    .OfType<RightColumnTeaserBlock>()
                    .ToList() ?? new List<RightColumnTeaserBlock>();

            var teasers = new List<RightColumnTeaser>();

            foreach (var teaserBlock in teaserBlocks)
            {
                teasers.Add(new RightColumnTeaser(
                    teaserBlock.Title ?? "",
                    teaserBlock.Text.ToString() ?? ""
                ));
            }

            return teasers;
        }

        public string GetText(string brand, string type)
        {
            var selfServiceHomePage = GetSelfServiceHomePage(brand);

            switch (type.ToLower())
            {
                case "user":
                    return selfServiceHomePage.UserTerms?.ToString();
                case "userarchive":
                    return selfServiceHomePage.UserTermsArchive?.ToString();
                case "subscription":
                    return selfServiceHomePage.SubscriptionTerms?.ToString();
                case "cookies":
                    return selfServiceHomePage.CookiesTerms?.ToString();
                case "pul":
                    return selfServiceHomePage.PulTerms?.ToString();
                case "footer":
                    return selfServiceHomePage.FooterText?.ToString();
                default:
                    return string.Empty;
            }
        }


        private SelfServiceHomePage GetSelfServiceHomePage(string brand)
        {
            //brand not supported yet... Hardcoded to DN
            StartPage startPage;

            try
            {
                startPage = _contentRepository.Get<StartPage>(ContentReference.StartPage);
            }
            catch (Exception)
            {
                // Fallback to hardcoded value for startpage. this is because when using the stage address ContentReference.StartPage is null for some reason.
                startPage = _contentRepository.Get<StartPage>(new ContentReference(5));
            }

            var selfServiceStartPage = _contentRepository.Get<SelfServiceHomePage>(startPage.DnSelfServiceStartPage);

            return selfServiceStartPage;
        }

        private string GetExternalUrl(ContentReference contentReference)
        {

            if (contentReference != null)
            {
                var uri = HttpContext.Current.Request.Url;

                return uri.Scheme + "://" + uri.Host + _urlHelper.ContentUrl(contentReference);
            }

            return string.Empty;


            //TODO: SiteDefinition.Current.SiteUrl results in null-reference when using the stage address. Maybe be same root cause as in GetSelfServiceHomePage
            //if (contentReference != null)
            //{
            //    var uri = EPiServer.Web.SiteDefinition.Current.SiteUrl;

            //    return uri != null 
            //        ? uri.Scheme + "://" + uri.Host + _urlHelper.ContentUrl(contentReference) 
            //        : string.Empty;
            //}
            //return string.Empty;

            //TODO: _urlResolver results in null-reference when using the stage address. Maybe be same root cause as in GetSelfServiceHomePage
            //var internalUrl = _urlResolver.GetUrl(contentReference);

            //var url = new UrlBuilder(internalUrl);
            //Global.UrlRewriteProvider.ConvertToExternal(url, null, System.Text.Encoding.UTF8);

            //var friendlyUrl = UriSupport.AbsoluteUrlBySettings(url.ToString());
            //return friendlyUrl;
        }
    }
}