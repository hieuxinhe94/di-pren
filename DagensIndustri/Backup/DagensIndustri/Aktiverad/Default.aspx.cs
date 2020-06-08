using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DagensIndustri.Aktiverad
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string siteUrl = EPiServer.Configuration.Settings.Instance.SiteUrl.ToString();
            string retUrl = HttpUtility.UrlEncode(siteUrl + "guide/plattformar");
            HyperLinkDig.NavigateUrl = siteUrl + "sso-login?ReturnUrl=" + retUrl;
        }
    }
}