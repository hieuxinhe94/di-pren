using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class LanguageLink : EPiServer.UserControlBase
    {
        #region Properties

        public string languageURL { get; set; }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("LanguageEnglish") && IsValue("LanguagePage"))
            {
                SwedishPlaceHolder.Visible = true;
            }
            else if (!IsValue("LanguageEnglish") && IsValue("LanguagePage"))
            {
                EnglishPlaceHolder.Visible = true;
            }

            if (IsValue("LanguagePage"))
            {
                PageData pd = EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference);
                languageURL = pd.LinkURL;
            }
            else
            {
                languageURL = String.Empty;
            }
        }
    }
}