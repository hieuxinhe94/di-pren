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
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Extras;
using DIClassLib.Membership;
using DIClassLib.CardPayment;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using System.Data;
using System.Text;
using System.Net.Mail;


namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignStreetForm : EPiServer.UserControlBase
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

        public bool IsDigitalCampaign
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsDigitalCampaign"); }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();

            //if (!IsPostBack)
            //{                
                //TryPopulateForm();
                //CampPage.DisplayPlaceHolders(true, false);
            //}

            //130410 - passwd fields removed - not adding cust to bondig
            //if (IsDigitalCampaign)
            //    StreetFormButton.Attributes.Add("onclick", "return jsCheckPasswds();");


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

        
        public Person GetPerson()
        {
            //string bonDigpasswd = (IsDigitalCampaign) ? PasswdInput1.Text : string.Empty;
            
            return new Person(true,
                              false,
                              FirstNameInput.Text,
                              LastNameInput.Text,
                              CareOfInput.Text,
                              CompanyInput.Text,
                              StreetInput.Text,
                              StreetNumberInput.Text,
                              DoorInput.Text,
                              StairsInput.Text,
                              AppartmentInput.Text,
                              ZipInput.Text,
                              StateInput.Text,
                              TelephoneInput.Text,
                              EmailInput.Text,
                              SocialSecurityNoInput.Text,
                              string.Empty,
                              string.Empty,
                              string.Empty);
        }

        protected void StreetFormButton_Click(object sender, EventArgs e)
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


            //save to cirix
            if (SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.Invoice || (SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit && !PayMethodsForm1.DirectDebitOtherPayer) )
            {
                String err = CampPage.SaveSubscription();
                if (String.IsNullOrEmpty(err))
                    CampPage.ShowThankYou();
                else 
                    CampPage.ShowMessage(err,false,true);

            }   //redirect to the next step
            else if (SelectedPayMethod == PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer || PayMethodsForm1.DirectDebitOtherPayer)   
            {
                //Response.Redirect(CurrentPage.LinkURL + "&op=1");
                CampPage.DisplayPlaceHolders(false, true);
            }   //redirect to Nets
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


        internal void Populate(string firstName, string lastName, string email, string streetName, string streetNum, string door, string stairs, string appartmentNum, string zip, string city, string careOf, string phoneMob, string company)
        {
            FirstNameInput.Text = firstName;
            LastNameInput.Text = lastName;
            EmailInput.Text = email;
            StreetInput.Text = streetName;
            StreetNumberInput.Text = streetNum;
            DoorInput.Text = door;
            StairsInput.Text = stairs;
            AppartmentInput.Text = appartmentNum;
            ZipInput.Text = zip;
            StateInput.Text = city;
            CareOfInput.Text = careOf;
            TelephoneInput.Text = phoneMob;
            CompanyInput.Text = company;
        }

        internal void Clear()
        {
            FirstNameInput.Text = "";
            LastNameInput.Text = "";
            EmailInput.Text = "";
            StreetInput.Text = "";
            StreetNumberInput.Text = "";
            DoorInput.Text = "";
            StairsInput.Text = "";
            ZipInput.Text = "";
            StateInput.Text = "";
            CareOfInput.Text = "";
            TelephoneInput.Text = "";
            CompanyInput.Text = "";
            SocialSecurityNoInput.Text = "";
            BirthNoInput.Text = "";
            BirthNoErrMess.Visible = false;
        }
    }
}