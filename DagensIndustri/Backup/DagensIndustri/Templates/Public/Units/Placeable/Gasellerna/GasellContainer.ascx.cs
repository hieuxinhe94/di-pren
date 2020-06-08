using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.Gasellerna
{
    public partial class GasellContainer : GasellUserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //no check !isPostBack, PageList is stupid and doesn't viewstate
            PlGasellContainer.PageLink = EPiFunctions.SettingsPage(CurrentPage)["GasellContainer"] as PageReference;

        }
    }
}