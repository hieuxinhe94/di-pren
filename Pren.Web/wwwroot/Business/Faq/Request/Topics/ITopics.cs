using System.Collections.Generic;
using Pren.Web.Business.Faq.Models.Topics;

namespace Pren.Web.Business.Faq.Request.Topics
{
    public interface ITopics
    {
        List<Topic> GetTopics(int limit, string sortorder);
    }
}
