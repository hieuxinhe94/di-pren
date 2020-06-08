using Bn.Subscription.RestApi.Requests.Customer;
using Bn.Subscription.RestApi.Requests.HolidayStop;
using Bn.Subscription.RestApi.Requests.Invoice;
using Bn.Subscription.RestApi.Requests.Issue;
using Bn.Subscription.RestApi.Requests.PermanentAddress;
using Bn.Subscription.RestApi.Requests.Reclaim;
using Bn.Subscription.RestApi.Requests.Subscription;
using Bn.Subscription.RestApi.Requests.TemporaryAddress;

namespace Bn.Subscription
{
    public interface ISubscriptionApi
    {
        IHolidayStop HolidayStop { get; }

        ICustomer Customer { get; }

        ISubscription Subscription { get; }

        IReclaim Reclaim { get; }

        IPermanentAddress PermanentAddress { get; }

        ITemporaryAddress TemporaryAddress { get; }

        IIssue Issue { get; }

        IInvoice Invoice { get; }
    }
}
