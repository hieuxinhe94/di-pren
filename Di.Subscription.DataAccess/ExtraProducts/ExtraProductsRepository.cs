using System;
using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.ExtraProducts
{
    internal class ExtraProductsRepository : IExtraProductsRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public ExtraProductsRepository(ISubscriptionDataAccess subscriptionDataAccess, IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<ExtraProduct> GetExtraProducts(long subscriptionCustomerNumber, string paperCode, DateTime orderDate)
        {
            var extraProductsDataSet = _subscriptionDataAccess.GetExtraProducts(subscriptionCustomerNumber, paperCode, orderDate);

            return _objectConverter.ConvertFromDataSet<ExtraProduct>(extraProductsDataSet);
        }
    }
}