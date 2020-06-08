using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Tools.Reports
{
    [GuiPlugIn(
    Area = PlugInArea.ReportMenu,
    Description = "Di Members Report",
    Category = "Di Reports", DisplayName = "Di Members",
    Url = "~/Tools/Reports/DiMembers.aspx")]
    public partial class DiMembers : EPiServer.TemplatePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.MasterPageFile = ResolveUrlFromUI("MasterPages/EPiServerUI.master");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DiRegularLiteral.Text = MsSqlHandler.GetNumCustsInRole(1).ToString();
            DiGoldLiteral.Text = MsSqlHandler.GetNumCustsInRole(2).ToString();
        }
    }
}