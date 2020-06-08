namespace Pren.Web.Business.Subscription
{
    public interface IConnectService
    {
        ConnectStatus GetConnectStatus(Subscriber subscriber);
    }
}