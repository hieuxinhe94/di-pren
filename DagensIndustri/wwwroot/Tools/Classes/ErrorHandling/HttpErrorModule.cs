using System.Web;
using System.Web.Configuration;
using System;

namespace DagensIndustri.Tools.Classes.ErrorHandling
{
    /// <summary>
    /// This module handles errors in application.
    /// </summary>
    public class HttpErrorModule : IHttpModule
    {
        private void Context_Error(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            // Get the Web application configuration. 
            System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~/web.config");
            // Get the section. 
            CustomErrorsSection customErrorsSection = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");

            // If customerrors mode == off, then just let IIS handle it
            if (customErrorsSection.Mode != CustomErrorsMode.Off)
            {
                // If mode == on or its a remote request, we want to get the pretty page
                if (customErrorsSection.Mode == CustomErrorsMode.On || !context.Request.IsLocal)
                {
                    Exception ex = context.Error;
                    HttpException httpException = (HttpException)ex;

                    //Set default redirect
                    string redirectURL = customErrorsSection.DefaultRedirect;
                    //Clears existing response headers and sets the desired ones. 
                    context.Response.ClearHeaders();

                    if (httpException != null)
                    {
                        int statusCode = httpException.GetHttpCode();
                        context.Response.StatusCode = statusCode;
                        // Get the collection 
                        CustomErrorCollection customErrorsCollection = customErrorsSection.Errors;
                        //If specific error exist, set redirect
                        if ((customErrorsCollection.Get(statusCode.ToString()) != null))
                            redirectURL = customErrorsCollection.Get(statusCode.ToString()).Redirect;
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                    }

                    context.Server.Transfer(redirectURL);
                }
            }
        }

        #region IHttpModule Members

        public void Dispose()
        {
            //Do nothing here 
        }

        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(Context_Error);
        }

        #endregion

    }
}
