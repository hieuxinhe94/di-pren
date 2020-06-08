namespace Pren.Web.Business.ServicePlus.Models
{
    public class BizSubscriptionDefinition
    {
        public string Id { get; set; }
        public string ExternalProductCode { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
    }
}
