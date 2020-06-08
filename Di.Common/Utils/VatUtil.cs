using System;

namespace Di.Common.Utils
{

    public class VatUtil
    {
            
        public static decimal GetAmountIncludingVat(decimal amountExcludningVat, decimal vatPercent)
        {
            if (amountExcludningVat < 0 || vatPercent < 0)
            {
                return 0;
            }

            var amountIncludingVat = amountExcludningVat * ((vatPercent / 100) + 1);

            return Math.Round(amountIncludingVat, MidpointRounding.AwayFromZero);
        }

        public static decimal GetAmountExcludingVat(decimal amountIncludningVat, decimal vatPercent)
        {
            var amountExcludingVat = amountIncludningVat / ((vatPercent / 100) + 1);

            return Math.Round(amountExcludingVat, MidpointRounding.AwayFromZero);
        }

    }

}
