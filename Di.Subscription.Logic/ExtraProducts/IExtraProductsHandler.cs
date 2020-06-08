using System.Collections.Generic;
using Di.Subscription.Logic.ExtraProducts.Retrievers.Types;

namespace Di.Subscription.Logic.ExtraProducts
{
    public interface IExtraProductsHandler
    {
        IEnumerable<ExtraProduct> GetExtraProducts();
        ExtraProduct GetExtraProduct(string productCode);
    }
}