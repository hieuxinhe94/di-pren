using Di.Subscription.DataAccess.AddressChange;
using Di.Subscription.DataAccess.Campaign;
using Di.Subscription.DataAccess.Customer;
using Di.Subscription.DataAccess.DataAccess;
using Di.Subscription.DataAccess.ExtraProducts;
using Di.Subscription.DataAccess.HolidayStop;
using Di.Subscription.DataAccess.Invoice;
using Di.Subscription.DataAccess.IssueDate;
using Di.Subscription.DataAccess.Package;
using Di.Subscription.DataAccess.Parameters;
using Di.Subscription.DataAccess.PostName;
using Di.Subscription.DataAccess.PublicationDays;
using Di.Subscription.DataAccess.Reclaim;
using Di.Subscription.DataAccess.Subscription;
using StructureMap;

namespace Di.Subscription.DataAccess.Initialization
{
    public class DependencyResolver
    {
        public static ConfigurationExpression ConfigureStructureMapDependecies(ConfigurationExpression container)
        {
            // Subscription Data Access
            container.For<ISubscriptionDataAccess>().Use<KayakDataAccess>();

            // Reclaim
            container.For<IReclaimRepository>().Use<ReclaimRepository>();

            // HolidayStop
            container.For<IHolidayStopRepository>().Use<HolidayStopRepository>();

            // Address change
            container.For<IAddressChangeRepository>().Use<AddressChangeRepository>();

            container.For<IPackageRepository>().Use<PackageRepository>();

            container.For<ICampaignRepository>().Use<CampaignRepository>();

            container.For<ICustomerRepository>().Use<CustomerRepository>();

            container.For<IPublicationDaysRepository>().Use<PublicationDaysRepository>();

            container.For<IExtraProductsRepository>().Use<ExtraProductsRepository>();

            container.For<ISubscriptionRepository>().Use<SubscriptionRepository>();

            container.For<IPostNameRepository>().Use<PostNameRepository>();

            container.For<IParametersRepository>().Use<ParametersRepository>();

            container.For<IIssueDateRepository>().Use<IssueDateRepository>();

            container.For<IInvoiceRepository>().Use<InvoiceRepository>();

            return container;
        }
    }
}
