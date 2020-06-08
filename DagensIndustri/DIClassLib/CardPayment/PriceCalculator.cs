using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHelpers;

namespace DIClassLib.CardPayment
{
    [Serializable]
    public class PriceCalculator
    {
        public double? PriceIncVat = null;
        public double? PriceExVat = null;
        public double? VatPct = null;
        public double? VatAmount = null;

        public PriceCalculator() { }

        
        public PriceCalculator(double? priceIncVat, double? priceExVat, double? vatPct, int numItems) 
        { 
            SetMembers(priceIncVat, priceExVat, vatPct, numItems);
        }

        public PriceCalculator(double price, double? vatAmount, double? vatPct, bool isPriceIncVat)
        {
            double? prIncVat = null;
            double? prExVat = null;

            if (isPriceIncVat)
                prIncVat = price;
            else
                prExVat = price;

            if (HasValue(vatAmount))
            {
                if (!HasValue(prIncVat))
                    prIncVat = prExVat + vatAmount;

                if (!HasValue(prExVat))
                    prExVat = prIncVat - vatAmount;
            }

            SetMembers(prIncVat, prExVat, vatPct, 1);
        }


        /// <summary>
        /// 2 of 3 inparams must have values > 0. 
        /// Send in null or 0 for unknown inparam.
        /// </summary>
        private void SetMembers(double? priceIncVat, double? priceExVat, double? vatPct, int numItems) 
        {
            PriceIncVat = priceIncVat;
            PriceExVat = priceExVat;
            VatPct = vatPct;

            if (!TwoOfThreeInParamsAreSet())
            {
                new Logger("SetMembers() - failed", "TwoOfThreeInParamsAreSet = false");
                return;
            }

            if (!HasValue(PriceIncVat))
                SetPriceIncVat();

            if (!HasValue(PriceExVat))
                SetPriceExVat();

            if (!HasValue(VatPct))
                SetVatPct();

            PriceIncVat = PriceIncVat * numItems;
            PriceExVat = PriceExVat * numItems;
            VatAmount = PriceIncVat - PriceExVat;
        }


        private void SetVatPct()
        {
            //eg: priceIncVat:100 priceExVat:80
            //((100/80)-1)*100=25%
            VatPct = ((PriceIncVat / PriceExVat) - 1) * 100;
            VatPct = SetTwoDecimals((double)VatPct);
        }

        private void SetPriceIncVat()
        {
            //eg: priceExVat:80 vatPct:25%
            //80*((25/100)+1)=100    
            PriceIncVat = PriceExVat * ((VatPct / 100) + 1);
            //PriceIncVat = SetTwoDecimals((double)PriceIncVat);
            PriceIncVat = Math.Round((double)PriceIncVat, MidpointRounding.AwayFromZero);   //price in even kr
        }

        private void SetPriceExVat()
        {
            //eg: priceIncVat:100 vatPct:25%
            //100/((25/100)+1)=80
            PriceExVat = PriceIncVat / ((VatPct / 100) + 1);
            //PriceExVat = SetTwoDecimals((double)PriceExVat);
            PriceExVat = Math.Round((double)PriceExVat, MidpointRounding.AwayFromZero);     //price in even kr
        }


        private double SetTwoDecimals(double num)
        {
            return (Math.Round(num * 100, MidpointRounding.AwayFromZero)) / 100;
        }

        private bool TwoOfThreeInParamsAreSet()
        {
            return ((HasValue(PriceIncVat) && HasValue(PriceExVat)) || (HasValue(PriceIncVat) && HasValue(VatPct)) || (HasValue(PriceExVat) && HasValue(VatPct)));
        }

        private bool HasValue(double? d)
        {
            return (d != null && d >= 0);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            if (PriceIncVat != null)
                sb.Append("PriceIncVat:" + PriceIncVat.ToString() + "<br>");

            if (PriceExVat != null)
                sb.Append("PriceExVat:" + PriceExVat.ToString() + "<br>");

            if (VatPct != null)
                sb.Append("VatPct:" + VatPct.ToString() + "<br>");

            if (VatAmount != null)
                sb.Append("VatAmount:" + VatAmount.ToString() + "<br>");

            return sb.ToString();
        }
    }
}
