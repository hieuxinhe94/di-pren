using System;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Pren.Web.Business.Helpers
{
    public class UrlHelper : IUrlHelper
    {
        private readonly UrlResolver _urlResolver;

        public UrlHelper()
        {
            _urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
        }

        public string GetContentUrlWithHost(ContentReference contentReference, bool removeTrailingSlash = false)
        {
            var url = GetSiteUrl().ToString().TrimEnd('/') + _urlResolver.GetUrl(contentReference);

            // Fix to prevent double pren error when behind CDN
            url = url.Replace("/pren/pren/", "/pren/");

            return removeTrailingSlash ? url.TrimEnd('/') : url;
        }

        public Uri GetSiteUrl()
        {
            return SiteDefinition.Current.SiteUrl;
        }

        /// <summary>
        /// Function used to replace host in url. Used to to handler Request.Url behind a CDN.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Uri</returns>
        public Uri ReplaceHostName(Uri uri)
        {
            var currentHost = uri.Host;
            var siteSettingHost = GetSiteUrl().Host;

            // Replace currentHost with siteSettingsHost
            if (currentHost.Equals(siteSettingHost)) return uri;
            var builder = new UriBuilder(uri) { Host = siteSettingHost };
            return builder.Uri;
        }
    }
}
