using System;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.AddCustAndSub;
using DIClassLib.Misc;

namespace DagensIndustri
{
    public partial class SubTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool displayTestGui = false;
                bool.TryParse(MiscFunctions.GetAppsettingsValue("DisplayTestGui").ToLower(), out displayTestGui);
                PlaceholderTestGui.Visible = displayTestGui;                
                if (!displayTestGui)
                    return;

                TextBoxSCampId.Text = "P1301001E";
                TextBoxSWantedSStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            var subscriber = new Subscriber(TextBoxSubFirstName.Text, TextBoxSubLastName.Text, TextBoxSubMobilePhone.Text, TextBoxSubEmail.Text, TextBoxSubCompany.Text,
                TextBoxSubCareOf.Text, TextBoxSubStreetName.Text, TextBoxSubHouseNo.Text, TextBoxSubStairCase.Text, TextBoxSubStairs.Text, TextBoxSubApartmentNo.Text, TextBoxSubZipCode.Text, TextBoxSubCity.Text);
            Payer payer = null;
            if (AnyPayerFieldIsSet())
            {
                payer = new Payer(TextBoxPayPhoneDayTime.Text, TextBoxPayCompany.Text, TextBoxPayCareOf.Text, TextBoxPayAttention.Text, TextBoxPayCompanyNo.Text,
                    TextBoxPayStreetName.Text, TextBoxPayHouseNo.Text, TextBoxPayStairCase.Text, TextBoxPayStairs.Text, TextBoxPayApartmentNo.Text, TextBoxPayZipCode.Text, TextBoxPayCity.Text);
            }

            var hdlr = new AddCustAndSubHandler();
            var ret = hdlr.TryAddCustAndSub(TextBoxSCampId.Text, TextBoxSTargetGroup.Text, CheckBoxSSubscriberAddressIsRequired.Checked, GetPaymethod(), 
                                            DateTime.Parse(TextBoxSWantedSStartDate.Text), false, TextBoxSServicePlusToken.Text, TextBoxSServicePlusUserId.Text, subscriber, payer);
            
            LabelResult.Text = ret.ToString();        
        }

        private bool AnyPayerFieldIsSet()
        {
            string s = (TextBoxPayPhoneDayTime.Text + TextBoxPayCompany.Text + TextBoxPayCareOf.Text + TextBoxPayAttention.Text + TextBoxPayCompanyNo.Text +
                        TextBoxPayStreetName.Text + TextBoxPayHouseNo.Text + TextBoxPayZipCode.Text).Trim();

            return (s.Length > 0);
        }

        private PaymentMethod.TypeOfPaymentMethod GetPaymethod()
        {
            switch (DropDownListSPayMet.SelectedValue)
            {
                case "CreditCard":
                    return PaymentMethod.TypeOfPaymentMethod.CreditCard;
                case "CreditCardAutowithdrawal":
                    return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;
                case "DirectDebit":
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;
                case "DirectDebitOtherPayer":
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebitOtherPayer;
                case "Invoice":
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;
                case "InvoiceOtherPayer":
                    return PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer;
                default:
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;
            }
        }

    }
}