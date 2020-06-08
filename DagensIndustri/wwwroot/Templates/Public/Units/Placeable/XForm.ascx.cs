using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.XForms;
using EPiServer.XForms.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class XFormControl : EPiServer.UserControlBase
    {
        private XForm _form;
        private string _xFormProperty;
        private string _heading;
        private string _headingProperty;
        private bool _showStatistics = false;

        /// <summary>
        /// Used if xform is located on another page than CurrentPage
        /// </summary>
        public PageData XFormPage { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SwitchButton.Visible = EnableStatistics;

            if (Form == null)
            {
                return;
            }

            SetupForm();
        }

        public void FormControl_AfterSubmitPostedData(object sender, SaveFormDataEventArgs e)
        {
            if (EnableStatistics)
            {
                SwitchView(null, null);
                SwitchButton.Visible = false;
            }
        }

        /// <summary>
        /// Toggles the form and statistics views
        /// </summary>
        protected void SwitchView(object sender, System.EventArgs e)
        {
            if (StatisticsPanel.Visible)
            {
                StatisticsPanel.Visible = false;
                SwitchButton.Text = Translate("/form/showstat");
            }
            else
            {
                Statistics.DataBind();
                NumberOfVotes.Text = String.Format(Translate("/form/numberofvotes"), Statistics.NumberOfVotes);
                StatisticsPanel.Visible = true;
                SwitchButton.Text = Translate("/form/showform");
            }
            FormPanel.Visible = !StatisticsPanel.Visible;
        }

        public void SetupForm()
        {
            FormControl.FormDefinition = Form;
            FormControl.AfterSubmitPostedData += new SaveFormDataEventHandler(FormControl_AfterSubmitPostedData);
        }

        /// <summary>
        /// Gets or sets the XForm property
        /// </summary>
        public XForm Form
        {
            get
            {
                if (_form == null)
                {
                    string formGuid = XFormPage != null ? XFormPage[XFormProperty] as string : CurrentPage[XFormProperty] as string;
                    //string formGuid = CurrentPage[XFormProperty] as string;
                    if (!String.IsNullOrEmpty(formGuid))
                    {
                        _form = XForm.CreateInstance(new Guid(formGuid));
                    }
                }
                return _form;
            }
            set
            {
                _form = value;
            }
        }

        /// <summary>
        /// Name of the page property pointing out the current XForm
        /// </summary>
        /// <value>The name of the page property used for the XForm</value>
        public string XFormProperty
        {
            get { return _xFormProperty; }
            set { _xFormProperty = value; }
        }

        /// <summary>
        /// The heading above the XForm
        /// </summary>
        /// <value>The heading that should be shown above the XForm</value>
        /// <remarks>If this property hasn't been set the page property pointed out by HeadingProperty will be used instead.</remarks>
        public string Heading
        {
            get
            {
                if (_heading == null)
                {
                    if (HeadingProperty != null)
                    {
                        _heading = CurrentPage[HeadingProperty] as string;
                    }
                }
                return _heading;
            }
            set { _heading = value; }
        }

        /// <summary>
        /// Name of the page property that should be used to generate the form heading
        /// </summary>
        /// <value>The page property name that should be used to generate the form heading</value>
        /// <remarks>If neither Heading or HeadingProperty are set, the form will be shown without any heading</remarks>
        public string HeadingProperty
        {
            get { return _headingProperty; }
            set { _headingProperty = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether XForm statistics should be shown.
        /// </summary>
        /// <value><c>true</c> if statistics should be shown otherwise, <c>false</c>.</value>
        public bool ShowStatistics
        {
            get { return _showStatistics; }
            set { _showStatistics = value; }
        }

        /// <summary>
        /// Enables XForm statistics
        /// </summary>
        protected bool EnableStatistics
        {
            get
            {
                return CurrentPage["EnableStatistics"] == null
                    ? false
                    : (bool)CurrentPage["EnableStatistics"];
            }
        }
    }
}