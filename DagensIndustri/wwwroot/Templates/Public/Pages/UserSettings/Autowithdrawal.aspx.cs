using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using System.Text;
using DIClassLib.DbHelpers;
using System.Web.UI.HtmlControls;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.DbHandlers;
using System.Data;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class Autowithdrawal : DiTemplatePage, IUserSettingsPage
    {

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


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
            {
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
                    ShowMessage("Prenumerationsnummer " + Subsno.ToString() + " tillhör inte inloggad person.", false, true);
                    return;
                }

                if (!MsSqlHandler.IsActiveAwdSubs(Subsno))
                {
                    HandleVisibility(true, false);
                    ShowMessage("Prenumerationsnummer " + Subsno.ToString() + " betalas inte via autodragning på kort.", false, true);
                    return;
                }

                TryPopulatePayHistory();
            }
        }

        //Amount,VAT,Purchase_date,Status,Status_code
        private void TryPopulatePayHistory()
        {
            DataSet ds = MsSqlHandler.GetAwdPayHistoryBySubsno(Subsno);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                List<AwdPayment> payments = new List<AwdPayment>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    payments.Add(new AwdPayment(int.Parse(dr["Amount"].ToString()), int.Parse(dr["VAT"].ToString()), DateTime.Parse(dr["Purchase_date"].ToString()), dr["Status"].ToString(), dr["Status_code"].ToString()));
                }
                
                PaymentRepeater.DataSource = payments;
                PaymentRepeater.DataBind();
                PaymentRepeater.Visible = true;
            }
            else
                PaymentRepeater.Visible = false;
        }

        private void HandleVisibility(bool showMenu, bool showButton)
        {
            MySettingsMenu1.Visible = showMenu;
            ButtonQuitAwd.Visible = showButton;
        }


        protected void ButtonQuitAwd_Click(object sender, EventArgs e)
        {
            MsSqlHandler.CancelAwdSubscription(Subsno);
            HandleVisibility(true, false);
            ShowMessage("Autodragning på kort har avslutats för prenumerationsnummer " + Subsno.ToString(), false, false);
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }



    internal class AwdPayment
    {
        public double Amount { get; set; }
        public double Vat { get; set; }
        public DateTime Purchase_date { get; set; }
        private string Status { get; set; }
        private string Status_code { get; set; }
        
        public string PaymentStatus { get { return MiscFunctions.GetNetsCardPayStatus(Status, Status_code); } }

        public AwdPayment(int amountInOren, int vatInOren, DateTime purchaseDate, string status, string statusCode)
        {
            if (amountInOren > 0)
                Amount = amountInOren / 100;
            
            if (vatInOren > 0)
                Vat = vatInOren / 100;

            Purchase_date = purchaseDate;
            Status = status;
            Status_code = statusCode;
        }
    }

}