using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;


namespace DagensIndustri.Templates.Public.Pages.DiCampaign
{
    public partial class CampaignAdr : DiTemplatePage
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //((DiTemplatePage)Page).ShowMessage("/mba/message/error", true, true);
            //ShowMessage("/mba/message/error", true, true);
        }

        protected void ButtonRedir_Click(object sender, EventArgs e)
        {
            //Response.Write(GetRedirUrl());
            Response.Redirect(GetRedirUrl());
        }


        private string GetRedirUrl()
        {
            PageData parent = EPiServer.DataFactory.Instance.GetPage(CurrentPage.ParentLink);
            UrlBuilder url = new UrlBuilder(parent.LinkURL);
            EPiServer.Global.UrlRewriteProvider.ConvertToExternal(url, parent.PageLink, System.Text.UTF8Encoding.UTF8);
            return url.ToString() + "?adr=" + TextBoxCode.Text;
        }

    }
}