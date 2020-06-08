using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHelpers;
using System.Text;
using System.Data;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class CampTip : DiTemplatePage
    {

        public const int MAX_NUM_TIP_FORMS = 10;

        public PageData CampaignPage 
        {
            get { return EPiServer.DataFactory.Instance.GetPage(CurrentPage["CampaignPage"] as PageReference); }
        }
        
        public string NoAccessText
        {
            get { return Translate("/camptip/noaccesstext"); }
        }

        public bool IsOpenTipPage 
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsOpenTipPage"); }
        }
        
        public int NumTips
        {
            get
            {
                int i = 0;
                if (EPiFunctions.HasValue(CurrentPage, "NumTips"))
                    int.TryParse(CurrentPage["NumTips"].ToString(), out i);

                return i;
            }
        }

        public string NoTipsLeftText
        {
            get { return Translate("/camptip/notipsleft"); }
        }

        public string ThankYouText
        {
            get { return Translate("/camptip/thankyou"); }
        }

        public bool SendTipMailAtOnce
        {
            get { return EPiFunctions.HasValue(CurrentPage, "SendTipMailAtOnce"); }
        }

        public string TipMailText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "TipMailText"))
                    return CurrentPage["TipMailText"].ToString();

                return Translate("/camptip/tipmail");
            }
        }

        public string CodeTipper
        {
            get 
            { 
                if(Request.QueryString["code"] != null)
                    return Request.QueryString["code"].ToString();

                return string.Empty;
            }
        }

        public int NumTipsLeft
        {
            get 
            {
                if (IsOpenTipPage)
                    return NumTips;

                return NumTips - MsSqlHandler.GetNumTipped(CodeTipper); 
            }
        }

        public string MailHeader
        {
            get
            {
                if (!string.IsNullOrEmpty(Tipper_.FirstName) && !string.IsNullOrEmpty(Tipper_.LastName))
                    return Tipper_.FirstName + " " + Tipper_.LastName + " " + Translate("/camptip/mailheadtip1");

                return Translate("/camptip/mailheadtip2");
            }
        }

        public Tipper Tipper_ 
        { 
            get 
            {
                if (ViewState["tipper"] == null)
                    ViewState["tipper"] = new Tipper(CodeTipper);

                return (Tipper)ViewState["tipper"];
            }
        }

        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!IsOpenTipPage)
                {
                    if (Tipper_.Id == 0)
                    {
                        ShowMessage2(NoAccessText, false, true);
                        return;
                    }
                    
                    if (NumTipsLeft <= 0)
                    {
                        ShowMessage2(NoTipsLeftText, false, true);
                        return;
                    }
                }

                //LabelFormHeader.Text = (NumTipsLeft == 1) ? Translate("/camptip/tipformheaderone") : Translate("/camptip/tipformheaderseveral");
                DisplayTipForms();
            }
        }
                
        
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            PlaceHolderForm.Visible = false;

            if (ParseTipForms())
                ShowMessInMainBody(Translate("/camptip/thankyou"));
            else
                ShowMessage2(Translate("/camptip/error"), false, true);
        }


        private bool ParseTipForms()
        {
            try
            {
                for (int i = 1; i <= MAX_NUM_TIP_FORMS; i++)
                {
                    Units.Placeable.CampTip.PersonForm pf = GetTipForm(i);
                    string fn = pf.FirstName.Trim();
                    string ln = pf.LastName.Trim();
                    string em = pf.Email.Trim();

                    if (!string.IsNullOrEmpty(fn) || !string.IsNullOrEmpty(ln) || !string.IsNullOrEmpty(em))
                    {
                        string code = Guid.NewGuid().ToString();
                        MsSqlHandler.InsertTipCustomer(CampaignPage.PageLink.ID, code, CodeTipper, em, fn, ln);

                        if (SendTipMailAtOnce)
                            MiscFunctions.SendMail("no-reply@di.se", em, MailHeader, GetPersonalMailText(code), true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                new Logger("ParseTipForms() - failed", ex.ToString());
                return false;
            }
        }

        
        private string GetPersonalMailText(string code)
        {
            string s = TipMailText;
            s = s.Replace("[#", "<a href='" + EPiFunctions.GetFriendlyAbsoluteUrl(CampaignPage) + "?excus=" + code + "' target='_blank'>");
            s = s.Replace("#]", "</a>");
            return s;
        }


        private void ShowMessInMainBody(string mess)
        {
            Heading1.Visible = false;
            Mainbody1.Text = mess;
            Mainbody1.Visible = true;
            Mainintro1.Visible = false;
            PlaceHolderForm.Visible = false;
        }

        private void ShowMessage2(string key, bool isKey, bool isErrMess) 
        {
            base.ShowMessage(key, isKey, isErrMess);

            //Heading1.Visible = false;
            Mainintro1.Visible = false;
            Mainbody1.Visible = false;
            PlaceHolderForm.Visible = false;
        }


        private void DisplayTipForms()
        {
            for (int i = 1; i <= NumTipsLeft; i++)
                GetTipForm(i).Visible = true;
        }

        private Units.Placeable.CampTip.PersonForm GetTipForm(int i)
        {
            switch (i)
            {
                case 1:
                    return p1;
                case 2:
                    return p2;
                case 3:
                    return p3;
                case 4:
                    return p4;
                case 5:
                    return p5;
                case 6:
                    return p6;
                case 7:
                    return p7;
                case 8:
                    return p8;
                case 9:
                    return p9;
                case 10:
                    return p10;
                default:
                    return null;
            }
        }

    }

    [Serializable]
    public class Tipper
    {
        public int Id = 0;
        public string CodeTipper = string.Empty;
        public int Cusno = 0;
        public string FirstName = string.Empty;
        public string LastName = string.Empty;
        public string Email = string.Empty;
        public int CampTipEpiPageId = 0;
        public int CampEpiPageId = 0;

        public Tipper(string codeTipper)
        {
            if (string.IsNullOrEmpty(codeTipper))
                return;

            try
            {
                DataSet ds = MsSqlHandler.GetTipper(codeTipper);

                if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows != null)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Id = int.Parse(dr["id"].ToString());
                    CodeTipper = codeTipper;
                    int.TryParse(dr["cusno"].ToString(), out Cusno);
                    FirstName = dr["firstName"].ToString();
                    LastName = dr["lastName"].ToString();
                    Email = dr["email"].ToString();
                    int.TryParse(dr["campTipEpiPageId"].ToString(), out CampTipEpiPageId);
                    int.TryParse(dr["campEpiPageId"].ToString(), out CampEpiPageId);
                }
            }
            catch (Exception ex)
            {
                new Logger("Tipper constructor failed", ex.ToString());
            }
        }
    }

}