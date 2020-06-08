using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.DbHelpers;

using EPiServer;
using EPiServer.Core;
using EPiServer.XForms.WebControls;

using PrenDiSe.Tools.Classes;

using Label = System.Web.UI.WebControls.Label;

namespace PrenDiSe
{
    public class global : EPiServer.Global
    {
        protected void Application_Start(Object sender, EventArgs e)
        {
            XFormControl.ControlSetup += new EventHandler(XForm_ControlSetup);
            EPiServer.DataFactory.Instance.PublishingPage += new PageEventHandler(OnPublish);
        }

        #region Global XForm Events

        /// <summary>
        /// Sets up events for each new instance of the XFormControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks>As the ControlSetup event is triggered for each instance of the XFormControl control
        /// you need to take into consideration that any event handlers will affect all XForms for the entire
        /// application. If the EPiServer UI is running in the same application this might also be affected depending
        /// on which events you attach to and what is done in the event handlers.</remarks>
        public void XForm_ControlSetup(object sender, EventArgs e)
        {
            XFormControl control = (XFormControl)sender;

            control.BeforeLoadingForm += new LoadFormEventHandler(XForm_BeforeLoadingForm);
            control.ControlsCreated += new EventHandler(XForm_ControlsCreated);
            control.BeforeSubmitPostedData += new SaveFormDataEventHandler(XForm_BeforeSubmitPostedData);
            control.AfterSubmitPostedData += new SaveFormDataEventHandler(XForm_AfterSubmitPostedData);
        }

        /// <summary>
        /// Handles the BeforeLoadingForm event of the XFormControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EPiServer.XForms.WebControls.LoadFormEventArgs"/> instance containing the event data.</param>
        public void XForm_BeforeLoadingForm(object sender, LoadFormEventArgs e)
        {
            XFormControl formControl = (XFormControl)sender;

            //We set the validation group of the form to match our global validation group in the master page.
            formControl.ValidationGroup = "XForm";
        }

        /// <summary>
        /// Handles the ControlsCreated event of the XFormControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void XForm_ControlsCreated(object sender, EventArgs e)
        {
            XFormControl formControl = (XFormControl)sender;

            //We set the inline error validation text to "*" as we use a
            //validation summary in the master page to display the detailed error message.
            foreach (Control control in formControl.Controls)
            {
                if (control is BaseValidator)
                {
                    ((BaseValidator)control).Text = "*";
                }
            }

            if (formControl.Page.User.Identity.IsAuthenticated)
            {
                formControl.Data.UserName = formControl.Page.User.Identity.Name;
            }
        }

        /// <summary>
        /// Handles the BeforeSubmitPostedData event of the XFormControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EPiServer.XForms.WebControls.SaveFormDataEventArgs"/> instance containing the event data.</param>
        public void XForm_BeforeSubmitPostedData(object sender, SaveFormDataEventArgs e)
        {
            XFormControl control = (XFormControl)sender;

            PageBase currentPage = control.Page as PageBase;

            if (currentPage == null)
            {
                return;
            }

            //We set the current page that the form has been posted from
            //This might differ from the actual page that the form property exists on.
            e.FormData.PageGuid = currentPage.CurrentPage.PageGuid;
        }

        /// <summary>
        /// Handles the AfterSubmitPostedData event of the XFormControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EPiServer.XForms.WebControls.SaveFormDataEventArgs"/> instance containing the event data.</param>
        public void XForm_AfterSubmitPostedData(object sender, SaveFormDataEventArgs e)
        {
            XFormControl control = (XFormControl)sender;

            if (control.FormDefinition.PageAfterPost != 0)
            {
                PageData redirectPage = DataFactory.Instance.GetPage(new PageReference(control.FormDefinition.PageAfterPost));
                control.Page.Response.Redirect(redirectPage.LinkURL);
                return;
            }

            //After the form has been posted we remove the form elements and add a "thank you message".
            control.Controls.Clear();
            Label label = new Label();
            label.CssClass = "thankyoumessage";
            label.Text = LanguageManager.Instance.Translate("/form/postedmessage");
            control.Controls.Add(label);
        }

        #endregion

        /// <summary>
        /// If user tries to publish campaign, check mandatory properties. 
        /// Just for safety, a campaign will not work if these properties are empty
        /// </summary>
        private void OnPublish(object sender, EPiServer.PageEventArgs e)
        {
            if (EPiFunctions.HasValue(EPiFunctions.SettingsPage(e.Page), "GasellMeetingPageType"))
            {
                try
                {
                    if (EPiFunctions.IsMatchingPageType(e.Page, e.Page.PageTypeID, "GasellMeetingPageType"))
                    {
                        if (e.Page.PageLink.ID <= 0)
                        {
                            e.CancelAction = true;
                            e.CancelReason = "You have first to save and then save and publish your gasell";
                        }
                        else
                        {
                            //DagensIndustri.Tools.Classes.Gasell.GasellObject gasellObject = new DagensIndustri.Tools.Classes.Gasell.GasellObject(e.Page.PageLink.ID, e.Page["GasellCity"].ToString(), e.Page["Date"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Logger("Global.asax OnPublished failed", ex.ToString());
                }
            }
        }

        /// <summary>
        /// Hacky fix to make epi shortcut redirects return statuscode 301 instead of 302
        /// </summary>
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            var statusCode = HttpContext.Current.Response.StatusCode;
            if (statusCode == 302)
            {
                HttpContext.Current.Response.StatusCode = 301;
            }
        }
    }
}