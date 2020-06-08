using Newtonsoft.Json;

namespace Pren.Web.Business.Faq.ResponseModels.Topics
{
    public class Category
    {
        [JsonProperty("selectable")]
        public bool Selectable { get; set; }

        [JsonProperty("heading_name")]
        public string Heading { get; set; }

        [JsonProperty("all_name")]
        public string ReadMoreHeading { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }
}