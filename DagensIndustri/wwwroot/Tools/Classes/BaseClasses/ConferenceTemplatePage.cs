using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace DagensIndustri.Tools.Classes.BaseClasses
{
    public class ConferenceTemplatePage : EPiServer.TemplatePage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);

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