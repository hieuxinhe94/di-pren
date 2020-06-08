using Moq;
using Pren.Web.Business.Subscription;


namespace Pren.Web.UnitTests.Fakes.Subscription

{
    public class FakeSubscriptionFactory
    {

        public static Subscriber FakeSubscriber()
        {
            return new Subscriber();
        }

        public static UserSubscription FakeSubscription(SubscriptionType type = SubscriptionType.Private)
        {
            return new UserSubscription{Type = type};
        }

        public static Mock<ISubscriberFacade> FakeSubscriberFacade()
        {
            var fakeSubscriberFacade = new Mock<ISubscriberFacade>();


            return fakeSubscriberFacade;
        }

    }
}
