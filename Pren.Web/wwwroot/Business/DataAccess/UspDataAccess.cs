using System.Collections.Generic;
using System.Linq;
using Pren.Web.Business.Campaign.Usp;

namespace Pren.Web.Business.DataAccess
{
    class UspDataAccess : IUspHandler
    {
        public List<CampaignUspProduct> GetUspProducts()
        {
            var dataContext = new Pren_Web_MiscDataContext();
            var uspProducts = (from p in dataContext.CampaignUspProducts select p).ToList();
            return uspProducts;
        }

        public List<CampaignUspText> GetUspTexts(int uspProductId)
        {
            var texts = new List<CampaignUspText>();
            using (var dataContext = new Pren_Web_MiscDataContext())
            {
                var productrelations = dataContext.CampaignUspProductTextRelations
                    .Where(r => r.fkUspProductId == uspProductId);
                
                foreach (var campaignUspProductTextRelation in productrelations)
                {
                    texts.AddRange(dataContext.CampaignUspTexts
                        .Where(p => p.Id == campaignUspProductTextRelation.fkUspTextId));
                }
            }

            return texts;
        }
    }
}
