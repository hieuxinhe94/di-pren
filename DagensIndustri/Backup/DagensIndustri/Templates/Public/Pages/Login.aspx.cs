using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DagensIndustri.Templates.Public.Units.Placeable.Login;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class Login : DiTemplatePage
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            //if (HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    //If user is logged in already, then redirect user to another page
            //    EPiFunctions.RedirectToURL(this.Page, "ReturnUrl");
            //}
            //else
            //{
                //if (!IsPostBack)
                //{
                    //If redirected from login controll because login failed, show error message
                    //if (Convert.ToBoolean(Request.QueryString["LoginFailed"]))
                    //{
                    //    LoginControl loginCtrl = LoginView.FindControl("LoginCtrl") as LoginControl;
                    //    loginCtrl.LoginUserName = Request.QueryString["UserName"];
                    //    ShowMessage("/dilogin/error/loginfail", true, true);
                    //}
                //}
            //}
        }
        #endregion       
    }
}