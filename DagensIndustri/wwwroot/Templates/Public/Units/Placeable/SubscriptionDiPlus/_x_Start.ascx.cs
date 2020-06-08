using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Subscriptions;


namespace DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus
{
    public partial class _1_Start : EPiServer.UserControlBase
    {
        DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus _page;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _page = (DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus)Page;
            
            LiteralPriceCompany1MonExVat.Text = _page.PriceStandAloneDiPlus.ToString();
            LiteralPricePerson1MonIncVat.Text = _page.GetPriceIncVat(_page.PriceStandAloneDiPlus).ToString();
        }

        
        protected void ButtonCompany_Click(object sender, EventArgs e)
        {
            _page.SetIsCompanyCust(true);
        }

        protected void ButtonPrivate_Click(object sender, EventArgs e)
        {
            _page.SetIsCompanyCust(false);
        }

    }
}