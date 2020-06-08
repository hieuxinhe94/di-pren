using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiServer.XForms.WebControls;
using DagensIndustri.Tools.Classes.Campaign;
using DIClassLib.DbHelpers;
using EPiServer;
using System.Web;
using DagensIndustri.Tools.Classes;
using Label = System.Web.UI.WebControls.Label;
using System.Web.UI;
using DIClassLib.Misc;


namespace DagensIndustri
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
            //PageData startPage = EPiServer.DataFactory.Instance.GetPage(PageReference.StartPage);
            //if (startPage["CampaignRootPage"] != null)
            //{
            //    PageData CampaignRootPage = EPiServer.DataFactory.Instance.GetPage((PageReference)startPage["CampaignRootPage"]);

            //    if (CampaignRootPage["CampaignPageType"] != null)
            //    {
            //        try
            //        {
            //            int pageTypeId = int.Parse(CampaignRootPage["CampaignPageType"].ToString());

            //            if (e.Page.PageTypeID == pageTypeId)
            //            {
            //                string errorText = "Du kan inte publicera en kampanj utan att ange målgrupp, erbjudandekod, kampanjtyp och kostnader";
            //                bool cancel = false;

            //                if (e.Page.GetTargetGroup().Tables[0].Rows.Count < 1)
            //                    cancel = true;
            //                else if (e.Page.GetCampaignType().Tables[0].Rows.Count < 1)
            //                    cancel = true;
            //                else if (e.Page.GetOfferCodes().Tables[0].Rows.Count < 1)
            //                    cancel = true;
            //                else if (e.Page.GetCosts().Tables[0].Rows.Count < 1)
            //                    cancel = true;

            //                if (cancel)
            //                {
            //                    e.CancelAction = true;
            //                    e.CancelReason = errorText;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            new Logger("Global.asax OnPublished failed", ex.ToString());
            //        }
            //    }
            //}

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
                            DagensIndustri.Tools.Classes.Gasell.GasellObject gasellObject = new DagensIndustri.Tools.Classes.Gasell.GasellObject(e.Page.PageLink.ID, e.Page["GasellCity"].ToString(), e.Page["Date"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Logger("Global.asax OnPublished failed", ex.ToString());
                }
            }
        }

        void Session_OnStart()
        {
            //string userid = HttpContext.Current.User.Identity.Name;

            //if (userid != "")
            //    LoginUtil.HandleCookieExpireDate(userid);
        }
    }
}