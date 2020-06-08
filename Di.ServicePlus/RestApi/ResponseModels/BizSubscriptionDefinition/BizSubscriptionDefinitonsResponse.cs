using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionDefinition
{
    public class BizSubscriptionDefinitonsResponse : ResponseBase
    {
        [JsonProperty("numItems")]
        public string NumItems { get; set; }

        [JsonProperty("startIndex")]
        public string StartIndex { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("bizSubscriptionDefs")]
        [JsonConverter(typeof(SingleOrArrayConverter<BizSubscriptionDefinition>))]
        public List<BizSubscriptionDefinition> BizSubscriptionDefinitions { get; set; }
    }
}
