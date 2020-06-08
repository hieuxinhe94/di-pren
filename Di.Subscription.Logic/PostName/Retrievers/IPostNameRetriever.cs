namespace Di.Subscription.Logic.PostName.Retrievers
{
    public interface IPostNameRetriever
    {
        string GetPostName(string zipCode);
    }
}