using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class UserMessage : UserControlBase
    {
        #region Events
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            MessageLabel.Text = string.Empty;
            MessageDiv.Visible = false;
        }
        #endregion

        #region Methods
        public void ShowMessage(string translateKey, bool isKey, bool isErrorMessage)
        {
            string message = isKey ? Translate(translateKey) : translateKey;
            MessageLabel.Text = message;
            MessageDiv.Visible = !string.IsNullOrEmpty(message);

            string cssClass = isErrorMessage ? "server-error" : "server-confirmation";
            MessageDiv.Attributes.Add("class", cssClass);
        }

        public void ClearMessage()
        {
            MessageLabel.Text = string.Empty;
            MessageDiv.Attributes.Remove("class");
            MessageDiv.Visible = false;
        }
        #endregion
    }
}