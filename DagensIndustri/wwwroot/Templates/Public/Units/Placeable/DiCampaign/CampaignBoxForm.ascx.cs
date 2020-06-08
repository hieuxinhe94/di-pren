using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DIClassLib.Subscriptions;
using DIClassLib.DbHelpers;
using System.Data;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes.BaseClasses;
using System.Net.Mail;
using System.Text;
using DIClassLib.CardPayment;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignBoxForm : EPiServer.UserControlBase
    {
        public Pages.DiCampaign.Campaign CampPage
        {
            get { return (Pages.DiCampaign.Campaign)this.Page; }
        }

        public PaymentMethod.TypeOfPaymentMethod SelectedPayMethod
        {
            get 
            {
                if (!PayMethodsForm1.Visible)
                    return PaymentMethod.TypeOfPaymentMethod.Invoice;

                return PayMethodsForm1.SelectedPayMethod; 
            }
        }

        public bool IsStudentCampaign
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsStudentCampaign"); }
        }

        //public bool IsDigitalCampaign
        //{
        //    get { return EPiFunctions.HasValue(CurrentPage, "IsDigitalCampaign"); }
        //}


        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (IsDigitalCampaign)
            //    BoxFormButton.Attributes.Add("onclick", "return jsCheckPasswdsBox();");

            if (CurrentPage["CampaignIsDiGold"] != null)
            {
                if (CurrentPage["LandingPage"] != null)
                {
                    DiGoldLandingPagePlaceHolder.Visible = true;
                    DiGoldPlaceHolder.Visible = false;
                    DiGoldInfo.Text = CampPage.GetAutomaticDiGuldMembershipInformation();
                }
                else
                {
                    DiGoldPlaceHolder.Visible = true;
                    DiGoldLandingPagePlaceHolder.Visible = false;

                }

                SocialSecurityPlaceHolder.Visible = true;
            }
            if (IsStudentCampaign)
                BirthNoPlaceHolder.Visible = true;
        }

        /// <summary>
        /// Create a person object from the input data
        /// </summary>
        /// <returns></returns>
        public Person GetPerson()
        {
            //string bonDigpasswd = (IsDigitalCampaign) ? PasswdInput1.Text : string.Empty;

            return new Person(true,
                              false,
                              FirstNameInput.Text,
                              LastNameInput.Text,
                              string.Empty,
                              CompanyInput.Text,
                              BoxInput.Text,
                              NumberInput.Text,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              ZipInput.Text,
                              StateInput.Text,
                              TelephoneInput.Text,
                              EmailInput.Text,
                              SocialSecurityNoInput.Text,
                              string.Empty,
                              string.Empty,
                              string.Empty);
                                                //130410 - last arg bonDigpasswd removed
        }

        internal void Populate(string firstName, string lastName, string email, string box, string boxNum, string zip, string city, string phoneMob, string company)
        {
            FirstNameInput.Text = firstName;
            LastNameInput.Text = lastName;
            EmailInput.Text = email;
            BoxInput.Text = box;
            NumberInput.Text = boxNum;
            ZipInput.Text = zip;
            StateInput.Text = city;
            TelephoneInput.Text = phoneMob;
            CompanyInput.Text = company;
        }

        internal void Clear()
        {
            FirstNameInput.Text = "";
            LastNameInput.Text = "";
            EmailInput.Text = "";
            BoxInput.Text = "";
            NumberInput.Text = "";
            ZipInput.Text = "";
            StateInput.Text = "";
            TelephoneInput.Text = "";
            CompanyInput.Text = "";
        }


        protected void BoxFormButton_Click(object sender, EventArgs e)
        {
            if (IsStudentCampaign && !CampPage.VerifyFullTimeStudent(BirthNoInput.Text.Trim()))
            {
                BirthNoErrMess.Visible = true;
                return;
            }

            if (CampPage.HasDiGuldLandingPage && !CampPage.ValidateSocialSecurityNumber(SocialSecurityNoInput.Text.Trim()))
            {
                SocialSecurityNoInputErrMess.Visible = true;
                return;
            }


            CampPage.Sub.SetMembersByPayMethod(SelectedPayMethod);
            CampPage.Sub.TargetGroup = CampPage.GetTargetGroup();
            CampPage.Sub.Subscriber = GetPerson();
            CampPage.Sub.SubscriptionPayer = null;

            if (CurrentPage["LandingPage"] != null && CurrentPage["CampaignIsDiGold"] != null)
            {
                CampPage.Sub.Subscriber.IsGoldMember = true;
            }


            //save to cirix (send a confirmation mail)
            if (SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.Invoice || SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit)
            {
                String err = CampPage.SaveSubscription();
                if (String.IsNullOrEmpty(err))
                    CampPage.ShowThankYou();
                else 
                    CampPage.ShowMessage(err, false, true);

            }  //redirect to the next step 
            else if (SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer)  
            {
                CampPage.DisplayPlaceHolders(false, true);
            }  //redirect to Nets
            else if (SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCard || SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal)
            {
                var err = string.Empty;
                if (SubscriptionController.DenyShortSub(CampPage.Sub, out err))
                {
                    CampPage.ShowMessage(err, false, true);
                    return;
                }

                CampPage.HandleCreditCardPayment();
                return;
            }
            
        }
        
    }
}