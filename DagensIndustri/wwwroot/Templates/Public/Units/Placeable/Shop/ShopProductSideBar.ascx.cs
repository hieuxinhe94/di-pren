using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;

namespace DagensIndustri.Templates.Public.Units.Placeable.Shop
{
    public partial class ShopProductSideBar : UserControlBase
    {
        #region Properties
        public List<string> InformationList { get; set; }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                InformationRepeater.DataSource = InformationList;
                InformationRepeater.DataBind();
            }
        }
        #endregion
    }
}