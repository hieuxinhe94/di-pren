﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;


namespace DIClassLib.Subscriptions
{
    [Serializable]
    public class Person
    {
        #region Constants
        private const int PHONE_MAX_NO_OF_DIGITS = 20;
        #endregion

        #region Properties
        public long Cusno { get; set; }
        public string UserName { get; set; }                //generated by oracle
        public string Password { get; set; }                //generated by oracle
        //public string PasswordBonDig { get; set; }        //130410 - removed (get from web form, send to bonDig)

        public bool IsSubscriber { get; set; }              //subscriber=true / payer=false
        public bool HasPostalPlaceAddress { get; set; }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = MiscFunctions.REC(value, true);
                SetCirixNames();
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = MiscFunctions.REC(value, true);
                SetCirixNames();
            }
        }
        public string CareOf { get; private set; }

        public string Company { get; set; }

        public string StreetName { get; private set; }
        public string HouseNo { get; set; }
        public string StairCase { get; private set; }

        private string _stairs = string.Empty;
        public string Stairs
        {
            get
            {
                if (!string.IsNullOrEmpty(_stairs) && !_stairs.EndsWith("TR"))
                    return _stairs + "TR";

                return _stairs;
            }
            private set { _stairs = value; }
        }

        public string ApartmentNo { get; private set; }
        public string ZipCode { get; set; }
        public string City { get; private set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string SocialSecurityNo { get; set; }
        public string CompanyNo { get; private set; }
        public string Attention { get; private set; }
        public string PhoneDayTime { get; set; }
        
        public string CirixName1 { get; set; }
        public string CirixName2 { get; set; }
        public string CirixStreet2 { get; set; }

        public bool? IsGoldMember = null;
        public bool? IsPersonalEx = null;

        public bool PhysicalAddressMissing
        {
            get
            {
                return string.IsNullOrEmpty(CareOf) &&
                       string.IsNullOrEmpty(StreetName) &&
                       string.IsNullOrEmpty(HouseNo) &&
                       string.IsNullOrEmpty(StairCase) &&
                       string.IsNullOrEmpty(Stairs) &&
                       string.IsNullOrEmpty(ApartmentNo) &&
                       (string.IsNullOrEmpty(ZipCode) || ZipCode == "10000") &&
                       string.IsNullOrEmpty(City);
            }
        }

        public string ServicePlusUserToken { get; set; }
        public string ServicePlusUserId { get; set; }

        private List<Subscription> _subsHistory = null;
        public List<Subscription> SubsHistory 
        {
            get 
            {
                if (_subsHistory == null && Cusno > 0)
                    _subsHistory = SubscriptionController.GetSubscriptions2(Cusno);

                if (_subsHistory == null)
                    return new List<Subscription>();

                return _subsHistory; 
            }
        }
        //public string CirixSocialSecurityNo { get; private set; }
        //public string CusState { get; set; }    //00–potential, 01–active, 03–passive
        #endregion


        #region Constructors
        public Person() { }

        public Person(bool isSubscriber)
        {
            IsSubscriber = isSubscriber;
        }

        public Person(bool isSubscriber, bool hasPostalPlaceAddress, string firstName, string lastName, string careOf, string company,
            string streetName, string houseNo, string stairCase, string stairs, string apartmentNo, string zipCode, string city,
            string mobilePhone, string email, string socialSecurityNo, string companyNo, string attention, string phoneDayTime
            , string servicePlusToken = "", string servicePlusUserId = "") //string passwordBonDig
        {
            IsSubscriber = isSubscriber;
            HasPostalPlaceAddress = hasPostalPlaceAddress;
            FirstName = firstName;
            LastName = lastName;
            CareOf = MiscFunctions.REC(careOf, true);
            Company = MiscFunctions.REC(company, true);
            StreetName = MiscFunctions.REC(streetName, true);
            HouseNo = MiscFunctions.REC(houseNo, true);
            StairCase = MiscFunctions.REC(stairCase, true);
            Stairs = MiscFunctions.REC(stairs, true);
            ApartmentNo = MiscFunctions.REC(apartmentNo, true);
            ZipCode = MiscFunctions.REC(zipCode, true);
            City = MiscFunctions.REC(city, true);
            MobilePhone = MiscFunctions.FormatPhoneNumber(MiscFunctions.REC(mobilePhone, true), PHONE_MAX_NO_OF_DIGITS, true);
            Email = MiscFunctions.REC(email, false).ToLower();
            SocialSecurityNo = MiscFunctions.FormatSocialSecurityNo(MiscFunctions.REC(socialSecurityNo, true));
            CompanyNo = MiscFunctions.REC(companyNo, true);
            Attention = MiscFunctions.REC(attention, true);
            PhoneDayTime = MiscFunctions.FormatPhoneNumber(MiscFunctions.REC(phoneDayTime, true), PHONE_MAX_NO_OF_DIGITS, false);
            ServicePlusUserToken = MiscFunctions.REC(servicePlusToken);
            ServicePlusUserId = MiscFunctions.REC(servicePlusUserId);
            //PasswordBonDig = MiscFunctions.REC(passwordBonDig);

            SetCirixNames();
            CirixStreet2 = (CareOf + " " + ApartmentNo).Trim();
            //CirixSocialSecurityNo = !string.IsNullOrEmpty(SocialSecurityNo) ? SocialSecurityNo : CompanyNo;
        }

        public Person(bool isSubscriber, bool hasPostalPlaceAddress, string name, string streetName, string zipCode, string mobilePhone, string email)
        {
            IsSubscriber = isSubscriber;
            HasPostalPlaceAddress = hasPostalPlaceAddress;
            FirstName = "";
            LastName = "";
            CareOf = "";
            Company = "";
            StreetName = MiscFunctions.REC(streetName, true);
            HouseNo = "";
            StairCase = "";
            Stairs = "";
            ApartmentNo = "";
            ZipCode = MiscFunctions.REC(zipCode, true);
            City = "";
            MobilePhone = MiscFunctions.FormatPhoneNumber(MiscFunctions.REC(mobilePhone, true), PHONE_MAX_NO_OF_DIGITS, true);
            Email = MiscFunctions.REC(email, false).ToLower();
            SocialSecurityNo = "";
            CompanyNo = "";
            Attention = "";
            PhoneDayTime = "";

            CirixName1 = MiscFunctions.REC(name, true);
            CirixName2 = "";

            //CirixStreet2 = (CareOf + " " + ApartmentNo).Trim();
            //CirixSocialSecurityNo = !string.IsNullOrEmpty(SocialSecurityNo) ? SocialSecurityNo : CompanyNo;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Set Cirix names that applies the Cirix naming rules
        /// </summary>
        private void SetCirixNames()
        {
            if (string.IsNullOrEmpty(Company))
            {
                CirixName1 = string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) ? string.Empty : string.Format("{0} {1}", LastName, FirstName);
                CirixName2 = string.Empty;
            }
            else
            {
                CirixName1 = Company;
                CirixName2 = string.Format("{0} {1} {2}", Attention, LastName, FirstName).Trim();
            }
        }

        /// <summary>
        /// used to logging
        /// </summary>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (IsSubscriber)
                sb.AppendFormat("<b>{0}</b><br>", "Prenumerant");
            else
                sb.AppendFormat("<b>{0}</b><br>", "Betalare");

            //sb.AppendFormat("{0}: {1}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/username"), UserName);
            //sb.AppendFormat("Password: {1}<br>", Password);
            //sb.AppendFormat("{0}: {1}<br>", "Har gatuadress", HasPostalPlaceAddress ? "ja" : "nej");
            sb.AppendFormat("{0}: {1}<br>", "Kundnummer", Cusno);
            sb.AppendFormat("{0}: {1}<br>", "Förnamn", FirstName);
            sb.AppendFormat("{0}: {1}<br>", "Efternamn", LastName);
            sb.AppendFormat("{0}: {1}<br>", "c/o", CareOf);
            sb.AppendFormat("{0}: {1}<br>", "Företag", Company);
            sb.AppendFormat("{0}: {1}<br>", "Gatuadress", StreetName);
            sb.AppendFormat("{0}: {1}<br>", "Gatunummer", HouseNo);
            sb.AppendFormat("{0}: {1}<br>", "Trappuppgång", StairCase);
            sb.AppendFormat("{0}: {1}<br>", "Trappor", Stairs);
            sb.AppendFormat("{0}: {1}<br>", "Lägenhetsnummer", ApartmentNo);
            sb.AppendFormat("{0}: {1}<br>", "Postnummer", ZipCode);
            sb.AppendFormat("{0}: {1}<br>", "Stad", City);
            sb.AppendFormat("{0}: {1}<br>", "Tel dagtid", PhoneDayTime);
            sb.AppendFormat("{0}: {1}<br>", "Tel mobil", MobilePhone);
            sb.AppendFormat("{0}: {1}<br>", "Epost", Email);
            sb.AppendFormat("{0}: {1}<br>", "Födelsedatum", SocialSecurityNo);
            sb.AppendFormat("{0}: {1}<br>", "Org.nr", CompanyNo);
            sb.AppendFormat("{0}: {1}<br>", "Att", Attention);
            sb.AppendFormat("{0}: {1}<br>", "CirixName1", CirixName1);
            sb.AppendFormat("{0}: {1}<br>", "CirixName2", CirixName2);
            sb.AppendFormat("{0}: {1}<br>", "CirixStreet2", CirixStreet2);
            //sb.AppendFormat("{0}: {1}<hr>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/cirixsocialsecurityno"), CirixSocialSecurityNo);

            return sb.ToString();
        }
        #endregion

    }
}