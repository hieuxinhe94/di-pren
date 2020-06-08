using Di.Subscription.Logic.Customer.Types;
using System.Collections.Generic;

namespace Di.Subscription.Logic.Customer
{
    public interface ICustomerHandler
    {
        Types.Customer GetCustomer(long customerNumber);
        long GetCustomerNumberByEcusno(long eCustomerNumber);
        long GetEcusnoByCustomerNumber(long customerNumber);
        IEnumerable<long> FindCustomerNumbersByEmail(string email);
        bool UpdateCustomerName(long customerNumber, string firstName, string lastName);
        bool UpdateCustomerEmail(long customerNumber, string email);
        bool UpdateCustomerPhone(long customerNumber, string phone);
        List<CustomerProperty> GetCustomerProperties(long customerNumber);
    }
}