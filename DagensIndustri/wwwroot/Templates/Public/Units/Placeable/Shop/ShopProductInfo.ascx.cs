using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Shop;

namespace DagensIndustri.Templates.Public.Units.Placeable.Shop
{
    public partial class ShopProductInfo : UserControlBase
    {
        #region Properties
        public Product ShopProduct 
        { 
            get
            {
                return (Product)ViewState["ShopProduct"];
            }
            set
            {
                ViewState["ShopProduct"] = value;
            }
        }

        public string shopFlowPageURL { get; set; }
        public string payMethodCreditCard = "1";
        public string payMethodInvoice = "2";
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (EPiFunctions.SettingsPageSetting(CurrentPage, "ShopFlowPage") != null)
            //{
            //    PageData ShopFlowPage = new PageData();

            //    ShopFlowPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "ShopFlowPage") as PageReference);

            //    shopFlowPageURL = ShopFlowPage.LinkURL;
            //}

            DataBind();

            RegisterScript();
        }

        protected void BuyLinkedButton_Click(object sender, EventArgs e)
        {
        }

        protected void PayWithCreditCardLinkButton_Click(object sender, EventArgs e)
        {
            //Response.Redirect(shopFlowPageURL + "&productid=" + CurrentPage.PageLink.ID.ToString() + "&pm=" + payMethodCreditCard + "&nop=" + QuantityTextBox.Text);
        }

        protected void PayWithInvoiceLinkButton_Click(object sender, EventArgs e)
        {
            //Response.Redirect(shopFlowPageURL + "&productid=" + CurrentPage.PageLink.ID.ToString() + "&pm=" + payMethodInvoice + "&nop=" + QuantityTextBox.Text);
        }

        protected void AdLibrisButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("http://www.adlibris.com/di/product.aspx?isbn=" + ShopProduct.ProductNumber);
        }
        #endregion

        #region Methods
        private void RegisterScript()
        {
            DiGoldMembershipPopup.DiGoldMembershipPopup diGoldmembershipPopup = EPiFunctions.FindDiGoldMembershipPopup(Page) as DiGoldMembershipPopup.DiGoldMembershipPopup;
            if (diGoldmembershipPopup != null)
            {
                //Register script for Buy hyperlink
                //diGoldmembershipPopup.RegisterSetReturnURLScript(MembershipRequiredBuyHyperLink, ShopProduct.ProductPageUrl);
            }
        }
        #endregion
    }
}