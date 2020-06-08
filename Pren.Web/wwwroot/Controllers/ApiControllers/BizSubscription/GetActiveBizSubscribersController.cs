using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.ServicePlus.Models;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class GetActiveBizSubscribersController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public GetActiveBizSubscribersController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public IEnumerable<BizSubscriber> GetSubscribers(string bizSubscriptionId, int skip, int take)
        {
            var subscribers = _servicePlusFacade.GetActiveBizSubscribers(bizSubscriptionId).Skip(skip);

            return take > 0 ? subscribers.Take(take) : subscribers;
        }
    }
}
