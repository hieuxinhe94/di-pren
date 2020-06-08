using System;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Web.PageExtensions;
using System.Web;

namespace EPiServer.Functions.Articles
{
    /// <summary>
    ///     
    /// </summary>
    public partial class ArticlePopUpNav : Page
    {
        protected string p_Title = string.Empty;
        protected string p_artlink = string.Empty;
        protected string p_Datepath = string.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {               
                p_artlink = Request.QueryString["art"];

                //do nothing if length is smaller than 11
                //this to prevent substring function to cause exception if user manipulates the querystring
                if (string.IsNullOrEmpty(p_artlink) || p_artlink.Length < 11)
                    return;

                p_Datepath = p_artlink.Substring(3, 4) + "/" + p_artlink.Substring(7, 2) + "/" + p_artlink.Substring(9, 2);
            }
        }



    }
}
