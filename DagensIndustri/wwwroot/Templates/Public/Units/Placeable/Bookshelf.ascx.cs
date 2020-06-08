using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Shop;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class Bookshelf : UserControlBase
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (!DIClassLib.ServiceVerifier.ServiceVerifier.AdlibrisIsValid)
                {
                    BookShelfPlaceHolder.Visible = false;
                    return;
                }

                try
                {
                    BookShelfRepeater.DataSource = Product.CreateAdlibrisShopProducts();
                    BookShelfRepeater.DataBind();
                }
                catch (Exception ex)
                {
                    new Logger("Bookshelf OnLoad() - failed", ex.ToString());
                    BookShelfPlaceHolder.Visible = false;
                }
            }
        }
        #endregion
    }
}