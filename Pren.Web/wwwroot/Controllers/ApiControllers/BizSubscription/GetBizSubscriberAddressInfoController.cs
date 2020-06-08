using System.Web.Http;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Customer.Types;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class GetBizSubscriberAddressInfoController : ApiController
    {
        private readonly ICustomerHandler _customerHandler;

        public GetBizSubscriberAddressInfoController(ICustomerHandler customerHandler)
        {
            _customerHandler = customerHandler;
        }

        [HttpGet]
        public Customer Get(long customerNumber)
        {
            return _customerHandler.GetCustomer(customerNumber);
        }
    }
}
