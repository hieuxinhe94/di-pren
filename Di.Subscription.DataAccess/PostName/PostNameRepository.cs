using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.PostName
{
    internal class PostNameRepository : IPostNameRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;

        public PostNameRepository(ISubscriptionDataAccess subscriptionDataAccess)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
        }

        public string GetPostName(string zipCode, string countryCode)
        {
            return _subscriptionDataAccess.GetPostName(zipCode, countryCode);
        }
    }
}