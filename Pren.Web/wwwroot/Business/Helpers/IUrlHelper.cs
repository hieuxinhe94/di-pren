using System;
using EPiServer.Core;

namespace Pren.Web.Business.Helpers
{
    public interface IUrlHelper
    {
        string GetContentUrlWithHost(ContentReference contentReference, bool removeTrailingSlash = false);

        Uri GetSiteUrl();

        Uri ReplaceHostName(Uri uri);
    }
}
