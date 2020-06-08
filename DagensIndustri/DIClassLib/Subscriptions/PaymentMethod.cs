using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DIClassLib.Subscriptions
{
    [Serializable]
    public class PaymentMethod
    {
        #region 
        public enum TypeOfPaymentMethod
        {
            [DescriptionAttribute("Faktura")]
            Invoice = 0,
            [DescriptionAttribute("Annan fakturamottagare")]
            InvoiceOtherPayer,
            [DescriptionAttribute("Autogiro")]
            DirectDebit,
            [DescriptionAttribute("Kort")]
            CreditCard,
            [DescriptionAttribute("Kort, auotodragning")]
            CreditCardAutowithdrawal,
            [DescriptionAttribute("Autogiro med annan betalare")]
            DirectDebitOtherPayer
        }
        #endregion
    }
}
