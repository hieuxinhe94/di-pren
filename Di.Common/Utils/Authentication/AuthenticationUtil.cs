using System;
using System.Collections.Generic;
using System.Text;

namespace Di.Common.Utils.Authentication
{
    public class AuthenticationUtil
    {
        public static KeyValuePair<string, string> GetBasicAuthenticationHeader(string userName, string password)
        {
            var svcCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));

            return new KeyValuePair<string, string>("Authorization", "Basic " + svcCredentials);
        }
    }
}
