using Pren.Web.Business.Faq.Request.Items;
using Pren.Web.Business.Faq.Request.Topics;

namespace Pren.Web.Business.Faq
{
    public interface IFaqApi
    {
        ITopics Topics { get; }
        IItems Items { get; }
    }

    public class FaqApi : IFaqApi
    {
        public ITopics Topics { get; private set; }
        public IItems Items { get; private set; }

        /// <summary>
        /// Creates a new instance of the Faq Api Wrapper with deafault implementations.
        /// </summary>
        /// <param name="faqApiUrl">The base url to Faq REST Api."</param>
        public FaqApi(string faqApiUrl) : this(new Topics(faqApiUrl), new Items(faqApiUrl))
        {

        }

        /// <summary>
        /// Creates a new instance of the Faq Api Wrapper.
        /// We make it possible to provide own implementations of the request interfaces by Constructor Injection
        /// </summary>
        /// <param name="topics">A concrete implementation of the ITopics interface</param>
        /// <param name="items">A concrete implementation of the IItems interface</param>
        public FaqApi(ITopics topics, IItems items)
        {
            Topics = topics;
            Items = items;
        }
    }
}