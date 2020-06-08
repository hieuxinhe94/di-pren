using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Shop;

namespace DagensIndustri.Templates.Public.Pages.Shop
{
    public partial class ShopProduct : DiTemplatePage
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack)
            {
                Product product = null;
                //If ISBN was sent in the query string then an adlibris product has to be shown. Otherwise it is product in the shop.
                if (!string.IsNullOrEmpty(Request.QueryString["ISBN"]))
                {
                    product = Product.CreateAdlibrisShopProduct(Request.QueryString["ISBN"]);
                    ShopProductSideBarControl.InformationList = product.AdditionalInformation;
                    ShopProductSideBarPlaceHolder.Visible = true;
                }
                else
                {
                    product = Product.CreateShopProduct(CurrentPage);
                    ShopProductSideBarPlaceHolder.Visible = false;
                }

                ShopProductControl.ShopProduct = product;
                ShopProductDescrControl.ShopProduct = product;
            }
        }
        #endregion
    }
}