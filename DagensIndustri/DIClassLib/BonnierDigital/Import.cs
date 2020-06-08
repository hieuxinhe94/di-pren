using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web;


namespace DIClassLib.BonnierDigital
{
    public class Import
    {
        public Import() { }

        //public UserOutput CreateUser(string email, string password)
        //public UserOutput CreateUser(string email, string firstName, string lastName, string phoneNumber, string password)
        //{
        //    //UserInput ui = new UserInput(email, password);
        //    UserInput ui = new UserInput(email, firstName, lastName, phoneNumber, password);
        //    string json = RequestHandler.WebReqPostJson(ConfigurationManager.AppSettings["BonDigUrlCreateUser"].ToString(), ui.ToJson());
        //    UserOutput uo = new UserOutput();
        //    uo = uo.GetUserOutput(json);
        //    return uo;
        //}

        public ImportOutput CreateImport(string productId, string userId, string externalSubscriberId, string externalSubscriptionId)
        {
            ImportInput impIn = new ImportInput(productId, userId, externalSubscriberId, externalSubscriptionId);
            string json = RequestHandler.WebReqPostParam(ConfigurationManager.AppSettings["BonDigUrlCreateImport"].ToString(), impIn.ToKeyValParams());

            //producttags can be a list
            if (!json.Contains("["))
            {
                json = json.Replace("\"productTags\":", "\"productTags\":[");
                json = json.Replace(",\"validFrom\"", "],\"validFrom\"");
            }

            ImportOutput impOut = new ImportOutput();
            impOut = impOut.GetImportOutput(json);
            return impOut;
        }
    }

    public class ImportInput
    {
        public string productId;
        public string userId;
        public string externalSubscriberId;
        public string externalSubscriptionId;
        public string createTemp;

        public ImportInput(string productId_, string userId_, string externalSubscriberId_, string externalSubscriptionId_)
        {
            productId = productId_;
            userId = userId_;
            //titleId = ConfigurationManager.AppSettings["BonDigTitleId"].ToString();
            externalSubscriberId = externalSubscriberId_;
            externalSubscriptionId = externalSubscriptionId_;
            createTemp = "true";
        }

        public string ToJson()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string s = js.Serialize(this);
            return s;
        }

        public string ToKeyValParams()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("productId=" + HttpUtility.UrlEncode(productId));
            sb.Append("&userId=" + HttpUtility.UrlEncode(userId));
            sb.Append("&externalSubscriberId=" + HttpUtility.UrlEncode(externalSubscriberId));
            sb.Append("&externalSubscriptionId=" + HttpUtility.UrlEncode(externalSubscriptionId));
            sb.Append("&createTemp=" + HttpUtility.UrlEncode(createTemp));
            return sb.ToString();
        }
    }


    public class ImportOutput
    {
        public string httpResponseCode;
        public string requestId;
        public ImportOutputData entitlement;

        public ImportOutput() { }

        public ImportOutput GetImportOutput(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            ImportOutput uo = js.Deserialize<ImportOutput>(json);
            return uo;
        }
    }

    
    public class ImportOutputData
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
        public List<string> productTags;
        //public string productTags;
        public string validFrom;
        public string validTo;
        public string externalSubscriptionId;
        public string externalSubscriberId;

        public ImportOutputData() { }

        #region return from bon dig
        //{
        //"@httpResponseCode":"201","@requestId":"5NWh6sL4SuGuLYk66zE74x",
        //"entitlement":
        //    {
        //    "id":"0HdEH876dQOzcrq8jCY50T",
        //    "created":"1329127003756",
        //    "updated":"1329127003756",
        //    "location":"/0HdEH876dQOzcrq8jCY50T",
        //    "brandId":"5DuzcZz0j8u0zArSNzZgHO",
        //    "productId":"6ylz1RFbtobUwK8AZMDTzs",
        //    "userId":"6mjjcSg3OCMK5OT93K1YWM",
        //    "renewable":"false",
        //    "type":"TEMPORARY_SUBSCRIPTION",
        //    "state":"VALID",
        //    "productTags":"DITABLET",
        //    "validFrom":"1329127003735",
        //    "validTo":"1329213403735",
        //    "externalSubscriptionId":"6506645",
        //    "externalSubscriberId":"3538834"
        //    }
        //}
        #endregion
    }

}
