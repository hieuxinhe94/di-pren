using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.ServiceVerifier;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class WineClubPage : DiTemplatePage
    {
        public int wineListPageID = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            wineListPageID = EPiServer.DataFactory.Instance.GetPage(CurrentPage["WineListSidebarPuff"] as PageReference).PageLink.ID;

            if (!ServiceVerifier.ApsisIsValid)
            {
                Wineclubform1.Visible = false;
                UserMessageControl.ShowMessage("/wineclub/servicenotavailable", true, true);
            }

            DataBind();
        }
    }
}