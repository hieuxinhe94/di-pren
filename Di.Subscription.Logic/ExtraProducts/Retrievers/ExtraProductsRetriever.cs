using System;
using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.ExtraProducts;
using ExtraProduct = Di.Subscription.Logic.ExtraProducts.Retrievers.Types.ExtraProduct;

namespace Di.Subscription.Logic.ExtraProducts.Retrievers
{
    internal class ExtraProductsRetriever : IExtraProductsRetriever
    {
        private readonly IExtraProductsRepository _extraProductsRepository;

        public ExtraProductsRetriever(IExtraProductsRepository extraProductsRepository)
        {
            _extraProductsRepository = extraProductsRepository;
        }

        public IEnumerable<ExtraProduct> GetExtraProducts()
        {
            var extraProductsExt = _extraProductsRepository.GetExtraProducts(-1, SubscriptionConstants.PaperCodeDi, DateTime.Now);

            var extraProducts = extraProductsExt.Select(GetExtraProduct);

            return extraProducts;
        }

        public ExtraProduct GetExtraProduct(string productCode)
        {
            return GetExtraProducts().FirstOrDefault(ep => ep.ProductCode == productCode);
        }

        private ExtraProduct GetExtraProduct(DataAccess.ExtraProducts.ExtraProduct extraProduct)
        {
            return new ExtraProduct
            {
                ProductCode = extraProduct.ProductCode,
                Price = extraProduct.Price
            };
        }
    }
}