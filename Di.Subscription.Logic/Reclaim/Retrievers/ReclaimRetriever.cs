using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Reclaim;

namespace Di.Subscription.Logic.Reclaim.Retrievers
{
    public class ReclaimRetriever : IReclaimRetriever
    {
        private readonly IReclaimRepository _reclaimRepository;

        public ReclaimRetriever(IReclaimRepository reclaimRepository)
        {
            _reclaimRepository = reclaimRepository;
        }

        public IEnumerable<Types.Reclaim> GetCustomerReclaims(long customerNumber)
        {
            var reclaimsExt = _reclaimRepository.GetCustomerReclaims(customerNumber);

            var reclaims = reclaimsExt.Select(reclaim => new Types.Reclaim()
            {
                Id = reclaim.ReclaimNumber,
                ReclaimText = reclaim.ReclaimText,
                ReclaimDate = reclaim.PublDate
            }).ToList();

            return reclaims;
        }
    }
}