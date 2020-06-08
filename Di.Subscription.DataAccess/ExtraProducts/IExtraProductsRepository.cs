using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.ExtraProducts
{
    public interface IExtraProductsRepository
    {
        IEnumerable<ExtraProduct> GetExtraProducts(long subscriptionCustomerNumber, string paperCode, DateTime orderDate);
    }
}