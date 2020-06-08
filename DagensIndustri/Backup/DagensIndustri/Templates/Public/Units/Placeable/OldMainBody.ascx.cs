using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class OldMainBody : EPiServer.UserControlBase
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PhHeading.Visible = !HideHeading;
            PhBody.Visible = !HideBody;

        }

        /// <summary>
        /// Returns the property Heading if set; otherwise the PageName is returned.
        /// </summary>
        protected string Heading 
        {
            get 
            {
                if (CurrentPage["Heading"] != null)
                {
                    return CurrentPage.Property["Heading"].ToWebString();
                }
                else
                {
                    return CurrentPage.Property["PageName"].ToWebString();
                }
            }
        }

        private bool _HideHeading;

        /// <summary>
        /// Gets or sets a value indicating whether [hide header].
        /// </summary>
        /// <value><c>true</c> if [hide header]; otherwise, <c>false</c>.</value>
        public bool HideHeading
        {
            get
            {
                return _HideHeading || IsValue("HideHeading");
            }
            set
            {
                _HideHeading = value;
            }
        }

        private bool _HideBody = false;
        /// <summary>
        /// Gets or sets a value indicating whether [hide body].
        /// </summary>
        /// <value><c>true</c> if [hide body]; otherwise, <c>false</c>.</value>
        public bool HideBody
        {
            get
            {
                return _HideBody;
            }
            set
            {
                _HideBody = value;
            }        
        }
    }
}