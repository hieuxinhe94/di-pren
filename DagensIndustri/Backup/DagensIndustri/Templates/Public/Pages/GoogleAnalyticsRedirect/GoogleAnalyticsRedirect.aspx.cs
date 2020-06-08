using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using DagensIndustri.Tools.Classes;

using DIClassLib.Misc;

using EPiServer.Editor.TinyMCE.Plugins;

namespace DagensIndustri.Templates.Public.Pages.GoogleAnalyticsRedirect
{
    /// <summary>
    /// This template is only used for redirection to a landingpage!
    /// It will initiate cookie needed for later tracking on landingpage
    /// before redirection is made.
    /// </summary>
    public partial class GoogleAnalyticsRedirect : EPiServer.TemplatePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!EPiFunctions.HasValue(this.CurrentPage, "UtmSource") || !EPiFunctions.HasValue(this.CurrentPage, "UtmMedium") || !EPiFunctions.HasValue(this.CurrentPage, "LandingPage"))
            {
                ShowError();
                return;
            }
            DoRedirect();
        }

        private void DoRedirect()
        {
            var landingPage = this.CurrentPage["LandingPage"] as string;
            if (string.IsNullOrEmpty(landingPage))
            {
                ShowError();
                return;
            }
            var redirectUrlString = (EPiFunctions.HasValue(this.CurrentPage, "IncludePropertiesAsQueryString")) ?
                BuildRedirectUrlWithGoogleParams(landingPage) : landingPage;

            //Set cookie here and read it in landingpage to use cookievalues when building Google Analytics virtual url
            var cookieValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(GoogleAnalyticsHelper.GoogleTrackParams.utm_source.ToString(), GetPageStringProperty("UtmSource")),
                new KeyValuePair<string, string>(GoogleAnalyticsHelper.GoogleTrackParams.utm_medium.ToString(), GetPageStringProperty("UtmMedium")),
                new KeyValuePair<string, string>(GoogleAnalyticsHelper.GoogleTrackParams.utm_campaign.ToString(), GetPageStringProperty("UtmCampaign")),
                new KeyValuePair<string, string>(GoogleAnalyticsHelper.GoogleTrackParams.utm_term.ToString(), GetPageStringProperty("UtmTerm")),
                new KeyValuePair<string, string>(GoogleAnalyticsHelper.GoogleTrackParams.utm_content.ToString(), GetPageStringProperty("UtmContent"))
            };
            GoogleAnalyticsHelper.SetCookie(this.Context, GoogleAnalyticsHelper.CookieName, cookieValues, GetCookieLifeTime());
            Response.Redirect(redirectUrlString, true);
        }

        private int GetCookieLifeTime()
        {
            return this.CurrentPage["CookieLifeTimeInMinutes"] != null ? (int)this.CurrentPage["CookieLifeTimeInMinutes"] : 10;
        }

        private string BuildRedirectUrlWithGoogleParams(string urlString)
        {
            var landingPageUri = new Uri(urlString);
            var urlBuilder = new UriBuilder(landingPageUri);
            var httpValueCollection = HttpUtility.ParseQueryString(landingPageUri.Query);

            // Read more: https://support.google.com/analytics/answer/1033863?hl=sv&ref_topic=1032998
            httpValueCollection.Add(GoogleAnalyticsHelper.GoogleTrackParams.utm_source.ToString(), GetPageStringProperty("UtmSource"));
            httpValueCollection.Add(GoogleAnalyticsHelper.GoogleTrackParams.utm_medium.ToString(), GetPageStringProperty("UtmMedium"));
            httpValueCollection.Add(GoogleAnalyticsHelper.GoogleTrackParams.utm_campaign.ToString(), GetPageStringProperty("UtmCampaign"));
            httpValueCollection.Add(GoogleAnalyticsHelper.GoogleTrackParams.utm_term.ToString(), GetPageStringProperty("UtmTerm"));
            httpValueCollection.Add(GoogleAnalyticsHelper.GoogleTrackParams.utm_content.ToString(), GetPageStringProperty("UtmContent"));
            urlBuilder.Query = httpValueCollection.ToString();
            return urlBuilder.ToString();
        }

        private string GetPageStringProperty(string propertyName)
        {
            return this.CurrentPage[propertyName] as string ?? string.Empty;
        }

        private void ShowError()
        {
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Status = "404 Filen existerar inte";
            Response.StatusCode = 404;
            Response.StatusDescription = "Sidan du försöker nå finns inte.";
            Response.Flush();
        }
    }
}