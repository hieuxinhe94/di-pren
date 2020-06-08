using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.SignUp;

namespace DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow
{
    public partial class PayWithInvoice : EPiServer.UserControlBase
    {
        #region Properties

        public string InvoiceAddress
        {
            get
            {
                return InvoiceAddressInput.Text.Trim();
            }
            set
            {
                InvoiceAddressInput.Text = value;
            }
        }

        public string InvoiceReference
        {
            get
            {
                return InvoiceReferenceInput.Text.Trim();
            }
            set
            {
                InvoiceReferenceInput.Text = value;
            }
        }

        public string OrgNumber
        {
            get
            {
                return OrgNumberInput.Text.Trim();
            }
            set
            {
                OrgNumberInput.Text = value;
            }
        }

        public string Zip
        {
            get
            {
                return ZipCodeInput.Text.Trim();
            }
            set
            {
                ZipCodeInput.Text = value;
            }
        }

        public string City
        {
            get
            {
                return StateInput.Text.Trim();
            }
            set
            {
                StateInput.Text = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //public SignUpCompany GetCompany()
        //{
        //    return new SignUpCompany(this);
        //}
    }
}