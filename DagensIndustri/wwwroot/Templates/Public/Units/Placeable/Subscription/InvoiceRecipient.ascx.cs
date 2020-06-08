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

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class InvoiceRecipient : UserControlBase
    {
        #region Properties
        public string Heading 
        {
            get
            {
                return Translate("/subscription/anotherinvoiceaddressee/invoicerecipientpayer");
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Check if data is valid. If not, show message to the user
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            //Validate the entered phone number
            if (string.IsNullOrEmpty(MiscFunctions.FormatPhoneNumber(PhoneDayTimeInput.Text, Settings.PhoneMaxNoOfDigits, false)))
            {
                ((DiTemplatePage)Page).ShowMessage("/common/validation/phonenumberrequired", true, true);
                return false;
            }            
            return true;
        }

        /// <summary>
        /// Create a person object from the input data
        /// </summary>
        /// <returns></returns>
        public Person GetPerson()
        {
            return new Person(false,
                              false,
                              FirstNameInput.Text,
                              LastNameInput.Text,
                              CareOfInput.Text,
                              CompanyInput.Text,
                              StreetAddressInput.Text,
                              HouseNoInput.Text,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              ZipCodeInput.Text,
                              CityInput.Text,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              CompanyNumberInput.Text,
                              AttentionInput.Text,
                              PhoneDayTimeInput.Text);
        }

         //bool isSubscriber, string firstName, string lastName, string careOf, string company, string streetName, 
        //   string houseNo, string stairCase, string stairs, string apartmentNo, string zipCode, string city,
        //   string mobilePhone, string email, string socialSecurityNo, string companyNo, string attention, string phoneDayTime

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
                ZipCodeInput.Text = person.ZipCode;
                CityInput.Text = person.City;                
                CompanyNumberInput.Text = person.CompanyNo;
                AttentionInput.Text = person.Attention;
                PhoneDayTimeInput.Text = person.PhoneDayTime;
            }
        }
        #endregion
    }
}