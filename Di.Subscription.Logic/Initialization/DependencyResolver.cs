using Di.Subscription.Logic.Address;
using Di.Subscription.Logic.Address.Modifiers;
using Di.Subscription.Logic.Address.Retrievers;
using Di.Subscription.Logic.Campaign;
using Di.Subscription.Logic.Campaign.Retrievers;
using Di.Subscription.Logic.Customer;
using Di.Subscription.Logic.Customer.Modifiers;
using Di.Subscription.Logic.Customer.Retrievers;
using Di.Subscription.Logic.ExtraProducts;
using Di.Subscription.Logic.ExtraProducts.Retrievers;
using Di.Subscription.Logic.HolidayStop;
using Di.Subscription.Logic.HolidayStop.Modifiers;
using Di.Subscription.Logic.HolidayStop.Retrievers;
using Di.Subscription.Logic.Invoice;
using Di.Subscription.Logic.Invoice.Retrievers;
using Di.Subscription.Logic.Package;
using Di.Subscription.Logic.Package.Retrievers;
using Di.Subscription.Logic.Parameters;
using Di.Subscription.Logic.Parameters.Retrievers;
using Di.Subscription.Logic.IssueDate;
using Di.Subscription.Logic.IssueDate.Retrievers;
using Di.Subscription.Logic.PostName;
using Di.Subscription.Logic.PostName.Retrievers;
using Di.Subscription.Logic.PublicationDays;
using Di.Subscription.Logic.PublicationDays.Retrievers;
using Di.Subscription.Logic.Reclaim;
using Di.Subscription.Logic.Reclaim.Modifiers;
using Di.Subscription.Logic.Reclaim.Retrievers;
using Di.Subscription.Logic.Subscription;
using Di.Subscription.Logic.Subscription.Retrievers;
using StructureMap;

namespace Di.Subscription.Logic.Initialization
{
    public class DependencyResolver
    {
        public static ConfigurationExpression ConfigureStructureMapDependecies(ConfigurationExpression container)
        {
            // Campaign
            container.For<ICampaignHandler>().Use<CampaignHandler>();
            container.For<ICampaignRetriever>().Use<CampaignRetriever>();
            container.For<ICampaignIdentifierRetriver>().Use<CampaignIdentifierRetriver>();

            // Address
            container.For<IAddressHandler>().Use<AddressHandler>();
            container.For<ITemporaryAddressCreator>().Use<TemporaryAddressCreator>();
            container.For<ITemporaryAddressRemover>().Use<TemporaryAddressRemover>();
            container.For<IPermanentAddressCreator>().Use<PermanentAddressCreator>();
            container.For<IPermanentAddressRemover>().Use<PermanentAddressRemover>();
            container.For<IAddressRetriever>().Use<AddressRetriever>();
            container.For<ITemporaryAddressChanger>().Use<TemporaryAddressChanger>();
            

            // HolidayStop
            container.For<IHolidayStopHandler>().Use<HolidayStopHandler>();
            container.For<IHolidayStopCreator>().Use<HolidayStopCreator>();
            container.For<IHolidayStopRemover>().Use<HolidayStopRemover>();
            container.For<IHolidayStopChanger>().Use<HolidayStopChanger>();
            container.For<IHolidayStopRetriever>().Use<HolidayStopRetriever>();

            // Reclaim
            container.For<IReclaimHandler>().Use<ReclaimHandler>();
            container.For<IDeliveryReclaimCreator>().Use<DeliveryReclaimCreator>();
            container.For<IReclaimTypeRetriever>().Use<ReclaimTypeRetriever>();
            container.For<IReclaimRetriever>().Use<ReclaimRetriever>();     

            // Customer
            container.For<ICustomerHandler>().Use<CustomerHandler>();
            container.For<ICustomerRetriever>().Use<CustomerRetriever>();
            container.For<ICustomerInformationChanger>().Use<CustomerInformationChanger>();
            container.For<ICustomerPropertyChanger>().Use<CustomerPropertyChanger>();

            // PublicationDays
            container.For<IPublicationDaysHandler>().Use<PublicationDaysHandler>();
            container.For<IPublicationDaysRetriever>().Use<PublicationDaysRetriever>();

            // ExtraProducts
            container.For<IExtraProductsHandler>().Use<ExtraProductsHandler>();
            container.For<IExtraProductsRetriever>().Use<ExtraProductsRetriever>();

            // Subscriptions
            container.For<ISubscriptionHandler>().Use<SubscriptionHandler>();
            container.For<ISubscriptionRetriever>().Use<SubscriptionRetriever>();

            // Postname
            container.For<IPostNameHandler>().Use<PostNameHandler>();
            container.For<IPostNameRetriever>().Use<PostNameRetriever>();

            // Parameters
            container.For<IParametersHandler>().Use<ParametersHandler>();
            container.For<IParametersRetriever>().Use<ParametersRetriever>();

            // IssueDate
            container.For<IIssueDateHandler>().Use<IssueDateHandler>();
            container.For<IIssueDateRetriever>().Use<IssueDateRetriever>();

            // Package
            container.For<IPackageHandler>().Use<PackageHandler>();
            container.For<IPackageRetriever>().Use<PackageRetriever>();

            // Invoice
            container.For<IInvoiceHandler>().Use<InvoiceHandler>();
            container.For<IInvoiceRetriever>().Use<InvoiceRetriever>();

            return container;
        }
    }
}
