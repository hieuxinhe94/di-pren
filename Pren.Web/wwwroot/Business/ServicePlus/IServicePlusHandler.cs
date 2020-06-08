using System;

namespace Pren.Web.Business.ServicePlus
{
    [Obsolete("Use ServicePlusFacade instead - Should be removed when old campaign is dead")]
    public interface IServicePlusHandler<out TUserOutput>
    {
        TUserOutput GetUserByToken(string token);

        string GetAutoRegisterUserUrl(string email, string firstName, string lastName, string phoneNumber, string productId, string callbackUrl);

        string GetAutoRegisterUserUrlForBizSubscription(string email, string firstName, string lastName, string phoneNumber, string callbackUrl);

        string GetProductId(string paperCode, string productNo);
    }
}
