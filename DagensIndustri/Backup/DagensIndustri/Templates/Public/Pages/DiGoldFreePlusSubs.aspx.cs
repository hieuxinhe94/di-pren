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
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.Misc;
using System.Text;
using DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class DiGoldFreePlusSubs : DiTemplatePage
    {
        long _cusno = 0;
        string _email = "";
        string _passwd = "";

        private bool CustHasAlreadyTakenDeal
        {
            get
            {
                DataSet ds = CirixDbHandler.GetSubscriptions(_cusno, false);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows != null)
                {
                    long campNo = 1169;
                    if (MiscFunctions.GetAppsettingsValue("UseCirixTestWS") == "true")
                        campNo = 1113;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (long.Parse(dr["CAMPNO"].ToString()) == campNo)
                            return true;
                    }
                }

                return false;
            }
        }

        public string MailHead
        {
            get { return "Inloggningsuppgifter för Dagens industri i läsplattan"; }
        }

        public string MailBody 
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Här kommer dina användaruppgifter för gratis Dagens industri i läsplattan hela juni, juli och augusti.<br><br>");
                sb.Append("Användarnamn: " + _email + "<br>");
                sb.Append("Lösenord: " + _passwd + "<br><br>");
                sb.Append("Trevlig sommarläsning!");
                return sb.ToString();
            }
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            Mainintro1.Visible = false;
            Mainbody1.Visible = false;
            PanelThanks.Visible = false;
            
            TrySaveSubs();
        }


        private void TrySaveSubs()
        {
            if (!SetMembers(HttpContext.Current.User.Identity.Name))
            {
                UserMessageControl.ShowMessage("/freeplussubs/contactus", true, true);
                return;
            }

            if (CustHasAlreadyTakenDeal)
            {
                UserMessageControl.ShowMessage("/freeplussubs/alreadyapplied", true, true);
                return;
            }


            bool okCirix = DIClassLib.DbHandlers.CirixDbHandler.AddSubsIpadSummer(_cusno);
            if (!okCirix)
            {
                UserMessageControl.ShowMessage("/freeplussubs/contactus", true, true);
            }
            else
            {
                //add to bonnier digital subscription system
                //if (DIClassLib.BonnierDigital.BonDigHandler.AddSubsToBonDig(_cusno, _email, _passwd))
                //    MiscFunctions.SendMail("no-reply@di.se", _email, MailHead, MailBody, true);

                PanelThanks.Visible = true;
            }

            new Logger(Settings.LogEvent_FreeDi, _cusno, okCirix);
        }

        
        /// <summary>
        /// sets: _cusno, _email, _passwd
        /// </summary>
        private bool SetMembers(string userName)
        {
            SqlDataReader DR = null;

            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    DR = SqlHelper.ExecuteReader("DisePren", "getUserInfo", new SqlParameter("@Login", userName));

                    if (DR.Read())
                    {
                        if (DR["cusno"] != DBNull.Value)
                            _cusno = long.Parse(DR["cusno"].ToString());

                        if (DR["email"] != DBNull.Value)
                            _email = DR["email"].ToString();

                        if (DR["password"] != DBNull.Value)
                            _passwd = DR["password"].ToString();

                        //if (DR["userid"] != DBNull.Value)
                        //    UserName = DR["userid"].ToString();
                        //if (DR["birthNo"] != DBNull.Value)
                        //    BirthNo = DR["birthNo"].ToString();

                        if (_cusno > 0 && MiscFunctions.IsValidEmail(_email) && !string.IsNullOrEmpty(_passwd))
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("SetMembers() failed", ex.ToString());
            }

            return false;
        }



        #region old code

        //public bool UrlArgIsSet 
        //{
        //    get
        //    {
        //        if (Request.QueryString["do"] != null && Request.QueryString["do"].ToString() == "1")
        //            return true;

        //        return false;
        //    }
        //}

        //protected override void OnLoad(EventArgs e)
        //{
            //HyperLinkDoLogin.NavigateUrl = EPiFunctions.GetLoginPageUrl(CurrentPage);
            //base.OnLoad(e);
            //base.UserMessageControl = UserMessageControl;

            //HyperLinkLogin.NavigateUrl = GetReturnUrl(true);
            //SetLinkVisibility(true);


            //not logged in
            //if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            //    CreatePopUpLink();
            //else
            //{
            //    //not gold member
            //    if (!HttpContext.Current.User.IsInRole("DiGold"))
            //    {
            //logged in user, not diGold will see join gold dialogue before this page (not perfect).
            //by creating this url they have to press a link to subscribe, else they would auto subscribe when loading page.
            //string url = (UrlArgIsSet) ? GetReturnUrl(true) : GetReturnUrl(false);
            //EPiFunctions.RedirectToPage(Page, EPiFunctions.GetDiGoldFlowPage(CurrentPage).PageLink, url);
            //    }
            //    else
            //    {
            //        if (UrlArgIsSet)
            //            TrySaveSubs();
            //    }
            //}
        //}


        //private string GetReturnUrl(bool withUrlArg)
        //{
        //    string s = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
        //    if (withUrlArg)
        //        return s + "?do=1";

        //    return s;
        //}


        //private void CreatePopUpLink()
        //{
        //    HyperLinkLogin.NavigateUrl = "#membership-required";
        //    HyperLinkLogin.CssClass = "ajax";

        //    DiGoldMembershipPopup goldPopUp = EPiFunctions.FindDiGoldMembershipPopup(Page) as DiGoldMembershipPopup;
        //    if (goldPopUp != null)
        //    {
        //        //string absFriendUrl = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
        //        goldPopUp.RegisterSetReturnURLScript(HyperLinkLogin, GetReturnUrl(true));
        //    }

        //    SetLinkVisibility(true);
        //}

        //private void SetLinkVisibility(bool login)
        //{
        //    HyperLinkLogin.Visible = login;
        //    //LinkButtonActivateDiPlus.Visible = activatePlus;
        //}


        //protected void LinkButtonActivateDiPlus_Click(object sender, EventArgs e)
        //{
        //    SaveSubs();
        //}

        #endregion

    }
}