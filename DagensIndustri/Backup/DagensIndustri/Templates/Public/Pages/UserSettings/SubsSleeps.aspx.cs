using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using System.Text;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.CirixMappers;
using System.Data;
using DIClassLib.EPiJobs.SyncSubs;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class SubsSleeps : DiTemplatePage
    {
        //personal code (from MBA db) enables auto login for user
        public string UrlCode
        {
            get
            {
                if (Request.QueryString["code"] != null)
                    return Request.QueryString["code"].ToString();

                return string.Empty;
            }
        }
        
        private long Subsno
        {
            get
            {
                if (Request.QueryString["sid"] != null && MiscFunctions.IsNumeric(Request.QueryString["sid"].ToString()))
                    return long.Parse(Request.QueryString["sid"].ToString());

                return 0;
            }
        }

        public SubscriptionCirixMap Subscription
        {
            get
            {
                if (ViewState["sub"] != null)
                    return (SubscriptionCirixMap)ViewState["sub"];

                foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
                {
                    if (sub.Subsno == Subsno && sub.SubsCusno == Subscriber.Cusno)
                    {
                        ViewState["sub"] = sub;
                        return (SubscriptionCirixMap)ViewState["sub"];
                    }
                }

                return null;
            }
        }
        
        public SubscriptionUser2 Subscriber
        {
            get
            {
                if (ViewState["Subscriber"] == null)
                    ViewState["Subscriber"] = new SubscriptionUser2();

                return (SubscriptionUser2)ViewState["Subscriber"];
            }
            set
            {
                ViewState["Subscriber"] = value;
            }
        }

        private bool SubsnoBelongsToSubscriber
        {
            get { return (Subscription != null); }
        }

        public bool SubIsAutoGiro
        {
            get { return (Subscription.Autogiro == "Y"); }
        }

        public bool SubIsQuarterlyPayment
        {
            get
            {
                if (ViewState["qp"] != null)
                    return (bool)ViewState["qp"];

                ViewState["qp"] = false;
                DataSet ds = CirixDbHandler.Ws.GetInvArgItems_(Subsno, Subscription.Extno);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    if (ds.Tables[0].Rows.Count > 1)
                        ViewState["qp"] = true;
                }

                return (bool)ViewState["qp"];
            }
        }

        public DateTime DateMinAddrChange
        {
            get
            {
                if (ViewState["DateMinAddrChange"] != null)
                    return (DateTime)ViewState["DateMinAddrChange"];

                ViewState["DateMinAddrChange"] = CirixDbHandler.GetNextIssueDateIncDiRules(DateTime.Now, Subscription.PaperCode, Subscription.ProductNo);

                return (DateTime)ViewState["DateMinAddrChange"];
            }
        }

        protected SubsSleepsCirixMap SelectedSubsSleep { get; set; }

        protected List<SubsSleepsCirixMap> FutureSubsSleeps
        {
            get
            {
                List<SubsSleepsCirixMap> ret = new List<SubsSleepsCirixMap>();

                foreach (SubscriptionCirixMap sub in Subscriber.SubsActive)
                {
                    if (sub.Subsno == Subsno)
                    {
                        ret = sub.SubsSleeps;
                        ret.Sort();
                        return ret;
                    }
                }

                return ret;
            }
        }

        private ObjHolderForSubsSleepFormClick ObjHolderForSubsSleepFormClick
        {
            get
            {
                if (ViewState["objHolderForSubsSleepFormClick"] != null)
                    return (ObjHolderForSubsSleepFormClick)ViewState["objHolderForSubsSleepFormClick"];

                return null;
            }
            set
            {
                ViewState["objHolderForSubsSleepFormClick"] = value;
            }
        }

        public DateTime DateStart 
        {
            get
            { 
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(Date1.Text, out dt);
                return dt;
            }
            set
            {
                Date1.Text = value.ToShortDateString();
            }
        }

        public DateTime DateEnd
        {
            get
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(Date2.Text, out dt);
                return dt;
            }
            set
            {
                Date2.Text = value.ToShortDateString();
            }
        }

        //public DateTime CirixMaxDate { get { return new DateTime(2078, 12, 31); } }   //130412 - changed to 1800-01-01
        public DateTime CirixDateNotSet { get { return new DateTime(1800, 1, 1); } }
        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvSubsSleeps.RowCommand += new GridViewCommandEventHandler(gvSubsSleeps_OnRowCommand);
            gvSubsSleeps.RowDeleting += new GridViewDeleteEventHandler(gvSubsSleeps_RowDeleting);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                #region try to log in user by code in url
                if (!string.IsNullOrEmpty(UrlCode) && MiscFunctions.IsGuid(UrlCode))
                {
                    HandleVisibility(false, false);

                    //get cusno from code
                    long cusno = MsSqlHandler.MdbGetCusnoByCode(new Guid(UrlCode));
                    if (cusno < 1)
                    {
                        new Logger("GetCusnoByCode() failed for code:" + UrlCode, "not an exception");
                        UserMessageControl.ShowMessage("Kunduppgifter hittades inte för kod: " + UrlCode, false, true);
                        return;
                    }

                    int ret = SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno);
                    if (ret != 1)
                    {
                        new Logger("SyncCustToMssqlLoginTables() failed for cusno:" + cusno + ", ret:" + ret, "not an exception: -1 does not have active subs, -2 could not find customer facts in cirix");
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Ett tekniskt problem uppstod när dina uppgifter skulle hämtas från prenumerationssystemet.<br>");
                        sb.Append("Var god kontakta kundtjänst.<br>");
                        sb.Append("Ditt kundnummer är: " + cusno.ToString());
                        UserMessageControl.ShowMessage(sb.ToString(), false, true);
                        return;
                    }

                    if (!LoginUtil.TryLoginUserToDagensIndstri(cusno))
                    {
                        new Logger("TryLoginUserToDagensIndstri() failed for cusno:" + cusno, "not an exception");
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Den automatiska inloggningen misslyckades.<br>");
                        sb.Append("Var god kontakta kundtjänst om problemet kvarstår.<br>");
                        sb.Append("Ditt kundnummer är: " + cusno.ToString());
                        UserMessageControl.ShowMessage(sb.ToString(), false, true);
                        return;
                    }

                    //redir on success
                    Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage) + "?sid=" + Subsno);
                    return;
                }
                #endregion
                
                
                if (!User.Identity.IsAuthenticated)
                {
                    HandleVisibility(false, false);
                    ShowMessage("/mysettings2/notloggedin", true, true);
                    return;
                }

                if (Subsno <= 0)
                {
                    HandleVisibility(true, false);
                    ShowMessage("Ogiltigt prenumerationsnummer.", false, true);
                    return;
                }

                if (!SubsnoBelongsToSubscriber)
                {
                    HandleVisibility(true, false);
                    ShowMessage("Prenumerationsnummer: " + Subsno.ToString() + " tillhör inte inloggad person.", false, true);
                    return;
                }

                Date1.MinValue = DateMinAddrChange.ToShortDateString();
                Date2.MinValue = DateMinAddrChange.ToShortDateString();


                if (SubIsQuarterlyPayment || (SubIsAutoGiro && Settings.HideSubsSleepsForAutogiroCust))
                    PlaceHolderAllowWebPaper.Visible = false;


                PlaceHolderForm.Visible = false;
                
                //DateTime dt = Subscriber.SubsActive.SingleOrDefault(x => x.Subsno == Subsno).SubsEndDate;
                //Address1.Date1Max = dt;
                //Address1.Date2Max = dt;
                //Address1.Visible = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            gvSubsSleeps.DataSource = FutureSubsSleeps;
            gvSubsSleeps.DataBind();
            PlaceHolderFutureSubsSleeps.Visible = (FutureSubsSleeps.Count > 0);
        }


        protected void LinkButtonNewAddress_Click(object sender, EventArgs e)
        {
            gvSubsSleeps.SelectedIndex = -1;
            ClearForm();
            ObjHolderForSubsSleepFormClick = new ObjHolderForSubsSleepFormClick("insert", null);
            PlaceHolderForm.Visible = true;
        }

        protected void DateFormButton_Click(object sender, EventArgs e)
        {
            #region validate input
            if (!Page.IsValid)
            {
                ShowMessage("/mysettings2/errorpagenotvalid", true, true);
                return;
            }

            if (DateStart == DateTime.MinValue)
            {
                ShowMessage("Ange fråndatum", false, true);
                return;
            }

            if (DateStart < DateMinAddrChange)
            {
                ShowMessage("Tidigaste möjliga fråndatum är " + DateMinAddrChange.ToShortDateString() + ".<br>Var god välj ett senare datum.", false, true);
                return;
            }

            if (DateStart > DateMinAddrChange)
                DateStart = CirixDbHandler.GetNextIssueDateIncDiRules(DateStart, Subscription.PaperCode, Subscription.ProductNo);

            if (!string.IsNullOrEmpty(Date2.Text))
            {
                //DateEnd = Subscriber.GetClosestIssueDateByUsersRole(Page.User, DateEnd).AddDays(-1);
                DateTime tmpEnd = CirixDbHandler.GetNextIssueDateIncDiRules(DateEnd, Subscription.PaperCode, Subscription.ProductNo);
                if (tmpEnd.Date > DateEnd.Date)
                    DateEnd = tmpEnd.AddDays(-1);

                if (DateStart >= DateEnd)
                {
                    ShowMessage("Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.", false, true);
                    return;
                }
            }

            if (!DateSpanOk(DateStart, DateEnd))
            {
                ShowMessage("Datumintervallet kolliderar med tidigare sparat uppehåll.<br>Var god försök igen.", false, true);
                return;
            }
            #endregion


            if (ObjHolderForSubsSleepFormClick.SqlCommand == "update")
            {
                string s = Subscriber.DeleteHolidayStop(Subsno, ObjHolderForSubsSleepFormClick.SubsSleepsOrg.SleepStartDate, ObjHolderForSubsSleepFormClick.SubsSleepsOrg.SleepEndDate);
                System.Threading.Thread.Sleep(1500);
            }

            string allowWebPaper = "Y";
            string creditType = Settings.CreditType_NoCompensation;
            if (!CheckBoxAllowWebPaper.Checked)
            {
                allowWebPaper = "N";
                if (Subscription.SubsKind != Settings.SubsKind_friex)  //friex: CreditType_NoCompensation
                    creditType = (SubIsAutoGiro || SubIsQuarterlyPayment) ? Settings.CreditType_Money : Settings.CreditType_Days;
            }

            if (string.IsNullOrEmpty(Date2.Text))
                DateEnd = CirixDateNotSet;

            string ret = Subscriber.CreateHolidayStop(Subsno, DateStart, DateEnd, Settings.SleepType_Break, allowWebPaper, creditType);
            if (ret == "OK")
            {
                new Logger(Settings.LogEvent_HolidayStop, Subscriber.Cusno, true);
                PlaceHolderForm.Visible = false;
                gvSubsSleeps.SelectedIndex = -1;
                SendCustMail(DateStart, DateEnd);
                ShowMessage("Uppehållet har sparats.", false, false);
            }
            else
            {
                //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
                new Logger("DateFormButton_Click() - failed for cusno: " + Subscriber.Cusno, ret);
                ShowMessage(GetErrMess(), false, true);
            }
        }

        
        protected void gvSubsSleeps_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime endDate = (DateTime)DataBinder.Eval(e.Row.DataItem, "SleepEndDate");
                if (endDate == CirixDateNotSet)
                    e.Row.Cells[1].Text = "(Tillsvidare)";
            }
        }  

        public void gvSubsSleeps_OnRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            SelectedSubsSleep = FutureSubsSleeps.SingleOrDefault(x => x.Id == e.CommandArgument.ToString());
        }

        protected void gvSubsSleeps_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateForm(SelectedSubsSleep);
            PlaceHolderForm.Visible = true;
            ObjHolderForSubsSleepFormClick = new ObjHolderForSubsSleepFormClick("update", SelectedSubsSleep);
        }

        public void gvSubsSleeps_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
            PlaceHolderForm.Visible = false;
            
            if (Subscriber.DeleteHolidayStop(Subsno, SelectedSubsSleep.SleepStartDate, SelectedSubsSleep.SleepEndDate) == "OK")
                ShowMessage("Uppehållet har raderats.", false, false);
            else
                ShowMessage(GetErrMess(), false, false);
        }


        private void PopulateForm(SubsSleepsCirixMap sleepMap)
        {
            Date1.Text = sleepMap.SleepStartDate.ToShortDateString();

            if (sleepMap.SleepEndDate != CirixDateNotSet)
                Date2.Text = sleepMap.SleepEndDate.ToShortDateString();
            else
                Date2.Text = "";

            CheckBoxAllowWebPaper.Checked = (sleepMap.AllowWebPaper == "Y");
        }
        
        private bool DateSpanOk(DateTime dateStart, DateTime dateEnd)
        {
            SubsSleepsCirixMap org = ObjHolderForSubsSleepFormClick.SubsSleepsOrg;

            foreach (SubsSleepsCirixMap old in FutureSubsSleeps)
            {
                if (org != null && old.AreEqual(org))
                    continue;

                //dateStart in old interval
                if (dateStart >= old.SleepStartDate && dateStart <= old.SleepEndDate)
                    return false;

                //dateEnd in old interval
                if (dateEnd >= old.SleepStartDate && dateEnd <= old.SleepEndDate)
                    return false;

                //overlapping entire old interval
                if (dateStart < old.SleepStartDate && dateEnd > old.SleepEndDate)
                    return false;
            }

            return true;
        }

        private void SendCustMail(DateTime dtStart, DateTime dtEnd)
        {
            if (!MiscFunctions.IsValidEmail(Subscriber.Email))
                return;

            StringBuilder sb = new StringBuilder();
            sb.Append("Tack, vi har mottagit ditt ");

            if (dtEnd > CirixDateNotSet)
                sb.Append("uppehåll för perioden " + dtStart.ToShortDateString() + " - " + dtEnd.ToShortDateString());
            else
                sb.Append("tillsvidareuppehåll från " + dtStart.ToShortDateString());

            if (CheckBoxAllowWebPaper.Checked)
                sb.Append("<br><br>Du har valt att ta del av tidningen digitalt under din uppehållsperiod.");

            sb.Append("<br><br><br>");
            sb.Append("Vid eventuella frågor kontakta vår kundtjänst på tel 08-573 651 00 eller <a href='mailto:pren@di.se'>pren@di.se</a>.<br><br>");
            sb.Append("Med vänliga hälsningar<br>");
            sb.Append("Dagens industri");

            MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), Subscriber.Email, "Bekräftelse uppehåll", sb.ToString(), true);
        }

        private string GetErrMess()
        {
            StringBuilder err = new StringBuilder();
            err.Append("Ett tekniskt fel uppstod.<br>");
            err.Append("Ring gärna kundtjänst på 08-573 651 00 så hjälper vi till att lösa problemet.");
            return err.ToString();
        }

        private void HandleVisibility(bool showMenu, bool showSubsSleepsAndForm)
        {
            MySettingsMenu1.Visible = showMenu;
            PlaceHolderFutureSubsSleepsAndForm.Visible = showSubsSleepsAndForm;
        }

        private void ClearForm()
        {
            Date1.Text = "";
            Date2.Text = "";
            CheckBoxAllowWebPaper.Checked = false;
        }

        


    }

    [Serializable]
    public class ObjHolderForSubsSleepFormClick
    {
        /// <summary>
        /// values: insert update delete
        /// </summary>
        public string SqlCommand;

        public SubsSleepsCirixMap SubsSleepsOrg;


        public ObjHolderForSubsSleepFormClick() { }

        public ObjHolderForSubsSleepFormClick(string sqlCommand, SubsSleepsCirixMap subsSleepsOrg)
        {
            SqlCommand = sqlCommand;
            SubsSleepsOrg = subsSleepsOrg;
        }

    }

}