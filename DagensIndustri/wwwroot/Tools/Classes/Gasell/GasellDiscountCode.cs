using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DagensIndustri.Templates.Public.Units.Placeable.GasellFlow;

namespace DagensIndustri.Tools.Classes.Gasell
{
    [Serializable]
    public class GasellDiscountCode
    {
        #region Properties

        public string DiscountCode { get; set; }

        #endregion

        public GasellDiscountCode(PayWithDiscountCode discountCodeForm)
        {
            DiscountCode = discountCodeForm.DiscountCode;
        }
    }
}