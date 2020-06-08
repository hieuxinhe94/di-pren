using System;
using Di.ServicePlus.RestApi.ResponseModels.User;

namespace Di.ServicePlus.RestApi.Requests.Users
{
    public interface IUsers
    {
        UserResponse GetUser(string token);
        
        UserResponse GetUserById(string userId, string token);

        CreateUserResponse CreateUser(
            string email,
            string phone,
            string firstName,
            string lastName,
            string password,
            string type,
            string brandId,
            string active,
            string termsAndConditionsAccepted,
            string token);

        SendActivationPasswordResponse SendActivationPassword(string userId, string token);

        SearchUserResponse SearchUserByEmail(string email, string token);

        [Obsolete("EndPoint not completed by S+", true)]
        UsersBipResponse FindUsersByEmail(string email, string token, string appId);

        OrderFlowMessageResponse OrderFlowMessage(string email, bool isLoggedIn, string token, string brandId);

        UpdateUserResponse UpdateUser(User userToUpdate, string token);
    }
}