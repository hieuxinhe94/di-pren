using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.Gasellerna
{
    public partial class GasellAdContainer : GasellUserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //no check !isPostBack, PageList is stupid and doesn't viewstate
            PlAdContainer.PageLink = EPiFunctions.SettingsPage(CurrentPage)["GasellAdsContainer"] as PageReference;

        }
    }
}