using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.AurigaTest;


namespace DIClassLib.DbHandlers.AurigaWrappers
{
    class AurigaTestWrapper : AdminService, IAuriga
    {

        public Ping_Response Ping_()
        {
            AdminPingResponse resp = this.ping();
            return new Ping_Response(resp.DateTime.ToString());
        }

        public RecurPay_Response RecurPay_(long merchantId, long aurigaSubsId, string customer_refno, long amountInOre, long vatInOre, string mac)
        {
            AdminExtendedResponse resp = this.recurPay(merchantId, aurigaSubsId, customer_refno, amountInOre, vatInOre, mac);
            return new RecurPay_Response(resp.Status, resp.StatusCode, resp.Mac, resp.TransactionId, resp.CardType);
        }

    }
}
