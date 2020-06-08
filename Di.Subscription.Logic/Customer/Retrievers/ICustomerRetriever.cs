using Di.Subscription.Logic.Customer.Types;
using System.Collections.Generic;

namespace Di.Subscription.Logic.Customer.Retrievers
{
    internal interface ICustomerRetriever
    {
        Types.Customer GetCustomer(long customerNumber);
        long GetCustomerNumberByEcusno(long eCustomerNumber);
        long GetEcusnoByCustomerNumber(long customerNumber);
        IEnumerable<long> FindCustomerNumbersByEmail(string email);
        Types.CustomerForUpdate GetCustomerForUpdate(long customerNumber);
        IEnumerable<CustomerProperty> GetCustomerProperties(long customerNumber);
    }
}