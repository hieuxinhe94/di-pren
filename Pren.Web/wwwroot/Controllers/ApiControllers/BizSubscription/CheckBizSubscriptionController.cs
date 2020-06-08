using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class CheckBizSubscriptionController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public CheckBizSubscriptionController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public bool Get(string userId)
        {            
            return _servicePlusFacade.HasBizSubscription(userId);
        }
    }
}
