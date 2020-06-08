using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Text;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Pages.Conference
{
    public partial class Conference : DiTemplatePage
    {
        public HiddenField HiddenFieldSelectedTab
        {
            get
            {
                return SelectedTabHiddenField;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("HideForm"))
            {
                FormPlaceHolder.Visible = false;
            }
            else
            {
                FormPlaceHolder.Visible = true;
                RegisterScript();
            }
        }

        /// <summary>
        /// When a postback is triggered, the selected tab and section should be shown.
        /// </summary>
        private void RegisterScript()
        {
            if (!string.IsNullOrEmpty(SelectedTabHiddenField.Value) || !string.IsNullOrEmpty(SelectedSectionHiddenField.Value))
            {
                StringBuilder scriptBuilder = new StringBuilder();
                scriptBuilder.AppendLine(@"$(document).ready(function() {");


                if (!string.IsNullOrEmpty(SelectedTabHiddenField.Value))
                {
                    scriptBuilder.AppendFormat(@"q.forms.switchSection('{0}');", SelectedTabHiddenField.Value);
                    scriptBuilder.AppendLine();
                }

                if (!string.IsNullOrEmpty(SelectedSectionHiddenField.Value))
                {
                    scriptBuilder.AppendFormat(@"q.forms.showEdit('#{0}');", SelectedSectionHiddenField.Value);
                    scriptBuilder.AppendLine();
                }

                scriptBuilder.AppendLine(@"});");

                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ControlShow", scriptBuilder.ToString(), true);
            }
        }

        #region //TODO: Change it?

        public void ShowError(string errorMsg)
        {
            LblError.Text = errorMsg;
            ShowModalWindow("error", "350", "auto");
        }

        public void ShowMessage(string msg)
        {
            LblMessage.Text = msg;
            ShowModalWindow("message", "350", "auto");
        }

        public void ShowModalWindow(string id, string width, string height)
        {
            Page.ClientScript.RegisterClientScriptBlock(
                GetType(),
                id,
                "<script type='text/javascript'>$(function() {showmodal('#" + id + "','" + width + "px', '" + height + "', true);});</script>");
        }

        #endregion

    }
}