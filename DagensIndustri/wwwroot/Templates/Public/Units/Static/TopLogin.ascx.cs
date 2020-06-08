using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.Extras;
using DagensIndustri.Tools.Classes.WebControls;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Units.Static
{
    public partial class TopLogin : EPiServer.UserControlBase
    {
        #region Properties
        public string EmailUserName { get; set; }

        /// <summary>
        /// Get the user name from the UserName control
        /// </summary>
        private string LoginUserName
        {
            get
            {
                HtmlInputText userNameInput = LoginView.FindControl("LoginControl$UserName$Input") as HtmlInputText;
                return (userNameInput != null) ? userNameInput.Value.Trim() : string.Empty;
            }
            set
            {
                HtmlInputText userNameInput = LoginView.FindControl("LoginControl$UserName$Input") as HtmlInputText;
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
                HtmlInputText passwordInput = LoginView.FindControl("LoginControl$Password$Input") as HtmlInputText;
                return (passwordInput != null) ? passwordInput.Value.Trim() : string.Empty;
            }
            set
            {
                HtmlInputText passwordInput = LoginView.FindControl("LoginControl$Password$Input") as HtmlInputText;
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
                InitializeLoginControls();
                InitializeLoggedInControls();
            }
        }

        /// <summary>
        /// Triggered on OnAuthenticate for LoginControl
        /// Checks if user tries to login with email. If so, get username from db
        /// </summary>
        protected void OnAuthenticate(object sender, AuthenticateEventArgs e)
        {
            System.Web.UI.WebControls.Login LoginControl = LoginView.FindControl("LoginControl") as System.Web.UI.WebControls.Login;

            string failureText = string.Empty;
            string tempUserName = LoginUserName;
            string tempEmailUserName = string.Empty;

            //Get login page in order to access its properties
            PageData loginPage = EPiFunctions.GetLoginPage(CurrentPage); //GetPage(EPiFunctions.SettingsPageSetting(CurrentPage, "LinkToLoginPage") as PageReference);

            //Authenticate the user
            e.Authenticated = LoginUtil.Authenticate(loginPage, ref tempUserName, LoginPassword, out tempEmailUserName, out failureText);

            //Set values that might have been changed
            LoginUserName = tempUserName;
            EmailUserName = tempEmailUserName;

            //If problem with finding user, then set the failure text
            if (!string.IsNullOrEmpty(failureText))
                LoginControl.FailureText = failureText;

            if (!e.Authenticated)
            {
                LoginPassword = string.Empty;
                Response.Redirect(string.Format("{0}?LoginFailed=true&UserName={1}", EPiFunctions.GetFriendlyAbsoluteUrl(loginPage), LoginUserName));
            }
        }

        /// <summary>
        /// Triggered on OnLoggedIn for LoginControl
        /// Changes Expiretime if user checked RememberMe
        /// </summary>
        protected void OnLoggedIn(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = LoginView.FindControl("LoginControl$RememberMeCheckBox") as CheckBox;
                
                if (cb != null && cb.Checked)
                    LoginUtil.HandleCookieExpireDate(LoginUserName);

            }
            catch (Exception ex)
            {
                new Logger("OnLoggedIn() failed", ex.ToString());
            }
        }

        /// <summary>
        /// Triggered on OnLoginError for LoginControl
        /// If user don't succeed with Emaillogin, reset username in LoginControl to emailaddress
        /// </summary>
        protected void OnLoginError(object sender, EventArgs e)
        {
            //if authentication did not success with email, reset username in LoginControl to emailaddress
            if (!string.IsNullOrEmpty(EmailUserName))
            {
                LoginUserName = EmailUserName;
            }
        }

        protected void PageListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Find the repeater and gets its data source
                Repeater pageListRepeater = LoginView.FindControl("PageListRepeater") as Repeater;
                PageDataCollection selectedPages = pageListRepeater.DataSource as PageDataCollection;

                //If the current item is the last item add a css class to it
                if (selectedPages[selectedPages.Count - 1].PageLink == ((EPiServer.Core.PageData)e.Item.DataItem).PageLink)
                {
                    HtmlGenericControl listItem = e.Item.FindControl("ListItem") as HtmlGenericControl;
                    listItem.Attributes.Add("class", "section-end");
                }
            }
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            LoginUtil.LogoutUser();
            
            //Redirect to start page
            Response.Redirect(EPiFunctions.StartPage().LinkURL);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initialize and bind controls that are shown when user is not logged in
        /// </summary>
        private void InitializeLoginControls()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                PlaceHolder bannerLinkPlaceHolder = LoginView.FindControl("LoginControl$BannerLinkPlaceHolder") as PlaceHolder;
                bannerLinkPlaceHolder.DataBind();

                //Bind the image
                Image bannerImage = new Image();// LoginView.FindControl("LoginControl$BannerImage") as Image;
                bannerImage.DataBind();

                HyperLink bannerHyperLink = new HyperLink(); //LoginView.FindControl("LoginControl$BannerHyperLink") as HyperLink;
                bannerHyperLink.DataBind();

                bannerHyperLink.NavigateUrl = EPiFunctions.GetLoginBannerLink(CurrentPage);

                if (!EPiFunctions.HideHyperLink(CurrentPage))
                {
                    bannerLinkPlaceHolder.Controls.Add(bannerImage);
                }
                else
                {
                    bannerLinkPlaceHolder.Controls.Add(bannerHyperLink);
                    bannerHyperLink.Controls.Add(bannerImage);
                }

                bannerImage.ImageUrl = EPiFunctions.GetLoginBanner(CurrentPage);
                bannerImage.AlternateText = "banner-login";
                bannerImage.Width = 234;
                bannerImage.Height = 103;

                PlaceHolder notLoggedInBannerPlaceHolder = LoginView.FindControl("LoginControl$NotLoggedInBannerPlaceHolder") as PlaceHolder;
                notLoggedInBannerPlaceHolder.DataBind();

                HyperLink forgotPasswordHyperLink = LoginView.FindControl("LoginControl$ForgotPasswordHyperLink") as HyperLink;
                forgotPasswordHyperLink.DataBind();

                if (EPiFunctions.GetLoginBanner(CurrentPage) == "")
                    bannerImage.Visible = false;
            }
            else
            {
                PlaceHolder loggedInBannerLinkPlaceHolder = LoginView.FindControl("LoggedInBannerLinkPlaceHolder") as PlaceHolder;

                Image loggedInBannerImage = new Image();// LoginView.FindControl("LoginControl$BannerImage") as Image;
                //loggedInBannerImage.DataBind();

                HyperLink loggedInHyperLink = new HyperLink(); //LoginView.FindControl("LoginControl$BannerHyperLink") as HyperLink;
                //loggedInHyperLink.DataBind();

                loggedInHyperLink.NavigateUrl = EPiFunctions.GetLoginBannerLink(CurrentPage);

                if (!EPiFunctions.HideHyperLink(CurrentPage))
                {
                    loggedInBannerLinkPlaceHolder.Controls.Add(loggedInBannerImage);
                }
                else
                {
                    loggedInBannerLinkPlaceHolder.Controls.Add(loggedInHyperLink);
                    loggedInHyperLink.Controls.Add(loggedInBannerImage);
                }

                loggedInBannerImage.ImageUrl = EPiFunctions.GetLoginBanner(CurrentPage);
                //loggedInBannerImage.AlternateText = "banner-login";
                loggedInBannerImage.Width = 234;
                loggedInBannerImage.Height = 103;

                if (EPiFunctions.GetLoginBanner(CurrentPage) == "")
                    loggedInBannerImage.Visible = false;
            }
        }

        /// <summary>
        /// Initialize the repeater that is shown when user is logged in
        /// </summary>
        private void InitializeLoggedInControls()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Fill the repeater with links that should be shown under My Account
                DiLinkCollection linkCollection = new DiLinkCollection(EPiFunctions.SettingsPage(CurrentPage), "MyAccountLinks");

                Repeater pageListRepeater = LoginView.FindControl("PageListRepeater") as Repeater;
                pageListRepeater.DataSource = linkCollection.SelectedPages(true);
                pageListRepeater.DataBind();
            }
        }        
        #endregion

    }
}