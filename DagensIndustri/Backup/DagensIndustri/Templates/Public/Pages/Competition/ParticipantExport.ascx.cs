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

namespace DagensIndustri.Templates.Public.Pages.Competition
{
    [GuiPlugIn(Area = PlugInArea.EditPanel, Description = "Exportera deltagare", RequiredAccess = EPiServer.Security.AccessLevel.Edit, DisplayName = "Exportera deltagare", UrlFromUi = "/Templates/Public/Pages/Competition/ParticipantExport.ascx")]
    public partial class ParticipantExport : UserControlBase, ICustomPlugInLoader
    {
        protected void BtnGetParticipantsOnClick(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            GvParticipants.DataSource = MsSqlHandler.GetParticipants(CurrentPage.PageLink.ID);
            GvParticipants.DataBind();
        }

        public PlugInDescriptor[] List()
        {
            //Only show plugin on Competition pagetype
            if (!EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "CompetitionPageType"))
                return new PlugInDescriptor[] { };

            return new[] { PlugInDescriptor.Load(typeof(ParticipantExport)) };
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

        protected void BtnExportOnClick(object sender, EventArgs e)
        {
            BindGrid();
            //Make a copy of gridview
            GridView gvToExport = GvParticipants;
            gvToExport.GridLines = GridLines.Both;
            gvToExport.AllowPaging = false;

            //Remove first three columns
            gvToExport.Columns.RemoveAt(0);
            gvToExport.DataBind();

            //Export to excel
            GridViewExportUtil.Export(CurrentPage.PageName.Replace(" ", "_") + "_export.xls", gvToExport);
        }

        protected void GvParticipantsPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvParticipants.PageIndex = e.NewPageIndex;
            BindGrid();
        }

    }
}