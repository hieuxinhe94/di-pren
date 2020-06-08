using System.Collections.Generic;
using Pren.Web.Business.DataAccess;

namespace Pren.Web.Business.Campaign.Usp
{
    public interface IUspHandler
    {
        List<CampaignUspProduct> GetUspProducts();
        List<CampaignUspText> GetUspTexts(int uspProductId);
    }
}
