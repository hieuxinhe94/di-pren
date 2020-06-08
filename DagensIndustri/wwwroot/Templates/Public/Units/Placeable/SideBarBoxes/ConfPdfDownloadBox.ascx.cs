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

            string name = NameInput.Text.Trim();
            string mailTo = EmailInput.Text.Trim();
            string phone = TelephoneInput.Text.Trim();

            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(mailTo) || String.IsNullOrEmpty(phone))
            {
                SetErrorMessage("Ange samtliga fält nedan");
                return;
            }

            if (!MiscFunctions.IsValidEmail(mailTo))
            {
                SetErrorMessage("Ange en giltig e-postadress");
                return;
            }


            try
            {
                string mailFrom = EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailFrom").ToString();
                if (string.IsNullOrEmpty(mailFrom)) 
                    mailFrom = "no-reply@di.se";
                

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
                    
                MailMessage message = new MailMessage(mailFrom, mailTo)
                {
                    IsBodyHtml = true,
                    Subject = string.Format(Translate("/conference/mail/pdf/subject"), CurrentPage.PageName),
                    Body = string.Format(Translate("/conference/mail/pdf/body"), NameInput.Text, CurrentPage.PageName).Replace("[nl]", "<br />")
                };

                MiscFunctions.SendMailWithVppFile(message, CurrentPage["PdfLink"].ToString());
                MsSqlHandler.InsertPdfDownloadLog(conference.ConferenceId, name, mailTo, phone);

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