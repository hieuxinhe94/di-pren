using System;
using System.Collections.Generic;
using System.Text;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Web.PropertyControls;
using EPiServer.Web.WebControls;
using System.Web.UI.WebControls;


namespace DagensIndustri.Tools.Properties
{
    /// <summary>
    /// Custom PropertyData implementation
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn]
    public class MasterPageSelector : EPiServer.Core.PropertyString
    {
        public override EPiServer.Core.IPropertyControl CreatePropertyControl()
        {
            return new MasterPageSelectorControl();
        }
    }

        /// <summary>
    /// PropertyControl implementation used for rendering CategoryDropDown data.
    /// </summary>
    public class MasterPageSelectorControl : EPiServer.Web.PropertyControls.PropertyTextBoxControlBase
    {
        public override bool SupportsOnPageEdit
        {
            get
            {
                return false;
            }
        }

        protected System.Web.UI.WebControls.DropDownList ddl;


        public override void CreateEditControls()
        {
            ddl = new System.Web.UI.WebControls.DropDownList();
            ddl.Items.Add(new ListItem("", ""));
            ddl.Items.Add(new ListItem("DI MasterPage", "/Templates/Public/Styles/dagensindustri.css"));
            ddl.Items.Add(new ListItem("DI Gold MasterPage", "/Templates/Public/Styles/diguld.css"));

            ddl.SelectedValue = MasterPageDropDown.Value as string;

            this.ApplyControlAttributes(ddl);
            Controls.Add(ddl);
        }

        public override void ApplyEditChanges()
        {
            SetValue(ddl.SelectedValue);
        }


        /// <summary>
        /// Gets the CategoryDropDown instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public MasterPageSelector MasterPageDropDown
        {
            get
            {
                return PropertyData as MasterPageSelector;
            }
        }
    }
}