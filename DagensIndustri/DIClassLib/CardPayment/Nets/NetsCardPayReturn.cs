using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Xml.Linq;

using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;


namespace DIClassLib.CardPayment.Nets
{

    //NetsCardReturn
    public class NetsCardPayReturn
    {
        //returns from NETS
        //http://localhost:53321/Rest.aspx?transactionId=586116eb518c4510ba6f602f902ffb19&responseCode=OK
        //http://localhost:53321/Rest.aspx?transactionId=a98146d0ec344b119387999bfc0e9138&responseCode=Cancel

        public string UrlTransactionId { get { return MiscFunctions.REC(HttpContext.Current.Request.QueryString["transactionId"]); } }
        public string UrlResponseCode { get { return MiscFunctions.REC(HttpContext.Current.Request.QueryString["responseCode"]); } }
        public string CardPayFailPagePath { get; set; }

        private NetsCardPayPrepare _netsPrepPersisted = null;
        /// <summary>
        /// Before going to nets the NetsPrepare object is saved to session and file.
        /// After return from nets this prop gets the NetsPrepare object back.
        /// </summary>
        public NetsCardPayPrepare NetsPreparePersisted
        {
            get
            {
                if (_netsPrepPersisted != null)
                    return _netsPrepPersisted;

                _netsPrepPersisted = ReadPersisted(UrlTransactionId);

                return _netsPrepPersisted;
            }
        }

        public QueryPayment QueryPayObj = null;
        private ProcessSale processSaleObj = null;


        public NetsCardPayReturn() { }

        public NetsCardPayReturn(string cardPayFailPagePath)
        {
            CardPayFailPagePath = cardPayFailPagePath;
        }


        public static NetsCardPayPrepare ReadPersisted(string transactionId)
        {
            if (!string.IsNullOrEmpty(transactionId))
            {
                try
                {
                    if (HttpContext.Current.Session["ap" + transactionId] != null)
                    {
                        NetsCardPayPrepare ap = (NetsCardPayPrepare)HttpContext.Current.Session["ap" + transactionId];
                        return ap;
                    }
                }
                catch (Exception ex)
                {
                    new Logger("NetsCardPayPrepare / ReadPersisted() - get obj from session failed for TransactionId:" + transactionId, ex.ToString());
                }

                //session is dead - read obj from file
                Serializer sz = new Serializer();
                return (NetsCardPayPrepare)sz.GetObjectFromFile(transactionId);
            }

            return null;
        }

        /// <summary>
        /// Payment ok: returns true and redirs to successUrl. 
        /// Payment not ok: returns false and redirs to cardPayFail page.
        /// </summary>
        public bool HandleNetsReturn(string successUrl, string mailAddressReceipt)
        {
            try
            {
                QueryPayObj = new QueryPayment(UrlTransactionId);

                bool registerOk = (UrlResponseCode == "OK" && QueryPayObj.PaymentOk);
                if (!registerOk)
                {
                    HandlePayFailed(QueryPayObj.ErrorResponseCode);
                    return false;
                }

                if (Settings.Nets_ProcessSale)
                {
                    processSaleObj = new ProcessSale(UrlTransactionId);
                    if (!processSaleObj.PaymentOk)
                    {
                        HandlePayFailed(processSaleObj.ResponseCode);
                        return false;
                    }
                }

                //pay success
                TrySaveDataAfterNetsPage("A", "0");
                TrySendReceiptMail(mailAddressReceipt);

                if (!string.IsNullOrEmpty(successUrl))
                    HttpContext.Current.Response.Redirect(successUrl, false);

                return true;
            }
            catch (Exception ex)
            {
                new Logger("HandleNetsReturn() - failed", ex.ToString());
            }

            return false;
        }

        private void HandlePayFailed(string errorResponseCode)
        {
            if (errorResponseCode != Settings.Nets_Err_TransNotFoundInNets)
                TrySaveDataAfterNetsPage("E", errorResponseCode);

            SendPayFailedStaffMail();

            if (!string.IsNullOrEmpty(CardPayFailPagePath))
            {
                string failUrl = CardPayFailPagePath + "?" + HttpContext.Current.Request.QueryString.ToString();
                HttpContext.Current.Response.Redirect(failUrl, false);
            }
        }

        private void SendPayFailedStaffMail()
        {
            string header = "Misslyckad kortbetalning";
            string body = this.ToString();

            //if (SendStaffMailOnPayFail)
            MiscFunctions.SendMail("no-reply@di.se", MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe"), header, body, true);

            new Logger(header + "<br><br>" + body);
        }

        public void TrySendReceiptMail(string mailAddressReceipt)
        {
            if (!MiscFunctions.IsValidEmail(mailAddressReceipt))
                return;

            try
            {
                NetsCardPayPrepare ap = NetsPreparePersisted;
                double amt = ap.AmountOre / 100;
                double vat = ap.VatAmountOre / 100;

                string from = MiscFunctions.GetAppsettingsValue("mailPrenDiSe");
                MailMessage message = new MailMessage(from, mailAddressReceipt);

                message.Subject = "Kvitto Dagens industri";
                message.Body = MiscFunctions.GetReceiptText(amt, vat, NetsPreparePersisted.CustomerRefNo.ToString());
                message.IsBodyHtml = true;

                MiscFunctions.SendMail(message);
            }
            catch (Exception ex)
            {
                new Logger("TrySendReceiptMail() - failed", ex.ToString());
            }
        }

        /// <summary>
        /// Update info in PayTrans in DisePren
        /// </summary>
        private void TrySaveDataAfterNetsPage(string status, string statusCode)
        {
            try
            {
                MsSqlHandler.SaveDataAfterCardPaymentNets(NetsPreparePersisted.CustomerRefNo, status, statusCode, QueryPayObj.PanHash, QueryPayObj.Issuer);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Betalningsstatusen uppdaterades inte vid följande kortköp<br>");
                sb.Append(this.ToString());

                MiscFunctions.SendMail(ConfigurationManager.AppSettings["mailPrenDiSe"],
                                       ConfigurationManager.AppSettings["mailPrenDiSe"],
                                       "Betalningsstatus ej uppdaterad vid kortköp",
                                       sb.ToString(),
                                       true);

                new Logger("TrySaveDataAfterNetsPage() failed", sb.ToString() + "<hr>" + ex.ToString());
            }
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            if (NetsPreparePersisted != null)
            {
                try
                {
                    sb.Append(NetsPreparePersisted.ToString());
                }
                catch (Exception ex)
                {
                    new Logger("NetsCardPayPreparePersisted.ToString() failed for TransactionId:" + UrlTransactionId, ex.ToString());
                }
            }

            sb.Append("<b>Betalningsfakta efter köp</b><br>");
            sb.Append("TransactionID: " + UrlTransactionId + "<br>");
            sb.Append("ResponseCode: " + UrlResponseCode + "<br>");

            if (QueryPayObj != null)
                sb.Append(QueryPayObj.ToString());

            if (processSaleObj != null)
                sb.Append(processSaleObj.ToString());

            sb.Append("<hr>");

            return sb.ToString();
        }

    }
}
