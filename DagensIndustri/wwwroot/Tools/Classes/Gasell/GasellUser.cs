using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using EPiServer.Core;
using DIClassLib.Misc;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Templates.Public.Units.Placeable.GasellFlow;
using System.Text;


namespace DagensIndustri.Tools.Classes.Gasell
{
    [Serializable]
    public class GasellUser
    {
        #region Properties

        private PageData CurrentPage
        {
            get
            {
                var page = System.Web.HttpContext.Current.Handler as EPiServer.PageBase;

                if (page == null)
                {
                    return null;
                }

                return page.CurrentPage;
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool IsGasellCompany { get; set; }

        public string Branch { get; set; }

        public string NumberOfEmployees { get; set; }

        public string GasellID { get; set; }

        #endregion

        public GasellUser(RegisterParticipants participantsForm)
        {
            FirstName = participantsForm.FirstName;
            LastName = participantsForm.LastName;
            Title = participantsForm.Title;
            Company = participantsForm.Company;
            Address = participantsForm.Street + " " + participantsForm.StreetNumber;
            Zip = participantsForm.Zip;
            City = participantsForm.City;
            Phone = participantsForm.Phone;
            Email = participantsForm.Email;
            IsGasellCompany = participantsForm.IsGasellCompany;
            Branch = participantsForm.Branch;
            NumberOfEmployees = participantsForm.NumberOfEmployees;
            GasellID = participantsForm.GasellID;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FirstName:" + FirstName + "<br>");
            sb.Append("LastName:" + LastName + "<br>");
            sb.Append("Title:" + Title + "<br>");
            sb.Append("Company:" + Company + "<br>");
            sb.Append("Address:" + Address + "<br>");
            sb.Append("Zip:" + Zip + "<br>");
            sb.Append("City:" + City + "<br>");
            sb.Append("Phone:" + Phone + "<br>");
            sb.Append("Email:" + Email + "<br>");
            sb.Append("GasellCompany:" + IsGasellCompany + "<br>");
            sb.Append("Branch:" + Branch + "<br>");
            sb.Append("NumberOfEmployees:" + NumberOfEmployees + "<br>");
            sb.Append("GasellID:" + GasellID + "<hr>");
            return sb.ToString();
        }
    }

}