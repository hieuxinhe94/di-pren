using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Address.Modifiers;
using Di.Subscription.Logic.Address.Retrievers;
using Di.Subscription.Logic.Address.Types;

namespace Di.Subscription.Logic.Address
{
    /// <summary>
    /// This is a facade for the address handling which clients should use, no logic in this class
    /// </summary>
    internal class AddressHandler : IAddressHandler
    {
        private readonly ITemporaryAddressCreator _temporaryAddressCreator;
        private readonly ITemporaryAddressRemover _temporaryAddressRemover;
        private readonly ITemporaryAddressChanger _temporaryAddressModifier;
        
        private readonly IAddressRetriever _addressRetriever;
        private readonly IPermanentAddressRemover _permanentAddressRemover;
        private readonly IPermanentAddressCreator _permanentAddressCreator;

        public AddressHandler(
            ITemporaryAddressCreator temporaryAddressCreator,
            ITemporaryAddressRemover temporaryAddressRemover,
            IAddressRetriever addressRetriever,
            IPermanentAddressRemover permanentAddressRemover,
            IPermanentAddressCreator permanentAddressCreator, 
            ITemporaryAddressChanger temporaryAddressModifier)
        {
            _temporaryAddressCreator = temporaryAddressCreator;
            _temporaryAddressRemover = temporaryAddressRemover;
            _addressRetriever = addressRetriever;
            _permanentAddressRemover = permanentAddressRemover;
            _permanentAddressCreator = permanentAddressCreator;
            _temporaryAddressModifier = temporaryAddressModifier;
        }

        public string CreateTemporaryAddressChange(
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
            return _temporaryAddressCreator.CreateTemporaryAddressChange(
                customerNumber,
                subscriptionNumber,
                externalNumber,
                startDate,
                endDate,
                streetAddress,
                streetNo,
                stairCase,
                floor,
                apartment,
                street2,
                zip);
        }



        public string DeleteTemporaryAddress(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            DateTime startDate)
        {
            return _temporaryAddressRemover.DeleteTemporaryAddress(
                customerNumber,
                subscriptionNumber,
                externalNumber,
                startDate);
        }

        public string DeletePermanentAddressChange(long customerNumber, DateTime startDate)
        {
            return _permanentAddressRemover.DeletePermanentAddressChange(customerNumber, startDate);
        }

        public string ChangeTemporaryAddressChange(long customerNumber, long subscriptionNumber, int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate)
        {
            return _temporaryAddressModifier.ChangeTemporaryAddressChangeDates(customerNumber, subscriptionNumber,
                externalNumber,
                oldStartDate, newStartDate, newEndDate, invoice, cusAddrEndDate);
        }

        public IEnumerable<AddressChange> GetTemporaryAddressChanges(long customerNumber, long subscriptionNumber)
        {
            return _addressRetriever.GetTemporaryAddressChanges(customerNumber, subscriptionNumber);
        }

        public IEnumerable<AddressChange> GetTemporaryAddresses(long customerNumber)
        {
            return _addressRetriever.GetTemporaryAddresses(customerNumber);
        }

        public string CreatePermanentAddressChange(
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
            return _permanentAddressCreator.CreatePermanentAddressChange(
                customerNumber,
                startDate,
                streetAddress,
                streetNo,
                stairCase,
                floor,
                apartment,
                street2,
                zip);
        }

        public IEnumerable<AddressChange> GetPermanentAddressChanges(long customerNumber, long subscriptionNumber)
        {
            return _addressRetriever.GetPermanentAddressChanges(customerNumber, subscriptionNumber);
        }

        public string AddAddressChangeFee(long customerNumber, long subscriptionNumber, int externalNumber, bool basicAddressChange)
        {
            return _temporaryAddressCreator.AddAddressChangeFee(customerNumber, subscriptionNumber, externalNumber, basicAddressChange);
        }
    }
}