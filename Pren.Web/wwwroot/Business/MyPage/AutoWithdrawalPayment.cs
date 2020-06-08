using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DIClassLib.Misc;

namespace Pren.Web.Business.MyPage
{
    public class AutoWithdrawalPayment
    {
        public double Amount { get; set; }
        public double Vat { get; set; }
        public DateTime PurchaseDate { get; set; }
        private string Status { get; set; }
        private string StatusCode { get; set; }
        public string PaymentStatus { get { return MiscFunctions.GetNetsCardPayStatus(Status, StatusCode); } }

        public AutoWithdrawalPayment(int amountInOren, int vatInOren, DateTime purchaseDate, string status, string statusCode)
        {
            if (amountInOren > 0)
                Amount = amountInOren / 100;

            if (vatInOren > 0)
                Vat = vatInOren / 100;

            PurchaseDate = purchaseDate;
            Status = status;
            StatusCode = statusCode;
        }
        
    }
}