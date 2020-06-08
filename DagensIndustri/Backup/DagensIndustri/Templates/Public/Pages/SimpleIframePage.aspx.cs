using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class SimpleIframePage : EPiServer.TemplatePage
    {
        public string frameSource { get; set; }
        public string frameHeight { get; set; }
        public string frameWidth { get; set; }
        public string frameBackgroundColor { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("FrameSource"))
            {
                frameSource = CurrentPage["FrameSource"].ToString();
            }
            else 
            {
                frameSource = string.Empty;
            }

            if (IsValue("FrameHeight"))
            {
                frameHeight = CurrentPage["FrameHeight"].ToString();
            }
            else
            {
                frameHeight = string.Empty;
            }

            if (IsValue("FrameWidth"))
            {
                frameWidth = CurrentPage["FrameWidth"].ToString();
            }
            else
            {
                frameWidth = string.Empty;
            }

            if (IsValue("FrameBackgroundColor"))
            {
                frameBackgroundColor = CurrentPage["FrameBackgroundColor"].ToString();
            }
            else
            {
                frameBackgroundColor = "#fffff";
            }

        }
    }
}