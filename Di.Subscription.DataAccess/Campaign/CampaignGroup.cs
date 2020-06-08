using Di.Common.Conversion.Attributes;
using Di.Common.Conversion.Types;

namespace Di.Subscription.DataAccess.Campaign
{
    public class CampaignGroup : IDataSetObject
    {
        [DataSet(ColumnName = "CAMPID")]
        public string CampaignId { get; set; }
        [DataSet(ColumnName = "CAMPNAME")]
        public string CampaignName { get; set; }
        [DataSet(ColumnName = "CAMPNO")]
        public int CampaignNumber { get; set; }
    }
}
