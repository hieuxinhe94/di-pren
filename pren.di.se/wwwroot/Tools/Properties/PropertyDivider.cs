using System;
using System.Web.UI;

using EPiServer.Core;
using EPiServer.PlugIn;

namespace PrenDiSe.Tools.Properties
{
    /// <summary>
    /// Custom PropertyData implementation
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn]
    public class PropertyDivider : PropertyString
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyDividerControl();
        }

    }

    /// <summary>
    /// PropertyControl implementation used for rendering PropertyDivider data.
    /// </summary>
    public class PropertyDividerControl : EPiServer.Web.PropertyControls.PropertyStringControl
    {
        /*
        Override CreateXXXControls to control the appearance of the property data in different rendering conditions.

        public override void CreateDefaultControls()        - Used when rendering the view mode.
        public override void CreateEditControls()           - Used when rendering the property in edit mode.
        public override void CreateOnPageEditControls()     - used when rendering the property for "On Page Edit".

        */

        public override void ApplyChanges()
        {
        }

        public override void CreateEditControls()
        {
            Controls.Add(new LiteralControl("</td></tr><tr><td style=\"border-top: 2px solid #000;font-size:1px;line-height:6px\" colspan=\"2\">&nbsp;"));
        }

        /// <summary>
        /// Gets the PropertyGender instance for this IPropertyControl.
        /// </summary>
        /// <value>The property that is to be displayed or edited.</value>
        public PropertyDivider PropertyDivider
        {
            get
            {
                return PropertyData as PropertyDivider;
            }
        }
    }
}