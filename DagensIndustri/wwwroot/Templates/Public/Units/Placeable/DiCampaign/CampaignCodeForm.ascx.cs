using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignCodeForm : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //DataBind();

        }

        protected void CodeFormButton_Click(object sender, EventArgs e)
        { 
            Response.Redirect(CurrentPage.LinkURL + "&excus=" + CodeInput.Text);
        }
    }
}