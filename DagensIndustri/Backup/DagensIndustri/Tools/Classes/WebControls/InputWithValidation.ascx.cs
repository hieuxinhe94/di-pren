using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core.Html;

namespace DagensIndustri.Tools.Classes.WebControls
{
    public partial class InputWithValidation : UserControl, IEditableTextControl
    {
        public event EventHandler TextChanged;

        #region Constants
        private const string DEFAULT_DATE_PATTERN = "(^(19|20)\\d\\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$)";
        private const string DEFAULT_TIME_PATTERN = @"(^(20|21|22|23|[01]\d|\d)(([:][0-5]\d){1,2})$)";  //HH:MM
        private const string DEFAULT_EMAIL_PATTERN = "^([_A-Za-z0-9-])+(\\.[_A-Za-z0-9-]+)*@(([A-Za-z0-9]){1,}([-.])?([A-Za-z0-9]){1,})+((\\.)?([A-Za-z0-9]){1,}([-])?([A-Za-z0-9]){1,})*(\\.[A-Za-z]{2,4})$";
        private const string DEFAULT_NUMERIC_PATTERN = "^[0-9]+$";
        private const string DEFAULT_PASSWORD_PATTERN = "^[\\W\\w]{6,}$";     //"^[a-zA-Z0-9_]{6,}$";
        private const string DEFAULT_HARDPASSWORD_PATTERN = "^[a-zA-Z0-9_]{6,}$";
        private const string DEFAULT_TEXT_PATTERN = "^.{1,}$";
        private const string DEFAULT_TELEPHONE_PATTERN = "(^([0-9-\\s\\+]){5,}$)";
        private const string DEFAULT_ORGNUMBER_PATTERN = "(^[\\d]{10}$|^[\\d]{6}\\-[\\d]{4}$)";
        private const string DEFAULT_ZIP_CODE = "(^[0-9]{5}$)";
        private const string DEFAULT_URL = "(^[http\\://]*[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,3}(/\\S*)?$)";
        //private const string DEFAULT_SOCIAL_SECURITY_NO = "(^[\\d]{10}$|^[\\d]{6}\\-[\\d]{4}$)";
        //private const string DEFAULT_SOCIAL_SECURITY_NO = "(^[\\d]{12}$)";
        private const string DEFAULT_SOCIAL_SECURITY_NO = "(^[\\d]{8}$)";       //120109 only YYYYMMDD required for gold membership
        private const string DEFAULT_BIRTH_NO = "(^[\\d]{10}$)";                //YYMMDDXXXX required for student verification
        #endregion 

        #region Enum
        public enum InputType
        {
            Text = 0,
            Password,
            HardPassword,
            Date,
            Email,
            Numeric,
            OrgNumber,
            Telephone,
            ZipCode,
            Url,
            SocialSecurityNumber,
            BirthNo,
            CheckBox,
            Time
        }
       
        #endregion 

        #region Properties
        public string InputClientID 
        {
            get
            {
                return IsTextArea ? TextArea.ClientID : Input.ClientID;
            }
        }
        /// <summary>
        /// Get/set name of the input control
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/set title on input control
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get/set cssclass on input control
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Get/set whether value is required
        /// </summary>
        private bool _required;
        public bool Required 
        {
            get
            {
                return _required;
            }
            set
            {
                _required = value;
                ModifyAttribute("required", "required", _required);
            }
        }
        
        /// <summary>
        /// Get/set whether value is required
        /// </summary>
        public bool IsTextArea { get; set; } 

        /// <summary>
        /// Get/set valid pattern for the input control
        /// </summary>
        public string Pattern { get; set; }        

        /// <summary>
        /// Get/set message to be displayed if input data not valid
        /// </summary>
        public string DisplayMessage { get; set; }

        /// <summary>
        /// Get/set min value allowed as input
        /// </summary>
        private string _minValue;
        public string MinValue 
        { 
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = !string.IsNullOrEmpty(value) ? value.Trim() : null;
                if (TypeOfInput == InputType.Date)
                {
                    ModifyAttribute("min", _minValue, !string.IsNullOrEmpty(_minValue));
                }
            }
        }

