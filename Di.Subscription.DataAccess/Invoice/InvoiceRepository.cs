using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Invoice
{
    internal class InvoiceRepository : IInvoiceRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public InvoiceRepository(
            ISubscriptionDataAccess subscriptionDataAccess, 
            IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<Invoice> GetOpenInvoices(long customerNumber, string userId)
        {
            var invoicesDataSet = _subscriptionDataAccess.GetOpenInvoices(customerNumber, userId);

            return _objectConverter.ConvertFromDataSet<Invoice>(invoicesDataSet);
        }
    }
}