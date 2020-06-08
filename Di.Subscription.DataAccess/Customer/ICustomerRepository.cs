using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Customer
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(long customerNumber);
        
        long GetCustomerNumberByEcusno(long eCustomerNumber);
        long GetEcusnoByCustomerNumbero(long customerNumber);
        
        IEnumerable<CustomerSimple> FindCustomers(long customerNumber, long invoiceNumber, long subscriptionNumber, string sName1, string sName2,
            string sName3, string phone, string socialSecurityNumber, string email, string streetName,
            string streetNumber, string stairCase, string floor, string apartment, string country, string zipCode, string userId,
            string postName, string otherCustomerNumber, string orderId, string paperCode, string referenceNumber);

        string UpdateCustomerName(long customerNumber, string rowText1, string rowText2, string rowText3,
            string firstName, string lastName, DateTime activeDate);

        string UpdateCustomerInformation(
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
            string extra3);

        string InsertCustomerProperty(long customerNumber, string propertyCode, string propertyValue,
            string profileSourceGroup, string profileSourceSpecification);

        IEnumerable<CustomerPropertyData> GetCustomerProperties(long customerNumber);
    }
}