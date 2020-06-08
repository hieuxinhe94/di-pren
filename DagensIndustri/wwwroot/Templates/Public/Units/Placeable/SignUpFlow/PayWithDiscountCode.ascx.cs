using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.SignUp;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow
{
    public partial class PayWithDiscountCode : EPiServer.UserControlBase
    {
        #region Properties
        public string DiscountCode
        {
            get
            {
                return DiscountCodeInput.Text.Trim();
            }
            set
            {
                DiscountCodeInput.Text = value;
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }



        public SignUpDiscountCode GetDiscountCode()
        {
            return new SignUpDiscountCode(this);
        }
    }
}