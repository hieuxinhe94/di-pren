using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DIClassLib.CardPayment.Autowithdrawal;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;

namespace DIClassLib.CardPayment.Nets
{
    public class NetsRecurringPayment
    {
        
        public NetsRecurringPayment(){}


        /// <summary>
        /// Makes a request to Nets register URL. 
        /// A Nets transactionId is returned on success.
        /// </summary>
        public string RegisterRecurringPayment(int orderNumber, double amountOre, double terminalVatOre, string panHash)
        {
            var url = new StringBuilder();
            url.Append(Settings.Nets_UrlRegister);
            url.Append("?language=" + Settings.Nets_Language);
            url.Append("&currencyCode=" + Settings.Nets_CurrencyCode);
            url.Append("&merchantId=" + Settings.Nets_MerchantId);
            url.Append("&token=" + Settings.Nets_Token);
            url.Append("&orderNumber=" + orderNumber);
            url.Append("&amount=" + amountOre);
            url.Append("&terminalVat=" + terminalVatOre);
            url.Append("&panHash=" + HttpUtility.UrlEncode(panHash));
            url.Append("&recurringType=R");
            url.Append("&serviceType=C");

            string logStr = "RegisterRecurringPayment() - failed for panHash: " + panHash + ", url: " + url.ToString();
            
            try
            {
                XDocument doc = XDocument.Load(url.ToString());
                var desc = doc.Descendants("TransactionId");
                if (desc.Any())
                    return desc.FirstOrDefault().Value;
                
                //log failed recurring payment info
                new Logger(logStr + Environment.NewLine + doc.ToString());
            }
            catch (Exception ex)
            {
                new Logger(logStr, ex.ToString());
            }

            return string.Empty;
        }

    }
}
