using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web;


namespace DIClassLib.BonnierDigital
{
    public class Entitlement
    {
        public Entitlement() { }

        public EntitlementOutput CreateEntitlement(string productId, string userId, string externalSubscriberId, string externalSubscriptionId, string validFrom, string validTo)
        {
            EntitlementInput entIn = new EntitlementInput(productId, userId, externalSubscriberId, externalSubscriptionId, validFrom, validTo);

            string json = RequestHandler.WebReqPostJson(ConfigurationManager.AppSettings["BonDigUrlCreateEntitlement"].ToString(), entIn.ToJson());
            //string json = RequestHandler.WebReqPostParam(ConfigurationManager.AppSettings["BonDigUrlCreateEntitlement"].ToString(), entIn.ToKeyValParams());
            
            EntitlementOutput entOut = new EntitlementOutput();
            entOut = entOut.GetEntitlementOutput(json);
            return entOut;
        }
    }


    public class EntitlementInput
    {
        public string brandId = ConfigurationManager.AppSettings["BonDigBrandId"];
        public string productId;
        public string userId;
        public string renewable;
        public string type;
        public string state;
        public List<string> productTags;
        public string validFrom;
        public string validTo;
        public string externalSubscriptionId;
        public string externalSubscriberId;


        //public EntitlementInput(string brandId_, string productId_, string userId_, string externalSubscriberId_, string externalSubscriptionId_, string _validFrom, string _validTo)
        public EntitlementInput(string productId_, string userId_, string externalSubscriberId_, string externalSubscriptionId_, string _validFrom, string _validTo)
        {
            //brandId = brandId_;
            productId = productId_;
            userId = userId_;
            renewable = "false";
            type = "TIME_BASED_SUBSCRIPTION";
            state = "VALID";

            //todo: think about this before using the Entitlement class...
            if (productId == ConfigurationManager.AppSettings["BonDigProductIdDiWeekend"])
                productTags = new List<string>() { "DIWEEKEND" };
            else
                productTags = new List<string>() { "DIPAPER", "DITABLET" };
            
            validFrom = _validFrom;
            validTo = _validTo;
            externalSubscriptionId = externalSubscriptionId_;
            externalSubscriberId = externalSubscriberId_;
        }

        public string ToJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string s = js.Serialize(this);
            return s;
        }

        //public string ToKeyValParams()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("brandId=" + HttpUtility.UrlEncode(brandId));
        //    sb.Append("&productId=" + HttpUtility.UrlEncode(productId));
        //    sb.Append("&userId=" + HttpUtility.UrlEncode(userId));
        //    sb.Append("&renewable=" + HttpUtility.UrlEncode(renewable));
        //    sb.Append("&type=" + HttpUtility.UrlEncode(type));
        //    sb.Append("&state=" + HttpUtility.UrlEncode(state));
        //    //sb.Append("&productTags=" + HttpUtility.UrlEncode("[" + productTags[0] + "]"));
        //    sb.Append("&productTags=" + HttpUtility.UrlEncode(productTags[0]));
        //    sb.Append("&validFrom=" + HttpUtility.UrlEncode(validFrom));
        //    sb.Append("&validTo=" + HttpUtility.UrlEncode(validTo));
        //    sb.Append("&externalSubscriptionId=" + HttpUtility.UrlEncode(externalSubscriptionId));
        //    sb.Append("&externalSubscriberId=" + HttpUtility.UrlEncode(externalSubscriberId));
        //    return sb.ToString();
        //}
    }


    public class EntitlementOutput
    {
        public string httpResponseCode;
        public string requestId;
        public EntitlementOutputData entitlement;

        public EntitlementOutput() { }

        public EntitlementOutput GetEntitlementOutput(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            EntitlementOutput eo = js.Deserialize<EntitlementOutput>(json);
            return eo;
        }
    }


    public class EntitlementOutputData
    {
        public string id;
        public string created;
        public string updated;
        public string location;
        public string brandId;
        public string productId;
        public string userId;
        public string renewable;
        public string type;
        public string state;
        public string productTags;
        public string validFrom;
        public string validTo;
        public string externalSubscriptionId;
        public string externalSubscriberId;

        public EntitlementOutputData() { }

        #region return from bon dig
        //{
        //"@httpResponseCode":"201",
        //"@requestId":"5T1DBhKfDNUqfgzA9v7jcx",
        //"entitlement":
        //    {
        //    "id":"1sjQvgKIJ6vt5O9qHtQTHs",
        //    "created":"1334328554391",
        //    "updated":"1334328554391",
        //    "location":"/1sjQvgKIJ6vt5O9qHtQTHs",
        //    "brandId":"5DuzcZz0j8u0zArSNzZgHO",
        //    "productId":"6ylz1RFbtobUwK8AZMDTzs",
        //    "userId":"7aIcBJuGym66bQ749YHaK7",
        //    "renewable":"false",
        //    "type":"TIME_BASED_SUBSCRIPTION",
        //    "state":"VALID",
        //    "productTags":"DITABLET",
        //    "validFrom":"1334317725000",
        //    "validTo":"1334854125000",
        //    "externalSubscriptionId":"tmp-6506779",
        //    "externalSubscriberId":"tmp-3538930"
        //    }
        //}
        #endregion
    }

    //Copy of EntitlementOutput with only difference: supporting productTags and entitlements as array as it should be, which is a known bug in S+
    public class EntitlementOutput2
    {
        public string httpResponseCode;
        public string requestId;
        public List<EntitlementOutputData2> entitlements;

        public EntitlementOutput2() { }

        public EntitlementOutput2 GetEntitlementOutput(string json)
        {
            var js = new JavaScriptSerializer();
            var eo = js.Deserialize<EntitlementOutput2>(json);
            return eo;
        }
    }

    //Copy of EntitlementOutputData with only difference: supporting productTags as array as it should be, which is a known bug in S+
    public class EntitlementOutputData2
    {
        public string id;
        public string created;
        public string updated;
        public string location;
        public string brandId;
        public string productId;
        public string userId;
        public string renewable;
        public string type;
        public string state;
        public string[] productTags;
        public string validFrom;
        public string validTo;
        public string externalSubscriptionId;
        public string externalSubscriberId;

        public EntitlementOutputData2() { }

    }
}
