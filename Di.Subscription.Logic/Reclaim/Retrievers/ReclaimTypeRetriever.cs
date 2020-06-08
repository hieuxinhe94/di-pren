using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Reclaim;

namespace Di.Subscription.Logic.Reclaim.Retrievers
{
    public class ReclaimTypeRetriever : IReclaimTypeRetriever
    {
        private readonly IReclaimRepository _reclaimRepository;

        public ReclaimTypeRetriever(IReclaimRepository reclaimRepository)
        {
            _reclaimRepository = reclaimRepository;
        }

        public IEnumerable<Types.ReclaimType> GetReclaimTypes()
        {
            var reclaimTypesExt = _reclaimRepository.GetReclaimTypes(SubscriptionConstants.PaperCodeAll);
            var reclaimTypes = reclaimTypesExt.Select(reclaimType => new Types.ReclaimType
            {
                Id = reclaimType.ReclaimItem,
                ReclaimText = reclaimType.ReclaimText,
                CarrierMessage = reclaimType.CarrierMessage,
                Compensation = reclaimType.Compensation,
                OrderNumber = reclaimType.OrderNumber,
                ReclaimKind = reclaimType.ReclaimKind,
                ReclaimPaper = reclaimType.ReclaimPaper
            }).ToList();

            return reclaimTypes;
        }
    }
}