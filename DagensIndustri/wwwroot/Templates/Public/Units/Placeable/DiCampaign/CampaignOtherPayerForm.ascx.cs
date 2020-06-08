using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using System.Data;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes.BaseClasses;
using System.Text;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignOtherPayerForm : EPiServer.UserControlBase
    {
        
        public Pages.DiCampaign.Campaign CampPage
        {
            get { return (Pages.DiCampaign.Campaign)this.Page; }
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("CampaignAnswerCardText"))
            {
                AnswerCardLiteral.Text = CurrentPage["CampaignAnswerCardText"].ToString();
            }

            if (IsValue("CampaignAnswerCheckBoxText"))
            {
                AnswerCheckBoxLiteral.Text = CurrentPage["CampaignAnswerCheckBoxText"].ToString();
            }
        }


        public Person GetPerson()
        {
            return new Person(true,
                              false,
                              FirstNameInput.Text,
                              LastNameInput.Text,
                              CareOfInput.Text,
                              CompanyInput.Text,
                              StreetInput.Text,
                              StreetNumberInput.Text,
                              string.Empty,
                              string.Empty,
                              string.Empty,
                              ZipInput.Text,
                              StateInput.Text,
                              TelephoneInput.Text,
                              string.Empty,
                              string.Empty,
                              OrgNumberInput.Text,
                              AttentionInput.Text,
                TelephoneInput.Text);
        }


        protected void OtherPayerFormButton_Click(object sender, EventArgs e)
        {
            CampPage.Sub.SubscriptionPayer = GetPerson();
            String err = CampPage.SaveSubscription();
            if (String.IsNullOrEmpty(err))
                CampPage.ShowThankYou();
            else
                CampPage.ShowMessage(err,false,true);
        }



        internal void Populate(string firstName, string lastName, string streetName, string streetNum, string zip, string city, string careOf, string phoneMob, string company, string companyNo)
        {
            FirstNameInput.Text = firstName;
            LastNameInput.Text = lastName;
            StreetInput.Text = streetName;
            StreetNumberInput.Text = streetNum;
            ZipInput.Text = zip;
            StateInput.Text = city;
            CareOfInput.Text = careOf;
            TelephoneInput.Text = phoneMob;
            CompanyInput.Text = company;
            OrgNumberInput.Text = companyNo;
        }

        internal void Clear()
        {
            FirstNameInput.Text = "";
            LastNameInput.Text = "";
            StreetInput.Text = "";
            StreetNumberInput.Text = "";
            ZipInput.Text = "";
            StateInput.Text = "";
            CareOfInput.Text = "";
            TelephoneInput.Text = "";
            CompanyInput.Text = "";
            OrgNumberInput.Text = "";
        }

        public String FormTitle
        {
            get
            {
                if (CurrentPage["CampaignIsAutogiro"] != null && ((bool)CurrentPage["CampaignIsAutogiro"]) == true)
                {
                    return Translate("/dicampaign/tabs/directdebitotherpayer");                    
                }
                else
                {
                    return Translate("/dicampaign/tabs/otherpayer");
                }
            }
        }

    }
}