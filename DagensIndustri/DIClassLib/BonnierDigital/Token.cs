using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Configuration;
namespace DIClassLib.BonnierDigital
{
    public class Token
    {
        public List<KeyVal> RequestKV
        {
            get
            {
                List<KeyVal> urlKeyVals = new List<KeyVal>();
                urlKeyVals.Add(new KeyVal("client_id", ConfigurationManager.AppSettings["BonDigClientId"].ToString()));
                urlKeyVals.Add(new KeyVal("client_secret", ConfigurationManager.AppSettings["BonDigClientSecret"].ToString()));
                urlKeyVals.Add(new KeyVal("grant_type", "client_credentials"));
                return urlKeyVals;
            }
        }
        public DateTime Expires = DateTime.MinValue;

        public string httpResponseCode;
        public string requestId;
        public string access_token;
        public string token_type;
        public string expires_in;

        public Token() { }
    }
}
