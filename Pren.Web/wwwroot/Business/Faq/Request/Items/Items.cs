using System.Collections.Generic;
using System.Linq;
using Di.Common.Utils;
using Di.Common.WebRequests;
using Pren.Web.Business.Faq.Models.Items;
using Pren.Web.Business.Faq.ResponseModels.Item;

namespace Pren.Web.Business.Faq.Request.Items
{
    public class Items : RequestBase, IItems
    {


        public Items(string faqApiUrl) : base(faqApiUrl)
        {
        }

        public Items(string faqApiUrl, IRequestService requestService) : base(faqApiUrl, requestService)
        {
        }

        public List<Item> GetItems(int limit, string sortorder)
        {
            var queryStringDictionary = GetQueryStringDictionary(limit, sortorder);

            var responseJsonString = CreateRequest(RequestVerb.Get, "dagens-industri.json", queryStringDictionary); 

            return responseJsonString.ConvertToObject<List<ItemsResponse>>().Select(t => new Item(){Title = t.Title, Url = t.Url}).ToList();
        }

        public List<Item> GetItemsByTopic(string topic, int limit, string sortorder)
        {
            var queryStringDictionary = GetQueryStringDictionary(limit, sortorder);

            var responseJsonString = CreateRequest(RequestVerb.Get, "dagens-industri/" + topic + ".json", queryStringDictionary);

            return responseJsonString.ConvertToObject<List<ItemsResponse>>().Select(t => new Item() { Title = t.Title, Url = t.Url }).ToList();
        }

    }
}