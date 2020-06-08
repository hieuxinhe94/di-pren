using Di.Subscription.DataAccess.Customer;

namespace Di.Subscription.Logic.Customer.Modifiers
{   
    public class CustomerPropertyChanger : ICustomerPropertyChanger
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerPropertyChanger(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public bool InsertUpdateCustomerProperty(long customerNumber, string propertyCode, string propertyValue)
        {
            var result = _customerRepository.InsertCustomerProperty(customerNumber, propertyCode, propertyValue, string.Empty, string.Empty);

            return result == "OK";
        }
    }
}