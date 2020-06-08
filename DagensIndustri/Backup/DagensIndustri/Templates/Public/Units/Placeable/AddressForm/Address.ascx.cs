using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DIClassLib.Misc;
using DIClassLib.Subscriptions.CirixMappers;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using DIClassLib.Subscriptions;
using System.Data;
using DagensIndustri.Templates.Public.Pages.UserSettings;


namespace DagensIndustri.Templates.Public.Units.Placeable.AddressForm
{
    public partial class Address : UserControlBase
    {
        //page or UC using this UC must implement IAddress 
        private IAddress _parent { get; set; }

        public bool ShowNames = false;
        public bool ShowCompany = false;
        
        public bool ShowDate1 = false;
        public bool ShowDate2 = false;
        
        public string Date1Header { get; set; }
        public string Date2Header { get; set; }
        
        public DateTime Date1Min = DateTime.MinValue;
        public DateTime Date1Max = DateTime.MaxValue;
        public DateTime Date2Min = DateTime.MinValue;
        public DateTime Date2Max = DateTime.MaxValue;

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _parent = GetClosestIAddressParent(Parent);

            if (!IsPostBack)
            {
                ClearForms();
                HandleFormFieldsByProps();
                HandleBoxerDealVisibility();
            }
        }

        private void HandleBoxerDealVisibility()
        {
            PlaceHolderBoxer.Visible = false;

            if (MiscFunctions.GetAppsettingsValue("ShowBoxerSummerDeal").ToLower() == "true" && CurrentPage.PageTypeName == "[DI] MySet AddressTemp")
            {
                AddressTemp page = (AddressTemp)Page;
                if (page.Subscriber.Cusno > 0)
                {
                    if (DbHelpMethods.DataSetHasRows(MsSqlHandler.GetDiAccountsByExtSubId(page.Subscriber.Cusno.ToString())))
                        PlaceHolderBoxer.Visible = true;
                }
            }
        }

        private IAddress GetClosestIAddressParent(Control par)
        {
            if (par is IAddress)
                return (IAddress)par;

            if (par is Page)
            {
                try { return (IAddress)par; }
                catch { throw; }
            }

            return GetClosestIAddressParent(par.Parent);
        }



        private void HandleFormFieldsByProps()
        {
            PlaceHolderNameStr.Visible = ShowNames;
            PlaceHolderNameBox.Visible = ShowNames;

            PlaceHolderCompStr.Visible = ShowCompany;
            PlaceHolderCompBox.Visible = ShowCompany;

            if (ShowDate1 || ShowDate2)
            {
                PlaceHolderDatesStr.Visible = true;
                PlaceHolderDatesBox.Visible = true;
                
                Date1Str.Visible = ShowDate1;
                Date1Box.Visible = ShowDate1;
                Date2Str.Visible = ShowDate2;
                Date2Box.Visible = ShowDate2;

                SetDateHeaders();
                SetDateMinMaxRanges();
            }
        }

        private void SetDateHeaders()
        {
            string dateFormat = " <i>(YYYY-MM-DD)</i>";

            if (!string.IsNullOrEmpty(Date1Header))
            {
                Date1Str.Title = Date1Header + dateFormat;
                Date1Box.Title = Date1Header + dateFormat;
            }

            if (!string.IsNullOrEmpty(Date2Header))
            {
                Date2Str.Title = Date2Header + dateFormat;
                Date2Box.Title = Date2Header + dateFormat;
            }
        }

        private void SetDateMinMaxRanges()
        {
            if (Date1Min > DateTime.MinValue)
            {
                Date1Str.MinValue = Date1Min.ToShortDateString();
                Date1Box.MinValue = Date1Min.ToShortDateString();
            }
            if (Date1Max < DateTime.MaxValue)
            {
                Date1Str.MaxValue = Date1Max.ToShortDateString();
                Date1Box.MaxValue = Date1Max.ToShortDateString();
            }

            if (Date2Min > DateTime.MinValue)
            {
                Date2Str.MinValue = Date2Min.ToShortDateString();
                Date2Box.MinValue = Date2Min.ToShortDateString();
            }
            if (Date2Max < DateTime.MaxValue)
            {
                Date2Str.MaxValue = Date2Max.ToShortDateString();
                Date2Box.MaxValue = Date2Max.ToShortDateString();
            }
        }



        public void ClearForms()
        {
            FirstNameStr.Text = "";
            LastNameStr.Text = "";
            CareOfStr.Text = "";
            CompanyStr.Text = "";
            StreetStr.Text = "";
            StreetNumStr.Text = "";
            EntranceStr.Text = "";
            StairsStr.Text = "";
            ApartmentNumStr.Text = "";
            ZipStr.Text = "";
            CityStr.Text = "";
            Date1Str.Text = "";
            Date2Str.Text = "";

            FirstNameBox.Text = "";
            LastNameBox.Text = "";
            CompanyBox.Text = "";
            StopOrBox.Text = "";
            BoxNum.Text = "";
            ZipBox.Text = "";
            CityBox.Text = "";
            Date1Box.Text = "";
            Date2Box.Text = "";
        }

        

