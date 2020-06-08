using System;
using System.Collections.Generic;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business.DataAccess.Logging;
using Pren.Web.Business.DataAccess.Logging.Entities;

namespace Pren.Web.Tools.Admin.LogSearch
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Sök i logg", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Loggsök", UrlFromUi = "/Tools/Admin/LogSearch/LogSearch.aspx", SortIndex = 1030)]
    public partial class LogSearch : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;
            TxtDateFrom.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TxtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected void BtnClearOnClick(object sender, EventArgs e)
        {
            TxtSource.Text = string.Empty;
            TxtDescription.Text = string.Empty;
            TxtException.Text = string.Empty;
        }

        public IEnumerable<LogEntity> GetResult(DateTime fromdate, DateTime todate, string source, string description, string exception)
        {
            var loggerDataHandler = ServiceLocator.Current.GetInstance<ILoggerDataHandler>();

            return loggerDataHandler.GetLogEntites(fromdate, todate.AddDays(1), source, description, exception);
        }
    }
}