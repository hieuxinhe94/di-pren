using System.Linq;
using System.Web;
using System.Web.Http;
using Pren.Web.Business.Subscription;

namespace Pren.Web.Business.Controllers
{
    public abstract class ExtendedApiController : ApiController, IApiController
    {
        private readonly IApiReferrerCheck _apiReferrerCheck;

        protected ExtendedApiController(IApiReferrerCheck apiReferrerCheck)
        {
            _apiReferrerCheck = apiReferrerCheck;
        }

        public bool VerifyDomain()
        {
            var context = HttpContext.Current.Request.UrlReferrer;
            return context != null && _apiReferrerCheck.VerifyDomain(context.Host);
        }

        public bool VerifySubscriber(Subscriber subscriber, long subscriptionId = 0)
        {
            var subscriberExists = subscriber != null;

            if (subscriptionId < 1) return subscriberExists;

            var subscriptionExist = subscriberExists && subscriber.SelectedSubscription != null;
            var isMySubscription = subscriptionExist &&
                                   subscriber.SelectedSubscription.SubscriptionItems.Any(
                                       sub => sub.SubscriptionNumber == subscriptionId);

            return isMySubscription;
        }
    }
}