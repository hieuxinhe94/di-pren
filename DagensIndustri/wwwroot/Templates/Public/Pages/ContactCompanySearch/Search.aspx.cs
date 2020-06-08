using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.Membership;

using EPiServer;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.ServiceVerifier;

namespace DagensIndustri.Templates.Public.Pages.ContactCompanySearch
{
    public partial class Search : DiTemplatePage
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            if (!IsPostBack)
            {
                if (!MembershipFunctions.UserAllowedToSeePage(DiRoleHandler.RoleDiGold))
                {
                    UserMessageControl.ShowMessage("/common/message/onlypren", true, true);
                    SearchControl.Visible = false;
                    MainIntro.Visible = false;
                    MainBody.Visible = false;
                    return;
                }

                if(!ServiceVerifier.OBOIsValid)
                {
                    SearchControl.Visible = false;
                    UserMessageControl.ShowMessage("/contactcompanysearch/search/servicenotavailable", true, true);
                }
                EPiFunctions.SetAttributeOnControl(Page.Master, "Body", "class", "bizbook");
            }
        }
        #endregion

        #region Methods
        public void HideControls()
        {
            MainIntro.Visible = false;
            MainBody.Visible = false;
        }        
        #endregion
    }
}