using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Extras;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Units.Placeable.Login
{
    public partial class LoginControl : UserControlBase
    {
        #region Properties
        public string HiddenFieldClientID
        {
            get
            {
                return ReturnURLHiddenField.ClientID;
            }
        }

        public bool ShowForgotPassword{ get; set; }
        public string EmailUserName { get; set; }

        /// <summary>
        /// Get the user name from the UserName control
        /// </summary>
        public string LoginUserName
        {
            get
            {
                HtmlInputText userNameInput = LoginCtrl.FindControl("UserName$Input") as HtmlInputText;
                return (userNameInput != null) ? userNameInput.Value.Trim() : string.Empty;
            }
            set
            {
                HtmlInputText userNameInput = LoginCtrl.FindControl("UserName$Input") as HtmlInputText;
                userNameInput.Value = value;
            }
        }

        /// <summary>
        /// Get the user name from the Password control
        /// </summary>
        private string LoginPassword
        {
            get
            {
                HtmlInputText passwordInput = LoginCtrl.FindControl("Password$Input") as HtmlInputText;
                return (passwordInput != null) ? passwordInput.Value.Trim() : string.Empty;
            }
            set
            {
                HtmlInputText passwordInput = LoginCtrl.FindControl("Password$Input") as HtmlInputText;
                passwordInput.Value = value;
            }
        }
        #endregion
        
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                Initialize();
            }
            DataBind();
        }

        /// <summary>
        /// Triggered on OnAuthenticate for LoginCtrl
        /// Checks if user tries to login with email. If so, get username from db
        /// </summary>
        protected void OnAuthenticate(object sender, AuthenticateEventArgs e)
        {
            string failureText = string.Empty;
            string tempUserName = LoginUserName;
            string tempEmailUserName = string.Empty;

            //Authenticate the user
            e.Authenticated = LoginUtil.Authenticate(CurrentPage, ref tempUserName, LoginPassword, out tempEmailUserName, out failureText);

            //Set values that might have been changed
            LoginUserName = tempUserName;
            EmailUserName = tempEmailUserName;

            //If problem with finding user, then set the failure text
            if (!string.IsNullOrEmpty(failureText))
                LoginCtrl.FailureText = failureText;

            if (e.Authenticated)
            {
                if (ShowForgotPassword)
                {
                    //Clear the hidden field.
                    ForgotPassword forgotPasswordControl = LoginCtrl.FindControl("ForgotPasswordPlaceHolder$ForgotPasswordControl") as ForgotPassword;
                    forgotPasswordControl.ClearHiddenFields();
                }
            }
            else
            {
                LoginPassword = string.Empty;
                EPiFunctions.ShowMessage(this.Page, "/dilogin/error/loginfail", true, true);
            }
        }

        /// <summary>
        /// Triggered on OnLoggedIn for LoginCtrl
        /// Changes Expiretime if user checked RememberMe
        /// </summary>
        protected void OnLoggedIn(object sender, EventArgs e)
        {
            try
            {
                CheckBox rememberMeCheckBox = LoginCtrl.FindControl("RememberMeCheckBox") as CheckBox;
                if (rememberMeCheckBox != null && rememberMeCheckBox.Checked)
                    LoginUtil.HandleCookieExpireDate(LoginUserName);

            }
            catch (Exception ex)
            {
                new Logger("OnLoggedIn() failed", ex.ToString());
            }

            RedirectUser();
        }

        /// <summary>
        /// Triggered on OnLoginError for LoginCtrl
        /// If user don't succeed with Emaillogin, reset username in LoginCtrl to emailaddress
        /// </summary>
        protected void OnLoginError(object sender, EventArgs e)
        {
            //if authentication did not success with email, reset username in LoginCtrl to emailaddress
            if (!string.IsNullOrEmpty(EmailUserName))
            {
                LoginUserName = EmailUserName;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Sets up which input control to focus and which button to use on enter key.
        /// </summary>
        private void Initialize()
        {
            Control control = LoginCtrl.FindControl("UserName$Input") as Control;
            if (control != null)
            {
                Page.Form.DefaultFocus = control.ClientID;
            }

            control = LoginCtrl.FindControl("LoginButton") as WebControl;
            if (control != null)
            {
                Page.Form.DefaultButton = control.UniqueID;
            }
        }
        
        private string GetReturnUrl()
        {
            return !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])
                ? Request.QueryString["ReturnUrl"] : ReturnURLHiddenField.Value;
        }

        /// <summary>
        /// If a return url was provided, redirect user to that page.        
        /// </summary>
        private void RedirectUser()
        {
            string url = GetReturnUrl();
            if (!string.IsNullOrEmpty(url))
            {
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Get link to login page and add a return url to it.
        /// </summary>
        /// <returns></returns>
        protected string GetLoginPageUrl()
        {
            return string.Format("{0}?ReturnUrl={1}", 
                                    EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetLoginPage(CurrentPage)),
                                    EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage));
        }
        #endregion
    }
}