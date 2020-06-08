using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business.DataAccess.Logging;
using Pren.Web.Business.DataAccess.Logging.Entities;

namespace Pren.Web.Tools.Admin.InfoSearch
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Sök logg info", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Logginfosök", UrlFromUi = "/Tools/Admin/InfoSearch/InfoSearch.aspx", SortIndex = 1040)]
    public partial class InfoSearch : System.Web.UI.Page
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

        /// <summary>
        /// Clear all input fields
        /// </summary>
        protected void BtnClearOnClick(object sender, EventArgs e)
        {
            TxtDescription.Text = string.Empty;           
        }

        /// <summary>
        /// Select method on ObjectDataSource
        /// </summary>
        public IEnumerable<LogInfoEntity> GetResult(DateTime fromdate, DateTime todate, string source, string description)
        {
            var loggerDataHandler = ServiceLocator.Current.GetInstance<ILoggerDataHandler>();

            return loggerDataHandler.GetLogInfoEntites(fromdate, todate, source, description);
        }
    }
}