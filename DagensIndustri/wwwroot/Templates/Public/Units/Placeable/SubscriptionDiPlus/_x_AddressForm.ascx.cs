using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable.SubscriptionDiPlus
{
    public partial class _4_AddressForm : EPiServer.UserControlBase
    {
        DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus _page;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            _page = (DagensIndustri.Templates.Public.Pages.SubscriptionDiPlus)Page;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            LiteralPriceExVat.Text = _page.FinalPriceExVat.ToString();
            LiteralPriceIncVat.Text = _page.GetPriceIncVat(_page.FinalPriceExVat).ToString();
        }


        protected void ButtonCreateInvoiceSubs_Click(object sender, EventArgs e)
        {
            string adr_company = InputCompany.Text;
            string adr_companyNum = InputCompanyNumber.Text;
            string adr_streetAdress = InputStreetAdress.Text;
            string adr_streetNumber = InputStreetNum.Text;
            string adr_entrance = InputEntrance.Text;
            string adr_stairs = InputStairs.Text;
            string adr_appartmentNum = InputAppartmentNum.Text;
            string adr_zip = InputZip.Text;
            string adr_city = InputCity.Text;
            string adr_stopOrBox = Input_s_stopOrBox.Text;
            string adr_stopOrBoxNum = Input_s_num.Text;
            string adr_stopOrBoxZip = Input_s_zip.Text;
            string adr_stopOrBoxCity = Input_s_city.Text;

            //_page.CreateInvoiceSubs(adr_company, adr_companyNum, adr_streetAdress, adr_streetNumber, adr_entrance, adr_stairs,
            //                            adr_appartmentNum, adr_zip, adr_city, adr_stopOrBox, adr_stopOrBoxNum,
            //                            adr_stopOrBoxZip, adr_stopOrBoxCity);
        }

    }
}