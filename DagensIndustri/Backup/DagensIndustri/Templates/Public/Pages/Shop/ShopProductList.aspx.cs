using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Shop;

namespace DagensIndustri.Templates.Public.Pages.Shop
{
    public partial class ShopProductList : DiTemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                bool topProductImageExists = false;
                if (EPiFunctions.HasValue(CurrentPage, "TopProductPage"))
                {
                    PageData pd = GetPage(CurrentPage["TopProductPage"] as PageReference);
                    TopProductHyperLink.NavigateUrl = EPiFunctions.GetFriendlyAbsoluteUrl(pd);
                    TopProductImage.ImageUrl = pd["TopImage"] as string;
                    topProductImageExists = EPiFunctions.HasValue(pd, "TopImage");
                }
                else if (EPiFunctions.HasValue(CurrentPage, "TopProductIndex"))
                {
                    Product product = Product.CreateAdlibrisShopProduct((int)CurrentPage["TopProductIndex"]);
                    if (product != null)
                    {
                        TopProductHyperLink.NavigateUrl = product.ProductPageUrl;
                        TopProductImage.ImageUrl = product.TopImageUrl;
                        topProductImageExists = !string.IsNullOrEmpty(product.TopImageUrl);
                    }
                }

                TopImagePlaceHolder.Visible = !topProductImageExists;
                TopProductPlaceHolder.Visible = topProductImageExists;
            }
        }
    }
}