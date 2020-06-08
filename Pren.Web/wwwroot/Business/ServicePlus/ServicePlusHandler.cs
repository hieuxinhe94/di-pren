using System;
using System.Collections.Generic;
using System.Globalization;
using DIClassLib.BonnierDigital;
using Pren.Web.Business.Configuration;
using HttpUtility = System.Web.HttpUtility;
using Settings = DIClassLib.Misc.Settings;

namespace Pren.Web.Business.ServicePlus
{
    [Obsolete("Use ServicePlusFacade instead - Should be removed when old campaign is dead")]
    public class ServicePlusHandler : IServicePlusHandler<UserOutput>
    {
        private readonly ISiteConfiguration _siteConfiguration;

        public ServicePlusHandler(
            ISiteConfiguration siteConfiguration)
        {
            _siteConfiguration = siteConfiguration;
        }

        public UserOutput GetUserByToken(string token)
        {
            return RequestHandler.GetUserByToken(token);
        }

        public string GetAutoRegisterUserUrl(string email, string firstName, string lastName, string phoneNumber, string productId, string callbackUrl)
        {
            var secretKey = _siteConfiguration.GetSetting(SettingConstants.BonDigSecretKey);

            var timeStamp = long.Parse(BonDigMisc.GetMsSince1970(DateTime.Now));

            var paramValues = new Dictionary<string, string>
            {
                {"firstName", firstName},
                {"lastName", lastName},
                {"email", email},
                {"phoneNumber", phoneNumber},
                {"appId", _siteConfiguration.GetSetting(SettingConstants.BonDigAppIdDagInd)},
                {"productId", productId},
                {"currency", "SEK"},
                {"price", "1"},
                {"ts", timeStamp.ToString(CultureInfo.InvariantCulture)},
                {"s", string.Empty},
                {"active", "0"},
                {"callback", callbackUrl}
            };

            var signature = ServicePlusUtility.Sign(secretKey, timeStamp, paramValues);

            paramValues["s"] = signature;

            var urlParams = string.Empty;

            foreach (var paramValue in paramValues)
            {
                urlParams += "&" + paramValue.Key + "=" + HttpUtility.UrlEncode(paramValue.Value);
            }

            urlParams = "?" + urlParams.TrimStart('&');

            var url = _siteConfiguration.GetSetting(SettingConstants.BonDigUrlAccount) + _siteConfiguration.GetSetting(SettingConstants.BonDigAutoRegister) + urlParams;

            return url;
        }

        public string GetAutoRegisterUserUrlForBizSubscription(string email, string firstName, string lastName, string phoneNumber, string callbackUrl)
        {
            var secretKey = _siteConfiguration.GetSetting(SettingConstants.BonDigSecretKey);

            var timeStamp = long.Parse(BonDigMisc.GetMsSince1970(DateTime.Now));

            var paramValues = new Dictionary<string, string>
            {
                {"firstName", firstName},
                {"lastName", lastName},
                {"email", email},
                {"phoneNumber", phoneNumber},
                {"appId", _siteConfiguration.GetSetting(SettingConstants.BonDigAppIdDagInd)},
                //{"productId", productId},
                {"customMasterCreationEmail", "1"},
                {"currency", "SEK"},
                {"price", "1"},
                {"ts", timeStamp.ToString(CultureInfo.InvariantCulture)},
                {"s", string.Empty},
                {"active", "0"},
                {"callback", callbackUrl}
            };

            var signature = ServicePlusUtility.Sign(secretKey, timeStamp, paramValues);

            paramValues["s"] = signature;

            var urlParams = string.Empty;

            foreach (var paramValue in paramValues)
            {
                urlParams += "&" + paramValue.Key + "=" + HttpUtility.UrlEncode(paramValue.Value);
            }

            urlParams = "?" + urlParams.TrimStart('&');

            var url = _siteConfiguration.GetSetting(SettingConstants.BonDigUrlAccount) + _siteConfiguration.GetSetting(SettingConstants.BonDigAutoRegister) + urlParams;

            return url;
        }

        public string GetProductId(string paperCode, string productNo)
        {
            return Settings.GetBonDigProductId(paperCode, productNo);
        }
    }
}
