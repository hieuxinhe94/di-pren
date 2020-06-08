using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Di.Common.WebRequests;
using Di.ServicePlus.RestApi.ResponseModels.User;
using Di.ServicePlus.Utils;

namespace Di.ServicePlus.RestApi.Requests.Users
{
    internal class Users : RequestBase, IUsers
    {
        public Users(string servicePlusApiUrl) : base(servicePlusApiUrl)
        {
        }

        public Users(string servicePlusApiUrl, IRequestService requestService)
            : base(servicePlusApiUrl, requestService)
        {
        }

        public UserResponse GetUser(string token)
        {
            var userResponse = new UserResponse();

            if (string.IsNullOrEmpty(token))
            {
                return userResponse;
            }

            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "users/current", token);

            return responseJsonString.ConvertServicePlusJsonToObject<UserResponse>();
        }

        public UserResponse GetUserById(string userId, string token)
        {
            var userResponse = new UserResponse();

            if (string.IsNullOrEmpty(userId))
            {
                return userResponse;
            }

            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "users/" + userId, token);
            return responseJsonString.ConvertServicePlusJsonToObject<UserResponse>();
        }

        public CreateUserResponse CreateUser(
            string email, 
            string phone, 
            string firstName, 
            string lastName, 
            string password, 
            string type, 
            string brandId, 
            string active, 
            string termsAndConditionsAccepted,
            string token)
        {
            var json = new JavaScriptSerializer().Serialize(new
            {
                email = email,
                phoneNumber = phone,
                firstName = firstName,
                lastName = lastName,
                password = password,
                type = type,
                brandId = brandId,
                active = active,
                termsAndConditionsAccepted = termsAndConditionsAccepted
            });

            var responseJsonString = CreateRequestWithToken(RequestVerb.Post, "users/", token, json);

            return responseJsonString.ConvertServicePlusJsonToObject<CreateUserResponse>();
        }

        public SendActivationPasswordResponse SendActivationPassword(string userId, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Post, "users/" + userId + "/send-activation-password", token);

            return responseJsonString.ConvertServicePlusJsonToObject<SendActivationPasswordResponse>();
        }

        public SearchUserResponse SearchUserByEmail(string email, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "users/", token, "", new Dictionary<string, string> { { "q", "email_s:" + email } });

            return responseJsonString.ConvertServicePlusJsonToObject<SearchUserResponse>();
        }

        [Obsolete("EndPoint not completed by S+", true)]
        public UsersBipResponse FindUsersByEmail(string email, string token, string appId)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "users/find-by-email", token, "", new Dictionary<string, string> { { "email", email }, { "appId", appId } });

            return responseJsonString.ConvertServicePlusJsonToObject<UsersBipResponse>();
        }

        public OrderFlowMessageResponse OrderFlowMessage(string email, bool isLoggedIn, string token, string brandId)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "users/orderflowmessage", token, "", new Dictionary<string, string> { { "email", email }, { "isLoggedIn", isLoggedIn.ToString().ToLower() }, { "brandId", brandId } });

            return responseJsonString.ConvertServicePlusJsonToObject<OrderFlowMessageResponse>();
        }

        public UpdateUserResponse UpdateUser(User userToUpdate, string token)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(userToUpdate);

            var responseJsonString = CreateRequestWithToken(RequestVerb.Put, "users/current", token, json);

            return responseJsonString.ConvertServicePlusJsonToObject<UpdateUserResponse>();
        }
    }
}