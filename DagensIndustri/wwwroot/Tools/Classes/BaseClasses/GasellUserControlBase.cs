using EPiServer.Core;
using System.Web;
using System.Web.UI;
using System;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Tools.Classes.BaseClasses
{
    public class GasellUserControlBase : DiUserControlBase
    {
        /// <summary>
        /// Get main target url.
        /// </summary>
        /// <returns>The target url. Iframepage at DI.se</returns>
        protected string GetMainTargetUrl()
        {
            //return ActualCurrentPage["GasellDiIframeUrl"] as string ?? string.Empty;
            return EPiFunctions.SettingsPage(CurrentPage)["GasellDiIframeUrl"] as string ?? string.Empty;
        }

        /// <summary>
        /// Get main target url with query.
        /// </summary>
        /// <param name="queryName">The name of the query to add</param>
        /// <param name="queryValue">The value of the query to add</param>
        /// <returns>The target url with querystring. Iframepage at DI.se</returns>
        protected string GetMainTargetUrlWithQuery(string queryName, string queryValue)
        {
            string url = "{0}?{1}={2}";

            return string.Format(url, GetMainTargetUrl(), queryName, HttpUtility.UrlEncode(queryValue));
        }

        /// <summary>
        /// Get encoded url from PageLink property
        /// </summary>
        /// <param name="propertyName">The name of the pagelink property</param>
        /// <param name="encode">True if return value should be UrlEncoded</param>
        /// <returns>Encoded friendly url</returns>
        protected string GetUrlFromPageProperty(String propertyName)
        {
            if (ActualCurrentPage[propertyName] != null)
            {
                PageData page = GetPage(ActualCurrentPage[propertyName] as PageReference);

                if (page != null)
                    return GetFriendlyAbsoluteUrl(page);
            }
            return string.Empty;
        }
    }
}