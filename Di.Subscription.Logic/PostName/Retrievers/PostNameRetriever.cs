using Di.Subscription.DataAccess.PostName;

namespace Di.Subscription.Logic.PostName.Retrievers
{
    public class PostNameRetriever : IPostNameRetriever
    {
        private readonly IPostNameRepository _postNameRepository;

        public PostNameRetriever(IPostNameRepository postNameRepository)
        {
            _postNameRepository = postNameRepository;
        }

        public string GetPostName(string zipCode)
        {
            return _postNameRepository.GetPostName(zipCode, "SE");
        }
    }
}