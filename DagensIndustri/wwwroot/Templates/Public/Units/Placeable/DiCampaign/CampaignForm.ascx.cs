using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignForm : EPiServer.UserControlBase
    {

        public bool IsDigitalCampaign
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsDigitalCampaign"); }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();

            LiteralTabHeaderStreet.Text = (IsDigitalCampaign) ? "Uppgifter" : "Gata";

            ((DiTemplatePage)Page).UserMessageControl = UserMessageControl;

            if (IsValue("CampaignAnswerCardText"))
            {
                AnswerCardLiteral.Text = CurrentPage["CampaignAnswerCardText"].ToString();
            }

            if (IsValue("CampaignAnswerCheckBoxText"))
            {
                AnswerCheckBoxLiteral.Text = CurrentPage["CampaignAnswerCheckBoxText"].ToString();
            }
            
        }


        public void PopulateChildForm(string firstName, string lastName, string email, string streetName, string streetNum, string door, string stairs, string appartmentNum, string zip, string city, string careOf, string phoneMob, string company)
        {
            if (!streetName.ToUpper().StartsWith("BOX"))
            {
                StreetForm.Populate(firstName, lastName, email, streetName, streetNum, door, stairs, appartmentNum, zip, city, careOf, phoneMob, company);
                BoxForm.Clear();
            }
            else
            {
                StreetForm.Clear();
                BoxForm.Populate(firstName, lastName, email, streetName, streetNum, zip, city, phoneMob, company);
                RegisterSwitchToBoxFormScript();
            }
        }

        /// <summary>
        /// Register clientscripts. Show the correct tab
        /// </summary>
        private void RegisterSwitchToBoxFormScript()
        {
            /*
            HiddenField SelectedTabHiddenField = Conference.HiddenFieldSelectedTab;
            HyperLink HyperLinkConferenceRegistration = ParentControl.HyperLinkRegistration;

            // Create script for click on Save buttons where selected tab and section will be stored in hiddenfields
            string script = string.Format(@"$(document).ready(function() {{
                                                    $('#{0}').click(function () {{
                                                    $('#{1}').val('{2}');
                                                }})                                                  
                                            }});",

                                            ConferenceRegistrationButton.ClientID,

                                            SelectedTabHiddenField.ClientID,

                                            HyperLinkConferenceRegistration.NavigateUrl
                                        );
            */
            
            string script = string.Format(@"$(document).ready(function() {{
                                                    q.forms.switchSection('{0}');                       
                                            }});", "#form-box");
            
          /*
           * $(document).ready(function() {
            q.forms.switchSection('#form-box');                       
    }); */
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "CampaignForm_Init", script, true);
             
        }


    }
}