using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Membership;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class ArchiveSearch : DiTemplatePage
    {
        #region Properties
        public HiddenField HiddenFieldSelectedTab
        {
            get
            {
                return SelectedTabHiddenField;
            }
        }

        public HyperLink HyperLinkTextSearch
        {
            get
            {
                return TextSearchHyperLink;
            }
        }

        public HyperLink HyperLinkDateSearch
        {
            get
            {
                return DateSearchHyperLink;
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    UserMessageControl.ShowMessage("Du måste vara inloggad som prenumerant för att söka i arikvet.", false, true);
                    PlaceHolderNotLoggedIn.Visible = false;
                    return;
                }

                if (User.IsInRole(DiRoleHandler.RoleDiWeekend))
                {
                    PlaceHolderTextSearchHeader.Visible = false;
                    TextSearch1.Visible = false;
                }
            }
            else
            {
                RegisterScript();
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// When a postback is triggered, the selected tab and section should be shown.
        /// </summary>
        private void RegisterScript()
        {
            if (!string.IsNullOrEmpty(SelectedTabHiddenField.Value))
            {
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.AppendLine(@"$(document).ready(function() {");

                scriptBuilder.AppendFormat(@"q.forms.switchSection('{0}');", SelectedTabHiddenField.Value);
                scriptBuilder.AppendLine();
                scriptBuilder.AppendLine(@"});");

                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SelectedTab", scriptBuilder.ToString(), true);
            }
        }
        #endregion
    }
}