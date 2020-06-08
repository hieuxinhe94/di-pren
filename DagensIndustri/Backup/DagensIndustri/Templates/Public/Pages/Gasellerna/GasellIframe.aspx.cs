using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Pages.Gasellerna
{
    public partial class GasellIframe : EPiServer.TemplatePage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


        }

        protected string GetURL()
        {
            string url = CurrentPage["IframeSrc"] as string ?? string.Empty;
            List<string> excludeKeys = new List<string> { "id", "epslanguage", "idkeep" };
            url += url.Contains("?") ? "&" : "?";

            foreach (string key in Request.QueryString.AllKeys)
            {
                //exclude epi parameters
                if (!excludeKeys.Contains(key))
                    url += key + "=" + Request.QueryString[key] + "&";
            }

            //trim & and ?
            return url.Trim('&', '?');
        }
    }
}