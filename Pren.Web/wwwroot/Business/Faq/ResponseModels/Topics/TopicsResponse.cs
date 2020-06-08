using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Pren.Web.Business.Faq.ResponseModels.Topics
{
    public class TopicsResponse
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }
    }
}