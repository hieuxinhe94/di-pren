using System.Collections.Generic;
using Di.Subscription.Logic.ExtraProducts.Retrievers.Types;

namespace Di.Subscription.Logic.ExtraProducts.Retrievers
{
    internal interface IExtraProductsRetriever
    {
        IEnumerable<ExtraProduct> GetExtraProducts();
        ExtraProduct GetExtraProduct(string productCode);
    }
}