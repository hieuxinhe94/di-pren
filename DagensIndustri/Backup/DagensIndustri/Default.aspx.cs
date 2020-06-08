using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;

namespace DagensIndustri
{
    public partial class Default : TemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                TopImageControl.Visible = Request.QueryString["std"] == null && Session["NewSubscriptionStartDate"] == null;
                SubscriptionWelcomeControl.Visible = Request.QueryString["std"] != null || Session["NewSubscriptionStartDate"] != null;

                //If top image control is shown then clear the session regarding new subscription start date. The user is no
                //longer in the subscription flow.
                if (TopImageControl.Visible)
                {
                    Session["NewSubscriptionStartDate"] = null;
                    Session["NewSubscriptionType"] = null;
                }
            }
        }
    }
}