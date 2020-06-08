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
using DIClassLib.Subscriptions;
using System.Text;
using System.Net.Mail;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;



namespace DagensIndustri.Templates.Public.Pages.InviteFriend
{
    public partial class InviteFriends : DiTemplatePage
    {

        #region Properties

        private const string QSTCODE = "code";
        private const string LinkText = "Klicka här för att komma till erbjudandet";
        private const string SentText = "Ditt tips har skickats. Tipsa gärna en till.";

        public long CusNo
        {
            get { return ViewState["CusNo"] != null ? (long)ViewState["CusNo"] : 0; }
            set { ViewState["CusNo"] = value; }
        }

        public Guid PrenGuid
        {
            get { return (Guid)ViewState["PrenGuid"]; }
            set { ViewState["PrenGuid"] = value; }
        }

        #endregion

        #region Events

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ((MasterPages.MasterPage)Page.Master).ShowSideBarBoxes(false);
            ((MasterPages.MasterPage)Page.Master).ContentWrapperClass = "class='invite'";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                SetUpPage();
            }
        }

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            try
            {
                PrenGuid = Guid.NewGuid();
                var body = new StringBuilder();
                var subject = PopulatePlaceHolders(CurrentPage["subject"] as string, false); 

                MailMessage msg = new MailMessage(CurrentPage["senderaddress"] as string, TxtFriendEmail.Text);
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                
                body.Append("<html><head><title>" + subject  + "</title><body style='font-family:arial;'>");
                body.Append(PopulatePlaceHolders(CurrentPage["body"] as string, true));
                body.Append("</body></html>");

                msg.Body = body.ToString();
                    

                MiscFunctions.SendMail(msg);

                MsSqlHandler.InsertInviteFriend(CurrentPage.PageLink.ID, CusNo, TxtSenderFirstName.Text, TxtSenderLastName.Text, TxtSenderMessage.Text, TxtFriendFirstName.Text, TxtFriendLastName.Text, TxtFriendEmail.Text, TxtFriendPhone.Text, false, PrenGuid);

                ClearForm();

                //Show message
                LblMsg.Text = SentText;
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Success", "$(document).ready(function () { displayArea('.form-box .success'); });", true);
            }
            catch (Exception ex)
            {
                LblError.Text = "Ett fel har uppstått, ditt meddelande kunde inte skickas.";
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Error", "$(document).ready(function () { displayArea('.form-box .error'); });", true);
            }
        }


        #endregion

        #region Methods and functions

        private void SetUpPage()
        {
            var valid = true;
            var errorText = string.Empty;

            if (!MiscFunctions.IsValidEmail(CurrentPage["senderaddress"] as string))
            {
                valid = false;
                errorText = "Du har angett en felaktig e-postadress";
            }

            if (CurrentPage.WorkPageID == 0 && IsValue("MandatoryCode") && !TryPopulateForm())
            {
                valid = false;
                errorText = CurrentPage["MandatoryText"] as string;
            }           

            LblFormError.Text = errorText;           
            PhForm.Visible = valid;

            TryPopulateForm();
        }

        private void ClearForm()
        {
            //Clear form
            TxtFriendFirstName.Text = string.Empty;
            TxtFriendLastName.Text = string.Empty;
            TxtFriendEmail.Text = string.Empty;
            TxtFriendPhone.Text = string.Empty;
        }

        private bool TryPopulateForm()
        {
            var code = Request.QueryString[QSTCODE];

            if (!MiscFunctions.IsGuid(code))
                return false;

            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    CusNo = MsSqlHandler.MdbGetCusnoByCode(new Guid(code));
                    if (CusNo > 0)
                    {
                        var subscriber = new SubscriptionUser2(CusNo);                        
                        var name = subscriber.IsCompanyCust ? subscriber.RowText2 : subscriber.RowText1;
                        
                        if (name != null)
                        {
                            var nameArray = name.Split(' ');
                            TxtSenderLastName.Text = nameArray.Length > 0 ? nameArray[0] : string.Empty;
                            TxtSenderFirstName.Text = nameArray.Length > 1 ? nameArray[1] : string.Empty;
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    new Logger("InviteFriends.TryPopulateForm() for url code '" + code + "' failed", ex.ToString());
                }
            }

            return false;
        }

        /// <summary>
        /// Replaces all placeholders [xxx] with values from form
        /// </summary>
        private string PopulatePlaceHolders(string text, bool isBody)
        {
            text = text.Replace("[sfname]", TxtSenderFirstName.Text);
            text = text.Replace("[slname]", TxtSenderLastName.Text);
            text = text.Replace("[fname]", TxtFriendFirstName.Text);
            text = text.Replace("[lfname]", TxtFriendLastName.Text);            

            if (isBody)
            {
                //Link to campaign
                var linkformat = "<a href='{0}?tref={1}'>" + LinkText + "</a>";                
                var campaignUrl = EPiFunctions.GetFriendlyAbsoluteUrl(new PageReference(int.Parse(CurrentPage["campaign"].ToString())));
                text = text.Replace("[link]", string.Format(linkformat, campaignUrl, PrenGuid));
                //Logo
                var logo = "<img src='" + EPiServer.Configuration.Settings.Instance.SiteUrl + "/templates/public/images/logo.png' />";
                text = text.Replace("[logo]", logo);
                //Sender message
                text = text.Replace("[msg]", TxtSenderMessage.Text);
            }

            return text;
        }

        #endregion

    }
}