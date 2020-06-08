using Di.ServicePlus.RestApi.ResponseModels.BizSubscriberActivation;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriberDelete;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriberInvitation;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscribers;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionCount;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionCreate;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionDefinition;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions;

namespace Di.ServicePlus.RestApi.Requests.BizSubscriptions
{
    public interface IBizSubscriptions
    {
        BizSubscriptionCreateResponse CreateBizSubscription(
           string brandId,
           string orgName,
           string businessSubscriptionDefinitionId,
           string organizationNumber,
           string email,
           string firstName,
           string lastName,
           string streetName,
           string streetNumber,
           string zipCode,
           string city,
           string country,
           string campNo,
           string phoneNumber,
           string token);

        BizSubscriptionsResponse GetBizSubscriptions(string userId, string token);
        BizSubscriptionResponse GetBizSubscription(string bizSubscriptionId, string token);
        BizSubscribersResponse GetActiveBizSubscribers(string bizSubscriptionId, string token);
        PendingBizSubscribersResponse GetPendingBizSubscribers(string bizSubscriptionId, string token);
        BizSubscriberInvitationResponse SendBizSubscriptionInvitationMail(string bizSubscriptionId, string email, string appId, string token);
        BizSubscriberInvitationResponse SendBizSubscriptionInvitationReminderMail(string bizSubscriptionId, string code, string token);
        BizSubscriberActivationResponse ActivateBizSubscriberWithCode(string bizSubscriptionId, string code, string token);
        MarkBizSubscriberForRemovalResponse MarkBizSubscriberForRemoval(string bizSubscriptionId, string userId, bool markForRemoval, string token);
        BizPendingSubscriberDeleteResponse DeletePendingBizSubscriber(string bizSubscriptionId, string code, string token);
        BizSubscriptionDefinitonResponse GetBizSubscriptionDefinition(string bizSubscriptionDefinitionId, string token);
        BizSubscriptionDefinitonsResponse GetBizSubscriptionDefinitions(string token);
        BizSubscriptionDefinitonsResponse GetBizSubscriptionDefinitionsByProductId(string productId, string token);
        BizSubscriptionCountResponse GetBizSubscriptionCount(string bizSubscriptionId, string token);
        InvitedBizSubscriberResponse GetInvitedBizSubscriber(string bizSubscriptionId, string code, string token);
        BizSubscriberActivationResponse ActiveInviteByEmail(string bizSubscriptionId, string email, string token);
    }
}