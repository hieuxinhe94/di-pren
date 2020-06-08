using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;


//namespace DIClassLib.BonnierDigital
//{
//    public class Subs
//    {
//        public Subs() { }

//        public SubsOutput CreateSubs(string userId, string externalId, string status)
//        {
//            SubsInput si = new SubsInput(userId, externalId, status);
//            string json = RequestHandler.WebReqPostJson(ConfigurationManager.AppSettings["BonDigUrlCreateSubs"].ToString(), si.ToJson());
//            SubsOutput so = new SubsOutput();
//            so = so.GetSubsOutput(json);
//            return so;
//        }
//    }


//    public class SubsInput
//    {
//        public string userId;
//        public string titleId;
//        public string externalId;
//        public string status;

//        public SubsInput(string userId_, string externalId_, string status_)
//        {
//            userId = userId_;
//            titleId = ConfigurationManager.AppSettings["BonDigTitleId"].ToString();
//            externalId = externalId_;
//            status = status_;
//        }

//        public string ToJson()
//        {
//            JavaScriptSerializer js = new JavaScriptSerializer();
//            string s = js.Serialize(this);
//            return s;
//        }
//    }

//    public class SubsOutput
//    {
//        public string httpResponseCode;
//        public string requestId;
//        public SubsOutputData subscription;

//        public SubsOutput() { }

//        public SubsOutput GetSubsOutput(string json)
//        {
//            JavaScriptSerializer js = new JavaScriptSerializer();
//            SubsOutput uo = js.Deserialize<SubsOutput>(json);
//            return uo;
//        }
//    }

//    public class SubsOutputData
//    {
//        public string id;
//        public string updated;
//        public string created;
//        public string location;
//        public string brandId;
//        public string externalId;
//        public string userId;
//        public string titleId;
//        public string validFrom;
//        public string validTo;
//        public string status;

//        public SubsOutputData() { }
//    }
//}
