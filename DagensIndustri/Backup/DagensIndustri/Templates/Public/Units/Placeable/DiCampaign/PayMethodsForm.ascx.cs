using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Subscriptions;
using System.Web.UI.HtmlControls;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class PayMethodsForm : EPiServer.UserControlBase
    {

        public bool IsAutogiro 
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignIsAutoGiro"); }
        }

        public bool IsAutowithdrawal
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignIsAutowithdrawal"); }
        }

        public bool IsDigitalCampaign
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsDigitalCampaign"); }
        }

        public bool HideInvoice
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignHideInvoice"); }
        }

        public bool HideOtherPayer
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignHideOtherPayer"); }
        }

        public bool HideCard
        {
            get { return EPiFunctions.HasValue(CurrentPage, "CampaignHideCard"); }
        }

        public bool HideAddressFields
        {
            get
            {
                if (IsDigitalCampaign && !RbInvoice.Checked)
                    return true;

                return false;
            }
        }

        public bool DirectDebitOtherPayer
        {
            get
            {
                return DirectDebitOtherPayerInput.Checked;
            }
        }

        public PaymentMethod.TypeOfPaymentMethod SelectedPayMethod
        {
            get
            {
                if (IsAutogiro)
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;
                
                if (IsAutowithdrawal)
                    return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;

                if (RbInvoiceOtherPayer.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer;

                if (RbCard.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.CreditCard;

                return PaymentMethod.TypeOfPaymentMethod.Invoice;
            }
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind();

            if(!IsPostBack)
                PopulatePayMethods();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            PreSelectCheckBox();
        }

        protected void PopulatePayMethods()
        {
            if (!IsAutogiro && !IsAutowithdrawal && HideInvoice && HideOtherPayer && HideCard)
            {
                this.Visible = false;
                return;
            }

            PlaceHolderMultipePayMethods.Visible = false;

            if (IsAutogiro)
            {
                PlaceHolderDirectDebit.Visible = true;
                PlaceHolderDirDebOtherPayer.Visible = !HideOtherPayer;
                return;
            }

            if (IsAutowithdrawal)
            { 
                PlaceHolderAwd.Visible = true;
                return;
            }

            PlaceHolderDirectDebit.Visible = false;
            PlaceHolderMultipePayMethods.Visible = true;

            if (HideInvoice)
                PlaceHolderInvoice.Visible = false;

            if (HideOtherPayer)
                PlaceHolderInvoiceOtherPayer.Visible = false;

            if (HideCard)
            {
                PlaceHolderCard.Visible = false;
                CreditCardsPlaceHolder.Visible = false;
            }

            if (IsDigitalCampaign)
            {
                RbInvoice.Attributes.Add("onclick",           "javascript:jsShowAddressFields('true')");
                RbInvoiceOtherPayer.Attributes.Add("onclick", "javascript:jsShowAddressFields('false')");
                RbCard.Attributes.Add("onclick",              "javascript:jsShowAddressFields('false')");
            }

            PreSelectCheckBox();
        }

        private void PreSelectCheckBox()
        {
            if (PlaceHolderCard.Visible)
            {
                RbCard.Checked = true;
                return;
            }
            
            if (PlaceHolderInvoice.Visible)
            {
                RbInvoice.Checked = true;
                return;
            }

            if (PlaceHolderInvoiceOtherPayer.Visible)
            {
                RbInvoiceOtherPayer.Checked = true;
                return;
            }
        }


        //if (IsDigitalCampaign)
        //    TryHideAddressFields();

        //private void TryHideAddressFields()
        //{
            //if (!string.IsNullOrEmpty(AddressFieldsName1) && !string.IsNullOrEmpty(AddressFieldsName2))
            //{
            //    Control c1 = this.Parent.FindControl(AddressFieldsName1);
            //    if (c1 != null)
            //        c1.Visible = false;

            //    Control c2 = this.Parent.FindControl(AddressFieldsName2);
            //    if (c2 != null)
            //        c2.Visible = false;
            //}
        //}


    }
}