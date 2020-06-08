using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow;

namespace DagensIndustri.Tools.Classes.SignUp
{
    [Serializable]
    public class SignUpDiscountCode
    {
        #region Properties

        public string DiscountCode { get; set; }

        #endregion

        public SignUpDiscountCode(PayWithDiscountCode discountCodeForm)
        {
            DiscountCode = discountCodeForm.DiscountCode;
        }
    }
}