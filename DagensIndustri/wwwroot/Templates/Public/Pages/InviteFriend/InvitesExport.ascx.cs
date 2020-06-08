using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.PlugIn;
using EPiServer;
using EPiServer.Core;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Extras;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Pages.InviteFriend
{
    [GuiPlugIn(Area = PlugInArea.EditPanel, Description = "Exportera tips", RequiredAccess = EPiServer.Security.AccessLevel.Edit, DisplayName = "Exportera tips", UrlFromUi = "/Templates/Public/Pages/InviteFriend/InvitesExport.ascx")]
    public partial class InvitesExport : UserControlBase, ICustomPlugInLoader
    {
        protected void BtnGetInvitesOnClick(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            GvInvites.DataSource = MsSqlHandler.GetInviteFriend(CurrentPage.PageLink.ID, TxtCusno.Text);
            GvInvites.DataBind();
        }

        public PlugInDescriptor[] List()
        {
            //Only show plugin on TipAFriend pagetype
            if (!EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "TipaFriendPageType"))
                return new PlugInDescriptor[] { };

            return new[] { PlugInDescriptor.Load(typeof(InvitesExport)) };
        }


        protected string GetReaderValue(string id)
        {
            switch (id) 
            { 
                case "0":
                    return "Jag har en prenumeration hem";
                case "1":
                    return "När jag kommer över den";
                default:
                    return "Inte alls";
            }
        }

        protected void BtnInvitesOnClick(object sender, EventArgs e)
        {
            BindGrid();
            //Make a copy of gridview
            GridView gvToExport = GvInvites;
            gvToExport.GridLines = GridLines.Both;
            gvToExport.AllowPaging = false;

            //Remove first three columns
            gvToExport.Columns.RemoveAt(0);
            gvToExport.DataBind();

            //Export to excel
            GridViewExportUtil.Export(CurrentPage.PageName.Replace(" ","_") + "_export.xls", gvToExport);
        }

        protected void GvInvitesPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvInvites.PageIndex = e.NewPageIndex;
            BindGrid();
        }

    }
}