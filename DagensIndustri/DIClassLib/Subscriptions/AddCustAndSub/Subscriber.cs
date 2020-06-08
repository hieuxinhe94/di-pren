using System;
using DIClassLib.Misc;
using System.Collections.Generic;
using System.Linq;

namespace DIClassLib.Subscriptions.AddCustAndSub
{
    public class Subscriber
    {
        private const int PHONE_MAX_NO_OF_DIGITS = 20;
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CareOf { get; set; }
        public string Company { get; set; }
        //public string CompanyNo { get; set; }
        //public string Attention { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string StairCase { get; set; }

        public string Stairs { get; set; }
        public string ApartmentNo { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        //public string PhoneDayTime { get; set; }
        public bool AddressIsRequired { get; set; }
        //public List<MessageStaff> ValidationErrors = new List<MessageStaff>();


        public Subscriber(){}


        public Subscriber(string firstName, string lastName, string mobilePhone, string email,string company, string careOf,                                
                            string streetName, string houseNo, string stairCase, string stairs, string apartmentNo,string zipCode, string city)
        {
            FirstName = MiscFunctions.REC(firstName, true);
            LastName = MiscFunctions.REC(lastName, true);
            Email = MiscFunctions.REC(email, false).ToLower();

            MobilePhone = MiscFunctions.REC(mobilePhone, true);
            
            Company = MiscFunctions.REC(company, true);
            CareOf = MiscFunctions.REC(careOf, true);
            
            StreetName = MiscFunctions.REC(streetName, true);
            HouseNo = MiscFunctions.REC(houseNo, true);
            StairCase = MiscFunctions.REC(stairCase, true);
            Stairs = MiscFunctions.REC(stairs, true);
            ApartmentNo = MiscFunctions.REC(apartmentNo, false);
            ZipCode = MiscFunctions.REC(zipCode, true);
            ZipCode = ZipCode.Replace(" ", "");
            City = MiscFunctions.REC(city, true);
        }



        internal void Validate(AddCustAndSubReturnObject ret)
        {

            if (string.IsNullOrEmpty(FirstName))
                ret.Messages.Add(Message.MessValidateSubscriberFirstName());

            if (string.IsNullOrEmpty(LastName))
                ret.Messages.Add(Message.MessValidateSubscriberLastName());

            if (string.IsNullOrEmpty(Email))
                ret.Messages.Add(Message.MessValidateSubscriberEmail());

            if (!string.IsNullOrEmpty(Email) && !MiscFunctions.IsValidEmail(Email))
                ret.Messages.Add(Message.MessValidateSubscriberEmailFormat());

            string mobFormatted = MiscFunctions.FormatPhoneNumber(MobilePhone, PHONE_MAX_NO_OF_DIGITS, true);
            if (!string.IsNullOrEmpty(MobilePhone) && string.IsNullOrEmpty(mobFormatted))
                ret.Messages.Add(Message.MessValidateSubscriberPhoneFormat());

            if (!string.IsNullOrEmpty(ZipCode) && !MiscFunctions.IsValidSweZipCode(ZipCode))
                ret.Messages.Add(Message.MessValidateSubscriberZipFormat());


            if (AddressIsRequired)
            {
                if (string.IsNullOrEmpty(StreetName))
                    ret.Messages.Add(Message.MessValidateSubscriberStreet());

                if (string.IsNullOrEmpty(ZipCode))
                    ret.Messages.Add(Message.MessValidateSubscriberZip());
            }
        }
    }
}
