using System.Collections.Generic;
using System.Linq;
using Di.Common.Utils;
using Di.Common.WebRequests;
using Pren.Web.Business.Faq.Models.Topics;
using Pren.Web.Business.Faq.ResponseModels.Topics;

namespace Pren.Web.Business.Faq.Request.Topics
{
    public class Topics : RequestBase, ITopics
    {
        public Topics(string faqApiUrl) : base(faqApiUrl)
        {
        }

        public Topics(string faqApiUrl, IRequestService requestService) : base(faqApiUrl, requestService)
        {
        }

        public List<Topic> GetTopics(int limit, string sortorder)
        {
            var queryStringDictionary = GetQueryStringDictionary(limit, sortorder);

            var responseJsonString = CreateRequest(RequestVerb.Get, "properties/dagens-industri.json", queryStringDictionary);

            var responseJson = responseJsonString.ConvertToObject<List<TopicsResponse>>();

            return responseJson[0].Categories.Select(t => new Topic(){ Heading =  t.Heading, Slug = t.Slug}).ToList();
        }
    }
}