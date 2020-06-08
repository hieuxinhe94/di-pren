using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Pren.Web.Business.Faq.ResponseModels.Item
{
    public class ItemsResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("absolute_url")]
        public string Url { get; set; }        
    }
}