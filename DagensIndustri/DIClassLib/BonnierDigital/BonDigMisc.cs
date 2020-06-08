using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.Misc;
using Microsoft.VisualBasic;
using System.Configuration;
using System.Web;
using DIClassLib.DbHelpers;


namespace DIClassLib.BonnierDigital
{
    public static class BonDigMisc
    {

        public static string GetMsSince1970(DateTime dtEnd)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long act = long.Parse((DateAndTime.DateDiff(DateInterval.Second, dtStart, dtEnd) * 1000).ToString());
            return act.ToString();   
        }

        public static string BonDigLoginRetHandlerPath { get { return ConfigurationManager.AppSettings["BonDigLoginRetHandler"]; } }
        private static string BonDigUrlAccount { get { return ConfigurationManager.AppSettings["BonDigUrlAccount"]; } }
        private static string AppIdAndLc { get { return "?appId=" + ConfigurationManager.AppSettings["BonDigAppIdDagInd"] + "&lc=sv"; } }

        

        /// <summary>
        /// Returns: urlToBonDigCheckLoginPage?appId=... lc=... callback=.. (ReturnUrl=..)
        /// </summary>
        public static string GetCheckLoggedInUrl(string callBack, string returnUrl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BonDigUrlAccount);
            sb.Append(ConfigurationManager.AppSettings["BonDigCheckLoginPage"]);
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBack));
            sb.Append(TryAddReturnUrl(callBack, returnUrl));
            return sb.ToString();
        }

        public static string GetForgotPasswordUrl()
        {
            return MiscFunctions.GetAppsettingsValue("BonDigForgotPasswordUrl");
        }

        /// <summary>
        /// Returns: urlToBonDigLogoutPage?appId=... lc=... callback=..
        /// </summary>
        public static string GetLogoutUrl(string callBack)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BonDigUrlAccount);
            sb.Append(ConfigurationManager.AppSettings["BonDigLogoutPage"]);
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBack));
            return sb.ToString();
        }

        /// <summary>
        /// Returns: urlToBonDigLoginPage?appId=.. lc=.. callback=.. (ReturnUrl=..)
        /// </summary>
        public static string GetLoginUrl(string callBack, string returnUrl)
        {
            //...
            StringBuilder sb = new StringBuilder();
            sb.Append(BonDigUrlAccount);
            sb.Append(ConfigurationManager.AppSettings["BonDigLoginPage"]);
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBack));
            sb.Append(TryAddReturnUrl(callBack, returnUrl));
            return sb.ToString();
        }

        /// <summary>
        /// Returns: urlToBonDigCreateAccountPage?appId=.. lc=.. callback=.. (ReturnUrl=..)
        /// </summary>
        public static string GetCreateAccountUrl(string callBack, string returnUrl)
        {
            //...
            StringBuilder sb = new StringBuilder();
            sb.Append(BonDigUrlAccount);
            sb.Append(ConfigurationManager.AppSettings["BonDigCreateAccountPage"]);
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBack));
            sb.Append(TryAddReturnUrl(callBack, returnUrl));
            return sb.ToString();
        }


        public static string GetVerifyEntitlementUrl(string externalResourceId, string token)
        {
            return ConfigurationManager.AppSettings["BonDigVerifyEntitlement"] + "?externalResourceId=" + externalResourceId + "&access_token=" + token;
        }

        private static string TryAddReturnUrl(string callBack, string returnUrl)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = returnUrl.Replace("token=&", "");
                returnUrl = returnUrl.Replace("remembered=false&", "");
                returnUrl = returnUrl.Replace("firstName=&", "");
                returnUrl = returnUrl.Replace("lastName=&", "");

                if (!callBack.Contains("?"))
                    sb.Append(HttpUtility.UrlEncode("?"));
                else
                    sb.Append(HttpUtility.UrlEncode("&"));

                sb.Append(HttpUtility.UrlEncode("ReturnUrl=" + returnUrl));
            }
            return sb.ToString();
        }

        

    }
}
