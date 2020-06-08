using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Net.Mail;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class WineClubForm : EPiServer.UserControlBase
    {
        protected override void  OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);

            DataBind();
        }


        protected void WineClubFormButton_Click(object sender, EventArgs e)
            {
                try
                {
                    if (Page.IsValid)
                    {
                        string mailFrom = CurrentPage["AntipodesMailFrom"].ToString();
                        if (string.IsNullOrEmpty(mailFrom)) mailFrom = "no-reply@di.se";

                        //Create message
                        MailMessage customerMessage = new MailMessage(mailFrom, EmailInput.Text)
                        {
                            IsBodyHtml = true,
                            Subject = string.Format(Translate("/wineclub/mail/customer/subject"), CurrentPage.PageName),
                            Body = string.Format(Translate("/wineclub/mail/customer/body"), FirstNameInput.Text)
                        };

                        MailMessage antipodesMessage = new MailMessage(EmailInput.Text, CurrentPage["AntipodesMailTo"] != null ? CurrentPage["AntipodesMailTo"].ToString() : "leads@antipodeswines.com")
                        {
                            IsBodyHtml = true,
                            Subject = string.Format(Translate("/wineclub/mail/antipodes/subject"), FirstNameInput.Text, LastNameInput.Text),
                            Body = string.Format(Translate("/wineclub/mail/antipodes/body"), FirstNameInput.Text, LastNameInput.Text) + "<br />" + "<br />" +
                                   "<b>" + FirstNameInput.Title + ": " + "</b>" + FirstNameInput.Text + "<br />" +
                                   "<b>" + LastNameInput.Title + ": " + "</b>" + LastNameInput.Text + "<br />" +
                                   "<b>" + EmailInput.Title + ": " + "</b>" + EmailInput.Text + "<br />" +
                                   "<b>" + TelephoneInput.Title + ": " + "</b>" + TelephoneInput.Text 
                        };

                        //Send mail
                        MiscFunctions.SendMail(customerMessage);
                        MiscFunctions.SendMail(antipodesMessage);

                        //Success
                        ((DiTemplatePage)Page).ShowMessage("/wineclub/message/success", true, false);

                        //Clear input fields
                        FirstNameInput.Text = string.Empty;
                        LastNameInput.Text = string.Empty;
                        EmailInput.Text = string.Empty;
                        TelephoneInput.Text = string.Empty;
                    }
                }
                catch (System.Exception ex)
                {
                    //TODO: Change it?
                    new Logger("WineClubForm() - failed", ex.ToString());
                    ((DiTemplatePage)Page).ShowMessage("/windeclub/message/error", true, true);
                }
            }
    }
}