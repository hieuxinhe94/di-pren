using System;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Web.PageExtensions;
using System.Web;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;

namespace DagensIndustri.Tools.Operations.Articles
{
    /// <summary>
    /// This page is used both to send mail to employees, but also to tip friends about Gasell conferences
    /// </summary>
    public partial class SendMail : DiTemplatePage
    {

        /// <summary>
        /// This TemplatePage doesn't have a matching pageType
        /// Use CurrentPage that is an instance of page with id equals the querystring custompageid
        /// </summary>
        //public SendMail() : base(CustomPageLink.OptionFlag, 1) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //send article
                if (!string.IsNullOrEmpty(Request.QueryString["FileId"]))
                {
                    //User must be authenticated
                    if(HttpContext.Current.User.Identity.IsAuthenticated)
                        PhSendArticleForm.Visible = true;
                }
            }
        }
        
        /// <summary>
        /// Send article to a friend
        /// </summary>
        protected void SendArticleMail(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string strEncryptedKey = MiscFunctions.RC4EnDeCrypt(Request.QueryString["FileId"], "pGEghLjDwkGDf8");
                    string url = MiscFunctions.RemoveWwwFromUrl(EPiServer.Configuration.Settings.Instance.SiteUrl.ToString()) + "Tools/Operations/Articles/ReadArticle.aspx?DIArtId=" + Server.UrlEncode(strEncryptedKey);
                    string subject = Translate("/articlesearch/mail/subject");
                    string body = string.Format(Translate("/articlesearch/mail/body"), InputFromEmail.Value, url, InputToMessage.Value);

                    MiscFunctions.SendMail(InputFromEmail.Value, InputToEmail.Value, subject, body.Replace("[nl]", Environment.NewLine), false);
                    ShowMessage("/articlesearch/mail/message");
                }
            }
            catch (Exception ex)
            {
                new Logger("SendArticleMail() - failed", ex.ToString());
                ShowMessage("/mail/error");
            }
        }

        private void ShowMessage(string translateKey)
        {
            PhSendArticleForm.Visible = false;
            PhMessage.Visible = true;
            LblMessage.Text = Translate(translateKey);
        }

    }
}
