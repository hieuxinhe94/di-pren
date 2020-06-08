using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DIClassLib.DbHandlers.AurigaWrappers
{
    
    
    public interface IAuriga
    {
        Ping_Response Ping_();
        
        RecurPay_Response RecurPay_(long merchantId, long aurigaSubsId, string customer_refno, long amountInOre, long vatInOre, string mac);
    }



    //need to create dedicated return objects, since types differ:
    //AurigaProd.XxxResponse
    //AurigaTest.XxxResponse
    #region response objects

    public class Ping_Response
    {
        public string PingResp { get; set; }

        public Ping_Response(string pingResp)
        {
            PingResp = pingResp;
        }
    }

    public class RecurPay_Response
    {
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string Mac { get; set; }
        public string TransactionId { get; set; }
        public string CardType { get; set; }

        public RecurPay_Response(string status, string statusCode, string mac, string transactionId, string cardType)
        {
            Status = status;
            StatusCode = statusCode;
            Mac = mac;
            TransactionId = transactionId;
            CardType = cardType;
        }
    }

    #endregion

}
