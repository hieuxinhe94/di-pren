namespace DIClassLib.Subscriptions.AddCustAndSub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DIClassLib.Misc;

    /// <summary>
    /// 
    /// </summary>
    public class Payer
    {
        private const int PHONE_MAX_NO_OF_DIGITS = 20;
        
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string CareOf { get; set; }
        public string Company { get; set; }
        public string CompanyNo { get; set; }
        public string Attention { get; set; }
        public string StreetName { get; set; }
        public string HouseNo { get; set; }
        public string StairCase { get; set; }

        public string Stairs { get; set; }
        public string ApartmentNo { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }
        //public string Email { get; set; }
        //public string MobilePhone { get; set; }
        public string PhoneDayTime { get; set; }
        //public List<MessageStaff> ValidationErrors = new List<MessageStaff>();


        //public Payer(){}


        public Payer(string phoneDayTime,
                        string company, string careOf,
                        string attention, string companyNo,
                        string streetName, string houseNo, string stairCase, string stairs, string apartmentNo,
                        string zipCode, string city)
        {
            PhoneDayTime = MiscFunctions.REC(phoneDayTime, true);

            Company = MiscFunctions.REC(company, true);
            CareOf = MiscFunctions.REC(careOf, true);

            Attention = MiscFunctions.REC(attention, true);
            CompanyNo = MiscFunctions.REC(companyNo, true);

            StreetName = MiscFunctions.REC(streetName, true);
            HouseNo = MiscFunctions.REC(houseNo, true);

            StairCase = MiscFunctions.REC(stairCase, true);
            Stairs = MiscFunctions.REC(stairs, false);
            ApartmentNo = MiscFunctions.REC(apartmentNo, false);

            ZipCode = MiscFunctions.REC(zipCode, true);
            ZipCode = ZipCode.Replace(" ", "");
            City = MiscFunctions.REC(city, true);
        }

        internal void Validate(AddCustAndSubReturnObject ret)
        {
            if (string.IsNullOrEmpty(PhoneDayTime))
                ret.Messages.Add(Message.MessValidatePayerPhone());

            string phoneFormatted = MiscFunctions.FormatPhoneNumber(PhoneDayTime, PHONE_MAX_NO_OF_DIGITS, false);
            if (!string.IsNullOrEmpty(PhoneDayTime) && string.IsNullOrEmpty(phoneFormatted))
                ret.Messages.Add(Message.MessValidatePayerPhoneFormat());

            if (string.IsNullOrEmpty(ZipCode))
                ret.Messages.Add(Message.MessValidatePayerZip());

            if (!string.IsNullOrEmpty(ZipCode) && !MiscFunctions.IsValidSweZipCode(ZipCode))
                ret.Messages.Add(Message.MessValidatePayerZipFormat());

        }
    }
}
