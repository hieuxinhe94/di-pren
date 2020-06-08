using System.Web.Optimization;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Pren.Web.Business.Initialization
{
    [InitializableModule]
    public class BundleConfig : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if (context.HostType == HostType.WebApplication)
            {
                RegisterBundles(BundleTable.Bundles);
            }
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Javascript Bundles
            BundleGlobalScripts(bundles);            
            BundleMyPageScripts(bundles);
            BundleCampaignScripts(bundles);
            BundleBusinessCampaignScripts(bundles);
            BundleBusinessCampaignAdminScripts(bundles);
            BundleCampaignIframeScripts(bundles);
            BundleBusinessCampaignActivationScripts(bundles);
            BundleCodePortalStyles(bundles);
            BundleCodePortalScripts(bundles);
            BundleSurveyScript(bundles);
            BundleOrderFlowScripts(bundles);

            // Style Bundles
            BundleGlobalStyles(bundles);
            BundleMySettingsStyles(bundles);
            BundleDiGlobalStyles(bundles);
            BundleMyPageStyles(bundles);
            BundleCampaignStyles(bundles);
            BundleCampaignIframeStyles(bundles);
            BundleCampaignRedirectStyles(bundles);
            BundleBusinessCampaignStyles(bundles);
            BundleBusinessCampaignAdminStyles(bundles);
            BundleOrderFlowStyles(bundles);
        }

        #region Javascript Bundles
        private static void BundleGlobalScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/global")
                .Include("~/Static/js/lib/jquery-1.11.2.min.js")
                .Include("~/Static/bootstrap/js/bootstrap.min.js")
                .Include("~/Static/js/lib/jquery.validate.min.js")
                .Include("~/Static/js/lib/datepicker/bootstrap-datepicker.js")
                .Include("~/Static/js/lib/datepicker/locales/bootstrap-datepicker.sv.js")
                .Include("~/Static/js/lib/tiny-pubsub.js")
                .IncludeDirectory("~/Static/js/global/", "*.js")
                );
        }

        private static void BundleBusinessCampaignAdminScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/businesscampaignadmin")
                .Include("~/Static/js/lib/underscore-min.js")
                .Include("~/Static/js/businesscampaign/max-account-replacer.js")
                .IncludeDirectory("~/Static/js/businesscampaignadmin/", "*.js"));
        }

        private static void BundleBusinessCampaignScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/businesscampaign")
                .Include("~/Static/js/campaign/postNameHandler.js")
                .IncludeDirectory("~/Static/js/businesscampaign/", "*.js"));
        }

        private static void BundleCampaignIframeScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/businesscampaigniframe")
                .Include("~/Static/js/campaignIframe/slider.js")
                .Include("~/Static/js/campaignIframe/campaign.js")
                .Include("~/Static/js/lib/splus-checkout.js")
                .Include("~/Static/js/lib/iframeResizer.min.js")
                .Include("~/Static/js/lib/jquery.bxslider.js"));
        }

        private static void BundleCampaignScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/campaign")
                .Include("~/Static/js/lib/jquery.bxslider.js")
                .IncludeDirectory("~/Static/js/campaign/", "*.js")
                .IncludeDirectory("~/Static/js/campaign/steps", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/campaignSPlus")
                .Include("~/Static/js/lib/jquery.bxslider.js")
                .IncludeDirectory("~/Static/js/campaignSPlus/", "*.js")
                .IncludeDirectory("~/Static/js/campaignSPlus/steps", "*.js"));
        }

        private static void BundleSurveyScript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/survey")
                .Include("~/Static/js/bundles/chatsurvey-min.js"));
        }


        private static void BundleMyPageScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/mypage")
                .IncludeDirectory("~/Static/js/mypage/", "*.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/mysettings")
                .IncludeDirectory("~/Static/js/bundles/", "mysettings-min.js"));
        }

        private static void BundleBusinessCampaignActivationScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/businessactivation")
                .IncludeDirectory("~/Static/js/businessactivation/", "*.js"));
        }

        private static void BundleCodePortalScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/codeportal")
                .IncludeDirectory("~/Static/js/codeportal/", "*.js"));
        }

        private static void BundleOrderFlowScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js/orderflow")
                .Include("~/Static/js/lib/jquery-1.11.2.min.js")
                .Include("~/Static/js/lib/splus-checkout.js")
                .Include("~/Static/js/lib/iframeResizer.min.js")
                .Include("~/Static/js/orderflow/main.js"));
        }
        #endregion

        #region Style Bundles

        private static void BundleOrderFlowStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/orderflow")
                .Include("~/Static/css/orderflow/css/screen.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/orderflow/css/style.css", new CssRewriteUrlTransform()));
        }

        private static void BundleGlobalStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/global")
                .Include("~/Static/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/lib/datepicker3.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/editmode.css", new CssRewriteUrlTransform()));
        }

        private static void BundleMySettingsStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/mysettings")
                .Include("~/Static/bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/lib/datepicker3.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/editmode.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/di/mysettings.css", new CssRewriteUrlTransform()));
        }

        private static void BundleDiGlobalStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/diglobal")
                .Include("~/Static/css/global.css", new CssRewriteUrlTransform()));
        }

        private static void BundleCampaignStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/campaign")
                .Include("~/Static/css/campaign.css", new CssRewriteUrlTransform()));
        }

        private static void BundleCampaignIframeStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/campaigniframe")
                .Include("~/Static/css/campaign.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/campaigniframe.css", new CssRewriteUrlTransform()));
        }

        private static void BundleCampaignRedirectStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/campaignredirect")
                .Include("~/Static/css/campaign.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/campaignredirect.css", new CssRewriteUrlTransform()));
        }

        private static void BundleMyPageStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/mypage")
                .Include("~/Static/css/global.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/mypage.css", new CssRewriteUrlTransform()));
        }

        private static void BundleBusinessCampaignStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/businesscampaign")
                .Include("~/Static/css/global.css", new CssRewriteUrlTransform())
                .Include("~/Static/css/businesscampaign.css", new CssRewriteUrlTransform()));
        }

        private static void BundleBusinessCampaignAdminStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/businesscampaignadmin")
                .Include("~/Static/css/businesssubscriptionadmin.css", new CssRewriteUrlTransform()));
        }

        private static void BundleCodePortalStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css/codeportal")
                .Include("~/Static/css/codeportal.css", new CssRewriteUrlTransform()));
        }

        #endregion

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
