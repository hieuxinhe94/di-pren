using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.Shop;

namespace DagensIndustri.Templates.Public.Units.Placeable.Shop
{
    public partial class ShopProductDescription : System.Web.UI.UserControl
    {
        #region Properties
        public Product ShopProduct { get; set; }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (ShopProduct != null)
            {
                MainIntroLiteral.Text = EPiServer.Core.Html.TextIndexer.StripHtml(ShopProduct.MainIntro, 0);
                
                //Remove starting and ending <div> tags if exists.
                if (!string.IsNullOrEmpty(ShopProduct.MainBody))
                {
                    string modifiedMainBody = (ShopProduct.MainBody.StartsWith("<div>") && ShopProduct.MainBody.EndsWith("</div>"))
                        ? ShopProduct.MainBody.Remove(0, 5).Remove(ShopProduct.MainBody.Length - 6, 6)
                        : ShopProduct.MainBody;

                    //Add <p>-tags if the main body is not surrounded with it.
                    if (!modifiedMainBody.StartsWith("<p>") && !modifiedMainBody.EndsWith("</p>"))
                        modifiedMainBody = string.Format("<p>{0}</p>", modifiedMainBody);

                    MainBodyLiteral.Text = modifiedMainBody;
                }
            }
        }
        #endregion
    }
}