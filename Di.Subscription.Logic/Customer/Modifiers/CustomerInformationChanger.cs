using System;
using Di.Subscription.DataAccess.Customer;
using Di.Subscription.Logic.Customer.Retrievers;

namespace Di.Subscription.Logic.Customer.Modifiers
{
    internal class CustomerInformationChanger : ICustomerInformationChanger
    {
        private readonly ICustomerRetriever _customerRetriever;
        private readonly ICustomerRepository _customerRepository;

        public CustomerInformationChanger(
            ICustomerRepository customerRepository, 
            ICustomerRetriever customerRetriever)
        {
            _customerRepository = customerRepository;
            _customerRetriever = customerRetriever;
        }

        public string UpdateCustomerInformation(
            long customerNumber,
            string email,
            string homePhone,
            string workPhone,
            string officePhone,
            string accountNumberBank,
            string accountNumberAccount,
            string notes,
            long eCustomerNumber,
            string otherCustomerNumber,
            string wwwUserId,
            string expDay,
            string terms,
            string socialSecurityNumber,
            string category,
            long masterCustomerNumber,            
            string customerType,
            string companyId)
        {
            return _customerRepository.UpdateCustomerInformation(
                customerNumber,
                email,
                homePhone,
                workPhone,
                officePhone,
                accountNumberBank,
                accountNumberAccount,
                false,
                notes,
                eCustomerNumber,
                otherCustomerNumber,
                wwwUserId,
                expDay,
                terms,
                socialSecurityNumber,
                category,
                masterCustomerNumber,
                string.Empty,
                string.Empty,
                DateTime.MinValue,
                customerType,
                string.Empty,
                false,
                companyId,
                string.Empty,
                string.Empty,
                string.Empty);
        }

        public string UpdateCustomerName(long customerNumber, string firstName, string lastName)
        {
            var customer = _customerRepository.GetCustomer(customerNumber);

            var fullName = lastName + " " + firstName;
            var rowText1 = string.IsNullOrEmpty(customer.RowText2) ? fullName : customer.RowText1;
            var rowText2 = !string.IsNullOrEmpty(customer.RowText2) ? fullName : customer.RowText2;

            return _customerRepository.UpdateCustomerName(customerNumber, rowText1, rowText2, string.Empty, firstName, lastName, DateTime.Now);
        }

        public string UpdateCustomerEmail(long customerNumber, string email)
        {
            var customerForUpdate = _customerRetriever.GetCustomerForUpdate(customerNumber);

            return UpdateCustomerInformation(
                customerForUpdate.CustomerNumber,
                email,
                customerForUpdate.PhoneHome ?? string.Empty,
                customerForUpdate.PhoneWork ?? string.Empty,
                customerForUpdate.PhoneOffice ?? string.Empty,
                customerForUpdate.AccountNumberBank ?? string.Empty,
                customerForUpdate.AccountNumberAccount ?? string.Empty,
                customerForUpdate.Notes ?? string.Empty,
                customerForUpdate.ECustomerNumber,
                customerForUpdate.OtherCustomerNumber ?? string.Empty,
                customerForUpdate.WwwUserId ?? string.Empty,
                customerForUpdate.ExpDay ?? string.Empty,
                customerForUpdate.Terms ?? string.Empty,
                customerForUpdate.SocialSecurityNumber ?? string.Empty,
                customerForUpdate.Category ?? string.Empty,
                customerForUpdate.MasterCustomerNumber,
                customerForUpdate.CustomerType ?? string.Empty,
                customerForUpdate.CompanyNumber ?? string.Empty);
        }

        public string UpdateCustomerPhone(long customerNumber, string phone)
        {
            var customerForUpdate = _customerRetriever.GetCustomerForUpdate(customerNumber);

            return UpdateCustomerInformation(
                customerForUpdate.CustomerNumber,
                customerForUpdate.Email ?? string.Empty,
                customerForUpdate.PhoneHome ?? string.Empty,
                customerForUpdate.PhoneWork ?? string.Empty,
                phone,
                customerForUpdate.AccountNumberBank ?? string.Empty,
                customerForUpdate.AccountNumberAccount ?? string.Empty,
                customerForUpdate.Notes ?? string.Empty,
                customerForUpdate.ECustomerNumber,
                customerForUpdate.OtherCustomerNumber ?? string.Empty,
                customerForUpdate.WwwUserId ?? string.Empty,
                customerForUpdate.ExpDay ?? string.Empty,
                customerForUpdate.Terms ?? string.Empty,
                customerForUpdate.SocialSecurityNumber ?? string.Empty,
                customerForUpdate.Category ?? string.Empty,
                customerForUpdate.MasterCustomerNumber,
                customerForUpdate.CustomerType ?? string.Empty,
                customerForUpdate.CompanyNumber ?? string.Empty);
        }
    }
}
