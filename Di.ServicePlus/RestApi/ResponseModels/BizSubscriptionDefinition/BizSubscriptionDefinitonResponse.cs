using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionDefinition
{
    public class BizSubscriptionDefinitonResponse : ResponseBase
    {
        [JsonProperty("bizSubscriptionDef")]
        [JsonConverter(typeof(SingleOrArrayConverter<BizSubscriptionDefinition>))]
        public List<BizSubscriptionDefinition> BizSubscriptionDefinition { get; set; }
    }
}
