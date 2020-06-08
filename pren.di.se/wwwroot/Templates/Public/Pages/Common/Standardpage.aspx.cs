using System;
using System.Collections.Generic;
using System.Linq;

namespace PrenDiSe.Templates.Public.Pages.Common
{
    public partial class Standardpage : EPiServer.TemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsValue("Logotype"))
            {
                imgLogotype.ImageUrl = CurrentPage["Logotype"] as string;
                imgLogotype.Visible = true;
            }
        }
    }
}