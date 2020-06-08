using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;



namespace DagensIndustri.Templates.Public.Pages.Campaign
{
    public partial class EventPage : CampaignTemplatePage
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