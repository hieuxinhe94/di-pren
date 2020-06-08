using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class SideBar : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DiLinkCollection SideBarListCollection = new DiLinkCollection(CurrentPage, "SidebarBoxList");
            SideBarList.DataSource = SideBarListCollection.SelectedPages();
            SideBarList.DataBind();
        }
    }
}