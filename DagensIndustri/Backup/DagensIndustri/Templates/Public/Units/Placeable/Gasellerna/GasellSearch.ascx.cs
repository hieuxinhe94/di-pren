using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable.Gasellerna
{
    public partial class GasellSearch : GasellUserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            DiLinkCollection searchLinksCollection = new DiLinkCollection(ActualCurrentPage, "SearchLinks");
            PlLinks.DataSource = searchLinksCollection.SelectedPages();
            PlLinks.DataBind();
        }
    }
}