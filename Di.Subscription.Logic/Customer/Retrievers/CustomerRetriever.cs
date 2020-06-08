using System;
using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Customer;
using Di.Subscription.Logic.Customer.Types;

namespace Di.Subscription.Logic.Customer.Retrievers
{
    internal class CustomerRetriever : ICustomerRetriever
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerRetriever(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Types.Customer GetCustomer(long customerNumber)
        {
            var customerExt = _customerRepository.GetCustomer(customerNumber);
            return GetCustomer(customerExt);
        }

        public long GetCustomerNumberByEcusno(long eCustomerNumber)
        {
            return _customerRepository.GetCustomerNumberByEcusno(eCustomerNumber);
        }

        public long GetEcusnoByCustomerNumber(long customerNumber)
        {
            return _customerRepository.GetEcusnoByCustomerNumbero(customerNumber);
        }


        public IEnumerable<long> FindCustomerNumbersByEmail(string email)
        {
            return _customerRepository.FindCustomers(0, 0, 0, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, email, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, string.Empty, string.Empty).Select(t => t.CustomerNumber);
        }

        private Types.Customer GetCustomer(DataAccess.Customer.Customer customerExt)
        {
            var customer = new Types.Customer
                {
                    CustomerNumber = customerExt.CustomerNumber,
                    ECustomerNumber = (long)customerExt.ECustomerNumber,
                    CompanyName = !string.IsNullOrEmpty(customerExt.RowText2) ? customerExt.RowText1 : string.Empty,
                    CompanyNumber = customerExt.CompanyId,
                    PhoneOffice = customerExt.OfficePhone,
                    PhoneWork = customerExt.WorkPhone,
                    Email = customerExt.EmailAddress,
                    AddressStreetName = customerExt.StreetName,
                    AddressStreetNumber = customerExt.HouseNumber,
                    AddressZip = customerExt.ZipCode,
                    AddressCity = customerExt.PostName,
                    AddressStairCase = customerExt.StairCase,
                    AddressStairs = customerExt.Apartment
                };

            if (customerExt.Street2 != null)
            {
                foreach (var item in customerExt.Street2.Split(' ').Where(item => !item.ToLower().StartsWith("lgh")))
                {
                    customer.AddressCareOf += item + " ";
                }
            }

            var fullName = !string.IsNullOrEmpty(customerExt.RowText2) ? customerExt.RowText2 : customerExt.RowText1;

            if (fullName == null) return customer;

            customer.FullName = fullName;
                
            var nameArray = fullName.Split(' ');

            customer.LastName = nameArray.Length > 0 ? nameArray[0] : string.Empty;
            customer.FirstName = nameArray.Length > 1 ? nameArray[1] : string.Empty;

            return customer;
        }

        public CustomerForUpdate GetCustomerForUpdate(long customerNumber)
        {
            var customerExt = _customerRepository.GetCustomer(customerNumber);
            var customer = GetCustomer(customerExt);

            var customerForUpdate = new CustomerForUpdate(customer)
            {
                AccountNumberAccount = customerExt.AccountNumberAccount ?? string.Empty,
                AccountNumberBank = customerExt.AccountNumberBank ?? string.Empty,
                PhoneHome = customerExt.HomePhone ?? string.Empty,
                PhoneWork = customerExt.WorkPhone ?? string.Empty,
                Notes = customerExt.Notes ?? string.Empty,
                OtherCustomerNumber = customerExt.OtherCustomerNumber ?? string.Empty,
                WwwUserId = customerExt.WwwUserId ?? string.Empty,
                ExpDay = customerExt.ExpDay ?? string.Empty,
                Terms = customerExt.Terms ?? string.Empty,
                SocialSecurityNumber = customerExt.SocialSecNo ?? string.Empty,
                Category = customerExt.Category ?? string.Empty,
                MasterCustomerNumber = customerExt.MasterCustomerNumber,
                CustomerType = customerExt.CusType ?? string.Empty,
            };

            return customerForUpdate;
        }

        public IEnumerable<CustomerProperty> GetCustomerProperties(long customerNumber)
        {
            return _customerRepository.GetCustomerProperties(customerNumber).Select(GetProperty);
        }

        private CustomerProperty GetProperty(CustomerPropertyData property)
        {
            return new CustomerProperty
            {
                PropertyCode = property.PropertyCode,
                PropertyName = property.PropertyName,
                PropertyType = property.PropertyType
            };
        }
    }
}