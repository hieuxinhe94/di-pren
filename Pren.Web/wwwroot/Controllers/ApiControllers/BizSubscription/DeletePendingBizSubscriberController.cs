using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class DeletePendingBizSubscriberController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public DeletePendingBizSubscriberController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public bool DeletePending(string bizSubscriptionId, string code)
        {
            return _servicePlusFacade.DeletePendingBizSubscriber(bizSubscriptionId, code);
        }
    }
}
