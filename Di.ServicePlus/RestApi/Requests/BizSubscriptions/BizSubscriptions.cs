using System.Collections.Generic;
using System.Web.Script.Serialization;
using Di.Common.WebRequests;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriberActivation;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriberDelete;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriberInvitation;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscribers;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionCount;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionCreate;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionDefinition;
using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions;
using Di.ServicePlus.Utils;

namespace Di.ServicePlus.RestApi.Requests.BizSubscriptions
{
    internal class BizSubscriptions : RequestBase, IBizSubscriptions
    {
        public BizSubscriptions(string servicePlusApiUrl) : base(servicePlusApiUrl)
        {
        }

        public BizSubscriptions(string servicePlusApiUrl, IRequestService requestService) : base(servicePlusApiUrl, requestService)
        {
        }

        public BizSubscriptionCreateResponse CreateBizSubscription(
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
            string token)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                brandId = brandId,
                orgName = orgName,
                businessSubscriptionDefinitionId = businessSubscriptionDefinitionId,
                organizationNumber = organizationNumber,
                orderData = new
                {
                    email = email,
                    firstName = firstName,
                    lastName = lastName,
                    streetName = streetName,
                    streetNumber = streetNumber,
                    zipcode = zipCode,
                    city = city,
                    country = country,
                    campNo = campNo,
                    phoneNumber = phoneNumber
                } 
            });

            var response = CreateRequestWithToken(RequestVerb.Post, "bizsubscriptions", token, json);

            return response.ConvertServicePlusJsonToObject<BizSubscriptionCreateResponse>();
        }

        /// <summary>
        /// Return the subscriptions where a user is admin. 
        /// </summary>
        /// <param name="userId">User S+ id</param>
        /// <returns>Response object that contains the subscriptions where a user is admin</returns>
        public BizSubscriptionsResponse GetBizSubscriptions(string userId, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/with-user-admin", token, "", new Dictionary<string, string> {{"adminId", userId}});

            return responseJsonString.ConvertServicePlusJsonToObject<BizSubscriptionsResponse>();
        }

        public BizSubscriptionResponse GetBizSubscription(string bizSubscriptionId, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/" + bizSubscriptionId, token);

            return responseJsonString.ConvertServicePlusJsonToObject<BizSubscriptionResponse>();
        }

        /// <summary>
        /// Get all activated users connected to the subscription.
        /// </summary>
        /// <param name="bizSubscriptionId">Business subscription id</param>
        /// <returns>Response object that contains the actived subscribers connected to a subscription</returns>
        public BizSubscribersResponse GetActiveBizSubscribers(string bizSubscriptionId, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/" + bizSubscriptionId + "/bizsubscribers", token);

            return responseJsonString.ConvertServicePlusJsonToObject<BizSubscribersResponse>();
        }

        /// <summary>
        /// Get all pending (waiting to get activated) users connected to the subscription.
        /// </summary>
        /// <param name="bizSubscriptionId">Business subscription id</param>
        /// <returns>Response object that contains the pending subscribers connected to a subscription</returns>
        public PendingBizSubscribersResponse GetPendingBizSubscribers(string bizSubscriptionId, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/" + bizSubscriptionId + "/invites", token);

            return responseJsonString.ConvertServicePlusJsonToObject<PendingBizSubscribersResponse>();
        }

        /// <summary>
        /// S+ endpoint POST bizsubscriptions/{bizSubscriptionId}/invites/
        /// </summary>
        /// <param name="bizSubscriptionId"></param>
        /// <param name="email"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public BizSubscriberInvitationResponse SendBizSubscriptionInvitationMail(string bizSubscriptionId, string email, string appId, string token)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                email = email
            });

            var response = CreateRequestWithToken(RequestVerb.Post, "bizsubscriptions/" + bizSubscriptionId + "/invites/", token, json);

            return response.ConvertServicePlusJsonToObject<BizSubscriberInvitationResponse>();
        }

        /// <summary>
        /// S+ endpoint GET /bizsubscriptions/{id}/invites/{sub_account_request_id}/send-reminder
        /// </summary>
        public BizSubscriberInvitationResponse SendBizSubscriptionInvitationReminderMail(string bizSubscriptionId, string code, string token)
        {
            var response = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/" + bizSubscriptionId + "/invites/" + code + "/send-reminder", token);

            return response.ConvertServicePlusJsonToObject<BizSubscriberInvitationResponse>();
        }

        public BizSubscriberActivationResponse ActivateBizSubscriberWithCode(string bizSubscriptionId, string code, string token)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                subUserId = ""
            });

            var response = CreateRequestWithToken(RequestVerb.Post, "bizsubscriptions/" + bizSubscriptionId + "/bizsubscriber-with-code/" + code, token, json);

            return response.ConvertServicePlusJsonToObject<BizSubscriberActivationResponse>();
        }

        public MarkBizSubscriberForRemovalResponse MarkBizSubscriberForRemoval(string bizSubscriptionId, string userId, bool markForRemoval, string token)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                markForRemoval = markForRemoval
            });

            var resonseJsonString = CreateRequestWithToken(RequestVerb.Put, "bizsubscriptions/" + bizSubscriptionId + "/bizsubscribers/" + userId, token, json);

            return resonseJsonString.ConvertServicePlusJsonToObject<MarkBizSubscriberForRemovalResponse>();
        }

        public BizPendingSubscriberDeleteResponse DeletePendingBizSubscriber(string bizSubscriptionId, string code, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Delete, "bizsubscriptions/" + bizSubscriptionId + "/invites/" + code, token);

            return responseJsonString.ConvertServicePlusJsonToObject<BizPendingSubscriberDeleteResponse>();
        }

        public BizSubscriptionDefinitonResponse GetBizSubscriptionDefinition(string bizSubscriptionDefinitionId, string token)
        {
            var resonseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptiondefs/" + bizSubscriptionDefinitionId, token);

            return resonseJsonString.ConvertServicePlusJsonToObject<BizSubscriptionDefinitonResponse>();
        }

        public BizSubscriptionDefinitonsResponse GetBizSubscriptionDefinitions(string token)
        {
            var resonseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptiondefs/", token);

            return resonseJsonString.ConvertServicePlusJsonToObject<BizSubscriptionDefinitonsResponse>();
        }

        public BizSubscriptionDefinitonsResponse GetBizSubscriptionDefinitionsByProductId(string productId, string token)
        {
            var resonseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptiondefs/", token, string.Empty, new Dictionary<string, string>{{"q","productId_s:" + productId}});

            return resonseJsonString.ConvertServicePlusJsonToObject<BizSubscriptionDefinitonsResponse>();
        }

        /// <summary>
        /// S+ endpoint GET bizsubscriptions/{bizSubscriptionId}/bizsubscribers/count
        /// </summary>
        /// <param name="bizSubscriptionId"></param>
        /// <returns></returns>
        public BizSubscriptionCountResponse GetBizSubscriptionCount(string bizSubscriptionId, string token)
        {
            var resonseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/" + bizSubscriptionId + "/bizsubscribers/count", token);

            return resonseJsonString.ConvertServicePlusJsonToObject<BizSubscriptionCountResponse>();
        }

        public InvitedBizSubscriberResponse GetInvitedBizSubscriber(string bizSubscriptionId, string code, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "bizsubscriptions/" + bizSubscriptionId + "/invites/" + code, token);

            return responseJsonString.ConvertServicePlusJsonToObject<InvitedBizSubscriberResponse>();
        }

        public BizSubscriberActivationResponse ActiveInviteByEmail(string bizSubscriptionId, string email, string token)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                email = email
            });

            var response = CreateRequestWithToken(RequestVerb.Post, "bizsubscriptions/" + bizSubscriptionId + "/bizsubscribers/attach/", token, json);

            return response.ConvertServicePlusJsonToObject<BizSubscriberActivationResponse>();
        }
    }
}