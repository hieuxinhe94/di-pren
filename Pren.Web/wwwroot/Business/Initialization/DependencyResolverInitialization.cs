using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Bn.CompanyService.ContentApi;
using Bn.SelfService.ContentApi;
using Bn.Subscription;
using Di.Common.Conversion;
using Di.Common.Logging;
using Di.Common.Security.Encryption;
using Di.Subscription.Logic.Campaign.Retrievers;
using Di.Subscription.Logic.Package.Retrievers;
using Di.Subscription.Logic.Parameters.Retrievers;
using Di.Subscription.Logic.Reclaim.Retrievers;
using DIClassLib.BonnierDigital;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Pren.Web.Business.Address;
using Pren.Web.Business.BusinessSubscription;
using Pren.Web.Business.Cache;
using Pren.Web.Business.CodePortal;
using Pren.Web.Business.Cache.Campaign.Retrievers;
using Pren.Web.Business.Cache.Package.Retrievers;
using Pren.Web.Business.Cache.Parameters.Retrievers;
using Pren.Web.Business.Cache.Reclaim.Retrievers;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.DataAccess.BusinessSubscription;
using Pren.Web.Business.DataAccess.Campaign;
using Pren.Web.Business.DataAccess.CodePortal;
using Pren.Web.Business.DataAccess.Logging;
using Pren.Web.Business.DataAccess.Package;
using Pren.Web.Business.DataAccess.Usp;
using Pren.Web.Business.Detection;
using Pren.Web.Business.EpiSelfServiceContentApi;
using Pren.Web.Business.Helpers;
using Pren.Web.Business.Invoice;
using Pren.Web.Business.Mail;
using Pren.Web.Business.Package;
using Pren.Web.Business.Rendering;
using Pren.Web.Business.ServicePlus;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Business.Session;
using Pren.Web.Business.Student;
using Pren.Web.Business.Subscription;
using StructureMap;

