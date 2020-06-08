using System;

namespace Di.Subscription.Logic.Address.Types
{
    internal class PermanentAddress
    {
        public PermanentAddress(
            long customerNumber,
            DateTime startDate,
            string streetAddress,
            string streetNo,
            string stairCase,
            string floor,
            string apartment,
            string street2,
            string zip)
        {
            CustomerNumber = customerNumber;
            StartDate = startDate;
            StreetAddress = streetAddress;
            StreetNumber = streetNo;
            StairCase = stairCase;
            Floor = floor;
            Apartment = apartment;
            Street2 = street2;
            Zip = zip;

            // Default Values
            UserId = SubscriptionConstants.DefaultUserId;
            ChangeImmediately = false;
            Street3 = string.Empty;
            CountryCode = AddressConstants.CountryCode;
            TempName1 = string.Empty;
            TempName2 = string.Empty;
            ReceiveType = SubscriptionConstants.DefaultRecieveType;
        }

        internal string UserId { get; set; }

        internal long CustomerNumber { get; set; }

        internal string StreetAddress { get; set; }

        internal string StreetNumber { get; set; }

        internal string StairCase { get; set; }

        internal string Floor { get; set; }

        internal string Apartment { get; set; }

        internal string Street2 { get; set; }

        internal string Street3 { get; set; }

        internal string CountryCode { get; set; }

        internal string Zip { get; set; }

        internal DateTime StartDate { get; set; }

        internal string TempName1 { get; set; }

        internal string TempName2 { get; set; }

        internal string ReceiveType { get; set; }

        internal bool ChangeImmediately { get; set; }
    }
}
