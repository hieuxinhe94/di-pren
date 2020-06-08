using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.CardPayment.Nets;

using EPiServer;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.CardPayment;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Units.Placeable.PayInvoiceFlow
{
    public partial class PayInvoice : UserControlBase
    {
        #region Properties
        protected SubscriptionUser2 Subscriber { get; set; }

        public string Heading 
        {
            get
            {
                return (string)CurrentPage["Heading"] ?? CurrentPage.PageName;
            }
        }
        #endregion


        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        
        protected void GetInvoiceButton_Click(object sender, EventArgs e)
        {
            GetOpenInvoices();
        }

        protected void PayInvoiceLinkButton_Click(object sender, EventArgs e)
        {
            //Get commandargument form Button containing OPENAMOUNT, INVAMOUNT, VATAMOUNT, INVNO, CustomerNumber, Email
            string invoiceInfo = ((LinkButton)sender).CommandArgument;
            string[] invoiceArr = invoiceInfo.Split('|');

            double price = double.Parse(invoiceArr[0]); //OPENAMOUNT
            double priceExcl = double.Parse(invoiceArr[1]); //INVAMOUNT
            double vatAmount = price - priceExcl;
            string responseURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            //string cancelURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string goodsDescription = invoiceArr[3]; //INVNO
            long invoiceNumber = Convert.ToInt64(invoiceArr[3]); //INVNO
            string comment = "Betala faktura ej inloggad";
            string consumerName = "Cusno " + invoiceArr[4]; //CustomerNumber
            string emailAddress = invoiceArr[5]; //Email            

            var prep = new NetsCardPayPrepare(price, vatAmount, null, true, false, responseURL, goodsDescription, comment, consumerName, emailAddress, invoiceNumber, PaymentMethod.TypeOfPaymentMethod.CreditCard, null);
        }

        #endregion


        #region Methods
        /// <summary>
        /// Get open invoices for a given customer number
        /// </summary>
        private void GetOpenInvoices()
        {
            GetOpenInvoices(CustomerNumberInput.Text.Trim());
        }

        /// <summary>
        /// Get open invoices for a certain customer number. 
        /// If an invoice is payed but not processed yet by Di personnel, it is not shown in the list.
        /// </summary>
        /// <param name="customerNumber"></param>
        public void GetOpenInvoices(string customerNumber)
        {
            MessagePlaceHolder.Visible = false;
            SearchResultPlaceHolder.Visible = false;

            if (string.IsNullOrEmpty(customerNumber))
                return;

            try
            {
                CustomerNumberInput.Text = customerNumber;

                long customerNo;
                if (long.TryParse(customerNumber, out customerNo))
                {
                    Subscriber = new SubscriptionUser2(customerNo);
                    if (Subscriber.Cusno > 0)
                    {
                        //Get open invoices and bind to repeater
                        //Check paycustomernumber
                        DataSet dsInvoices = Subscriber.GetOpenInvoices();

                        if (DbHelpMethods.DataSetHasRows(dsInvoices))
                        {
                            //Find out if invoice is payed. If payed but not processed yet by Di personnel, remove from list. Payed = 1, Not payed = 0
                            for (int i = dsInvoices.Tables[0].Rows.Count-1; i >= 0; i--)
                            {
                                DataRow dr = dsInvoices.Tables[0].Rows[i];
                                string invoiceID = dr["INVNO"].ToString();
                                string result = SqlHelper.ExecuteScalar("DisePren", "CardPayGetCompleteInvoicePayment2", new SqlParameter("@invoiceNumber", invoiceID)).ToString();

                                if (result == "1")
                                    dsInvoices.Tables[0].Rows.RemoveAt(i);
                            }                           

                            int noOfHits = dsInvoices.Tables[0].Rows.Count;
                            if (noOfHits > 0)
                            {
                                InvoiceRepeater.DataSource = dsInvoices;
                                InvoiceRepeater.DataBind();

                                SearchResultLiteral.Text = string.Format(Translate("/payinvoice/showresult"), 1, noOfHits, noOfHits);
                                SearchResultPlaceHolder.Visible = true;
                            }
                            else
                            {
                                //All invoices were paid (but not processed in Cirix). 
                                MessagePlaceHolder.Visible = true;
                            }
                        }
                        else
                        {
                            MessagePlaceHolder.Visible = true;
                        }
                    }
                    else
                    {
                        MessagePlaceHolder.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetOpenInvoices() - failed", ex.ToString());
                ((DiTemplatePage)Page).ShowMessage("/common/errors/error", true, true);
            }
        }
        #endregion
    }
}