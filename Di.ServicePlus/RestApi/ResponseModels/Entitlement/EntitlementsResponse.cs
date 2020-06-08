using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.Entitlement
{
    public class EntitlementsResponse : SolrQueryResponseBase
    {
        [JsonProperty("entitlements")]
        [JsonConverter(typeof(SingleOrArrayConverter<Entitlement>))]
        public List<Entitlement> Entitlements { get; set; }
    }
}
