using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using EPiServer;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.DiPlus;


namespace DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus
{
    public partial class _1_SelectSubsType : EPiServer.UserControlBase
    {

        DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus _page;


        protected void Page_Load(object sender, EventArgs e)
        {
            _page = (DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus)Page;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            LiteralPriceNewIncVat.Text = DiPlusCampaigns.StandAlone.PriceIncVat.ToString();     //_page.GetPriceIncVat(_page.PriceStandAloneDiPlus).ToString();
            LiteralPriceNewExVat.Text = DiPlusCampaigns.StandAlone.PriceExVat.ToString();       //_page.PriceStandAloneDiPlus.ToString();

            //LiteralPriceNew.Text = _page.GetPriceIncVat(_page.PriceStandAloneDiPlus).ToString();
            //LiteralPriceUpgrade.Text = _page.GetPriceIncVat(_page.PriceUpgDi6DaySub).ToString();
        }


        protected void ButtonNewSubs_Click(object sender, EventArgs e)
        {
            _page.SetIsNewSubs(true);
        }

        protected void ButtonUpgradeSubs_Click(object sender, EventArgs e)
        {
            _page.SetIsNewSubs(false);
        }

    }
}