using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.ServicePlus.Models;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class GetBizSubscriptionCountController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public GetBizSubscriptionCountController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public BizSubscriptionCount GetCount(string bizSubscriptionId)
        {
            return _servicePlusFacade.GetBizSubscriptionCount(bizSubscriptionId);
        }
    }
}
