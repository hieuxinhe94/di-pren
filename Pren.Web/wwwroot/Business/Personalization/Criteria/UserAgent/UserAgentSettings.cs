using System.ComponentModel.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Web.Mvc.VisitorGroups;

namespace Pren.Web.Business.Personalization.Criteria.UserAgent
{
    public class UserAgentSettings : CriterionModelBase
    {
        [DojoWidget(
            SelectionFactoryType = typeof(EnumSelectionFactory),
            AdditionalOptions = "{ selectOnClick: true }"),
            Required]
        public CustomCompare Contains { get; set; }

        [Required]
        public string UserAgentValue { get; set; }

        public override ICriterionModel Copy()
        {
            return ShallowCopy();
        }
    }

    public enum CustomCompare
    {
        Equal,
        NotEqual
    }
}