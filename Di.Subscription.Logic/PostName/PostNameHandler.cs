using Di.Subscription.Logic.PostName.Retrievers;

namespace Di.Subscription.Logic.PostName
{
    public class PostNameHandler : IPostNameHandler
    {
        private readonly IPostNameRetriever _postNameRetriever;

        public PostNameHandler(IPostNameRetriever postNameRetriever)
        {
            _postNameRetriever = postNameRetriever;
        }

        public string GetPostName(string zipCode)
        {
            return _postNameRetriever.GetPostName(zipCode);
        }
    }
}