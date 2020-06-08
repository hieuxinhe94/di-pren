using Di.Common.Logging;
using Pren.Web.Business.DataAccess.BusinessSubscription;
using Pren.Web.Business.DataAccess.Campaign;
using Pren.Web.Business.DataAccess.CodePortal;
using Pren.Web.Business.DataAccess.Logging;
using Pren.Web.Business.DataAccess.Package;
using Pren.Web.Business.DataAccess.Usp;

namespace Pren.Web.Business.DataAccess
{
    class EntityDataAccess : IDataAccess
    {
        public ILogger Logger { get; private set; }
        public ILoggerDataHandler EntityLoggerDataHandler { get; private set; }
        public IEmailLogger EmailLogger { get; private set; }
        public IUspDataHandler UspHandler { get; private set; }
        public ICampaignDataHandler CampaignHandler { get; private set; }
        public IBusinessSubscriptionDataHandler BusinessSubscriptionHandler { get; private set; }
        public ICodePortalCodeDataHandler CodePortalCodeDataHandler { get; private set; }
        public ICodePortalListDataHandler CodePortalListDataHandler { get; private set; }
        public ICodePortalGiveAwayDatahandler CodePortalGiveAwayDatahandler { get; private set; }
        public IPackageRelationDataHandler PackageRelationDataHandler { get; private set; }
        public IPackageRelationItemDataHandler PackageRelationItemDataHandler { get; private set; }
        public IPackageRelationListDataHandler PackageRelationListDataHandler { get; private set; }

        public EntityDataAccess(
            ILogger entityLogger, 
            IEmailLogger entityEmailLogger, 
            IUspDataHandler uspHandler, 
            ICampaignDataHandler campaignHandler, 
            IBusinessSubscriptionDataHandler businessSubscriptionHandler,
            ICodePortalCodeDataHandler codePortalCodeDataHandler,
            ICodePortalListDataHandler codePortalListDataHandler,
            ICodePortalGiveAwayDatahandler codePortalGiveAwayDatahandler,
            IPackageRelationDataHandler packageRelationDataHandler,
            IPackageRelationItemDataHandler packageRelationItemDataHandler,
            IPackageRelationListDataHandler packageRelationListDataHandler,           
            ILoggerDataHandler entityLoggerDataHandler)
        {
            CampaignHandler = campaignHandler;
            UspHandler = uspHandler;
            Logger = entityLogger;
            EmailLogger = entityEmailLogger;
            BusinessSubscriptionHandler = businessSubscriptionHandler;
            CodePortalCodeDataHandler = codePortalCodeDataHandler;
            CodePortalListDataHandler = codePortalListDataHandler;
            CodePortalGiveAwayDatahandler = codePortalGiveAwayDatahandler;
            PackageRelationDataHandler = packageRelationDataHandler;
            PackageRelationItemDataHandler = packageRelationItemDataHandler;
            PackageRelationListDataHandler = packageRelationListDataHandler;
            EntityLoggerDataHandler = entityLoggerDataHandler;
        }
    }
}