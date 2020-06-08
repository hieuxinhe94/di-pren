using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable.Login
{
    public partial class ForgotPassword : UserControlBase
    {
        #region Constants
        private const string REMEMBER_EMAIL = "ForgottenPasswordEmailV2";
        private const string REMEMBER_USERNAME = "ForgottenPasswordUserNameV2";
        private const string REMEMBER_CUSTOMERNUMBER = "ForgottenPasswordCusNoV2";
        #endregion 

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                //Initialize controls
                Initialize();
            }

            //Register client scripts 
            RegisterScript();            
        }

        protected void RememberDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //Change the type of input for the Remember What input control depending on what the user remembers
            switch (RememberDropDownList.SelectedValue)
            {
                case REMEMBER_EMAIL:
                    RememberWhatInput.TypeOfInput = Tools.Classes.WebControls.InputWithValidation.InputType.Email;
                    break;
                case REMEMBER_USERNAME:
                    RememberWhatInput.TypeOfInput = Tools.Classes.WebControls.InputWithValidation.InputType.Text;
                    break;
                case REMEMBER_CUSTOMERNUMBER:
                    RememberWhatInput.TypeOfInput = Tools.Classes.WebControls.InputWithValidation.InputType.Numeric;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Send mail with login info to user
        /// </summary>
        protected void SendPasswordButton_Click(object sender, EventArgs e)
        {
            string storedProcedureName = RememberDropDownList.SelectedValue;
            string inputText = RememberWhatInput.Text.Trim();

            if (string.IsNullOrEmpty(inputText))
                return;

            SendPassword(storedProcedureName, inputText);
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Register clientscripts. When "Send my Login information" is clicked, the div id request-form is saved in a hidden field. 
        /// When div for sending login information triggers a postback, the div should remain open.
        /// </summary>
        private void RegisterScript()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                return;
            
            // Create script for click on SendLoginHyperLink (hiddenfield will be set) and CancelSendLoginHyperLink (hiddenfield will be cleared)
            string script1 = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('request-form');
                                                }}),
                                                    $('#{2}').click(function () {{
                                                    $('#{1}').val('');
                                                }})
                                            }});",
                                            SendLoginHyperLink.ClientID,
                                            SendLoginHiddenField.ClientID,
                                            CancelSendLoginHyperLink.ClientID
                                            );

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SendLoginData", script1, true);

            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(SendLoginHiddenField.Value))
                {
                    string script2 = string.Format(@"$(document).ready(function() {{
                                                        $('#{0}').show();
                                                    }});",
                                                    SendLoginHiddenField.Value);

                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "SendLoginDataShow", script2, true);
                }
            }
        }

        /// <summary>
        /// Initialize the controls, set focus etc.
        /// </summary>
        private void Initialize()
        {
            //If the order of list items is changed, the TypeOfInput on RememberWhatInput has to be changed as well.
            if (RememberDropDownList != null)
            {
                RememberDropDownList.Items.Add(new ListItem(Translate("/dilogin/email"), REMEMBER_EMAIL));
                RememberDropDownList.Items.Add(new ListItem(Translate("/dilogin/username"), REMEMBER_USERNAME));
                RememberDropDownList.Items.Add(new ListItem(Translate("/dilogin/customernumber"), REMEMBER_CUSTOMERNUMBER));
            }            

            //Set the type of input of RememberWhatInput control            
            RememberWhatInput.TypeOfInput = Tools.Classes.WebControls.InputWithValidation.InputType.Email;
        }

        public void ClearHiddenFields()
        {
            SendLoginHiddenField.Value = string.Empty;
        }

        /// <summary>
        /// Set and send password to user
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="inputText"></param>
        protected void SendPassword(string storedProcedureName, string inputText)
        {
            bool userExist = false;
            string emailAddress = string.Empty;
            string userid = string.Empty;
            string passwd = string.Empty;
            SqlDataReader DR = null;

            try
            {
                DR = SqlHelper.ExecuteReader("DisePren", storedProcedureName, new SqlParameter("@strID", inputText));

                if (DR.Read())
                {
                    if (DR["result"].ToString() == "1")
                    {
                        if (DR["emailaddress"] != System.DBNull.Value)
                            emailAddress = DR["emailaddress"].ToString();
                        if (DR["userid"] != System.DBNull.Value)
                            userid = DR["userid"].ToString();
                        if (DR["passwd"] != System.DBNull.Value)
                            passwd = DR["passwd"].ToString();

                        userExist = true;
                    }
                    else if (DR["result"].ToString() == "-2")
                    {
                        //multiple users with same emailaddress
                        //result = "-2" only returned from SP ForgottenPasswordEmail
                        EPiFunctions.ShowMessage(this.Page, CurrentPage["ForgotPasswordErrorTextMultiple"] as string, false, true);
                        return;
                    }
                }

                if (userExist)
                {
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        string senderEmailAddress = CurrentPage["ForgotPasswordSender"] as string;

                        bool isValidEmailAddress = MiscFunctions.IsValidEmail(emailAddress);
                        bool isValidSenderEmailAddress = MiscFunctions.IsValidEmail(senderEmailAddress);

                        //Check valid email for both sender and receiver
                        if (isValidEmailAddress && isValidSenderEmailAddress)
                        {
                            string mailSubject = Translate("/dilogin/forgotpassword/mail/subject");
                            //if email is unique, send email instead of userid
                            string mailBody = string.Format(Translate("/dilogin/forgotpassword/mail/body"), emailAddress, passwd).Replace("[nl]", Environment.NewLine);
                            MiscFunctions.SendMail(senderEmailAddress, emailAddress, mailSubject, mailBody, false);
                            EPiFunctions.ShowMessage(this.Page, "/dilogin/forgotpassword/sendpasswordconfirmtext", true, false);
                        }
                        else if (!isValidEmailAddress)
                        {
                            EPiFunctions.ShowMessage(this.Page, CurrentPage["ForgotPasswordErrorTextMissingMail"] as string, false, true);
                        }
                        else 
                        {
                            EPiFunctions.ShowMessage(this.Page, "/dilogin/forgotpassword/error", true, true);
                            new Logger("SendPassword() - failed", "Missing sender email");
                        }
                    }
                    else
                    {
                        EPiFunctions.ShowMessage(this.Page, CurrentPage["ForgotPasswordErrorTextMissingMail"] as string, false, true);
                    }
                }
                else
                {
                    EPiFunctions.ShowMessage(this.Page, CurrentPage["ForgotPasswordErrorText"] as string, false, true);
                }
            }
            catch (Exception ex)
            {
                new Logger("SendPassword() - failed", ex.ToString());
                EPiFunctions.ShowMessage(this.Page, "/dilogin/forgotpassword/error", true, true);
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }
        }
        
        #endregion
    }
}