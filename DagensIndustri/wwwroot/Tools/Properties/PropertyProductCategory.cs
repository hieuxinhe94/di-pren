using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Web.PropertyControls;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Tools.Properties
{
    [Serializable]
    [PageDefinitionTypePlugIn]
    public class PropertyProductCategory : PropertyString
    {
        /// <summary>
        /// Create an instance of PropertyProductCategoryControl that is used
        /// for displaying the property.
        /// </summary>
        /// <returns></returns>
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyProductCategoryControl();
        }
    }

    public class PropertyProductCategoryControl : PropertyDataControl
    {
        DropDownList ddlProductCategories;

        /// <summary>
        /// Gets the PropertyProductCategory instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public PropertyProductCategory PropertyProductCategory
        {
            get
            {
                return PropertyData as PropertyProductCategory;
            }
        }

        /// <summary>
        /// Get a value either this property supports On Page Edit mode or not.
        /// </summary>
        public override bool SupportsOnPageEdit
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Creates an edit control.
        /// </summary>
        public override void CreateEditControls()
        {
            ddlProductCategories = new DropDownList();
            ApplyControlAttributes(ddlProductCategories);
            Controls.Add(ddlProductCategories);
            SetupEditControls();
        }

        /// <summary>
        ///  Initialize the value of the Dropdown list control.
        /// </summary>
        protected override void SetupEditControls()
        {
            ddlProductCategories.Items.Clear();

            PageReference pr = EPiFunctions.SettingsPageSetting(CurrentPage, "ProductCategories") as PageReference;
            if (pr != null)
            {
                ddlProductCategories.Items.Add(new ListItem("", ""));
                PageDataCollection pdColl = DataFactory.Instance.GetChildren(pr);
                foreach (PageData pd in pdColl)
                {
                    string productCategory = pd.PageName.Replace("[", "");
                    productCategory = productCategory.Replace("]", "");
                    ddlProductCategories.Items.Add(new ListItem(productCategory));
                }

                ddlProductCategories.SelectedValue = PropertyProductCategory.Value as string;
            }
        }

        /// <summary>
        /// Applies changes for the posted data to the page's properties. 
        /// </summary>
        public override void ApplyEditChanges()
        {
            base.SetValue(ddlProductCategories.SelectedValue);
        }
    }
}