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
    public partial class GasellMeetingTop : EPiServer.UserControlBase
    {
        public string gasellFlowPageURL { get; set; }
        public string payMethodCreditCard = "1";
        public string payMethodInvoice = "2";
        public string payMethodDiscountCode = "3";

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (EPiFunctions.SettingsPageSetting(CurrentPage, "GasellFlowPage") != null)
            {
                PageData GasellFlowPage = new PageData();

                GasellFlowPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "GasellFlowPage") as PageReference);

                gasellFlowPageURL = GasellFlowPage.LinkURL;

                if (CurrentPage["GasellIsFullBooked"] != null)
                {
                    HidePaymentOptions.Visible = false;
                    FullBookedPlaceHolder.Visible = true;
                }
            }
        }

        protected void BuyLinkedButton_Click(object sender, EventArgs e)
        {
        }

        protected void PayWithCreditCardLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(gasellFlowPageURL + "&gasellid=" + CurrentPage.PageLink.ID.ToString() + "&pm=" + payMethodCreditCard + "&nop=" + QuantityTextBox.Text);
        }

        protected void PayWithInvoiceLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(gasellFlowPageURL + "&gasellid=" + CurrentPage.PageLink.ID.ToString() + "&pm=" + payMethodInvoice + "&nop=" + QuantityTextBox.Text);
        }

        protected void PayWithDiscountCodeLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(gasellFlowPageURL + "&gasellid=" + CurrentPage.PageLink.ID.ToString() + "&pm=" + payMethodDiscountCode + "&nop=" + QuantityTextBox.Text);
        }

        #endregion
    }
}