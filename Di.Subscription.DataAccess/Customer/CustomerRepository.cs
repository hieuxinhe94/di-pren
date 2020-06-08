using System;
using System.Collections.Generic;
using System.Linq;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Customer
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public CustomerRepository(
            ISubscriptionDataAccess subscriptionDataAccess, 
            IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public Customer GetCustomer(long customerNumber)
        {
            var customerDataSet = _subscriptionDataAccess.GetCustomer(customerNumber);

            return _objectConverter.ConvertFromDataSet<Customer>(customerDataSet).FirstOrDefault();
        }

        public long GetCustomerNumberByEcusno(long eCustomerNumber)
        {
            return _subscriptionDataAccess.GetCustomerNumberByEcusno(eCustomerNumber);
        }

        public long GetEcusnoByCustomerNumbero(long customerNumber)
        {
            return _subscriptionDataAccess.GetEcusnoByCustomerNumber(customerNumber);
        }

        public IEnumerable<CustomerSimple> FindCustomers(long customerNumber, long invoiceNumber, long subscriptionNumber, string sName1, string sName2,
            string sName3, string phone, string socialSecurityNumber, string email, string streetName, string streetNumber,
            string stairCase, string floor, string apartment, string country, string zipCode, string userId, string postName,
            string otherCustomerNumber, string orderId, string paperCode, string referenceNumber)
        {
            var customerDataSet = _subscriptionDataAccess.FindCustomers(customerNumber, invoiceNumber, subscriptionNumber, sName1,
                sName2, sName3, phone,
                socialSecurityNumber, email, streetName, streetNumber, stairCase, floor, apartment, country, zipCode,
                userId, postName, otherCustomerNumber, orderId, paperCode, referenceNumber);

            return _objectConverter.ConvertFromDataSet<CustomerSimple>(customerDataSet);
        }

        public string UpdateCustomerName(long customerNumber, string rowText1, string rowText2, string rowText3, string firstName, string lastName, DateTime activeDate)
        {
            return _subscriptionDataAccess.UpdateCustomerName(customerNumber, rowText1, rowText2, rowText3, firstName, lastName, activeDate);
        }

        public string UpdateCustomerInformation(            
            long customerNumber,
            string email,
            string homePhone,
            string workPhone,
            string officePhone,
            string accountNumberBank,
            string accountNumberAccount,
            bool collectInv,
            string notes,
            long eCustomerNumber,
            string otherCustomerNumber,
            string wwwUserId,
            string expDay,
            string terms,
            string socialSecurityNumber,
            string category,
            long masterCustomerNumber,
            string marketingDenial,
            string marketingDenialReason,
            DateTime marketingDenialDate,
            string customerType,
            string title,
            bool protectedIdentity,
            string companyId,
            string extra1,
            string extra2,
            string extra3)
        {
            return _subscriptionDataAccess.UpdateCustomerInformation(
                customerNumber,
                email,
                homePhone,
                workPhone,
                officePhone,
                accountNumberBank,
                accountNumberAccount,
                collectInv,
                notes,
                eCustomerNumber,
                otherCustomerNumber,
                wwwUserId,
                expDay,
                terms,
                socialSecurityNumber,
                category,
                masterCustomerNumber,
                marketingDenial,
                marketingDenialReason,
                marketingDenialDate,
                customerType,
                title,
                protectedIdentity,
                companyId,
                extra1,
                extra2,
                extra3);
        }

        public string InsertCustomerProperty(long customerNumber, string propertyCode, string propertyValue, string profileSourceGroup,
            string profileSourceSpecification)
        {
            return _subscriptionDataAccess.InsertCustomerProperty(customerNumber, propertyCode, propertyValue,
                profileSourceGroup, profileSourceSpecification);
        }

        public IEnumerable<CustomerPropertyData> GetCustomerProperties(long customerNumber)
        {
            var propertiesDataSet = _subscriptionDataAccess.GetCustomerProperties(customerNumber);           

            return _objectConverter.ConvertFromDataSet<CustomerPropertyData>(propertiesDataSet);
        }
    }
}