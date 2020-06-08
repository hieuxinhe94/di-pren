using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using DIClassLib.DbHelpers;

namespace DIClassLib.Membership
{
    public class AutoLoginByIPHTTPModule : IHttpModule
    {
        const string _sectionName = "autoLoginByIPSettings";
        string _role              = DiRoleHandler.RoleDiIp;
        string _userName          = DiRoleHandler.RoleDiIp;

        //public void Init(HttpApplication context) { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
        }

        private static void Application_BeginRequest(object source, EventArgs e)
        {
            HttpContext context = ((HttpApplication)source).Context;

            // If user not authenticated, check ip
            if (!context.Request.IsAuthenticated)
            {
                AutoLoginByIPHTTPModule al = new AutoLoginByIPHTTPModule();
                GenericPrincipal principal = al.GetAutoLoginPrincipal(HttpContext.Current);

                if (principal != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
        }

        public void Dispose() { /* clean up */ }



        public GenericPrincipal GetAutoLoginPrincipal(HttpContext context)
        {
            string[] asRoles = new string[1];
            GenericPrincipal principal = null;
            bool bAutoLogin = false;

            try
            {
                AutoLoginByIPSettings section = (AutoLoginByIPSettings)ConfigurationManager.GetSection(_sectionName);
                if (section != null)
                    bAutoLogin = section.IsIPAddressAcceptable(context.Request.Url.AbsolutePath, context.Request.UserHostAddress);
            }
            catch (Exception ex)
            {
                new Logger("AutoLoginByIPHTTPModule.GetAutoLoginPrincipal(HttpContext context) - failed", ex.ToString());
            }

            //There are problems with section or there is no section
            if (bAutoLogin)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, _userName, DateTime.Now, DateTime.Now.AddMinutes(20), false, "");
                asRoles.SetValue(_role, 0);
                principal = new GenericPrincipal(new FormsIdentity(ticket), asRoles);
            }
            return principal;
        }

    }
}