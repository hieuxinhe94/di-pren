using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.ServicePlus.Models;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class GetPendingBizSubscribersController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public GetPendingBizSubscribersController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public IEnumerable<PendingBizSubscriber> GetSubscribers(string bizSubscriptionId, int skip, int take)
        {
            var subscribers = _servicePlusFacade.GetPendingBizSubscribers(bizSubscriptionId).Skip(skip);

            return take > 0 ? subscribers.Take(take) : subscribers;
        }
    }
}
