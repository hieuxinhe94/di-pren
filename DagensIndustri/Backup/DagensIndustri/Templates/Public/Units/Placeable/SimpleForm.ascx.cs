using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class SimpleForm : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void SimpleFormButton_Click(object sender, EventArgs e)
        {
            try
            {
                string successMessage = CurrentPage["SuccessMessage"].ToString() ?? Translate("/common/forms/message/success");

                MsSqlHandler.InsertSimpleFormEntry(CurrentPage.PageLink.ID, FirstNameInput.Text, LastNameInput.Text, EmailInput.Text, TelephoneInput.Text, MessageInput.Text);
                ClearForm();
                UserMessageControl.ShowMessage(successMessage, true, false);
            }
            catch (Exception ex)
            {
                new Logger("InsertSimpleFormEntry() - failed", ex.ToString());
                UserMessageControl.ShowMessage("/common/errors/error", true, true);
            }
        }

        private void ClearForm()
        {
            FirstNameInput.Text = string.Empty;
            LastNameInput.Text = string.Empty;
            EmailInput.Text = string.Empty;
            TelephoneInput.Text = string.Empty;
            MessageInput.Text = string.Empty;
        }
    }
}