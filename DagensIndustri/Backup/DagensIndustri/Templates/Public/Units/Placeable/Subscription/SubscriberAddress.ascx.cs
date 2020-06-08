using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Templates.Public.Pages;
using DIClassLib.Subscriptions;

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class SubscriberAddress : UserControlBase
    {
        #region Properties
        public bool AcceptedPromotionalOffer 
        {
            get
            {
                return DiGoldControl.AcceptedPromotionalOffer;
            }
            set
            {
                DiGoldControl.AcceptedPromotionalOffer = value;
            }
        }

        public string MobilePhoneNo
        {
            get
            {
                return MobilePhoneInput.Text.Trim();
            }
        }

        public string SocialSecurityNo
        {
            get
            {
                return SocialSecurityNoInput.Text.Trim();
            }
        }
        #endregion 

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            SocialSecurityNoInput.Required = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DiGoldControl.Visible = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DataBind();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create a person object from the input data
        /// </summary>
        /// <returns></returns>
        public Person GetPerson()
        {
            return new Person(true,
                              false,
                              FirstNameInput.Text,
                              LastNameInput.Text,
                              CareOfInput.Text,
                              CompanyInput.Text,
                              StreetAddressInput.Text,
                              HouseNoInput.Text,
                              StairCaseInput.Text,
                              StairsInput.Text,
                              AparmentNoInput.Text,
                              ZipCodeInput.Text,
                              CityInput.Text,
                              MobilePhoneInput.Text,
                              EmailInput.Text,
                              SocialSecurityNoInput.Text,
                              CompanyNumberInput.Text,
                              string.Empty,
                              string.Empty);
        }

        /// <summary>
        /// Fill control with values from the person object
        /// </summary>
        /// <param name="person"></param>
        public void FillControl(Person person)
        {           
            if (person != null)
            {
                FirstNameInput.Text = person.FirstName;
                LastNameInput.Text = person.LastName;
                CareOfInput.Text = person.CareOf;
                CompanyInput.Text = person.Company;
                StreetAddressInput.Text = person.StreetName;
                HouseNoInput.Text = person.HouseNo;
                StairCaseInput.Text = person.StairCase;
                StairsInput.Text = person.Stairs;
                AparmentNoInput.Text = person.ApartmentNo;
                ZipCodeInput.Text = person.ZipCode;
                CityInput.Text = person.City;
                MobilePhoneInput.Text = person.MobilePhone;
                EmailInput.Text = person.Email;
                SocialSecurityNoInput.Text = person.SocialSecurityNo;
                CompanyNumberInput.Text = person.CompanyNo;
            }
        }


        public String MinSubstartDate
        {
            get; set;           
        }

   

        #endregion

        internal DateTime GetSelectedSubsStartDate()
        {
            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse(SubStartDate.Text, out dt))
            {
                return dt;
            }
            return DateTime.MinValue;
        }
    }
}