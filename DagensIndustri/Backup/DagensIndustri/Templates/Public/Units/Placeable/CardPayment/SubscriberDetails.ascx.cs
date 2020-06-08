using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.CardPayment;


namespace DagensIndustri.Templates.Public.Units.Placeable.CardPayment
{
    public partial class SubscriberDetails : UserControlBase
    {

        public bool IsStreetAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(SubscriberAddressControl.StreetAddressInput.Text) && !string.IsNullOrEmpty(SubscriberAddressControl.ZipCodeInput.Text))
                    return true;

                return false;
            }
        }

        public void PopulateCardPaymentDataHolder(CardPaymentDataHolder dh)
        {
            if(IsStreetAddress)
            {
                dh.CareOf = SubscriberAddressControl.CareOfInput.Text;
                dh.Company = SubscriberAddressControl.CompanyInput.Text;
                dh.Street = SubscriberAddressControl.StreetAddressInput.Text;
                dh.StreetNum = SubscriberAddressControl.HouseNoInput.Text;
                dh.Entrance = SubscriberAddressControl.StairCaseInput.Text;
                dh.StairsNum = SubscriberAddressControl.StairsInput.Text;
                dh.ApartmentNum = SubscriberAddressControl.AparmentNoInput.Text;
                dh.Zip = SubscriberAddressControl.ZipCodeInput.Text;
                dh.City = SubscriberAddressControl.CityInput.Text;
                dh.CompanyNum = SubscriberAddressControl.CompanyNumberInput.Text;
            }
            else
            {
                dh.Company = PostalPlaceControl.CompanyInput.Text;
                dh.StopOrBox = PostalPlaceControl.PostalplaceInput.Text;
                dh.StopOrBoxNum = PostalPlaceControl.PostalPlaceNoInput.Text;
                dh.Zip = PostalPlaceControl.ZipCodeInput.Text;
                dh.City = PostalPlaceControl.CityInput.Text;
                dh.CompanyNum = PostalPlaceControl.CompanyNumberInput.Text;
            }
        }


        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    SelectJsTab();
        //}


        /// <summary>
        /// Check if data is valid. If not, show message to the user
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            //todo: move
            string mobilePhoneNo = "";

            if (string.IsNullOrEmpty(MiscFunctions.FormatPhoneNumber(mobilePhoneNo, Settings.PhoneMaxNoOfDigits, true)))
            {
                ((DiTemplatePage)Page).ShowMessage("/common/validation/mobilephonenumberrequired", true, true);
                return false;
            }
            
            //Validate the entered social security number
            //string socialSecurityNo = IsAddressPostalPlace ? PostalPlaceControl.SocialSecurityNoInput.Text : SubscriberAddressControl.SocialSecurityNoInput.Text;

            //if (!string.IsNullOrEmpty(socialSecurityNo) &&
            //    string.IsNullOrEmpty(MiscFunctions.FormatSocialSecurityNo(socialSecurityNo)))
            //{
            //    ((DiTemplatePage)Page).ShowMessage("/common/validation/socialsecuritynumberrequired", true, true);
            //    return false;
            //}

            return true;
        }


        //private void SelectJsTab()
        //{
        //    StreetListItem.Attributes.Remove("class");
        //    PostalPlaceListItem.Attributes.Remove("class");

        //    if (IsAddressPostalPlace)
        //        PostalPlaceListItem.Attributes.Add("class", "current");
        //    else
        //        StreetListItem.Attributes.Add("class", "current");
        //}




        
    }
}