using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.Gasell;

namespace DagensIndustri.Templates.Public.Units.Placeable.GasellFlow
{
    public partial class RegisterParticipants : EPiServer.UserControlBase
    {
        #region Properties

        private Pages.DiGasell.GasellFlow GasellFlow
        {
            get
            {
                return (Pages.DiGasell.GasellFlow)Page;
            }
        }

        public string FirstName
        {
            get
            {
                return FirstNameInput.Text.Trim();
            }
            set
            {
                FirstNameInput.Text = value;
            }
        }

        public string LastName
        {
            get
            {
                return LastNameInput.Text.Trim();
            }
            set
            {
                LastNameInput.Text = value;
            }
        }

        public string Title
        {
            get
            {
                return TitleInput.Text.Trim();
            }
            set
            {
                TitleInput.Text = value;
            }
        }

        public string Company
        {
            get
            {
                return CompanyInput.Text.Trim();
            }
            set
            {
                CompanyInput.Text = value;
            }
        }

        public string Street
        {
            get
            {
                return StreetInput.Text.Trim();
            }
            set
            {
                StreetInput.Text = value;
            }
        }
        
        public string StreetNumber
        {
            get
            {
                return StreetNumberInput.Text.Trim();
            }
            set
            {
                StreetNumberInput.Text = value;
            }
        }

        public string Zip
        {
            get
            {
                return ZipCodeInput.Text.Trim();
            }
            set
            {
                ZipCodeInput.Text = value;
            }
        }

        public string City
        {
            get
            {
                return StateInput.Text.Trim();
            }
            set
            {
                StateInput.Text = value;
            }
        }

        public string Phone
        {
            get
            {
                return TelephoneInput.Text.Trim();
            }
            set
            {
                TelephoneInput.Text = value;
            }
        }

        public string Email
        {
            get
            {
                return EmailInput.Text.Trim();
            }
            set
            {
                EmailInput.Text = value;
            }
        }

        public bool IsGasellCompany
        {
            get
            {
                return GasellCompanyCheckBox.Checked;
            }
        }
        public string Branch
        {
            get
            {
                return BranchInput.Text.Trim();
            }
            set
            {
                BranchInput.Text = value;
            }
        }

        public string NumberOfEmployees
        {
            get
            {
                return EmployeesInput.Text.Trim();
            }
            set
            {
                EmployeesInput.Text = value;
            }
        }

        public string GasellID
        {
            get
            {
                return Request.QueryString["gasellid"];
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        public GasellUser GetUser()
        {
            return new GasellUser(this);
        }
    }
}