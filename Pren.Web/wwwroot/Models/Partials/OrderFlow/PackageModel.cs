using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pren.Web.Models.Partials.OrderFlow
{
    public class PackageModel
    {
        public string Id { get; set; }

        public string PackageCssClass { get; set; }

        public string ImgCssClass { get; set; }

        public string ImgUrl { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string LinkText { get; set; }

        public string Price { get; set; }

        public string ListTitle { get; set; }

        public string ListDividerText { get; set; }

        public List<string> ListItems { get; set; }

        public string ListDividerTextMobile { get; set; }

        public string BottomText { get; set; }

        public List<string> ListItemsMobile { get; set; }

        public List<PackagePeriod> PeriodsPrivate { get; set; }

        public List<PackagePeriod> PeriodsBusiness { get; set; }

        /*************************************************************************/

        public bool IsPayWall { get; set; }

        public bool IsStudent { get; set; }

        public string TargetGroup { get; set; }
    }

    public class PackagePeriod
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductDescription { get; set; }

        public long CampaignNumber { get; set; }

        public bool IsDigital { get; set; }

        public bool IsTrial { get; set; }

        public bool IsTrialFree { get; set; }

        public bool IsUpgradeCampaign { get; set; }

        public bool HideCardPayment { get; set; }

        public bool HideInvoicePayment { get; set; }

        public string SubsKind { get; set; }

        public List<SummaryRow> SummaryRows { get; set; }
    }

    public class SummaryRow
    {
        public SummaryRow(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public SummaryRow()
        {

        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}