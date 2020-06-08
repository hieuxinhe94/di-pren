namespace Pren.Web.Business.Subscription
{
    public class ConnectService: IConnectService
    {
        public ConnectStatus GetConnectStatus(Subscriber subscriber)
        {  
            if (subscriber == null)
            {
                return ConnectStatus.NothingToConnect;
            }
            if (subscriber.ServicePlusUser == null)
            {
                return subscriber.SelectedSubscription != null
                    ? ConnectStatus.ConnectExistingPrenWithServicePlus
                    : ConnectStatus.NothingToConnect;   
            }
            if (subscriber.SelectedSubscription == null)
            {
                return ConnectStatus.ConnectExistingServicePlusWithPren;
            }
            if (subscriber.SelectedSubscription.IsConnected)
            {
                return ConnectStatus.IsConnected;
            }                

            if (subscriber.SelectedSubscription.HasMultipleCustomerNumbers || subscriber.SelectedSubscription.Type == SubscriptionType.Business)
            {
                return ConnectStatus.UnableToConnectPrenWithServicePlus;   
            }

            return ConnectStatus.ConnectExistingServicePlusWithExistingPren;
        }
    }
}