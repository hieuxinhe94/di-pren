using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class SMSLandingPage : DiTemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string username = HttpContext.Current.User.Identity.Name;
                System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(username);
                string password = Session["password"] as string;

                UsernameLiteral.Text = string.Format(Translate("/sms/username"), username);
                PasswordLiteral.Text = string.Format(Translate("/sms/password"), password);
            }
        }
    }
}