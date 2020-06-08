using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.CardPayment.Nets;

using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.CardPayment.Autowithdrawal;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;
using DIClassLib.CardPayment;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class AutowithdrawalPayPage : DiTemplatePage
    {

        public Awd _Awd
        {
            get
            {
                if (ViewState["awd"] != null)
                    return (Awd)ViewState["awd"];
                
                return null;
            }
            set
            {
                ViewState["awd"] = value;
            }
        }

        public Guid PageGuid 
        {
            get
            {
                if (Request.QueryString["code"] != null)
                {
                    try { return new Guid(Request.QueryString["code"]); }
                    catch { }
                }

                return Guid.Empty;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserMessageControl1.Visible = false;

            #region return from nets
            if (Request.QueryString["responseCode"] != null)
            {
                NetsCardPayReturn ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                _Awd = (Awd)ret.NetsPreparePersisted.PersistedObj;

                MsSqlHandler.InsertAwdPayPagePayment(_Awd.PageGuid, ret.NetsPreparePersisted.CustomerRefNo);
                
                string email = TryGetCustEmail();
                bool payOk = ret.HandleNetsReturn("", email);
                if (payOk)
                {
                    //stop subs for old aurigaSubsId
                    MsSqlHandler.SetAwdIncludeInBatchFlag(_Awd.AurigaSubsId, false);

                    //set new AurigaSubsId
                    _Awd.AurigaSubsId = ret.QueryPayObj.PanHash;

                    //add new AWD line
                    MsSqlHandler.InsertAwdNets(ret, _Awd.Cusno, _Awd.Subsno, _Awd.Campno, _Awd.Sub.SubsEndDate);

                    var awdHandl = new AutowithdrawalHandler();
                    if (awdHandl.TryCreateRenewalInCirix(_Awd))
                        awdHandl.HandleInvoiceInCirix(_Awd, false);

                    MsSqlHandler.AppendToPayTransComment(ret.NetsPreparePersisted.CustomerRefNo, "cusno: " + _Awd.Cusno);

                    Response.Redirect(CurrentPage.LinkURL + "&pay=1");
                }
                else
                {
                    UserMessageControl1.ShowMessage("Ett tekniskt fel uppstod. Var god kontakta kundtjäst på tel 08-573 651 00.", false, true);
                    UserMessageControl1.Visible = true;
                    PlaceHolderSub.Visible = false;
                }

                return;
            }
            #endregion

            #region thank you (url has been cleaned)
            if (Request.QueryString["pay"] != null && Request.QueryString["pay"] == "1")
            {
                UserMessageControl1.ShowMessage("Din prenumeration är nu betald. Nästa betalning sker via autodragning på ditt kontokort.", false, false);
                UserMessageControl1.Visible = true;
                PlaceHolderSub.Visible = false;
                return;
            }
            #endregion

            if (!IsPostBack)
            {
                if (!TryPopulateAwd())
                {
                    UserMessageControl1.ShowMessage("Din prenumeration kunde inte hittas. Var god kontakta kundtjäst på tel 08-573 651 00.", false, true);
                    UserMessageControl1.Visible = true;
                    PlaceHolderSub.Visible = false;
                    return;
                }
            }
        }

        private string TryGetCustEmail()
        {
            DataSet ds = CirixDbHandler.GetCustomer(_Awd.Cusno);
            if (DbHelpMethods.DataSetHasRows(ds))
                return ds.Tables[0].Rows[0]["EMAILADDRESS"].ToString();

            return string.Empty;
        }

        private bool TryPopulateAwd()
        {
            if (PageGuid == Guid.Empty)
                return false;

            DataSet ds = MsSqlHandler.GetAwdByPageGuid(PageGuid);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string aurigaSubsId = dr["aurigaSubsId"].ToString();
                int cusno = int.Parse(dr["cusno"].ToString());
                int subsno = int.Parse(dr["subsno"].ToString());
                int campno = int.Parse(dr["campno"].ToString());
                Guid pageGuid = new Guid(dr["pageGuid"].ToString());
                DateTime dateSubsEnd = DateTime.Parse(dr["dateSubsEnd"].ToString());

                Subscription sub = new Subscription(string.Empty, campno, PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal, DateTime.Now, false);
                List<Subscription> subs = new List<Subscription>();
                subs.Add(sub);

                _Awd = new Awd(aurigaSubsId, cusno, subsno, campno, pageGuid, dateSubsEnd.Date, subs);

                LiteralProduct.Text = Settings.GetName_Product(sub.PaperCode, sub.ProductNo);
                LiteralCusno.Text = cusno.ToString();
                LiteralSubsno.Text = subsno.ToString();
                LiteralSubsEndDate.Text = dateSubsEnd.ToShortDateString();
                LiteralPrice.Text = _Awd.PriceIncVat.ToString();

                PlaceHolderSub.Visible = true;
                return true;
            }

            return false;
        }


        protected void ButtonBuy_Click(object sender, EventArgs e)
        {
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string cusnoSubsno = "cusno:" + _Awd.Cusno.ToString() + ", subsno:" + _Awd.Subsno.ToString();
            var prep = new NetsCardPayPrepare((double)_Awd.PriceIncVat, _Awd.VatAmount, null, true, false, url, "Avd: Upplaga", "Kort-autodragningsbet. via manuell bet.sida.", cusnoSubsno, "", null, PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal, _Awd);
        }
    }
}