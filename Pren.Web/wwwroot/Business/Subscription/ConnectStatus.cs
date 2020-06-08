
namespace Pren.Web.Business.Subscription
{
    public enum ConnectStatus
    {
        ConnectExistingPrenWithServicePlus,
        ConnectExistingServicePlusWithPren,
        ConnectExistingServicePlusWithExistingPren,
        UnableToConnectPrenWithServicePlus,
        InvalidCode,
        IsConnected,
        NothingToConnect
    }
}