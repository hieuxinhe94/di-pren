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

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class Page : DiTemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            if (EPiFunctions.IsMatchingPageType(EPiServer.DataFactory.Instance.GetPage(CurrentPage.ParentLink), EPiServer.DataFactory.Instance.GetPage(CurrentPage.ParentLink).PageTypeID, "ConferencePageType"))
            {
                ConferenceSubMenuPlaceHolder.Visible = true;
            }

            if (Request.QueryString["UserMessage"] != null)
            {
                if (EPiFunctions.HasValue(CurrentPage, "UserMessage"))
                {
                    //bool isError = EPiFunctions.HasValue(CurrentPage, "UserMessageIsError");
                    UserMessageControl.ShowMessage(CurrentPage["UserMessage"].ToString(), false, false);
                }
            }
        }
    }
}