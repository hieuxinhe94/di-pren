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
    [PageDefinitionTypePlugIn(DisplayName = "Kostnader")]
    public class PropertyCost : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyCostControl();
        }

    }

    /// <summary>
    /// PropertyControl implementation used for rendering PropertyDivider data.
    /// </summary>
    public class PropertyCostControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        ScriptManager scriptManager = null;
        CostControl editControl;

        public override void ApplyChanges()
        {
            //Save changes to db

            //If pagelink doesn't exist, don't save
            //if (CurrentPage.PageLink.ID > 0)
            //{
            //    ListItemCollection items = editControl.GetCosts();

            //    if (items.Count > 0)
            //    {
            //        //Clear all costs
            //        CurrentPage.ClearCosts();
            //        //Insert costs
            //        foreach (ListItem item in items)
            //        {
            //            CurrentPage.InsertCost(item.Text, int.Parse(item.Value));
            //        }
            //    }
            //    else
            //        AddErrorValidator("\"Kostnader\" kan inte vara tom.");
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
                lbl.Text = "Du måste spara sidan innan du kan ange kostnader";
                this.Controls.Add(lbl);
            }
            else
            {
                editControl = (CostControl)Page.LoadControl("/Tools/Properties/CostControl.ascx");
                //Fill list with all available
                //editControl.FillCosts(CurrentPage);

                this.Controls.Add(editControl);
            }
        }

        /// <summary>
        /// Gets the PropertyGender instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public PropertyCost PropertyCost
        {
            get
            {
                return PropertyData as PropertyCost;
            }
        }
    }
}