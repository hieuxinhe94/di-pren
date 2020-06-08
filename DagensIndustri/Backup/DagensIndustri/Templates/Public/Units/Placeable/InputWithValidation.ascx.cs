using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Core.Html;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class InputWithValidation : EPiServer.UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                InitializeTextArea();

                PhReqVal.Visible = Validate;
                ReqVal.ErrorMessage = Translate(HeadingTranslateKey) + " " + Translate("/various/validators/mandatory");

                if (!string.IsNullOrEmpty(ValidationGroup))
                    ReqVal.ValidationGroup = ValidationGroup;

                PhRegExpVal.Visible = RegularExpressionValidate;
                RegVal.ErrorMessage = Translate(HeadingTranslateKey) + " " + Translate("/various/validators/format");
                if (!string.IsNullOrEmpty(ValidationGroup))
                    RegVal.ValidationGroup = ValidationGroup;
                if (!string.IsNullOrEmpty(ValidationExpression))
                    RegVal.ValidationExpression = ValidationExpression;

                if (!string.IsNullOrEmpty(CssClass))
                    TxtControl.CssClass = CssClass;
            }
        }

        /// <summary>
        /// Set properties on text area
        /// </summary>
        private void InitializeTextArea()
        {
            if (!string.IsNullOrEmpty(CssClass))
                TxtControl.CssClass = CssClass;
            if (Rows > 0)
                TxtControl.Rows = Rows;
            if (!Width.IsEmpty)
                TxtControl.Width = Width;
            if (!string.IsNullOrEmpty(TextMode.ToString()))
                TxtControl.TextMode = TextMode;
            if (MaxLength > 0)
                TxtControl.MaxLength = MaxLength;
            if (!string.IsNullOrEmpty(ToolTip))
                TxtControl.ToolTip = Translate(ToolTip);
        }

        /// <summary>
        /// Sets validationexpression on regexpvalidator, default expression is email
        /// </summary>
        public string ValidationExpression { get; set; }

        /// <summary>
        /// If true. All html will be removed on control.value
        /// </summary>
        public bool StripHtml { get; set; }

        /// <summary>
        /// If true. A required validator will check text area
        /// </summary>
        public bool Validate { get; set; }

        /// <summary>
        /// Get/set validation group for validators
        /// </summary>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// If true. A regular expression validator will check for valid email address in text area
        /// </summary>
        public bool RegularExpressionValidate { get; set; }

        /// <summary>
        /// Get/set cssclass on text area
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Get/set rows on text area
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Get/set width on text area
        /// </summary>
        public Unit Width { get; set; }

        /// <summary>
        /// Get/set textmode on text area
        /// </summary>
        public TextBoxMode TextMode { get; set; }

        /// <summary>
        /// Get/set maxlength on text area
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Get/set translate key for heading. Key will be used by Translate object to render heading above text area
        /// </summary>
        public string HeadingTranslateKey { get; set; }

        /// <summary>
        /// Get/set tooltip on text area
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Gets value from text area. Html-code will be removed.
        /// </summary>
        public string Value
        {
            get
            {
                if (StripHtml)
                    return TextIndexer.StripHtml(TxtControl.Text, TxtControl.Text.Length);
                else
                    return TxtControl.Text;
            }
            set
            {
                TxtControl.Text = value;
            }
        }

        public RequiredFieldValidator GetValidationControl()
        {
            return ReqVal;
        }

        public RegularExpressionValidator GetRegularValidationControl()
        {
            return RegVal;
        }
    }
}