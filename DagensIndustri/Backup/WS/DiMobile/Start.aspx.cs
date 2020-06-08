using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;

namespace WS.DiMobile
{
    public partial class Start : System.Web.UI.Page
    {

        //infomaker returns:
        //?entitled=
        //  notAllowedToRead
        //  concurrentUsers
        //  invalidToken
        //  notLoggedId


        private static string UrlPaperOnNet             { get { return ConfigurationManager.AppSettings["UrlPaperOnNet"]; } }
        private static string BonDigCheckLoginPage      { get { return ConfigurationManager.AppSettings["BonDigCheckLoginPage"]; } }
        private static string BonDigUrlAccount          { get { return ConfigurationManager.AppSettings["BonDigUrlAccount"]; } }
        private static string AppIdAndLc                { get { return "?appId=di.se&lc=sv"; } }
        private static string BonDigLoginPage           { get { return ConfigurationManager.AppSettings["BonDigLoginPage"]; } }
        private static string BonDigLogoutPage          { get { return ConfigurationManager.AppSettings["BonDigLogoutPage"]; } }
        private static string BonDigCreateAccountPage   { get { return ConfigurationManager.AppSettings["BonDigCreateAccountPage"]; } }

        private string UrlToThisPage                    { get { return Request.Url.GetLeftPart(UriPartial.Path); } }
        private string UrlToLoginPage                   { get { return BonDigUrlAccount + BonDigLoginPage + AppIdAndLc + "&callback=" + HttpUtility.UrlEncode(UrlToThisPage); } }
        private string UrlToLogoutPage                  { get { return BonDigUrlAccount + BonDigLogoutPage + AppIdAndLc + "&callback=" + HttpUtility.UrlEncode(UrlToThisPage); } }

        public string Token 
        {
            get
            {
                if (Request.QueryString["token"] != null)
                    return Request.QueryString["token"].ToString();

                return string.Empty;
            }
        }

        
        UserOutput _bonDigUser = null;
        public UserOutput BonDigUser 
        {
            get
            {
                if (!string.IsNullOrEmpty(Token) && _bonDigUser == null)
                    _bonDigUser = RequestHandler.GetUserByToken(Token);

                return _bonDigUser;
            }
        }

        public bool ReturnFromBonDigPage 
        {
            get
            {
                string url = HttpContext.Current.Request.Url.ToString();

                //http://localhost:4992/DiMobile/Start.aspx?token=&remembered=false&firstName=&lastName=
                //http://localhost:4992/DiMobile/Start.aspx?token=4iTgPstd9oc4027A50jdZL&remembered=false&firstName=Petter&lastName=Luotsinen

                if (url.Contains("token="))     //131010 removed: ...&& url.Contains("firstName=") && url.Contains("lastName=")
                    return true;

                return false;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetLinkUrls();
                HandlePanels(false, false, false);

                #region return from infomaker
                if (Request.QueryString["entitled"] != null)
                {
                    string ent = Request.QueryString["entitled"];

                    if (ent == "notAllowedToRead" || ent == "false")
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Du är inloggad med ditt Di-konto, men tycks sakna behörighet för att läsa tidningen.<br><br>");
                        sb.Append("<a href='http://dagensindustri.se/Prenumerera/'>Köp prenumeration</a><br>");
                        sb.Append("<a href='http://dagensindustri.se/Kontakta-oss/'>Kontakta kundtjänst</a><br>");
                        sb.Append("<a href='http://www.di.se/'>Gå till di.se</a><br>");
                        sb.Append("<a href='http://dagensindustri.se/'>Gå till dagensindustri.se</a><br>");

                        LiteralInfoMakerReturnHeader.Text = "Behörighet saknas";
                        LiteralInfoMakerReturnBody.Text = sb.ToString();
                        HandlePanels(false, false, true);
                        return;
                    }

                    if (ent == "concurrentUsers")
                    {
                        LiteralInfoMakerReturnHeader.Text = "För många samtidiga användare";
                        LiteralInfoMakerReturnBody.Text = "Det är just nu flera samtidiga användare inloggade på detta konto. Tjänsten tillåter endast en samtidig användare per konto. För att läsa tidningen måste du därför ta kontroll över kontot genom att logga ut och sedan logga in igen.<br><br>" + GetHtmlButton(UrlToLogoutPage, "Logga ut");
                        HandlePanels(false, false, true);
                        return;
                    }

                    if (ent == "invalidToken")
                    {
                        LiteralInfoMakerReturnHeader.Text = "Ogiltig inloggning";
                        LiteralInfoMakerReturnBody.Text = "Din inloggning har upphört att gälla. Klicka nedan för att logga in igen.<br><br>" + GetHtmlButton(UrlToLoginPage, "Logga in");
                        HandlePanels(false, false, true);
                        return;
                    }

                    if (ent == "notLoggedIn" || ent == "notLoggedId")
                    {
                        LiteralInfoMakerReturnHeader.Text = "Ej inloggad";
                        LiteralInfoMakerReturnBody.Text = "Du tycks inte vara korrekt inloggad.<br><br>" + GetHtmlButton(UrlToLoginPage, "Logga in");
                        HandlePanels(false, false, true);
                        return;
                    }
                }
                #endregion

                //check if user is logged in on bondig page
                if (!ReturnFromBonDigPage)
                {
                    RedirToBonDigCheckLoggedIn();
                    return;
                }

                //not logged in: show login / create acc / buy subs
                if (string.IsNullOrEmpty(Token))
                {
                    HandlePanels(true, false, false);
                    return;
                }
                else //token has value
                {
                    List<long> extCusnosInBonDig = RequestHandler.TryGetCirixCusnosFromBonDig(Token);

                    //not activated Di-acc
                    if (extCusnosInBonDig.Count == 0)
                    {
                        if (BonDigUser != null)
                        {
                            StringBuilder sb = new StringBuilder();
                            //url parameter "new" only exists after return from S+ create account page (not after return from login page)
                            if (Request.QueryString["new"] == null) 
                                sb.Append("Du är inloggad med Di-konto: ");
                            else
                                sb.Append("Ditt Di-konto är nu skapat och du är inloggad. Du behöver bekräfta kontot inom 24 timmar genom att klicka på aktiveringslänken vi skickat till ");

                            sb.Append("<b>" + BonDigUser.user.email + "</b>");
                            LiteralLoggedInInfo.Text = sb.ToString();
                        }

                        HandlePanels(false, true, false);
                        return;
                    }

                    //has active Di-acc - redir to paper
                    Response.Redirect(UrlPaperOnNet);
                    return;
                }
            }
        }

