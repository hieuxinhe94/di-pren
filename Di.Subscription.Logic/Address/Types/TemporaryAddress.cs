﻿using System;

namespace Di.Subscription.Logic.Address.Types
{
    internal class TemporaryAddress
    {
        public TemporaryAddress(
            long customerNumber, 
            long subscriptionNumber, 
            int externalNumber, 
            DateTime startDate,
            DateTime endDate, 
            string streetAddress, 
            string streetNo, 
            string stairCase, 
            string floor,
            string apartment, 
            string street2,
            string zip)
        {
            CustomerNumber = customerNumber;
            SubscriptionNumber = subscriptionNumber;
            ExternalNumber = externalNumber;
            StartDate = startDate;
            EndDate = endDate;
            StreetAddress = streetAddress;
            StreetNumber = streetNo;
            StairCase = stairCase;
            Floor = floor;
            Apartment = apartment;
            Street2 = street2;
            Zip = zip;

            // Default values
            Name1 = string.Empty;
            Name2 = string.Empty;
            InvoiceToTemporaryAddress = string.Empty;
            PaperCode = SubscriptionConstants.PaperCodeDi;
            ReceiveType = SubscriptionConstants.DefaultRecieveType;
            SaveAllPackageSubs = false;
            UserId = SubscriptionConstants.DefaultUserId;
            Street3 = string.Empty;
            CountryCode = AddressConstants.CountryCode;
        }

        internal string UserId { get; set; }

        internal long CustomerNumber { get; set; }

        internal long SubscriptionNumber { get; set; }

        internal int ExternalNumber { get; set; }

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

        internal DateTime EndDate { get; set; }

        internal string InvoiceToTemporaryAddress { get; set; }

        internal string Name1 { get; set; }

        internal string Name2 { get; set; }

        internal string PaperCode { get; set; }

        internal string ReceiveType { get; set; }

        internal bool SaveAllPackageSubs { get; set; }
    }
}
