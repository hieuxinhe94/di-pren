using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Threading;
using System.Net.Mail;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Conference;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DagensIndustri.Templates.Public.Pages;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Units.Placeable.Conference
{
    public partial class ConferenceGroupRegistration : EPiServer.UserControlBase
    {
        #region Properties

        private Pages.Conference.Conference Conference
        {
            get
            {
                return (Pages.Conference.Conference)Page;
            }
        }

        /// <summary>
        /// Get the container page of this usercontrol
        /// </summary>
        private Units.Placeable.Conference.ConferenceApplicationForm ParentControl
        {
            get
            { 
                return (Units.Placeable.Conference.ConferenceApplicationForm) Parent;
            }
        }

        protected string CaptchaTitle { get; set; }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            if (CurrentPage["LanguageEnglish"] != null)
            {
                System.Globalization.CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
                System.Globalization.CultureInfo oldUICulture = Thread.CurrentThread.CurrentUICulture;
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            if (!IsPostBack)
            {
                SetCaptchaDetails();
            }
            DataBind();

            RegisterScript();


            if (CurrentPage["HideForm"] == null)
            {
                FormHiddenTextPlaceHolder.Visible = false;
            }
            else
            {
                if (IsValue("HideFormText"))
                    FormHiddenTextLiteral.Text = CurrentPage["HideFormText"].ToString();

                GroupRegistrationPlaceHolder.Visible = false;
            }
        }

        protected void ConferenceGroupRegistrationButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateCaptcha())
                {
                    ((DiTemplatePage)Page).ShowMessage("/conference/forms/registration/captcha.error", true, true);
                    SetCaptchaDetails();
                    return;
                }
                
                if (Page.IsValid)
                {
                    string mailFrom = EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailFrom").ToString();
                    if (string.IsNullOrEmpty(mailFrom)) mailFrom = "no-reply@di.se";

                    //Create message
                    MailMessage customerMessage = new MailMessage(mailFrom, EmailInput.Text)
                    {
                        IsBodyHtml = true,
                        Subject = string.Format(Translate("/conference/mail/groupregistration/customer/subject"), CurrentPage.PageName),
                        Body = string.Format(Translate("/conference/mail/groupregistration/customer/body"), FirstNameInput.Text, CurrentPage.PageName).Replace("[nl]", "<br />")
                    };

                    MailMessage diMessage = new MailMessage(EmailInput.Text, EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailTo") != null ? EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailTo").ToString() : MiscFunctions.GetAppsettingsValue("mailPrenDiSe"))
                    {
                        IsBodyHtml = true,
                        Subject = string.Format(Translate("/conference/mail/groupregistration/di/subject"), CurrentPage.PageName, FirstNameInput.Text, LastNameInput.Text),
                        Body = string.Format(Translate("/conference/mail/groupregistration/di/body"), CurrentPage.PageName, FirstNameInput.Text, LastNameInput.Text) + "<br />" + "<br />" +
                               "<b>" + FirstNameInput.Title + ": " + "</b>" + FirstNameInput.Text + "<br />" +
                               "<b>" + LastNameInput.Title + ": " + "</b>" + LastNameInput.Text + "<br />" +
                               "<b>" + EmailInput.Title + ": " + "</b>" + EmailInput.Text + "<br />" +
                               "<b>" + TelephoneInput.Title + ": " + "</b>" + TelephoneInput.Text + "<br />" +
                               "<b>" + MessageInput.Title + ": " + "</b>" + MessageInput.Text 
                    };

                    //Send mail
                    MiscFunctions.SendMail(customerMessage);
                    MiscFunctions.SendMail(diMessage);

                    //Success
                    ((DiTemplatePage)Page).ShowMessage("/conference/success/groupregistration", true, false);                    

                    //Clear input fields
                    FirstNameInput.Text = string.Empty;
                    LastNameInput.Text = string.Empty;
                    EmailInput.Text = string.Empty;
                    TelephoneInput.Text = string.Empty;
                    MessageInput.Text = string.Empty;
                    captchaNumber1.Value = string.Empty;
                    captchaNumber2.Value = string.Empty;
                    txtCaptchaConf.Text = string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                //TODO: Change it?
                new Logger("DownLoadPdf() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/conference/errors/error", true, true);
            }
            SetCaptchaDetails();
        }

        /// <summary>
        /// Register clientscripts. When "Save buttons" are clicked, the id of the selected tab and section is saved in hidden fields.
        /// </summary>
        private void RegisterScript()
        {
            HiddenField SelectedTabHiddenField = Conference.HiddenFieldSelectedTab;
            HyperLink HyperLinkGroupRegistration = ParentControl.HyperLinkGroupRegistration;

            // Create script for click on Save buttons where selected tab and section will be stored in hiddenfields
            string script = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('{2}');
                                                }})                                                  
                                            }});",

                                            ConferenceGroupRegistrationButton.ClientID,

                                            SelectedTabHiddenField.ClientID,

                                            HyperLinkGroupRegistration.NavigateUrl
                                        );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ConferenceGroupRegistrationButton_Click", script, true);
        }

        private void SetCaptchaDetails()
        {
            var rnd = new Random();
            captchaNumber1.Value = rnd.Next(0, 9).ToString();
            captchaNumber2.Value = rnd.Next(0, 9).ToString();
            CaptchaTitle = string.Format(Translate("/conference/forms/registration/captchaquestiontemplate"), captchaNumber1.Value, captchaNumber2.Value);
        }

        private bool ValidateCaptcha()
        {
            int captchaGuess;
            var captchaCheck = int.TryParse(txtCaptchaConf.Text, out captchaGuess);
            return (captchaCheck && captchaGuess == (int.Parse(captchaNumber1.Value) + int.Parse(captchaNumber2.Value)));
        }
    }
}