using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Customer.Modifiers;
using Di.Subscription.Logic.Customer.Retrievers;
using Di.Subscription.Logic.Customer.Types;
using System.Linq;

namespace Di.Subscription.Logic.Customer
{
    internal class CustomerHandler : ICustomerHandler
    {
        private readonly ICustomerRetriever _customerRetriever;
        private readonly ICustomerInformationChanger _customerInformationChanger;
        private readonly ICustomerPropertyChanger _customerPropertyChanger;

        public CustomerHandler(
            ICustomerRetriever customerRetriever, 
            ICustomerInformationChanger customerInformationChanger, 
            ICustomerPropertyChanger customerPropertyChanger)
        {
            _customerRetriever = customerRetriever;
            _customerInformationChanger = customerInformationChanger;
            _customerPropertyChanger = customerPropertyChanger;
        }

        public Types.Customer GetCustomer(long customerNumber)
        {
            return _customerRetriever.GetCustomer(customerNumber);
        }

        public long GetCustomerNumberByEcusno(long eCustomerNumber)
        {
            return _customerRetriever.GetCustomerNumberByEcusno(eCustomerNumber);
        }

        public long GetEcusnoByCustomerNumber(long customerNumber)
        {
            return _customerRetriever.GetEcusnoByCustomerNumber(customerNumber);
        }


        public IEnumerable<long> FindCustomerNumbersByEmail(string email)
        {
            return _customerRetriever.FindCustomerNumbersByEmail(email);
        }

        public bool UpdateCustomerName(long customerNumber, string firstName, string lastName)
        {
            return _customerInformationChanger.UpdateCustomerName(customerNumber, firstName, lastName) == "OK";
        }

        public bool UpdateCustomerEmail(long customerNumber, string email)
        {
            var updated = _customerInformationChanger.UpdateCustomerEmail(customerNumber, email) == "OK";

            return updated && _customerPropertyChanger.InsertUpdateCustomerProperty(customerNumber, SubscriptionConstants.PropertyCodeEmail, email);
        }

        public bool UpdateCustomerPhone(long customerNumber, string phone)
        {
            var updated = _customerInformationChanger.UpdateCustomerPhone(customerNumber, phone) == "OK";

            return updated && _customerPropertyChanger.InsertUpdateCustomerProperty(customerNumber, SubscriptionConstants.PropertyCodeMobilePhone, phone);
        }

        public List<CustomerProperty> GetCustomerProperties(long customerNumber)
        {
            return _customerRetriever.GetCustomerProperties(customerNumber).ToList();
        }
    }
}