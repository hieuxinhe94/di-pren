using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class MarkActiveBizSubscriberForRemovalController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public MarkActiveBizSubscriberForRemovalController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public bool MarkForRemoval(string bizSubscriptionId, string userId, bool markForRemoval)
        {
            return _servicePlusFacade.MarkActiveBizSubscriberForRemoval(bizSubscriptionId, userId, markForRemoval);
        }
    }
}
