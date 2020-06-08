using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DIClassLib.WineTipSms
{
    public class WineTipCustomer
    {
        public int Cusno { get; set; }
        public string PhoneMobile { get; set; }

        public WineTipCustomer(int cusno, string phoneMob)
        {
            Cusno = cusno;
            PhoneMobile = phoneMob;
        }

    }
}
