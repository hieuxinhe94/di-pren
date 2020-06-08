namespace Di.Subscription.DataAccess.PostName
{
    public interface IPostNameRepository
    {
        string GetPostName(string zipCode, string countryCode);
    }
}