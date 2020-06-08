using System;
using System.Collections.Generic;
using System.Linq;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class PapersMobile : EPiServer.TemplatePage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((MasterPages.MasterPage)Page.Master).ShowHeader(false);
            ((MasterPages.MasterPage)Page.Master).ShowFooter(false);
        }
    }
}