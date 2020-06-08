using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Web.UI.HtmlControls;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Units.Placeable.Conference
{
    public partial class ConferenceApplicationForm : EPiServer.UserControlBase
    {

        #region Properties

        /// <summary>
        /// Get the container page of this usercontrol
        /// </summary>
        private Pages.Conference.Conference Conference
        {
            get
            {
                return (Pages.Conference.Conference)Page;
            }
        }

        public HyperLink HyperLinkRegistration
        {
            get
            {
                return RegistrationHyperLink;
            }
        }

        public HyperLink HyperLinkGroupRegistration
        {
            get
            {
                return GroupRegistrationHyperLink;
            }
        }

        public HyperLink HyperLinkPDFForm
        {
            get
            {
                return PDFFormHyperLink;
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ((DiTemplatePage)Page).UserMessageControl = UserMessageControl;
        }
    }
}