using System.Linq;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Cache;
using Pren.Web.Models.Pages.MySettings.Di;

namespace Pren.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class OnPagePublishEventInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {  
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.PublishingContent += ContentEventsPublishingContent;
        }

        private void ContentEventsPublishingContent(object sender, EPiServer.ContentEventArgs e)
        {
            if (!(e.Content is MyStartPage)) return;

            // Clear MyPage cache items when publishing MyStartPage.
            // This to prevent inconsistent data when toggling BN-API features.
            var cache = ServiceLocator.Current.GetInstance<IObjectCache>();
            var cacheItems = cache.GetSiteCacheInfo().Where(t => t.Key.StartsWith("Pren.Web_MyPage_"));

            foreach (var keyValuePair in cacheItems)
            {
                cache.RemoveFromCache(keyValuePair.Key);
            }            
        }

        public void Preload(string[] parameters)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {   
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.PublishingContent -= ContentEventsPublishingContent;
        }
    }
}