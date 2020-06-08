using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.CardPayment;
using DagensIndustri.Tools.Classes;

using DIClassLib.CardPayment.Nets;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class PayInvoiceFlow : DiTemplatePage
    {
        #region Events
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            PayInvoiceMultiView.ActiveViewIndex = 0;

            if (!IsPostBack)
            {
                //This happens only when the visitor have been to Nets, paymethod credit card
                if (Request.QueryString["responseCode"] != null)
                {
                    //string successUrl = string.Format("{0}&pv=t&Transaction_id={1}", CurrentPage.LinkURL, Request.QueryString["Transaction_id"]);
                    var ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                    bool payOk = ret.HandleNetsReturn("", "");
                    if (payOk)
                    {
                        //Show the invoice payment receipt
                        PayInvoiceMultiView.ActiveViewIndex = 1;
                        double amt = ret.NetsPreparePersisted.AmountOre / 100;
                        double vat = ret.NetsPreparePersisted.VatAmountOre / 100;
                        InvoiceReceiptControl.ShowInvoiceReceipt(amt, vat, ret.UrlTransactionId);
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            BackLinkButton.Visible = PayInvoiceMultiView.ActiveViewIndex > 0;
        }

        protected void BackLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(CurrentPage.LinkURL);
        }
        #endregion

        #region Mehods
        /// <summary>
        /// Get heading of the page
        /// </summary>
        /// <returns></returns>
        protected string GetHeading()
        {
            string heading = string.Empty;

            if (PayInvoiceMultiView.GetActiveView() == null || PayInvoiceMultiView.GetActiveView().ID == PayInvoiceView.ID)
                heading = PayInvoiceControl.Heading;
            else if (PayInvoiceMultiView.GetActiveView().ID == ReceiptView.ID)
                heading = InvoiceReceiptControl.Heading;

            return heading;
        }

        /// <summary>
        /// Get open invoices for a customer number stored in the session and then clear the session
        /// </summary>
        private void ShowInvoices(int cusno)
        {
            PayInvoiceMultiView.ActiveViewIndex = 0;
            PayInvoiceControl.GetOpenInvoices(cusno.ToString());
        }
        #endregion
    }
}