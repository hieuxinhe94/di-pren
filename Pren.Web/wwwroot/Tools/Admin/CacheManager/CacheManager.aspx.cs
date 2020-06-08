using System;
using System.Linq;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business.Cache;

namespace Pren.Web.Tools.Admin.CacheManager
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Verktyg för att rensa cache", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Cache manager", UrlFromUi = "/Tools/Admin/CacheManager/CacheManager.aspx", SortIndex = 2050)]
    public partial class CacheManager : System.Web.UI.Page
    {
        private IObjectCache _objectCache;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            _objectCache = ServiceLocator.Current.GetInstance<IObjectCache>();

            BindDataSource();
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected void BtnRemoveClick(object sender, EventArgs e)
        {
            var cacheKey = ((Button) sender).CommandArgument;
            _objectCache.RemoveFromCache(cacheKey);

            BindDataSource();
        }

        protected void BtnRefreshClick(object sender, EventArgs e)
        {
            BindDataSource();
        }

        private void BindDataSource()
        {
            RepCacheItems.DataSource = _objectCache.GetSiteCacheInfo().Where(t => !t.Key.StartsWith("Pren.Web_MyPage_")).OrderBy(t => t.Key);
            RepCacheItems.DataBind();
        }
    }
}