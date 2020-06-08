using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DagensIndustri.Templates.Public.Pages.C_Campaign
{
    /// <summary>
    /// Summary description for GetParData
    /// </summary>
    public class GetJson : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var action = context.Request.QueryString["action"];

            if (!string.IsNullOrEmpty(action))
            {
                if (action == "zip") {
                    ActionZip(context);
                }
                else if (action == "email") {
                    ActionEmail(context);
                }
                
                return;
            }

            ActionPar(context);            
        }

        private void ActionEmail(HttpContext context)
        {
            var email = context.Request.QueryString["email"];
            var status = CampaignHelper.SaveEmail(email);

            context.Response.ContentType = "text/plain";
            context.Response.Write(status);
        }

        private void ActionZip(HttpContext context) 
        {
            var zip = context.Request.QueryString["zip"];
            var city = CampaignHelper.GetCity(zip);

            context.Response.ContentType = "text/plain";
            context.Response.Write(city);        
        }

        private void ActionPar(HttpContext context) 
        {

            var pno = context.Request.QueryString["pno"];
            var pageid = context.Request.QueryString["pageId"];
            var campaign = context.Request.QueryString["campaign"];

            var parData = CampaignHelper.GetNormalizedParData(pno, campaign, pageid);

            JavaScriptSerializer js = new JavaScriptSerializer();
            string s = js.Serialize(parData);

            context.Response.ContentType = "text/plain";
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}