namespace Pren.Web.Business.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Container.Configure(ConfigureContainer);

            var resolver = new StructureMapDependencyResolver(context.Container);
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static void ConfigureContainer(ConfigurationExpression container)
        {
            //Swap out the default ContentRenderer for our custom
            container.For<IContentRenderer>().Use<ErrorHandlingContentRenderer>();
            container.For<ContentAreaRenderer>().Use<CustomContentAreaRenderer>();

            //Implementations for custom interfaces can be registered here.            
            container.For<ISubscriptionHandler>().Use<KayakHandler>();
            container.For<IServicePlusHandler<UserOutput>>().Use<ServicePlusHandler>();
            container.For<IApiReferrerCheck>().Use<ApiReferrerCheck>();

            container.For<IUrlHelper>().Use<Helpers.UrlHelper>();
            container.For<IStudentHandler>().Use<StudentHandler>();

            container.For<ISessionData>().Use<SessionData>();
            container.For<IPageSpecificSession>().Use<PageSpecificSession>();

            container.For<IDetectionHandler>().Use<DetectionHandler>();
            container.For<ISiteSettings>().Use<SiteSettings>();
            container.For<ISubscriptionService<DIClassLib.Subscriptions.Subscription, SubscriptionUser2>>().Use<SubscriptionService>();
            container.For<ISiteConfiguration>().Use<SiteConfiguration>();
            container.For<ISubscriptionUser<SubscriptionUser2>>().Use<SubscriptionUser>();
            container.For<IMailHandler>().Use<MailHandler>();
            container.For<IAddressService>().Use<ParService>();
            container.For<IMaskedAddressService>().Use<MaskedAddressService>();
            container.For<IConnectService>().Use<ConnectService>();
            
            container.For<IDataAccess>().Use<EntityDataAccess>();
            container.For<IEmailLogger>().Use<EntityEmailLogger>();
            container.For<IUspDataHandler>().Use<EntityUspHandler>();
            container.For<IBusinessSubscriptionDataHandler>().Use<EntityBusinessSubscriptionDataHandler>();
            container.For<ICampaignDataHandler>().Use<EntityCampaignHandler>();
            container.For<ICodePortalCodeDataHandler>().Use<EntityCodePortalCodeDataHandler>();
            container.For<ICodePortalListDataHandler>().Use<EntityCodePortalListDataHandler>();
            container.For<ICodePortalGiveAwayDatahandler>().Use<EntityCodePortalGiveAwayDatahandler>();
            container.For<IPackageRelationDataHandler>().Use<EntityPackageRelationDataHandler>();
            container.For<IPackageRelationListDataHandler>().Use<EntityPackageRelationListDataHandler>();
            container.For<IPackageRelationItemDataHandler>().Use<EntityPackageRelationItemDataHandler>();
            container.For<IPackageRelationManager>().Use<PackageRelationManager>();

            container.For<ISubscriptionChecker>().Use<SubscriptionChecker>();         

            container.For<ICodePortalService>().Use<CodePortalService>();  
            container.For<ILoggerDataHandler>().Use<EntityLoggerDataHandler>();  

            container.For<IAntiForgeryValidator>().Use<AntiForgeryValidator>();
            container.For<IInvoiceFacade>().Use<InvoiceFacade>();

            //Common
            container.For<ICryptographyService>().Use<RijndaelCryptographyService>();

            container.For<Di.Common.WebRequests.IRequestService>().Use<Di.Common.WebRequests.RequestService>()
                .Ctor<ILogger>().Is<Log4NetLogger>();
            container.For<ILogger>().Use<EntityLogger>();
            container.For<IObjectConverter>().Use<ObjectConverter>();
            container.For<IDataSetUtilites>().Use<DataSetUtilities>();
            //container.For<IPropertyUtilities>().Use<CachedPropertyUtilities>()
            //    .Ctor<IPropertyUtilities>().Is<PropertyUtilities>()
            //    .Ctor<Di.Common.Cache.IObjectCache>().Is<Di.Common.Cache.ObjectCache>();
            container.For<IPropertyUtilities>().Use<CachedPropertyUtilities>()
                .Ctor<IPropertyUtilities>().Is<PropertyUtilities>();
            container.For<Di.Common.Cache.IObjectCache>().Use<Di.Common.Cache.ObjectCache>();

            // Cache
            container.For<IObjectCache>().Use<ObjectCache>();

            container.For<IServicePlusFacade>().Use<ServicePlusFacade>();

            // Subscription DataAccess and Logic
            ConfigureSubscriptionDependencies(container);
            // Override
            container.For<ICampaignRetriever>().Use<CachedCampaignRetriever>()
                .Ctor<ICampaignRetriever>().Is<CampaignRetriever>();
            container.For<IPackageRetriever>().Use<CachedPackageRetriever>()
                .Ctor<IPackageRetriever>().Is<PackageRetriever>();
            container.For<ICampaignIdentifierRetriver>().Use<CachedCampaignIdentifierRetriver>()
                .Ctor<ICampaignIdentifierRetriver>().Is<CampaignIdentifierRetriver>();
            container.For<IParametersRetriever>().Use<CachedParametersRetriever>()
                .Ctor<IParametersRetriever>().Is<ParametersRetriever>();
            container.For<IReclaimTypeRetriever>().Use<CachedReclaimTypeRetriever>()
                .Ctor<IReclaimTypeRetriever>().Is<ReclaimTypeRetriever>();

            // Call the ConfigureStructureMapDependecies to set up dependencies in Di.ServicePlus
            Di.ServicePlus.Initialization.DependencyResolver.ConfigureStructureMapDependecies(container);

            //BusinessSubscription
            container.For<IPriceHandler>().Use<PriceHandler>();            
            container.For<IServicePlus>().Use<ServicePlus.Logic.ServicePlus>();
            container.For<IInviteImporter>().Use<InviteImporter>();
            container.For<ISubscriberFacade>().Use<SubscriberFacade>();

            container.For<ISubscriptionApi>().Use<SubscriptionApi>()
                .SelectConstructor(() => new SubscriptionApi("", "", ""))
                .Ctor<string>("apiUrl").Is(ConfigurationManager.AppSettings.Get("BonnierNewsSubscriptionApiBaseUrl"))
                .Ctor<string>("apiClient").Is(ConfigurationManager.AppSettings.Get("BonnierNewsSubscriptionApiClientId"))
                .Ctor<string>("apiSecret").Is(ConfigurationManager.AppSettings.Get("BonnierNewsSubscriptionApiClientSecret"));

            container.For<IExternalMailTrigger>().Use<NeolaneMailTrigger>();

            container.For<ISelfServiceContentService>().Use<EpiSelfServiceContentService>();
            container.For<ICompanyServiceContentService>().Use<EpiCompanyServiceContentService>();
        }
        
        private static void ConfigureSubscriptionDependencies(ConfigurationExpression container)
        {
            // Call the ConfigureStructureMapDependecies to set up dependencies in Di.Subscription
            Di.Subscription.DataAccess.Initialization.DependencyResolver.ConfigureStructureMapDependecies(container);

            // Call the ConfigureStructureMapDependecies to set up dependencies in Di.Subscription.Logic
            Di.Subscription.Logic.Initialization.DependencyResolver.ConfigureStructureMapDependecies(container);
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
