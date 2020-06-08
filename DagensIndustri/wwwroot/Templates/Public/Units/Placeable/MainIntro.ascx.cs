using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class MainIntro : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string mainIntro = "";

            if (IsValue("MainIntro"))
            {
                mainIntro = CurrentPage.Property["MainIntro"].ToString();

                MainIntroLiteral.Text = EPiServer.Core.Html.TextIndexer.StripHtml(mainIntro, 0);
            }
        }
    }
}