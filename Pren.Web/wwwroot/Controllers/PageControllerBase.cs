using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Web.Mvc;
using Pren.Web.Business;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;
using System;
using System.Web.Mvc;

namespace Pren.Web.Controllers
{
    /// <summary>
    /// All controllers that renders pages should inherit from this class so that we can 
    /// apply action filters, such as for output caching site wide, should we want to.
    /// </summary>
    public abstract class PageControllerBase<T> : PageController<T>, IModifyLayout where T : SitePageData 
    {

        public virtual void ModifyLayout(LayoutModel layoutModel)
        {

        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!PageEditing.PageIsInEditMode && PageContext?.Page != null && PageContext.Page.StopPublish <= DateTime.Now)
            {
                var pageType = PageContext.Page.PageTypeName;

                if (pageType == "CampaignPageIframe" || pageType == "OrderFlowCampaignPage" || pageType == "CampaignPageSplus")
                {
                    IContentRepository contentRepository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
                    var startPage = contentRepository.Get<StartPage>(ContentReference.StartPage);
                    var campaignNotActivePage = startPage.CampaignNotActivePage;

                    if (campaignNotActivePage != null)
                    {
                        filterContext.Result = Redirect(EPiServer.Web.Routing.UrlResolver.Current.GetUrl(campaignNotActivePage));
                        return;
                    }
                    else
                    {
                        filterContext.Result = new HttpStatusCodeResult(404, "Not found");
                        return;
                    }
                }
                else
                {
                    base.OnAuthorization(filterContext);
                }
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}
