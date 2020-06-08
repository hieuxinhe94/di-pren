using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Campaign;
using DIClassLib.DbHandlers;
using DIClassLib.CardPayment;


namespace DagensIndustri.Templates.Public.Pages.Campaign
{
    /// <summary>
    /// </summary>
    public partial class CampaignRootPage : CampaignTemplatePage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {

            }
        }
    }
}