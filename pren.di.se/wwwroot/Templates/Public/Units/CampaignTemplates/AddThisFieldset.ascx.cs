using System;
using System.Linq;
using System.Text;

namespace PrenDiSe.Templates.Public.Units.CampaignTemplates
{
    public partial class AddThisFieldset : System.Web.UI.UserControl
    {
        // Name of template used in AddThis email functionality. Edit templates inside AddThis account!
        public string EmailTemplateName { get; set; }
        public string Headline { get; set; }
        protected string ShareSettingsOutput { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!string.IsNullOrEmpty(EmailTemplateName))
            {
                ShareSettingsOutput = string.Format("var addthis_share = {{email_template: \"{0}\"}};", EmailTemplateName);
            }
        }
    }
}