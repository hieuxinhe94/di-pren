using System;
using EPiServer.PlugIn;
using System.Web.UI;
using System.Web;
using System.Data;
using EPiServer.Core;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.Campaign;
using DagensIndustri.Templates.Public.Units.Static;

namespace DagensIndustri.Tools.Properties
{
    /// <summary>
    /// Custom PropertyData implementation
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn(DisplayName = "Kampanjtyp")]
    public class PropertyCampaignType : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyCampaignTypeControl();
        }

    }

    /// <summary>
    /// PropertyControl implementation used for rendering PropertyDivider data.
    /// </summary>
    public class PropertyCampaignTypeControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        ScriptManager scriptManager = null;
        TypeControl editControl;

        public override void ApplyChanges()
        {
            //Save changes to db

            //If pagelink doesn't exist, don't save
            //if (CurrentPage.PageLink.ID > 0)
            //{
            //    if (!string.IsNullOrEmpty(editControl.GetValue()))
            //    {
            //        CurrentPage.InsertType(int.Parse(editControl.GetValue()), editControl.GetComment());
            //    }
            //    else
            //        AddErrorValidator("\"Kampanjtyp\" kan inte vara tom.");
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

            //The page must be save before you can choose targetgroup
            if (CurrentPage.PageLink.ID == 0)
            {
                Label lbl = new Label();
                lbl.Text = "Du måste spara sidan innan du kan ange kampanjtyp";
                this.Controls.Add(lbl);
            }
            else
            {
                editControl = (TypeControl)Page.LoadControl("/Tools/Properties/TypeControl.ascx");
                //Fill list with types
                editControl.FillTypes();
                //Set selected value
                editControl.SetSelectedValue(CurrentPage);

                this.Controls.Add(editControl);
            }
        }

        ///// <summary>
        ///// Gets the current page
        ///// </summary>
        //public static PageData CurrentPage
        //{
        //    get { return ((PageBase)HttpContext.Current.Handler).CurrentPage; }
        //}


        /// <summary>
        /// Gets the PropertyGender instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public PropertyCampaignType PropertyCampaignType
        {
            get
            {
                return PropertyData as PropertyCampaignType;
            }
        }
    }
}
