using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Pages.DiGasell
{
    public partial class GasellWinners : DiTemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DiLinkCollection GasellWinnerCollection = new DiLinkCollection(CurrentPage, "GasellWinnersCollection");
                
            if(GasellWinnerCollection.SelectedPages().Count > 0)
                GasellWinnersPageList.DataSource = GasellWinnerCollection.SelectedPages();
            else
                GasellWinnersPageList.DataSource = EPiServer.DataFactory.Instance.GetChildren(CurrentPage.PageLink);
                
            GasellWinnersPageList.DataBind();                 
        }
    }
}