        private string GetHtmlButton(string href, string buttonText)
        {
            return "<a href='" + href + "' class='btn btn-large btn-block btn-success' style='max-width:150px;'>" + buttonText + "</a>";
        }

        private void SetLinkUrls()
        {
            HyperLinkToLogin.NavigateUrl         = UrlToLoginPage;
            HyperLinkToCreateAccount.NavigateUrl = BonDigUrlAccount + BonDigCreateAccountPage + AppIdAndLc + "&callback=" + HttpUtility.UrlEncode(UrlToThisPage + "?new=1");
            HyperLinkToBuyDi.NavigateUrl         = ConfigurationManager.AppSettings["DiseMobUrlCampaignPage"];
        }

        private void RedirToBonDigCheckLoggedIn()
        {
            //http://account.qa.newsplus.se/check-logged-in?appId=di.se&lc=sv&callback=http%3a%2f%2flocalhost%3a4992%2fDiMobile%2fStart.aspx 
            StringBuilder sb = new StringBuilder();
            sb.Append(BonDigUrlAccount);
            sb.Append(BonDigCheckLoginPage);
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(Request.Url.GetLeftPart(UriPartial.Path)));
            Response.Redirect(sb.ToString());
        }

        private void HandlePanels(bool showPanelNotLoggedIn, bool showPanelLoggedInNotActivated, bool showPanelNotAllowed)
        {
            PanelNotLoggedIn.Visible = showPanelNotLoggedIn;
            PanelLoggedInNotActivated.Visible = showPanelLoggedInNotActivated;
            PanelNotAllowedToReadPaper.Visible = showPanelNotAllowed;
        }

        protected void ButtonActivateAccount_Click(object sender, EventArgs e)
        {
            LiteralErrMess.Visible = false;
            string s = MiscFunctions.REC(TextBoxCusnoOrEmail.Text.Trim());

            if (!MiscFunctions.IsNumeric(s) && !MiscFunctions.IsValidEmail(s))
            {
                ShowActivateErr("Ange kundnummer eller e-post");
                return;
            }

            
            if (MiscFunctions.IsNumeric(s))
            {
                long cusno = long.Parse(s);
                if (cusno <= 0)
                {
                    ShowActivateErr("Ogiltigt kundnummer");
                    return;
                }

                if (cusno > 0)
                {
                    string err = TryAddSubsToBonDig(cusno);
                    if (!string.IsNullOrEmpty(err))
                    {
                        ShowActivateErr(err);
                        return;
                    }

                    //Response.Write("kundens tidningsprenumeration(er) har lagts in i S+ - skicka användaren till 'Tidningen på nätet'");
                    //Response.End();
                    Response.Redirect(UrlPaperOnNet);
                    return;
                }
            }


            if (MiscFunctions.IsValidEmail(s))
            {
                List<long> cusnos = CirixDbHandler.GetCusnosByEmail(s);
                
                if (cusnos.Count != 1)
                {
                    string err = (cusnos.Count == 0) ? "Angiven e-postadress återfanns inte" : "Angiven e-postadress var inte unik";
                    ShowActivateErr(err + " i vårt system. Ange om möjligt kundnummer och försök igen. Var god kontakta kundtjänst om du inte kommer vidare.");
                    return;
                }
                
                string err2 = TryAddSubsToBonDig(cusnos[0]);
                if (!string.IsNullOrEmpty(err2))
                {
                    ShowActivateErr(err2);
                    return;
                }

                //Response.Write("kundens mailadress var unik i cirix, tidningsprenumeration(er) har lagts in i S+ - skicka användaren till 'Tidningen på nätet'");
                //Response.End();
                Response.Redirect(UrlPaperOnNet);
                return;
            }
        }

        private string TryAddSubsToBonDig(long cusno)
        {
            //if user has different cusno in S+ user is redirected to read-paper-site on page_load
            //string diffCusnoInBonDig = BonDigHandler.TryGetCustHasDiffCusnosInBonDigMess(Token, cusno);
            //if (diffCusnoInBonDig.Length > 0)
            //    return diffCusnoInBonDig;

            //UserOutput bonDigUser = RequestHandler.GetUserByToken(Token);
            if (!BonDigHandler.TryAddSubsAsBonDigImports(Token, cusno, BonDigUser))
                return "Aktivering av Di-konto misslyckades. Var god kontakta kundtjänst.";
            
            return string.Empty;
        }

        private void ShowActivateErr(string mess)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div style='border: 1px dotted red; padding:5px; color:red'>");
            sb.Append(mess);
            sb.Append("</div><br>");
            LiteralErrMess.Text = sb.ToString();
            LiteralErrMess.Visible = true;
        }


    }
}