using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.Misc;
using System.Net.Mail;
using EPiServer.Web.Hosting;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Conference;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using System.Threading;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Units.Placeable.Conference
{
    public partial class ConferencePDFForm : EPiServer.UserControlBase
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
                return (Units.Placeable.Conference.ConferenceApplicationForm)Parent;
            }
        }

        ConferenceObject conference;

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

            DataBind();
            
            RegisterScript();

            if (!IsValue("PdfLink"))
                PDFPlaceHolder.Visible = false;
            else
                PDFMessagePlaceHolder.Visible = false;
                
        }

        protected void PDFFormButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string mailFrom = EPiFunctions.SettingsPageSetting(CurrentPage, "ConferenceMailFrom").ToString();
                    if (string.IsNullOrEmpty(mailFrom)) mailFrom = "no-reply@di.se";

                    // Send mail with pdf

                    //Success
                    ((DiTemplatePage)Page).ShowMessage("/conference/success/pdf", true, false);

                    //Clear input fields
                    NameInput.Text = string.Empty;
                    EmailInput.Text = string.Empty;
                    TelephoneInput.Text = string.Empty;
                }
            }
            catch (System.Exception ex)
            {
                //TODO: Change it?
                new Logger("DownLoadPdf() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/conference/errors/error", true, true);
            }
        }

        /// <summary>
        /// Register clientscripts. When "Save buttons" are clicked, the id of the selected tab and section is saved in hidden fields.
        /// </summary>
        private void RegisterScript()
        {
            HiddenField SelectedTabHiddenField = Conference.HiddenFieldSelectedTab;
            HyperLink HyperLinkPDFForm = ParentControl.HyperLinkPDFForm;

            // Create script for click on Save buttons where selected tab and section will be stored in hiddenfields
            string script = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('{2}');
                                                }})                                                  
                                            }});",

                                            PDFFormButton.ClientID,

                                            SelectedTabHiddenField.ClientID,

                                            HyperLinkPDFForm.NavigateUrl
                                        );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "PDFFormButton_Click", script, true);
        }       
    }
}