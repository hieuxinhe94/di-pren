using System.IO.Pipes;
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
    public class SubscriptionApi : ISubscriptionApi
    {
        public IHolidayStop HolidayStop { get; }
        public ICustomer Customer { get; }
        public ISubscription Subscription { get; }
        public IReclaim Reclaim { get; }
        public IPermanentAddress PermanentAddress { get; }
        public ITemporaryAddress TemporaryAddress { get; }
        public IIssue Issue { get; }
        public IInvoice Invoice { get; }

        public SubscriptionApi(string apiUrl, string apiClient, string apiSecret)
        {
            HolidayStop = new HolidayStop(apiUrl, apiClient, apiSecret);
            Customer = new Customer(apiUrl, apiClient, apiSecret);
            Subscription = new RestApi.Requests.Subscription.Subscription(apiUrl, apiClient, apiSecret);
            Reclaim = new Reclaim(apiUrl, apiClient, apiSecret);
            PermanentAddress = new PermanentAddress(apiUrl, apiClient, apiSecret);
            TemporaryAddress = new TemporaryAddress(apiUrl, apiClient, apiSecret);
            Issue = new Issue(apiUrl, apiClient, apiSecret);
            Invoice = new Invoice(apiUrl, apiClient, apiSecret);
        }

        public SubscriptionApi(
            IHolidayStop holidayStop, 
            ICustomer customer, 
            ISubscription subscription, 
            IReclaim reclaim, 
            IPermanentAddress permanentAddress, 
            ITemporaryAddress temporaryAddress,
            IIssue issue,
            IInvoice invoice)
        {
            HolidayStop = holidayStop;
            Customer = customer;
            Subscription = subscription;
            Reclaim = reclaim;
            PermanentAddress = permanentAddress;
            TemporaryAddress = temporaryAddress;
            Issue = issue;
            Invoice = invoice;
        }
    }
}
