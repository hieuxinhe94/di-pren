using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIClassLib.CardPayment.Autowithdrawal
{
    public class Invoice
    {
        public long Invno { get; set; }
        public string Refno { get; set; }
        public bool PayOk { get; set; }

        public Invoice(long invno, string refno, bool payOk)
        {
            Invno = invno;
            Refno = refno;
            PayOk = payOk;
        }
    }
}
