using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;


namespace DIClassLib.BonnierDigital
{

    public class ExternalIdsOutput
    {
        public string httpResponseCode;
        public string requestId;
        public string totalItems;
        public List<ExternalIds> externalIds;

        public ExternalIdsOutput() { }

        //public ExternalIdsOutput GetExternalIdsOutput(string json)
        //{
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    ExternalIdsOutput ids = js.Deserialize<ExternalIdsOutput>(json);
        //    return ids;
        //}
    }


    public class ExternalIds
    {
        public string externalSubscriberId;
        public string externalProductId;
        //public List<string> externalSubscriptionIds;

        public ExternalIds() { }
    }


    //bug in json return

    //External ids: 1
    //------------------
    //"requestId":"4PSwR8zvICYDQruAwQzHRV",
    //"httpResponseCode":"200",
    //"totalItems":"1",
    //"externalIds":
    //  {"externalSubscriberId":"1","externalProductId":"IPAD-01","externalSubscriptionIds":"11"}

    //External ids: 2
    //-------------------
    //"@requestId":"7MA4pGxhFKGAAj3JUtcfUE",
    //"@httpResponseCode":"200",
    //"totalItems":"2",
    //"externalIds":
    //  [
    //    {"externalSubscriberId":"2","externalProductId":"IPAD-01","externalSubscriptionIds":"22"},
    //    {"externalSubscriberId":"1","externalProductId":"IPAD-01","externalSubscriptionIds":"11"}
    //  ]

}
