using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;

namespace DIClassLib.CardPayment.Nets
{
    [Serializable]
    public class NetsCardPayPrepare
    {
        #region Properties
        public string TransactionId { get; set; }
        public int CustomerRefNo { get; set; }        //OrderNumber
        public double AmountOre { get; set; }
        public double VatAmountOre { get; set; }    //VatAmountOre  TerminalVatOre
        public string RedirectUrl { get; set; }
        public string GoodsDescription { get; set; }
        public string Comment { get; set; }
        public string ConsumerName { get; set; }
        public string EmailAddress { get; set; }
        public string PurchaseDate { get; set; }
        public long? InvoiceNumber { get; set; }
        public object PersistedObj { get; set; }
        public PaymentMethod.TypeOfPaymentMethod PayMethod { get; set; }
        #endregion


        //public NetsCardPayPrepare(){}

        public NetsCardPayPrepare(double price, double? vatAmount, double? vatPct,
                                  bool isPriceIncVat, bool displayVat,
                                  string redirectUrl, string goodsDescription,
                                  string comment, string consumerName,
                                  string emailAddress, long? invoiceNumber, PaymentMethod.TypeOfPaymentMethod payMethod,
                                  object persistedObj)
        {
            try
            {
                PersistedObj = persistedObj;

                PriceCalculator pc = new PriceCalculator(price, vatAmount, vatPct, isPriceIncVat);
                AmountOre = (double)pc.PriceIncVat * 100;
                VatAmountOre = (double)pc.VatAmount * 100;

                RedirectUrl = redirectUrl;
                GoodsDescription = goodsDescription;
                Comment = comment;
                ConsumerName = consumerName;
                EmailAddress = emailAddress;
                PurchaseDate = DateTime.Now.ToString("yyyyMMddHHmm");
                InvoiceNumber = invoiceNumber;
                PayMethod = payMethod;
                CustomerRefNo = MsSqlHandler.GetPayTransCusRefNo();
                TransactionId = RegisterPayment(displayVat);

                SaveDataBeforeNetsPage();
                PersistThis();

                HttpContext.Current.Response.Redirect(GetUrlToNetsTerminal(), false);

            }
            catch (Exception ex)
            {
                new Logger("NetsCardPayPrepare() - failed", ex.ToString());
                throw ex;
            }
        }


        private void SaveDataBeforeNetsPage()
        {
            try
            {
                MsSqlHandler.SaveDataBeforeCardPaymentNets(CustomerRefNo, Settings.Nets_MerchantId, Settings.Nets_CurrencyCode, Convert.ToInt32(AmountOre),
                                                           Convert.ToInt32(VatAmountOre), PayMethod.ToString(), GoodsDescription,
                                                           Comment, ConsumerName, EmailAddress, TransactionId, InvoiceNumber);
            }
            catch (Exception ex)
            {
                new Logger("SaveDataBeforeNetsPage() failed for OrderNumber: " + CustomerRefNo, ex.ToString());
                throw ex;
            }
        }


        /// <summary>
        /// Saves 'this' to session and file
        /// </summary>
        private void PersistThis()
        {
            if (CustomerRefNo > 0)
            {
                try
                {
                    HttpContext.Current.Session["ap" + TransactionId] = this;
                }
                catch (Exception ex)
                {
                    new Logger("PersistThis to session failed for TransactionId:" + TransactionId, ex.ToString());
                }

                var sz = new Misc.Serializer();
                sz.SaveObjectToFile(TransactionId, this);
            }
        }


        public string GetUrlToNetsTerminal()
        {
            if (string.IsNullOrEmpty(TransactionId))
                return string.Empty;

            var url = new StringBuilder();
            url.Append(Settings.Nets_UrlTerminal);
            url.Append("?MerchantID=" + Settings.Nets_MerchantId);
            url.Append("&TransactionID=" + TransactionId);

            return url.ToString();
        }

        /// <summary>
        /// Makes a request to Nets register URL. A Nets transactionId is returned.
        /// </summary>
        private string RegisterPayment(bool displayVat)
        {
            var url = new StringBuilder();
            url.Append(Settings.Nets_UrlRegister);
            url.Append("?language=" + Settings.Nets_Language);
            url.Append("&currencyCode=" + Settings.Nets_CurrencyCode);
            url.Append("&merchantId=" + Settings.Nets_MerchantId);
            url.Append("&token=" + Settings.Nets_Token);
            url.Append("&orderNumber=" + CustomerRefNo);
            url.Append("&amount=" + AmountOre);
            
            if (displayVat)
                url.Append("&terminalVat=" + VatAmountOre);

            if (PayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
            {
                url.Append("&recurringType=R");
                url.Append("&recurringFrequency=0");
                url.Append("&recurringExpiryDate=" + DateTime.Now.AddYears(10).ToString("yyyyMMdd"));
            }

            if (!string.IsNullOrEmpty(RedirectUrl))
                url.Append("&redirectUrl=" + HttpUtility.UrlEncode(RedirectUrl));


            try
            {
                XDocument doc = XDocument.Load(url.ToString());
                var desc = doc.Descendants("TransactionId");
                if (desc.Any())
                    return desc.FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                new Logger("RegisterPayment() - failed for url: " + url.ToString(), ex.ToString());
            }

            return string.Empty;
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Betalningsfakta innan köp</b><br>");
            sb.Append("OrderNumber: " + CustomerRefNo + "<br>");
            sb.Append("TransactionId: " + TransactionId + "<br>");
            sb.Append("AmountOre: " + AmountOre + "<br>");
            sb.Append("TerminalVatOre: " + VatAmountOre + "<br>");
            sb.Append("GoodsDescription: " + GoodsDescription + "<br>");
            sb.Append("Comment: " + Comment + "<br>");
            sb.Append("ConsumerName: " + ConsumerName + "<br>");
            sb.Append("EmailAddress: " + EmailAddress + "<br>");
            sb.Append("PurchaseDate: " + PurchaseDate + "<br>");
            sb.Append("InvoiceNumber: " + InvoiceNumber + "<br>");
            sb.Append("MerchantId: " + Settings.Nets_MerchantId + "<br>");
            sb.Append("PayMethod: " + PayMethod.ToString() + "<hr>");


            if (PersistedObj != null)
            {
                try
                {
                    sb.Append(PersistedObj.ToString());
                }
                catch (Exception ex)
                {
                    new Logger("PersistedObj.ToString() failed for TransactionId:" + TransactionId, ex.ToString());
                }
            }

            return sb.ToString();
        }
    
    }
}
