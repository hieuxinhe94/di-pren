using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers.AurigaWrappers;
using DIClassLib.Misc;
using DIClassLib.DbHelpers;


namespace DIClassLib.DbHandlers
{
    public class AurigaDbHandler
    {
        private IAuriga _ws
        {
            get
            {
                bool useTestWS = true;
                bool.TryParse(MiscFunctions.GetAppsettingsValue("UseAurigaTestWS"), out useTestWS);

                if (useTestWS)
                    return new AurigaTestWrapper();
                else
                    return new AurigaProdWrapper();
            }
        }

        public Ping_Response Ping()
        {
            try
            {
                return _ws.Ping_();
            }
            catch (Exception ex)
            {
                return new Ping_Response(ex.ToString());
            }
        }

        public RecurPay_Response RecurPay(long merchantId, string aurigaSubsId, string customer_refno, long amountInOre, long vatInOre, string mac)
        {
            try
            {
                return _ws.RecurPay_(merchantId, long.Parse(aurigaSubsId), customer_refno, amountInOre, vatInOre, mac);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("merchantId:" + merchantId);
                sb.Append(", aurigaSubsId:" + aurigaSubsId);
                sb.Append(", customer_refno:" + customer_refno);
                sb.Append(", amountInOre:" + amountInOre);
                sb.Append(", vatInOre:" + vatInOre);
                sb.Append(", mac:" + mac);
                new Logger("RecurPay() failed for params:" + sb.ToString(), ex.ToString());
            }

            return null;
        }

    }
}
