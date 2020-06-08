using Di.Common.Logging;
using Pren.Web.Business.DataAccess.BusinessSubscription;
using Pren.Web.Business.DataAccess.Campaign;
using Pren.Web.Business.DataAccess.CodePortal;
using Pren.Web.Business.DataAccess.Logging;
using Pren.Web.Business.DataAccess.Package;
using Pren.Web.Business.DataAccess.Usp;

namespace Pren.Web.Business.DataAccess
{
    public interface IDataAccess
    {
        ILogger Logger { get; }
        IEmailLogger EmailLogger { get; }
        IUspDataHandler UspHandler { get; }
        ICampaignDataHandler CampaignHandler { get; }
        IBusinessSubscriptionDataHandler BusinessSubscriptionHandler { get; }
        ICodePortalCodeDataHandler CodePortalCodeDataHandler { get; }
        ICodePortalListDataHandler CodePortalListDataHandler { get; }
        ICodePortalGiveAwayDatahandler CodePortalGiveAwayDatahandler { get; }
        IPackageRelationDataHandler PackageRelationDataHandler { get; }
        IPackageRelationItemDataHandler PackageRelationItemDataHandler { get; }
        IPackageRelationListDataHandler PackageRelationListDataHandler { get; }
    }
}
