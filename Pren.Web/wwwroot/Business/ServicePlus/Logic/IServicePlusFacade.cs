using System;
using System.Collections.Generic;
using Pren.Web.Business.ServicePlus.Models;

namespace Pren.Web.Business.ServicePlus.Logic
{
    public interface IServicePlusFacade
    {
        User CreateUser(string email, string phone, string firstName, string lastName, string password, bool sendActivationMail = true);
        User GetUserById(string id);
        User GetUserByToken(string token);
        User GetUserByEmail(string email);
        [Obsolete("EndPoint not completed by S+", true)]
        User FindUserByEmail(string email);
        OrderFlowMessage GetOrderFlowMessage(string email, bool isLoggedIn);
        bool UpdateUser(string token, string firstName, string lastName, string phoneNumber, string email);

        IEnumerable<BizSubscriber> GetActiveBizSubscribers(string bizSubscriptionId);
        IEnumerable<PendingBizSubscriber> GetPendingBizSubscribers(string bizSubscriptionId);
 
        BizSubscriber GetBizSubscriberByInviteCode(string bizSubscriptionId, string inviteCode);
        bool ActivateBizSubscriber(string bizSubscriptionId, string email);
        bool DeletePendingBizSubscriber(string bizSubscriptionId, string code);
        bool MarkActiveBizSubscriberForRemoval(string bizSubscriptionId, string userId, bool markForRemoval);
        bool InviteBizSubscriberByEmail(string bizSubscriptionId, string email);
        bool RemindInvitedSubscriberByEmail(string bizSubscriptionId, string code);

        IEnumerable<BizSubscriptionDefinition> GetBizSubscriptionDefinitions();

        IEnumerable<BizSubscriptionDefinition> GetBizSubscriptionDefinitionsByProductId(string productId);

        BizSubscriptionDefinition GetBizSubscriptionDefinition(string definitionId);

        bool CreateBizSubscription(
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
            string campNo,
            string phoneNumber);

        IEnumerable<BizSubscription> GetBizSubscriptions(string userId);

        BizSubscription GetBizSubscription(string bizSubscriptionId);

        BizSubscriptionCount GetBizSubscriptionCount(string bizSubscriptionId);

        Entitlement GetEntitlement(string entitlementId);
        IEnumerable<Entitlement> GetEntitlements(string userId);

        IEnumerable<ExternalId> GetExternalIds(string token);
        IEnumerable<ExternalId> GetFilteredExternalIds(string token);
        bool VerifyEntitlement(string externalResourceId, string token);

        bool CreateOrUpdateOffer(string userToken, string offerOrigin);

        bool HasBizSubscription(string userId);

        bool ImportEntitlement(string userId, long externalSubscriberId, long externalSubscriptionId,
            string paperCode, string productNumber);
    }
}
