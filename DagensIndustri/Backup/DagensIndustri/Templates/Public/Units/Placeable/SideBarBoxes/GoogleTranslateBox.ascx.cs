using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes;
using EPiServer.Core;
using System.Web.UI.HtmlControls;


namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class GoogleTranslateBox : EPiServer.UserControlBase
    {
        public int PageID { get; set; }

        //protected override void OnLoad(EventArgs e)
        //{
            //base.OnLoad(e);
            
            //line is inserted multiple times when added this way. Added line in header.ascx instead
            //HtmlMeta meta = new HtmlMeta();
            //meta.Name = "google-translate-customization";
            //meta.Content = "7660c05ddb7537f7-337b6759ceb9c498-g60ccd5d6edca6eda-11";
            //this.Page.Header.Controls.Add(meta);
        //}
    }
}