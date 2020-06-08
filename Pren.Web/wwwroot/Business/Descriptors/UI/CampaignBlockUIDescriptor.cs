using EPiServer.Shell;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Business.Descriptors.UI
{
    [UIDescriptorRegistration]
    // ReSharper disable once InconsistentNaming
    public class CampaignBlockUIDescriptor : UIDescriptor<CampaignBlock>
    {
        public CampaignBlockUIDescriptor()            
        {
            DefaultView = CmsViewNames.AllPropertiesView;
        }
    }
}
