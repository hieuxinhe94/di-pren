using System;
using EPiServer.PlugIn;
using System.Web.UI;
using System.Web;
using EPiServer.Core;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DagensIndustri.Tools.Classes.Campaign;
using DagensIndustri.Templates.Public.Units.Static;
using EPiServer.UI.Edit;

namespace DagensIndustri.Tools.Properties
{
    /// <summary>
    /// Custom PropertyData implementation
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn(DisplayName = "Temaspår")]
    public class PropertySeminar : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertySeminarControl();
        }

    }

    /// <summary>
    /// PropertyControl implementation used for rendering PropertyDivider data.
    /// </summary>
    public class PropertySeminarControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        ScriptManager scriptManager = null;
        SeminarControl editControl;

        public override void ApplyChanges()
        {

        }

        public override void CreateEditControls()
        {
            
            if (scriptManager == null)
            {
                //first check to see if there is a ScriptManager loaded on the page already
                scriptManager = ScriptManager.GetCurrent(this.Page);
                if (scriptManager == null)
                {
                    //since there wasn't one on the page, we create a new one and add it to the controls collection
                    scriptManager = new ScriptManager();
                    scriptManager.EnablePartialRendering = true;
                    this.Controls.Add(scriptManager);
                }
            }
            

            //The page must be save before you can choose offercode
            if (CurrentPage.PageLink.ID == 0)
            {
                Label lbl = new Label();
                lbl.Text = "Du måste spara sidan innan du kan ange temaspår";
                this.Controls.Add(lbl);
            }
            else if (CurrentPage["GetFormFromAnotherPage"] != null)
            {
                Label lbl = new Label();
                lbl.Text = "Aktiviter hämtas från" + " " + EPiServer.DataFactory.Instance.GetPage(CurrentPage["LanguagePage"] as PageReference).PageName;
                this.Controls.Add(lbl);
            }
            else
            {
                editControl = (SeminarControl)Page.LoadControl("/Tools/Properties/SeminarControl.ascx");
                editControl.InitConference(CurrentPage);
                editControl.ShowEvents();
                this.Controls.Add(editControl);
            }
        }

        /// <summary>
        /// Gets the PropertyGender instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public PropertySeminar PropertySeminar
        {
            get
            {
                return PropertyData as PropertySeminar;
            }
        }
    }
}