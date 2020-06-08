using System.Web.Http;
using Pren.Web.Business.ServicePlus.Logic;

namespace Pren.Web.Controllers.ApiControllers.BizSubscription
{
    public class InviteBizSubscriberController : ApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;

        public InviteBizSubscriberController(IServicePlusFacade servicePlusFacade)
        {
            _servicePlusFacade = servicePlusFacade;
        }

        [HttpGet]
        public bool Invite(string bizSubscriptionId, string email)
        {
            return _servicePlusFacade.InviteBizSubscriberByEmail(bizSubscriptionId, email);
        }

        [HttpGet]
        public bool Remind(string bizSubscriptionId, string code)
        {
            return _servicePlusFacade.RemindInvitedSubscriberByEmail(bizSubscriptionId, code);
        }
    }
}
