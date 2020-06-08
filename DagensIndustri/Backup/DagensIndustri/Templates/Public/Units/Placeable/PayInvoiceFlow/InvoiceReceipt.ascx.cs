using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;
using EPiServer;

namespace DagensIndustri.Templates.Public.Units.Placeable.PayInvoiceFlow
{
    public partial class InvoiceReceipt : UserControlBase
    {
        #region Properties
        public string Heading
        {
            get
            {
                return Translate("/payinvoice/receipt");
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion

        #region Methods
        public void ShowInvoiceReceipt(double amount, double vat, string transactionId)
        {
            try
            {
                if (amount > 0 && vat >= 0)
                    ReceiptLiteral.Text = MiscFunctions.GetReceiptText(amount, vat, transactionId);
            }
            catch (Exception ex)
            {
                new Logger("OnLoad() - failed", ex.ToString());
            }
        }
        #endregion
    }
}