        /// <summary>
        /// Get/set max value allowed as input
        /// </summary>
        private string _maxValue;
        public string MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = !string.IsNullOrEmpty(value) ? value.Trim() : null;
                if (TypeOfInput == InputType.Date)
                {
                    ModifyAttribute("max", _maxValue, !string.IsNullOrEmpty(_maxValue));
                }
            }
        }

        /// <summary>
        /// Get/set if autocomplete should be allowed or not
        /// </summary>
        public bool AutoComplete { get; set; }

        /// <summary>
        /// Get/set if input should be disabled llowed or not
        /// </summary>
        private bool _disabled;
        public bool Disabled 
        { 
            get
            {
                return _disabled;
            }
            set
            {
                _disabled = value;
                ModifyAttribute("disabled", "disabled", _disabled);
            }
        }
        
        /// <summary>
        /// Get/set type of input control
        /// </summary>
        private InputType typeOfInput;
        public InputType TypeOfInput 
        {
            get
            {
                return typeOfInput;
            }
            set
            {
                typeOfInput = value;
            }
        }

        /// <summary>
        /// If true, all html will be removed on control.value
        /// </summary>
        public bool StripHtml { get; set; }

        /// <summary>
        /// Gets/set value from/to input control
        /// </summary>
        public string Text
        {
            get
            {
                if (IsTextArea)
                {
                    return StripHtml ? TextIndexer.StripHtml(TextArea.Value, TextArea.Value.Length).Trim() : TextArea.Value.Trim();
                }
                else
                {
                    return StripHtml ? TextIndexer.StripHtml(Input.Value, Input.Value.Length).Trim() : Input.Value.Trim();
                }
            }
            set
            {
                if (IsTextArea)
                {
                    TextArea.Value = value;
                }
                else
                {
                    Input.Value = value;
                }
            }
        }

        public bool Checked
        {
            get
            {                
                bool isChecked = false;
                if (TypeOfInput == InputType.CheckBox && !string.IsNullOrEmpty(Input.Attributes["value"]))
                {
                    if (Input.Attributes["value"] == "on")
                    {
                        isChecked = true;
                        Input.Attributes["checked"] = "checked";
                    }
                    else
                    {
                        Input.Attributes.Remove("checked");
                    }
                }

                return isChecked;
            }
            set
            {
                if (TypeOfInput == InputType.CheckBox)
                {
                    if (value)
                    {
                        Input.Attributes["checked"] = "checked";
                    }
                    else
                    {
                        Input.Attributes.Remove("checked");
                    }
                }
            }
        }

        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsTextArea)
            {
                InputPlaceHolder.Visible = false;
                TextAreaPlaceHolder.Visible = true;
            }
            else
            {
                InputPlaceHolder.Visible = true;
                TextAreaPlaceHolder.Visible = false;              
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Initialize();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initialize properties used by input control
        /// </summary>
        private void Initialize()
        {
            //Set default pattern if not given
            switch (TypeOfInput)
            {
                case InputType.Date:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_DATE_PATTERN;
                    break;
                case InputType.Time:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_TIME_PATTERN;
                    break;
                case InputType.Email:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_EMAIL_PATTERN;
                    //TODO: Nazgol om man har angett MinValue och/eller MaxValue ska man använda sig av en string.format(DYNAMIC_EMAIL_Pattern,...) och skicka med min och max
                    break;
                case InputType.Password:
                    //Set type of input
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_PASSWORD_PATTERN;
                    Input.Attributes.Add("type", InputType.Password.ToString());
                    //TODO: Nazgol om man har angett MinValue och//eller MaxValue ska man använda sig av en string.format(TEXT_PATTERN,...) och skicka med min och max
                    break;
                case InputType.HardPassword:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_HARDPASSWORD_PATTERN;
                    Input.Attributes.Add("type", InputType.Password.ToString());
                    break;
                case InputType.Numeric:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_NUMERIC_PATTERN;
                    //Pattern = SetNumericPattern();
                    break;
                case InputType.Telephone:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_TELEPHONE_PATTERN;
                    break;
                case InputType.OrgNumber:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_ORGNUMBER_PATTERN;
                    break;
                case InputType.ZipCode:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_ZIP_CODE;
                    break;
                case InputType.Url:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_URL;
                    break;
                case InputType.SocialSecurityNumber:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_SOCIAL_SECURITY_NO;
                    break;
                case InputType.BirthNo:
                    Pattern = (!string.IsNullOrEmpty(Pattern)) ? Pattern : DEFAULT_BIRTH_NO;
                    break;
                case InputType.Text:
                    //Set type of input
                    Pattern = SetTextPattern();
                    Input.Attributes.Add("type", InputType.Text.ToString());
                    break;
                case InputType.CheckBox:
                    Input.Attributes.Add("type", InputType.CheckBox.ToString());
                    break;
                default:
                    break;
            }

            DataBind();
        }        

        private string SetTextPattern()
        {
            string newPattern = Pattern;
            if (string.IsNullOrEmpty(newPattern))
            {
                newPattern = DEFAULT_TEXT_PATTERN;
                if (!string.IsNullOrEmpty(MinValue) || !string.IsNullOrEmpty(MaxValue))
                {
                    if (string.IsNullOrEmpty(MinValue))
                        MinValue = Required ? "1" : "0";
                    else if (MinValue == "0" && Required)
                        MinValue = "1";
                    
                    newPattern = string.Format("^.{{{0},{1}}}$", MinValue, MaxValue);
                }
            }
            return newPattern;
        }

        //TODO: Nazgol Fixa den är inte riktigt korrekt. 
        private string SetNumericPattern()
        {
            string newPattern = Pattern;
            if (string.IsNullOrEmpty(newPattern))
            {
                newPattern = DEFAULT_NUMERIC_PATTERN;
                if (!string.IsNullOrEmpty(MinValue) || !string.IsNullOrEmpty(MaxValue))                
                {
                    if (string.IsNullOrEmpty(MinValue))
                        MinValue = Required ? "1" : "0";

                    newPattern = string.Format("^[0-9]{{{0}:{1}}}$", MinValue, MaxValue.Length);
                }
            }
            return newPattern;
        }

        /// <summary>
        /// Modifies a certain attribute on either TextArea or Input control.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="add"></param>
        private void ModifyAttribute(string attribute, string value, bool add)
        {
            if (add)
            {
                if (IsTextArea)
                    TextArea.Attributes.Add(attribute, value);
                else
                    Input.Attributes.Add(attribute, value);
            }
            else
            {
                if (IsTextArea)
                    TextArea.Attributes.Remove(attribute);
                else
                    Input.Attributes.Remove(attribute);
            }
        }
        #endregion        
    }
}