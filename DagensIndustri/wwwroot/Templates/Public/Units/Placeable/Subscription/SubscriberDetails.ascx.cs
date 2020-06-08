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
using DIClassLib.DbHandlers;
using System.Data;


namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class SubscriberDetails : UserControlBase
    {
        #region Properties
        public string Heading 
        {
            get
            {
                return Translate("/subscription/subscriberdetails/fillyourdetails");
            }
        }

        public PaymentMethod.TypeOfPaymentMethod PaymentMethod
        {
            get
            {
                return PaymentMethodControl.SelectedPaymentMethod;
            }
            set
            {
                PaymentMethodControl.SelectedPaymentMethod = value;
            }
        }

        public bool AcceptedPromotionalOffer
        {
            get
            {
                if (!IsAddressPostalPlace)
                    return SubscriberAddressControl.AcceptedPromotionalOffer;
                else
                    return PostalPlaceControl.AcceptedPromotionalOffer;
            }
            set
            {
                if (!IsAddressPostalPlace)
                    SubscriberAddressControl.AcceptedPromotionalOffer = value;
                else
                    PostalPlaceControl.AcceptedPromotionalOffer = value;
            }
        }

        private bool IsAddressPostalPlace
        {
            get
            {
                //return !string.IsNullOrEmpty(PostalPlaceListItem.Attributes["class"]) && PostalPlaceListItem.Attributes["class"].ToLower() == "current";
                Person p = PostalPlaceControl.GetPerson();
                if (!string.IsNullOrEmpty(p.FirstName) && !string.IsNullOrEmpty(p.LastName) && !string.IsNullOrEmpty(p.StreetName))
                    return true;

                return false;
            }
            //set
            //{
            //    if (value)
            //    {
            //        PostalPlaceListItem.Attributes.Add("class", "current");
            //        StreetListItem.Attributes.Remove("class");
            //    }
            //    else
            //    {
            //        StreetListItem.Attributes.Add("class", "current");
            //        PostalPlaceListItem.Attributes.Remove("class");
            //    }                
            //}
        }

        // store campaign number, so we can set min startdate in the child controls
        //
        public long CampaignNo1
        {
            get;
            set;
        }

        public bool IsDirectDebit { get; set; }

          #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SelectJsTab();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check if data is valid. If not, show message to the user
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            //Validate the entered mobile number
            string mobilePhoneNo = IsAddressPostalPlace ? PostalPlaceControl.MobilePhoneNo : SubscriberAddressControl.MobilePhoneNo;

            if (string.IsNullOrEmpty(MiscFunctions.FormatPhoneNumber(mobilePhoneNo, Settings.PhoneMaxNoOfDigits, true)))
            {
                ((DiTemplatePage)Page).ShowMessage("/common/validation/mobilephonenumberrequired", true, true);
                return false;
            }
            
            //Validate the entered social security number
            string socialSecurityNo = IsAddressPostalPlace ? PostalPlaceControl.SocialSecurityNo : SubscriberAddressControl.SocialSecurityNo;

            if (!string.IsNullOrEmpty(socialSecurityNo) && string.IsNullOrEmpty(MiscFunctions.FormatSocialSecurityNo(socialSecurityNo)))
            {
                ((DiTemplatePage)Page).ShowMessage("/common/validation/socialsecuritynumberrequired", true, true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get a person object filled with data from either SubscriberAddressControl or PostalPlaceControl
        /// </summary>
        /// <returns></returns>
        public Person GetPerson()
        {
            Person person = IsAddressPostalPlace ? PostalPlaceControl.GetPerson() : SubscriberAddressControl.GetPerson();
            
            return person;
        }

        /// <summary>
        /// If payment method is direct debit, then the direct debit information has to be made visible
        /// </summary>
        /// <param name="show"></param>
        public void ShowDirectDebit(bool show)
        {
            IsDirectDebit = show;
            PaymentMethodControl.ShowDirectDebit(show);
            ((MasterPages.MasterPage)Page.Master).ShowSideBarBoxes(show);
        }

        /// <summary>
        /// Fill control with values from the person object
        /// </summary>
        /// <param name="person"></param>
        public void FillControl(Person person)
        {
            if (person != null)
            {
                //IsAddressPostalPlace = person.HasPostalPlaceAddress;
                StreetListItem.Attributes.Remove("class");
                PostalPlaceListItem.Attributes.Remove("class");
                
                if (person.HasPostalPlaceAddress)
                {
                    PostalPlaceListItem.Attributes.Add("class", "current");
                    PostalPlaceControl.FillControl(person);
                }
                else
                {
                    StreetListItem.Attributes.Add("class", "current");
                    SubscriberAddressControl.FillControl(person);
                }
            }
        }

        private void SelectJsTab()
        {
            StreetListItem.Attributes.Remove("class");
            PostalPlaceListItem.Attributes.Remove("class");

            if (IsAddressPostalPlace)
                PostalPlaceListItem.Attributes.Add("class", "current");
            else
                StreetListItem.Attributes.Add("class", "current");
        }
        #endregion

        /// <summary>
        /// Set the min value for the subscription start date
        /// </summary>
        /// <param name="dtFirst"></param>
        public void SetFirstStartSubsDate(DateTime dtFirst)
        {
            String sDate = "";
            if (dtFirst != DateTime.MinValue)
                sDate = dtFirst.ToString("yyyy-MM-dd");
            SubscriberAddressControl.MinSubstartDate = sDate;
            PostalPlaceControl.MinSubstartDate = sDate;
        }

        internal DateTime GetSelectedSubsStartDate()
        {
            DateTime selectedSubsStartDate = IsAddressPostalPlace ? PostalPlaceControl.GetSelectedSubsStartDate() : SubscriberAddressControl.GetSelectedSubsStartDate();
            return selectedSubsStartDate;
        }
    }
}