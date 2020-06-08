using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class SubscriptionPaymentMethod : UserControlBase
    {
        #region Properties
        public PaymentMethod.TypeOfPaymentMethod SelectedPaymentMethod
        {
            get
            {
                if (InvoiceRadioButton.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;
                else if (InvoiceAnotherAddresseeRadioButton.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer;
                else if (CardPaymentRadioButton.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.CreditCard;
                else
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;
            }
            set
            {
                InvoiceRadioButton.Checked = (value == PaymentMethod.TypeOfPaymentMethod.Invoice);
                InvoiceAnotherAddresseeRadioButton.Checked = (value == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer);
                CardPaymentRadioButton.Checked = (value == PaymentMethod.TypeOfPaymentMethod.CreditCard);
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();
        }
        #endregion

        #region Methods
        public void ShowDirectDebit(bool showDirectDebit)
        {
            //If subscription with Direct Debit was chosen, hide the different payment methods' sections
            InvoiceCreditCardPlaceHolder.Visible = !showDirectDebit;
            InvoiceRadioButton.Checked = !showDirectDebit;
            InvoiceAnotherAddresseeRadioButton.Checked = false;
            CardPaymentRadioButton.Checked = false;

            DirectDebitPlaceHolder.Visible = showDirectDebit;
        }
        #endregion
    }
}