using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class SignUpTop : EPiServer.UserControlBase
    {
        public string SignUpFlowPageURL { get; set; }
        public string payMethodCreditCard = "1";
        public string payMethodInvoice = "2";



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (EPiFunctions.SettingsPageSetting(CurrentPage, "SignUpFlowPage") != null)
            {
                SignUpFlowPageURL = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "SignUpFlowPage") as PageReference).LinkURL;
            }

            if (CurrentPage["HideCreditCard"] != null)
                PlaceHolderCreditCard.Visible = false;

            if (CurrentPage["HideInvoice"] != null)
                PlaceHolderInvoice.Visible = false;

            if (CurrentPage["IsFullBooked"] != null)
            {
                HidePaymentOptions.Visible = false;
                FullBookedPlaceHolder.Visible = true;
            }

        }

        protected void BuyLinkedButton_Click(object sender, EventArgs e)
        {
        }

        protected void PayWithCreditCardLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetUrl(payMethodCreditCard));
        }

        protected void PayWithInvoiceLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetUrl(payMethodInvoice));
        }


        private string GetUrl(string payMethod)
        {
            return SignUpFlowPageURL + "&eventId=" + CurrentPage.PageLink.ID.ToString() + "&pm=" + payMethod + "&nop=" + QuantityTextBox.Text;
        }


    }
}