using Di.ServicePlus.RedirectApi.Orders;

namespace Di.ServicePlus.RedirectApi
{
    public interface IRedirectHandler
    {
        IOrders Orders { get; }

        string GetCheckedLoginUrl(string callBackUrl);
        string GetLoginUrl(string callBackUrl);
        string GetLogoutUrl(string callBackUrl);
        string GetCreateAccountUrl(string callBackUrl);
    }
}