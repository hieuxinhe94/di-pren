using System.Collections.Generic;
using Di.Subscription.Logic.ExtraProducts.Retrievers;
using Di.Subscription.Logic.ExtraProducts.Retrievers.Types;

namespace Di.Subscription.Logic.ExtraProducts
{
    internal class ExtraProductsHandler : IExtraProductsHandler
    {
        private readonly IExtraProductsRetriever _extraProductsRetriever;

        public ExtraProductsHandler(IExtraProductsRetriever extraProductsRetriever)
        {
            _extraProductsRetriever = extraProductsRetriever;
        }

        public IEnumerable<ExtraProduct> GetExtraProducts()
        {
            return _extraProductsRetriever.GetExtraProducts();
        }

        public ExtraProduct GetExtraProduct(string productCode)
        {
            return _extraProductsRetriever.GetExtraProduct(productCode);
        }
    }
}