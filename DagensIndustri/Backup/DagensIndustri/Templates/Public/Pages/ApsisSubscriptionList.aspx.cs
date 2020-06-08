using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.EPiJobs.Apsis;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using System.Data;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class ApsisSubscriptionList : DiTemplatePage
    {

        ApsisWsHandler _ws;
        public ApsisWsHandler Ws 
        {
            get 
            { 
                if(_ws == null)
                    _ws = new ApsisWsHandler();

                return _ws;
            }
        }

        
        public bool IsSmsList 
        { 
            get { return EPiFunctions.HasValue(CurrentPage, "IsSmsList"); } 
        }
        
        public string ApsisListId 
        {  
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "ApsisListId"))
                    return CurrentPage["ApsisListId"].ToString();

                return string.Empty;
            }
        }

        public ApsisListSubscriber CustVisiting
        {
            get
            {
                if (ViewState["cv"] != null)
                    return (ApsisListSubscriber)ViewState["cv"];

                ViewState["cv"] = SetCustVisitingFromDb();

                return (ApsisListSubscriber)ViewState["cv"];
            }
        }

        ApsisListSubscriber _cial;
        public ApsisListSubscriber CustInApsisList 
        {
            get 
            {
                if (_cial != null)
                    return _cial;

                _cial = Ws.TryGetApsisListSubscriber(CustVisiting);

                return _cial;
            }
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl1;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            HandleGuiVisibility(true, true, false, false);
            
            if (string.IsNullOrEmpty(ApsisListId))
            {
                UserMessageControl1.ShowMessage(Translate("/apsisSubsList/nolistid"), true, true);
                return;
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HandleGuiVisibility(false, true, false, false);
                Mainbody1.Text = "<p>" + string.Format(Translate("/apsisSubsList/notloggedin"), EPiFunctions.GetLoginPageUrl(CurrentPage)) + "</p>";
                return;
            }

            if (!IsPostBack)
            {
                if (CustInApsisList == null)
                {
                    SetGuiInfo();
                    HandleGuiVisibility(true, true, true, false);
                }
                else
                    HandleGuiVisibility(true, true, false, true);
            }

        }

        private void SetGuiInfo()
        {
            if (!IsSmsList)
            {
                InputEmail.Text = CustVisiting.Email;
                InputPhone.Visible = false;
            }
            else
            {
                InputEmail.Visible = false;
                InputPhone.Text = CustVisiting.PhoneMob;
            }
        }

        private void HandleGuiVisibility(bool mainIntro, bool mainBody, bool subscribe, bool unSubscribe)
        {
            Mainintro1.Visible = mainIntro;
            Mainbody1.Visible = mainBody;
            PlaceHolderSubscribe.Visible = subscribe;
            PlaceHolderUnSubscribe.Visible = unSubscribe;
        }


        protected void ButtonSubscribe_Click(object sender, EventArgs e)
        {
            HandleGuiVisibility(false, false, false, false);

            if (!IsSmsList)
                CustVisiting.Email = InputEmail.Text;
            else
                CustVisiting.PhoneMob = InputPhone.Text;

            if(Ws.AddCustToApsisList(CustVisiting))
                UserMessageControl1.ShowMessage("/apsisSubsList/subsadded", true, false);
            else
                UserMessageControl1.ShowMessage("/apsisSubsList/error", true, true);
        }

        protected void ButtonUnSubscribe_Click(object sender, EventArgs e)
        {
            HandleGuiVisibility(false, false, false, false);
            
            if(Ws.DeleteCustFromApsisList(CustInApsisList))
                UserMessageControl1.ShowMessage("/apsisSubsList/subsremoved", true, false);
            else
                UserMessageControl1.ShowMessage("/apsisSubsList/error", true, true);
        }


        private ApsisListSubscriber SetCustVisitingFromDb()
        {
            string userid = HttpContext.Current.User.Identity.Name;

            try
            {
                long cusno = MembershipDbHandler.GetCusno(userid);
                string name = "";
                string email = "";
                string phoneMob = "";

                DataSet ds = CirixDbHandler.Ws.GetCustomer_(cusno);
                if (ds != null && ds.Tables != null && ds.Tables[0].Rows != null)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    name = dr["ROWTEXT1"] as string;
                    email = dr["EMAILADDRESS"] as string;
                    phoneMob = dr["O_PHONE"] as string;
                }

                return new ApsisListSubscriber(ApsisListId, cusno.ToString(), name, email, phoneMob);
            }
            catch (Exception ex)
            {
                new Logger("SetCustVisitingFromDb() failed for userid:" + userid, ex.ToString());
            }

            return null;
        }


    }
}