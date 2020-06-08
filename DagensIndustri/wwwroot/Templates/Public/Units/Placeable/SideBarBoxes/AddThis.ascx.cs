using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes;
using EPiServer.Core;


namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class AddThis : EPiServer.UserControlBase
    {

        public int PageID { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (PageID > 0)
                CurrentPage = EPiServer.DataFactory.Instance.GetPage(new PageReference(PageID));

            PhAddthis.DataBind();
        }
    }
}