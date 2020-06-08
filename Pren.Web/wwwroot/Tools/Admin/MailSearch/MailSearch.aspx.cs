using System;
using System.Collections.Generic;
using EPiServer;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business.DataAccess.Logging;
using Pren.Web.Business.DataAccess.Logging.Entities;

namespace Pren.Web.Tools.Admin.MailSearch
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Sök skickade mail", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Loggmailsök", UrlFromUi = "/Tools/Admin/MailSearch/MailSearch.aspx", SortIndex = 1040)]
    public partial class MailSearch : System.Web.UI.Page
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
            TxtToAddress.Text = string.Empty;
            TxtFromAddress.Text = string.Empty;
            TxtBody.Text = string.Empty;
            TxtSubject.Text = string.Empty;
        }

        /// <summary>
        /// Select method on ObjectDataSource
        /// </summary>
        public IEnumerable<LogEmailEntity> GetResult(DateTime fromdate, DateTime todate, string fromaddress, string toaddress, string subject, string body)
        {
            var loggerDataHandler = ServiceLocator.Current.GetInstance<ILoggerDataHandler>();

            return loggerDataHandler.GetLogEmailEntites(fromdate, todate.AddDays(1), fromaddress, toaddress, subject,body); 

        }
    }
}