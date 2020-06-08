using System;
using EPiServer.PlugIn;
using System.Web.UI;
using System.Web;
using EPiServer.Core;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DagensIndustri.Tools.Classes.Campaign;
using DagensIndustri.Templates.Public.Units.Static;

namespace DagensIndustri.Tools.Properties
{
    /// <summary>
    /// Custom PropertyData implementation
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn(DisplayName = "Erbjudandekoder")]
    public class PropertyOfferCode : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyOfferCodeControl();
        }

    }

    /// <summary>
    /// PropertyControl implementation used for rendering PropertyDivider data.
    /// </summary>
    public class PropertyOfferCodeControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        ScriptManager scriptManager = null;
        OfferCodeControl editControl;

        public override void ApplyChanges()
        {
            //Save changes to db

            //If pagelink doesn't exist, don't save
            //if (CurrentPage.PageLink.ID > 0)
            //{
            //    ListItemCollection items = editControl.GetSelectedOfferCodes();

            //    if (items.Count > 0)
            //    {
            //        CurrentPage.ClearOfferCodes();

            //        foreach (ListItem item in items)
            //        {
            //            CurrentPage.InsertOfferCode(item.Value);
            //        }
            //    }
            //    else
            //        AddErrorValidator("\"Erbjudandekod\" kan inte vara tom.");
            //}
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
                    this.Controls.Add(scriptManager);
                }
            }

            //The page must be save before you can choose offercode
            if (CurrentPage.PageLink.ID == 0)
            {
                Label lbl = new Label();
                lbl.Text = "Du måste spara sidan innan du kan ange erbjudandekod";
                this.Controls.Add(lbl);
            }
            else
            {
                editControl = (OfferCodeControl)Page.LoadControl("/Tools/Properties/OfferCodeControl.ascx");
                //Fill list with all available
                editControl.FillAvailableOfferCodes();
                //editControl.FillSelectedOfferCodes(CurrentPage);

                this.Controls.Add(editControl);
            }
        }

        /// <summary>
        /// Gets the PropertyGender instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public PropertyOfferCode PropertyOfferCode
        {
            get
            {
                return PropertyData as PropertyOfferCode;
            }
        }
    }
}