using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Templates.Public.Pages.UserSettings;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.SyncSubs;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.CirixMappers;
using EPiServer;

namespace DagensIndustri.Templates.Public.Units.Placeable.UserSettings
{
    public partial class SubsSleepsUC : UserControlBase  //, IAddress
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

        private long UrlSubsno
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
                    if (sub.Subsno == UrlSubsno && sub.SubsCusno == Subscriber.Cusno)
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
                DataSet ds = SubscriptionController.GetInvArgItems(UrlSubsno, Subscription.Extno);
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

                ViewState["DateMinAddrChange"] = SubscriptionController.GetNextIssueDateIncDiRules(DateTime.Now, Subscription.PaperCode, Subscription.ProductNo);

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
                    if (sub.Subsno == UrlSubsno)
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
                Date1.Text = value.ToString("yyyy-MM-dd");
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
                Date2.Text = value.ToString("yyyy-MM-dd");
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
            //base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
                if (Subscriber == null || Subscription == null)
                    return;

                Date1.MinValue = DateMinAddrChange.ToString("yyyy-MM-dd");
                Date2.MinValue = DateMinAddrChange.ToString("yyyy-MM-dd");


                if (SubIsQuarterlyPayment || (SubIsAutoGiro && Settings.HideSubsSleepsForAutogiroCust))
                {
                    PlaceHolderAllowWebPaper.Visible = false;
                    //PlaceHolderInfoReg.Visible = false;
                    PlaceHolderInfoAutogiro.Visible = true;
                }

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
                UserMessageControl.ShowMessage("/mysettings2/errorpagenotvalid", true, true);
                return;
            }

            if (DateStart == DateTime.MinValue)
            {
                UserMessageControl.ShowMessage("Ange fråndatum", false, true);
                return;
            }

            if (DateStart < DateMinAddrChange)
            {
                UserMessageControl.ShowMessage("Tidigaste möjliga fråndatum är " + DateMinAddrChange.ToString("yyyy-MM-dd") + ".<br>Var god välj ett senare datum.", false, true);
                return;
            }

            if (DateStart > DateMinAddrChange)
                DateStart = SubscriptionController.GetNextIssueDateIncDiRules(DateStart, Subscription.PaperCode, Subscription.ProductNo);

            if (!string.IsNullOrEmpty(Date2.Text))
            {
                //DateEnd = Subscriber.GetClosestIssueDateByUsersRole(Page.User, DateEnd).AddDays(-1);
                DateTime tmpEnd = SubscriptionController.GetNextIssueDateIncDiRules(DateEnd, Subscription.PaperCode, Subscription.ProductNo);
                if (tmpEnd.Date > DateEnd.Date)
                    DateEnd = tmpEnd.AddDays(-1);

                if (DateStart >= DateEnd)
                {
                    UserMessageControl.ShowMessage("Fråndatum måste vara ett tidigare datum än tilldatum.<br>Var god försök igen.", false, true);
                    return;
                }

                //max 8 weeks subsSleep
                // Removed restriction on sleep length: https://bdigital-jira.atlassian.net/browse/DIU-241
                //int diffDays = (int)Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Day, DateStart, DateEnd);
                //if (diffDays > (8 * 7))
                //{
                //    UserMessageControl.ShowMessage("Uppehållet får max vara 8 veckor långt.<br>Var god försök igen.", false, true);
                //    return;
                //}
            }

            if (!DateSpanOk(DateStart, DateEnd))
            {
                UserMessageControl.ShowMessage("Datumintervallet kolliderar med tidigare sparat uppehåll.<br>Var god försök igen.", false, true);
                return;
            }
            #endregion


            if (ObjHolderForSubsSleepFormClick.SqlCommand == "update")
            {
                string s = Subscriber.DeleteHolidayStop(UrlSubsno, ObjHolderForSubsSleepFormClick.SubsSleepsOrg.SleepStartDate, ObjHolderForSubsSleepFormClick.SubsSleepsOrg.SleepEndDate);
                System.Threading.Thread.Sleep(1500);
            }

            string allowWebPaper = "Y";
            string creditType = Settings.CreditType_NoCompensation;
            if (!CheckBoxAllowWebPaper.Checked)
            {
                allowWebPaper = "N";
                if (Subscription.SubsKind != Settings.SubsKind_friex) //friex: CreditType_NoCompensation
                {
                    creditType = (SubIsQuarterlyPayment) ? Settings.CreditType_Money : Settings.CreditType_Days;
                    //creditType = (SubIsAutoGiro || SubIsQuarterlyPayment) ? Settings.CreditType_Money : Settings.CreditType_Days;
                }

                if (SubIsAutoGiro)
                {
                    allowWebPaper = "Y";
                    creditType = Settings.CreditType_NoCompensation;
                }
            }

            if (string.IsNullOrEmpty(Date2.Text))
                DateEnd = CirixDateNotSet;
            
            string ret = Subscriber.CreateHolidayStop(UrlSubsno, DateStart, DateEnd, Settings.SleepType_Break, allowWebPaper, creditType);
            if (ret == "OK")
            {
                new Logger(Settings.LogEvent_HolidayStop, Subscriber.Cusno, true);
                PlaceHolderForm.Visible = false;
                gvSubsSleeps.SelectedIndex = -1;
                SendCustMail(DateStart, DateEnd);
                UserMessageControl.ShowMessage("Uppehållet har sparats.", false, false);
            }
            else
            {
                //MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), "Spara permanent adressändring misslyckades", "Kundnummer: " + Subscriber.Cusno + "<br><br>Önskad adress<br>" + dh.ToString(), true);
                new Logger(
                    string.Format(
                        "SubsSleepsUC.DateFormButton_Click() CreateHolidayStop - failed. Cusno:{0}, UrlSubsno:{1}, DateStart:{2}, DateEnd:{3}, allowWebPaper:{4}, creditType:{5}",
                        Subscriber.Cusno, UrlSubsno, DateStart, DateEnd, allowWebPaper, creditType), ret);
                UserMessageControl.ShowMessage(GetErrMess(), false, true);
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

            if (Subscriber.DeleteHolidayStop(UrlSubsno, SelectedSubsSleep.SleepStartDate, SelectedSubsSleep.SleepEndDate) == "OK")
                UserMessageControl.ShowMessage("Uppehållet har raderats.", false, false);
            else
                UserMessageControl.ShowMessage(GetErrMess(), false, false);
        }


        private void PopulateForm(SubsSleepsCirixMap sleepMap)
        {
            Date1.Text = sleepMap.SleepStartDate.ToString("yyyy-MM-dd");

            if (sleepMap.SleepEndDate != CirixDateNotSet)
                Date2.Text = sleepMap.SleepEndDate.ToString("yyyy-MM-dd");
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
                sb.Append("uppehåll för perioden " + dtStart.ToString("yyyy-MM-dd") + " - " + dtEnd.ToString("yyyy-MM-dd"));
            else
                sb.Append("tillsvidareuppehåll från " + dtStart.ToString("yyyy-MM-dd"));

            if (SubIsAutoGiro || CheckBoxAllowWebPaper.Checked)
                sb.Append("<br><br>Du har digital tillgång till tidningen under din uppehållsperiod.");
                //sb.Append("<br><br>Du har valt att ta del av tidningen digitalt under din uppehållsperiod.");

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

        private void ClearForm()
        {
            Date1.Text = "";
            Date2.Text = "";
        }

        private void HandleVisibility(bool showSubsSleepsAndForm)
        {
            //MySettingsMenu1.Visible = showMenu;
            PlaceHolderFutureSubsSleepsAndForm.Visible = showSubsSleepsAndForm;
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