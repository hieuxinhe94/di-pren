using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Threading;
using DagensIndustri.Tools.Classes.Conference;
using System.Text;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Units.Placeable.Conference
{
    public partial class ConferenceRegistration : EPiServer.UserControlBase
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

        public List<ListItem> SelectedActivities { get; set; }

        public ConferenceObject conference;

        public string ConferenceName
        {
            get
            {
                string pageName = String.Empty;

                if (CurrentPage["GetFormFromAnotherPage"] != null)
                {
                    pageName = EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference).PageName; 
                }
                else
                {
                    pageName = CurrentPage.PageName;
                }
                
                return pageName;
            }
        }

        public string FirstName
        {
            get
            {
                return FirstNameInput.Text.Trim();
            }
            set
            {
                FirstNameInput.Text = value;
            }
        }

        public string LastName
        {
            get
            {
                return LastNameInput.Text.Trim();
            }
            set
            {
                LastNameInput.Text = value;
            }
        }

        public string Company
        {
            get
            {
                if (HideInvoiceFields)
                    return CompanyInputTop.Text.Trim();
                    
                return CompanyInput.Text.Trim();
            }
            set
            {
                if (HideInvoiceFields)
                    CompanyInputTop.Text = value;
                else
                    CompanyInput.Text = value;
            }
        }

        public string Title
        {
            get
            {
                return TitleInput.Text.Trim();
            }
            set
            {
                TitleInput.Text = value;
            }
        }

        public string OrgNo
        {
            get
            {
                return OrgNumberInput.Text.Trim();
            }
            set
            {
                OrgNumberInput.Text = value;
            }
        }

        public string Phone
        {
            get
            {
                return TelephoneInput.Text.Trim();
            }
            set
            {
                TelephoneInput.Text = value;
            }
        }

        public string Email
        {
            get
            {
                return EmailInput.Text.Trim();
            }
            set
            {
                EmailInput.Text = value;
            }
        }

        public string InoviceAddress
        {
            get
            {
                return InvoiceAddressInput.Text.Trim();
            }
            set
            {
                InvoiceAddressInput.Text = value;
            }
        }

        public string InvoiceReference
        {
            get
            {
                return InvoiceReferenceInput.Text.Trim();
            }
            set
            {
                InvoiceReferenceInput.Text = value;
            }
        }

        public string Zip
        {
            get
            {
                return ZipCodeInput.Text.Trim();
            }
            set
            {
                ZipCodeInput.Text = value;
            }
        }

        public string City
        {
            get
            {
                return StateInput.Text.Trim();
            }
            set
            {
                StateInput.Text = value;
            }
        }

        public string Code
        {
            get
            {
                return CodeInput.Text.Trim();
            }
            set
            {
                CodeInput.Text = value;
            }
        }

        public string InformationChannel
        {
            get
            {
                return DdlInfoChannel.SelectedValue;
            }
            set
            {
                DdlInfoChannel.SelectedValue = value;
            }
        }

        public List<string> radioButtonGroups = new List<string>();

        public List<ListItem> radioButtons = new List<ListItem>();

        public bool HideInvoiceFields 
        {
            get 
            {
                if (CurrentPage["HideInvoiceFields"] != null)
                    return true;

                return false;
            }
        }

        public bool ShowStudentFormHeadlines
        {
            get
            {
                if (CurrentPage["ShowStudentFormHeadlines"] != null)
                    return true;

                return false;
            }
        }

        protected string TitleTitle
        {
            get
            {
                return (!ShowStudentFormHeadlines) ? Translate("/conference/forms/registration/title") : Translate("/conference/forms/registration/education");
            }
        }

        protected string DispTitle
        {
            get
            {
                return (!ShowStudentFormHeadlines) ? Translate("/conference/forms/registration/title.message") : Translate("/conference/forms/registration/education.message");
            }
        }

        protected string TitleCompany
        {
            get
            {
                return (!ShowStudentFormHeadlines) ? Translate("/conference/forms/registration/company") : Translate("/conference/forms/registration/university");
            }
        }

        protected string DispCompany
        {
            get
            {
                return (!ShowStudentFormHeadlines) ? Translate("/conference/forms/registration/company.message") : Translate("/conference/forms/registration/university.message");
            }
        }

        protected string CaptchaTitle { get; set; }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (HideInvoiceFields)
                {
                    PlaceHolderCompanyTop.Visible = true;
                    PlaceHolderInvoice.Visible = false;
                }

                if (CurrentPage["LanguageEnglish"] != null)
                {
                    System.Globalization.CultureInfo oldCulture = Thread.CurrentThread.CurrentCulture;
                    System.Globalization.CultureInfo oldUICulture = Thread.CurrentThread.CurrentUICulture;
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

                    GetInfoChannels.SelectMethod = "GetInfoChannelsEn";
                }
                else
                {
                    GetInfoChannels.SelectMethod = "GetInfoChannels";
                }
                SetCaptchaDetails();
                DataBind();
            }

            RegisterScript();

            if (IsValue("RegisterTabText"))
                RefisterFormTextLiteral.Text = CurrentPage["RegisterTabText"].ToString();

            if (CurrentPage["GetFormFromAnotherPage"] != null)
            {
                if (IsValue("LanguagePage"))
                    conference = new ConferenceObject(EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference));
            }
            else
            {
                conference = new ConferenceObject(CurrentPage);
            }

            List<ConferenceEvent> conferenceEvents = conference.GetEvents();
            if (conferenceEvents == null || conferenceEvents.Count() == 0)
                eventsDivider.Visible = false;

            ConferenceRepeater.DataSource = conferenceEvents;
            ConferenceRepeater.DataBind();

            if (CurrentPage["HideForm"] == null)
            {
                FormHiddenTextPlaceHolder.Visible = false;
            }
            else
            {
                if (IsValue("HideFormText"))
                    FormHiddenTextLiteral.Text = CurrentPage["HideFormText"].ToString();

                ConferenceRegistrationPlaceHolder.Visible = false;
            }
        }

        /// <summary>
        /// Add first item in Channel Information Dropdownlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlInfoChannelOnPreRender(object sender, EventArgs e)
        {
            InsertDefaultInfoChannel((DropDownList)sender);
        }

        /// <summary>
        /// Saves Registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConferenceRegistrationButton_Click(object sender, EventArgs e)
        {
            try
            {
                var errMsg = ValidateActivityList();
                var captchaStatus = ValidateCaptcha();
                if (!ValidateCaptcha())
                {
                    errMsg = "Captcha failed!";
                }

                if (Page.IsValid && String.IsNullOrEmpty(errMsg))
                {
                    var confUser = new ConferenceUser(this);

                    //Save user
                    confUser.Save();

                    //Clear form
                    ClearForm();

                    //Show message
                    ((DiTemplatePage)Page).ShowMessage("/conference/success/registration", true, false);

                }
                else
                {
                    ((DiTemplatePage)Page).ShowMessage(captchaStatus ?
                        "/conference/errors/registration/error" :
                        "/conference/forms/registration/captcha.error", true, true);
                }
                SetCaptchaDetails();
            }
            catch (Exception ex)
            {
                new Logger("Save() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/conference/errors/error", true, true);
                SetCaptchaDetails();
            }
        }

        /// <summary>
        /// Validates Activity List
        /// </summary>
        /// <returns></returns>
        private string ValidateActivityList()
        {          
            var errorMsg = string.Empty;
            var failed = false;
            SelectedActivities = new List<ListItem>();
            foreach (var group in radioButtonGroups)
            {
                if (!failed)
                {
                    var groupedRadioButtons = new List<string>();
                    foreach (ListItem listItem in radioButtons)
                    {
                        var control = FindControlRecursive(ConferenceRepeater, listItem.Value);
                        var radioButton = control as RadioButton;
                        if (radioButton.GroupName == group && radioButton.Enabled)
                        {
                            groupedRadioButtons.Add(radioButton.ID);
                        }
                    }

                    var i = 0;

                    if (groupedRadioButtons.Count > 0)
                    {
                        foreach (string groupedRadioButton in groupedRadioButtons)
                        {
                            RadioButton groupedRadioButtonControl = FindControlRecursive(ConferenceRepeater, groupedRadioButton) as RadioButton;

                            if (groupedRadioButtonControl.Checked)
                            {
                                SelectedActivities.Add(new ListItem(groupedRadioButtonControl.Text, groupedRadioButtonControl.ID.ToString().Replace("eventid_", "")));
                                break;
                            }
                            i = i + 1;
                            if (groupedRadioButtons.Count == i)
                            {
                                failed = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    errorMsg = "Failed";
                    break;
                }
            }

            if(failed)
            {
                errorMsg = "Failed";
            }
            return errorMsg;
        }

        private bool ValidateCaptcha()
        {
            int captchaGuess;
            var captchaCheck = int.TryParse(txtCaptchaConf.Text, out captchaGuess);
            return (captchaCheck && captchaGuess == (int.Parse(captchaNumber1.Value) + int.Parse(captchaNumber2.Value)));
        }

        /// <summary>
        /// Register clientscripts. When "Save buttons" are clicked, the id of the selected tab and section is saved in hidden fields.
        /// </summary>
        private void RegisterScript()
        {
            HiddenField SelectedTabHiddenField = Conference.HiddenFieldSelectedTab;
            HyperLink HyperLinkConferenceRegistration = ParentControl.HyperLinkRegistration;

            // Create script for click on Save buttons where selected tab and section will be stored in hiddenfields
            string script = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('{2}');
                                                }})                                                  
                                            }});",

                                            ConferenceRegistrationButton.ClientID,

                                            SelectedTabHiddenField.ClientID,

                                            HyperLinkConferenceRegistration.NavigateUrl
                                        );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ConferenceRegistrationButton_Click", script, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConferenceRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater EventTimesRepeater = e.Item.FindControl("EventTimesRepeater") as Repeater;

            ConferenceEvent conferenceEvent = e.Item.DataItem as ConferenceEvent;

            if (CurrentPage["GetFormFromAnotherPage"] != null)
            {
                if (IsValue("LanguagePage"))
                    conference = new ConferenceObject(EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference));
            }
            else
            {
                conference = new ConferenceObject(CurrentPage);
            }

            List<EventTime> eventTimes = conference.GetEventTimes(conferenceEvent.Id);
            EventTimesRepeater.DataSource = eventTimes;
            EventTimesRepeater.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EventTimesRepeater_ItemDatabound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal nameLiteral = e.Item.FindControl("NameLiteral") as Literal;
                nameLiteral.Text = ((HiddenField)e.Item.Parent.Parent.FindControl("ConferenceNameHiddenField")).Value;

                Literal dateLiteral = e.Item.FindControl("DateLiteral") as Literal;
                dateLiteral.Text = Convert.ToDateTime(((HiddenField)e.Item.Parent.Parent.FindControl("ConferenceDateHiddenField")).Value).ToShortDateString();

                Repeater ActivitiesRepeater = e.Item.FindControl("ActivitiesRepeater") as Repeater;

                EventTime eventTime = e.Item.DataItem as EventTime;

                if (CurrentPage["GetFormFromAnotherPage"] != null)
                {
                    if(IsValue("LanguagePage"))
                        conference = new ConferenceObject(EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference));
                }
                else
                {
                    conference = new ConferenceObject(CurrentPage);
                }

                PlaceHolder radioButtonPlaceholder = e.Item.FindControl("RadioButtonPlaceHolder") as PlaceHolder;

                System.Web.UI.HtmlControls.HtmlGenericControl ul = e.Item.FindControl("list") as System.Web.UI.HtmlControls.HtmlGenericControl;

                List<EventActivity> confActivities = conference.GetEventActivities(eventTime.Id);
                if (confActivities.Count() == 0)
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl row = e.Item.FindControl("RadioListRow") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (row != null)
                        row.Visible = false;
                }
                foreach (EventActivity eventActivity in confActivities)
                {
                    //radioButtonPlaceholder.Visible = true;

                    System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");

                    RadioButton radiobutton = new RadioButton();
                    
                    radiobutton.GroupName = eventTime.Id.ToString();
                    radiobutton.ID = "eventid_" + eventActivity.Id.ToString();

                    radiobutton.Attributes.Add("ActivityID", eventActivity.Id.ToString());
                    radiobutton.Attributes.Add("ActivityName", eventActivity.Name);
                    radiobutton.CssClass = "label";

                    if (eventActivity.NrOfParticipants < eventActivity.MaxParticipants)
                        radiobutton.Text = eventActivity.Name;
                    else
                    {
                        radiobutton.Text = eventActivity.Name + " " + "(Fullbokat)";
                        radiobutton.Enabled = false;
                    }

                    ul.Controls.Add(li);

                    li.Controls.Add(radiobutton);

                    if (!radioButtonGroups.Contains(eventTime.Id.ToString()))
                        radioButtonGroups.Add(eventTime.Id.ToString());

                    ListItem listItem = new ListItem("ID", radiobutton.ID);

                    if (!radioButtons.Contains(listItem))
                        radioButtons.Add(listItem);
                }
            }
            
        }

        /// <summary>
        /// Clear all input fields in form
        /// </summary>
        private void ClearForm()
        {
            foreach (object control in ConferenceRegistrationPlaceHolder.Controls)
            {
                if (control.GetType().BaseType == typeof(DagensIndustri.Tools.Classes.WebControls.InputWithValidation))
                {
                    DagensIndustri.Tools.Classes.WebControls.InputWithValidation input = (DagensIndustri.Tools.Classes.WebControls.InputWithValidation)control;
                    input.Text = string.Empty;
                }
            }

            foreach (ListItem listItem in radioButtons)
            {
                Control control = FindControlRecursive(ConferenceRepeater, listItem.Value);

                RadioButton radioButton = control as RadioButton;
                radioButton.Checked = false;
            }

            InsertDefaultInfoChannel(DdlInfoChannel);
            if (DdlInfoChannel.SelectedIndex > 0)
                DdlInfoChannel.SelectedIndex = 0;
        }

        /// <summary>
        /// Finds a control by ID
        /// </summary>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Control FindControlRecursive(Control control, string id)
        {
            Control returnControl = control.FindControl(id);
            if (returnControl == null)
            {
                foreach (Control child in control.Controls)
                {
                    returnControl = FindControlRecursive(child, id);
                    if (returnControl != null && returnControl.ID == id)
                    {
                        return returnControl;
                    }
                }
            }
            return returnControl;
        }

        /// <summary>
        /// Insert default item to Info channel dropdown list if needed
        /// </summary>
        /// <param name="ddlInfoChannel"></param>
        private void InsertDefaultInfoChannel(DropDownList ddlInfoChannel)
        {
            //Add default item to dropdownlist            
            ListItem chooseItem = new ListItem(Translate("/conference/forms/registration/information.channels/channel0"), "0");

            if (!ddlInfoChannel.Items.Contains(chooseItem))
                ddlInfoChannel.Items.Insert(0, chooseItem);
        }

        private void SetCaptchaDetails()
        {
            var rnd = new Random();
            captchaNumber1.Value = rnd.Next(0, 9).ToString();
            captchaNumber2.Value = rnd.Next(0, 9).ToString();
            CaptchaTitle = string.Format(Translate("/conference/forms/registration/captchaquestiontemplate"), captchaNumber1.Value, captchaNumber2.Value);
        }
    }
}