using EPiServer.Shell;
using Pren.Web.Models.Pages;

namespace Pren.Web.Business.Descriptors.UI
{
    [UIDescriptorRegistration]
    public class ContainerPageUIDescriptor : UIDescriptor<ContainerPage>
    {
        public ContainerPageUIDescriptor() : base(ContentTypeCssClassNames.Container)
        {
            DefaultView = CmsViewNames.AllPropertiesView;
        }
    }
}
