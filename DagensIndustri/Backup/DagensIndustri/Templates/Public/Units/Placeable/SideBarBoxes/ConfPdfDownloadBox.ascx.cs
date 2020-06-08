using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using EPiServer.Web.Hosting;
using System.Net.Mail;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes.Conference;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;


namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{

    public partial class ConfPdfDownloadBox : UserControlBase
    {
        protected bool ShowForm { get; set; }

        protected ConferenceObject conference = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            String pdfLink = CurrentPage["PdfLink"] as string;
            if (String.IsNullOrEmpty(pdfLink))
            {
                this.Visible = false;
                return;
            }

        }

        protected void PDFFormButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Visible = false;

            if (String.IsNullOrEmpty(NameInput.Text) || String.IsNullOrEmpty(EmailInput.Text) || String.IsNullOrEmpty(TelephoneInput.Text))
            {
                SetErrorMessage("Ange samtliga fält nedan");
                return;
            }


            try
            {

                    string mailFrom = EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailFrom").ToString();
                    if (string.IsNullOrEmpty(mailFrom)) mailFrom = "no-reply@di.se";
                    String pdfLink = CurrentPage["PdfLink"] as string;
                    if (!String.IsNullOrEmpty(pdfLink))
                    {
                        pdfLink = HttpUtility.UrlDecode(pdfLink);
                    }

                    ConferenceObject conference = null;
                    if (CurrentPage["GetFormFromAnotherPage"] != null)
                    {
                        if (IsValue("LanguagePage"))
                            conference = new ConferenceObject(EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference));
                    }
                    else
                    {
                        conference = new ConferenceObject(CurrentPage);
                    }
                    

                    String subject = string.Format(Translate("/conference/mail/pdf/subject"), CurrentPage.PageName);
                    String body = string.Format(Translate("/conference/mail/pdf/body"), NameInput.Text, CurrentPage.PageName).Replace("[nl]", "<br />");

                    conference.SendPdfMail(NameInput.Text, EmailInput.Text, TelephoneInput.Text, pdfLink, subject, body, mailFrom, CurrentPage.PageName);

                    //Create message
                    MailMessage message = new MailMessage(mailFrom, EmailInput.Text)
                    {
                        IsBodyHtml = true,
                        
                    };


                    //Success
                    //((DiTemplatePage)Page).ShowMessage("/conference/success/pdf", true, false);
                    FormPlaceHolder.Visible = false;
                    SuccessPlaceHolder.Visible = true;
                    ShowForm = true;

                    //Clear input fields
                    NameInput.Text = string.Empty;
                    EmailInput.Text = string.Empty;
                    TelephoneInput.Text = string.Empty;
                
            }
            catch (System.Exception ex)
            {
                //TODO: Change it?
                //new Logger("PDFFormButton_Click() - failed", ex.ToString());
                SetErrorMessage(Translate("/conference/errors/error"));
                
                //((DiTemplatePage)Page).ShowMessage("/conference/errors/error", true, true);
            }
        
        }

        private void SetErrorMessage(string msg)
        {
            ErrorLabel.Text = msg;
            ErrorLabel.Visible = true;
            ShowForm = true;
        }   

    }
}