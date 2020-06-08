using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Subscriptions.DiPlus;


namespace DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus
{
    public partial class _3_ThankYou : EPiServer.UserControlBase
    {
        DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus _page;


        protected void Page_Load(object sender, EventArgs e)
        {
            _page = (DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus)Page;
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Request.QueryString["pay"] != null)
            {
                string s = Request.QueryString["pay"];
                
                if (s == "1")
                    LiteralPrice.Text = DiPlusCampaigns.StandAlone.PriceIncVat.ToString();
                
                if (s == "2")
                    LiteralPrice.Text = DiPlusCampaigns.Upgrade6Days.PriceIncVat.ToString();

                if (s == "3")
                    LiteralPrice.Text = DiPlusCampaigns.UpgradeWeekend.PriceIncVat.ToString();
            }
        }


    }
}