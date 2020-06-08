using System.Collections.Generic;
using Pren.Web.Business.Faq.Models.Items;

namespace Pren.Web.Business.Faq.Request.Items
{
    public interface IItems
    {
        List<Item> GetItems(int limit, string sortorder);

        List<Item> GetItemsByTopic(string topic, int limit, string sortorder);
    }
}
