using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace DagensIndustri.Tools.Classes.BaseClasses
{
    public class CampaignTemplatePage : EPiServer.TemplatePage
    {

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);

            //if property is set, change masterpage file
            if (IsValue("OnlyShowColumn2"))
                this.MasterPageFile = "/Templates/DI/MasterPages/MasterPageWide.master";
        }

    }
}