        protected void StreetFormButton_Click(object sender, EventArgs e)
        {
            if (CheckBoxBoxer.Checked)
            { 
                AddressTemp page = (AddressTemp)Page;
                if (page.Subscriber.Cusno > 0)
                    new Logger(8, page.Subscriber.Cusno, true);
            }

            _parent.HandleAddressButtonClick(GetAdrDataHolder(true));
        }

        protected void BoxFormButton_Click(object sender, EventArgs e)
        {
            _parent.HandleAddressButtonClick(GetAdrDataHolder(false));
        }


        private AddressDataHolder GetAdrDataHolder(bool isStreetAddress)
        { 
            AddressDataHolder dh = new AddressDataHolder();
            dh.IsStreetAddress = isStreetAddress;

            if (isStreetAddress)
            {
                dh.FirstName = FirstNameStr.Text;
                dh.LastName = LastNameStr.Text;
                dh.CareOf = CareOfStr.Text;
                dh.Company = CompanyStr.Text;
                dh.StreetName = StreetStr.Text;
                dh.HouseNum = StreetNumStr.Text;
                dh.Staircase = EntranceStr.Text;
                dh.Stairs = StairsStr.Text;
                dh.ApartmentNo = ApartmentNumStr.Text;
                dh.Zip = ZipStr.Text;
                dh.City = CityStr.Text;
                dh.Date1 = TryGetDate(Date1Str.Text);
                dh.Date2 = TryGetDate(Date2Str.Text);
            }
            else
            {
                dh.FirstName = FirstNameBox.Text;
                dh.LastName = LastNameBox.Text;
                dh.Company = CompanyBox.Text;
                dh.StreetName = StopOrBox.Text;
                dh.HouseNum = BoxNum.Text;
                dh.Zip = ZipBox.Text;
                dh.City = CityBox.Text;
                dh.Date1 = TryGetDate(Date1Box.Text);
                dh.Date2 = TryGetDate(Date2Box.Text);
            }

            return dh;
        }

        private DateTime TryGetDate(string date)
        {
            DateTime dt = DateTime.MinValue;
            DateTime.TryParse(date, out dt);
            return dt;
        }


        //public void Populate(string firstName, string lastName, string email, string streetName, string streetNum, string door, string stairs, string appartmentNum, string zip, string city, string careOf, string phoneMob, string company)
        //{
        //    if (!streetName.ToUpper().StartsWith("BOX"))
        //    {

        //    }
        //    else
        //    {

        //    }
        //}


        internal void SetFieldsFromAddressMap(AddressMap address)
        {
            CareOfStr.Text = GetCareOf(address.Street2);
            StreetStr.Text = address.StreetName;
            StreetNumStr.Text = address.Houseno;
            EntranceStr.Text = address.Staircase;
            StairsStr.Text = (string.IsNullOrEmpty(address.Apartment)) ? string.Empty : address.Apartment.Replace("TR", "").Trim();
            ApartmentNumStr.Text = GetApartmentNo(address.Street2);
            ZipStr.Text = address.ZipCode;
            CityStr.Text = address.Postname;
            Date1Str.Text = address.StartDate.ToShortDateString();
            Date2Str.Text = "";

            //CompanyBox.Text = "";
            StopOrBox.Text = address.StreetName;
            BoxNum.Text = address.Houseno;
            ZipBox.Text = address.ZipCode;
            CityBox.Text = address.Postname;
            Date1Box.Text = address.StartDate.ToShortDateString();
            Date2Box.Text = "";

            //if (address.IsInProgress)
            //{
            //    Date1Str.Disabled = true;
            //    Date1Box.Disabled = true;
            //}

            if (address.EndDate > DateTime.MinValue && address.EndDate != new DateTime(2078, 12, 31))
            {
                Date2Str.Text = address.EndDate.ToShortDateString();
                Date2Box.Text = address.EndDate.ToShortDateString();
            }
        }

        private string GetCareOf(string street2)
        {
            if (string.IsNullOrEmpty(street2))
                return string.Empty;

            string co = street2;
            int lghIndex = street2.IndexOf("LGH");
            if (lghIndex > -1)
                co = street2.Substring(0, lghIndex).Trim();

            return co;
        }

        private string GetApartmentNo(string street2)
        {
            if (string.IsNullOrEmpty(street2))
                return string.Empty;

            string appNo = "";
            int lghIndex = street2.IndexOf("LGH");
            if (lghIndex > -1)
                appNo = street2.Substring(lghIndex + 3).Trim();

            return appNo;
        }

    }
}