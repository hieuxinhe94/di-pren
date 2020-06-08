using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DagensIndustri.Tools.Classes.Campaign;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Units.Placeable.Campaign
{
    public partial class CampaignForm : EPiServer.UserControlBase
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                PhHeading.Visible = !HideHeading;
                SetUpForm();
            }
        }

        public bool HideHeading { get; set; }

        private void SetUpForm()
        {
            //Set default selected tab
            if (string.IsNullOrEmpty(ActiveTab))
                ActiveTab = LbTab1.CommandArgument;
            ShowTab();

            //Show advanced areas if property is set
            //Advanced areas contains personnummer, företag och organisationsnummer
            IsAdvancedForm = CurrentPage["CampaignFormType"] != null ? CurrentPage["CampaignFormType"].Equals("advanced") : false;

            PhAdvanced.Visible = IsAdvancedForm;
            PhBirthNo.Visible = IsAdvancedForm;
        }

        public void ShowBirthNo(bool show)
        {
            PhBirthNo.Visible = show;
        }

        public void FillForm(CampaignUser campaignUser)
        {
            ActiveTab = campaignUser.ActiveTab;
            ShowTab();

            FirstName = campaignUser.FirstName;
            LastName = campaignUser.LastName;
            StreetName = campaignUser.StreetName;
            StreetNo = campaignUser.HouseNo;
            Entrance = campaignUser.Staircase;
            Stairs = campaignUser.Apartment;
            Apartment = campaignUser.ApartmentNo;
            Zip = campaignUser.ZipCode;
            City = campaignUser.City;
            Phone = campaignUser.OtherPhone;
            Email = campaignUser.Email;
            Co = campaignUser.CareOf;
            BirthNo = campaignUser.BirthNo;
            Company = campaignUser.Company;
            OrgNo = campaignUser.OrgNo;
        }

        public string FormHeading { get; set; }

        public string FirstName
        {
            get
            {
                return InputFirstName.Value;
            }
            set
            {
                InputFirstName.Value = value;
            }
        }

        public string LastName
        {
            get
            {
                return InputLastName.Value;
            }
            set
            {
                InputLastName.Value = value;
            }
        }

        public string StreetName
        {
            get
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street address
                    return InputStreetName.Value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co address
                    return InputCoStreetName.Value;
                else if (this.ActiveTab.Equals(LbTab3.CommandArgument)) //Box
                    return InputStopBox.Value;

                return string.Empty;
            }
            set
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street address
                    InputStreetName.Value = value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co address
                    InputCoStreetName.Value = value;
                else if (this.ActiveTab.Equals(LbTab3.CommandArgument)) //Box
                    InputStopBox.Value = value;
            }
        }

        public string StreetNo
        {
            get
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street no
                    return InputStreetNo.Value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co no
                    return InputCoStreetNo.Value;
                else if (this.ActiveTab.Equals(LbTab3.CommandArgument)) //Box no
                    return InputStopBoxNo.Value;

                return string.Empty;
            }
            set
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street no
                    InputStreetNo.Value = value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co no
                    InputCoStreetNo.Value = value;
                else if (this.ActiveTab.Equals(LbTab3.CommandArgument)) //Box no
                    InputStopBoxNo.Value = value;
            }
        }

        public string Entrance
        {
            get
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street entrance
                    return InputEntrance.Value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co entrance
                    return InputCoEntrance.Value;

                return string.Empty;
            }
            set
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street entrance
                    InputEntrance.Value = value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co entrance
                    InputCoEntrance.Value = value;
            }
        }

        public string Stairs
        {
            get
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street stairs
                    return InputStairs.Value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co stairs
                    return InputCoStairs.Value;

                return string.Empty;
            }
            set
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street stairs
                    InputStairs.Value = value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co stairs
                    InputCoStairs.Value = value;
            }
        }

        public string Apartment
        {
            get
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street apartment
                    return InputApartment.Value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co apartment
                    return InputCoApartment.Value;

                return string.Empty;
            }
            set
            {
                if (this.ActiveTab.Equals(LbTab1.CommandArgument)) //Street apartment
                    InputApartment.Value = value;
                else if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co apartment
                    InputCoApartment.Value = value;
            }
        }

        public string Zip
        {
            get
            {
                return InputZip.Value;
            }
            set
            {
                InputZip.Value = value;
            }
        }

        public string City
        {
            get
            {
                return InputCity.Value;
            }
            set
            {
                InputCity.Value = value;
            }
        }

        public string Phone
        {
            get
            {
                return InputPhone.Value;
            }
            set
            {
                InputPhone.Value = value;
            }
        }

        public string Email
        {
            get
            {
                return InputEmail.Value;
            }
            set
            {
                InputEmail.Value = value;
            }
        }

        public string Co
        {
            get
            {
                if (this.ActiveTab.Equals(LbTab2.CommandArgument)) //Co
                    return InputCo.Value;

                return string.Empty;
            }
            set
            {
                if (this.ActiveTab.Equals(LbTab2.CommandArgument))
                    InputCo.Value = value;
            }
        }

        #region Advanced form

        public string BirthNo
        {
            get
            {
                return InputBirthNo.Value;
            }
            set
            {
                InputBirthNo.Value = value;
            }
        }

        public string Company
        {
            get
            {
                return InputCompany.Value;
            }
            set
            {
                InputCompany.Value = value;
            }
        }

        public string OrgNo
        {
            get
            {
                return InputOrgNo.Value;
            }
            set
            {
                InputOrgNo.Value = value;
            }
        }

        public string Attention
        {
            get
            {
                return InputAttention.Value;
            }
            set
            {
                InputAttention.Value = value;
            }
        }

        #endregion

        //If false, invoice to other payer
        public bool IsPayer { get; set; }

        public bool IsAdvancedForm { get; set; }

        #region Tab handling

        public string ActiveTab
        {
            get
            {
                return (string)ViewState["ActiveTab"];
            }
            set
            {
                ViewState["ActiveTab"] = value;
            }
        }

        protected void TabOnclick(object sender, EventArgs e)
        {
            LinkButton tmpBtn = ((LinkButton)sender);

            //Set active tabe
            ActiveTab = tmpBtn.CommandArgument;
            //Show active tab placeholder
            ShowTab();
        }

        private void ShowTab()
        {
            PhTab1.Visible = LbTab1.CommandArgument.Equals(ActiveTab);
            PhTab2.Visible = LbTab2.CommandArgument.Equals(ActiveTab);
            PhTab3.Visible = LbTab3.CommandArgument.Equals(ActiveTab);

            LbTab1.CssClass = PhTab1.Visible ? "streetactive" : "street";
            LbTab2.CssClass = PhTab2.Visible ? "coactive" : "co";
            LbTab3.CssClass = PhTab3.Visible ? "boxactive" : "box";
        }

        #endregion
    }
}