using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class ReadTodaysPaper : EPiServer.UserControlBase
    {
        public string ReadTodaysPaperLink { get; set; }

        public string ettansURL {get; set;}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ettansURL = "/Tools/Operations/Stream/ShowImage.aspx?w=98&what=ettan";

            if (IsValue("ReadTodaysPaperHeading"))
            {
                ReadTodaysPaperHeadingLiteral.Text = CurrentPage["ReadTodaysPaperHeading"].ToString();
            }
            
            // Is the PDF version open for everyone?
            //
            bool? bIsOpen = (bool?)EPiFunctions.SettingsPageSetting(null, "PdfsOpenForAllVisitors");
            if (bIsOpen.HasValue && bIsOpen.Value == true)
            {
                //ReadTodaysPaperTextLiteral.Text = "<p>Idag öppen för alla pga<br>distributionsproblem.</p>";
                if (IsValue("ReadTodaysPaperOpenText"))
                    ReadTodaysPaperTextLiteral.Text = CurrentPage["ReadTodaysPaperOpenText"].ToString();
            }
            else
            {
                if (IsValue("ReadTodaysPaperText"))
                    ReadTodaysPaperTextLiteral.Text = CurrentPage["ReadTodaysPaperText"].ToString();
            }

            if (IsValue("ReadTodaysPaperPage"))
            {
                PageData ReadTodaysPaperPage = EPiServer.DataFactory.Instance.GetPage(CurrentPage["ReadTodaysPaperPage"] as PageReference);
                ReadTodaysPaperLink = ReadTodaysPaperPage.LinkURL;
            }

            ReadTodaysPaperLinkText.Text = "Läs tidningen";
        }
    }
}