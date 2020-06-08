using System;
using System.Collections.Generic;
using System.Linq;

namespace PrenDiSe.Templates.Public.Units.Static
{
    public partial class GoogleAnalytics : System.Web.UI.UserControl
    {
        public bool WriteTrackPageview { get; set; }

        protected string TrackPageViewContent { get; set; }

        public GoogleAnalytics()
        {
            WriteTrackPageview = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            TrackPageViewContent = WriteTrackPageview ? "_gaq.push(['_trackPageview']);" : string.Empty;
        }
    }
}