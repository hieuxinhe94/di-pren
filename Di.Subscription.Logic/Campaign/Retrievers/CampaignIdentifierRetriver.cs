using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Campaign;
using Di.Subscription.Logic.Campaign.Types;
using Di.Subscription.Logic.Package.Retrievers;
using Di.Subscription.Logic.Utils;

namespace Di.Subscription.Logic.Campaign.Retrievers
{
    public class CampaignIdentifierRetriver : ICampaignIdentifierRetriver
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPackageRetriever _packageRetriever;

        public CampaignIdentifierRetriver(
            ICampaignRepository campaignRepository, 
            IPackageRetriever packageRetriever)
        {
            _campaignRepository = campaignRepository;
            _packageRetriever = packageRetriever;
        }

        public IEnumerable<CampaignIdentifier> GetActiveCampignIdentifiers(bool forceRefresh = false)
        {
            var productPackages = _packageRetriever.GetProductPackages().DistinctBy(pp => pp.PackageId).ToList();

            var activeCampaigns = new List<CampaignGroup>();

            foreach (var productPackage in productPackages)
            {
                activeCampaigns.AddRange(_campaignRepository.GetActiveCampaigns(productPackage.PackageId));
            }

            activeCampaigns = activeCampaigns.Distinct().OrderByDescending(c => c.CampaignId).ToList();

            var activeCampignIdentifiers = activeCampaigns.Select(ac => new CampaignIdentifier
            {
                CampaignId = ac.CampaignId,
                CampaignName = ac.CampaignName,
                CampaignNumber = ac.CampaignNumber
            });

            return activeCampignIdentifiers.OrderBy(c => c.CampaignId).ToList();
        }
    }
}