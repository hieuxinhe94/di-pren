using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Di.Subscription.Logic.Campaign;

namespace Pren.Web.Controllers.ApiControllers
{
    public class CampaignController : ApiController
    {
        private readonly ICampaignHandler _campaignHandler;
        
        public CampaignController(ICampaignHandler campaignHandler)
        {
            _campaignHandler = campaignHandler;
        }

        [Authorize]
        public List<CampaignInfoLw> Get(bool updateCache)
        {
            return _campaignHandler.GetActiveCampignIdentifiers(updateCache).Select(aci => new CampaignInfoLw
            {
                CampId = aci.CampaignId,
                CampName = aci.CampaignName,
                CampNo = aci.CampaignNumber
            }).ToList();
        }
    }

    //todo: kj - remove this object and use the CampignIdentifier object instead
    public class CampaignInfoLw
    {
        public int CampNo { get; set; }
        public string CampId { get; set; }
        public string CampName { get; set; }
    }
}