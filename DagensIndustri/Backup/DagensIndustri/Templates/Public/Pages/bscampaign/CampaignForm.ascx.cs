using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.StudentVerification;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using System.Data;
using DagensIndustri.Tools.Classes;
using DIClassLib.CardPayment;
//using DagensIndustri.OneByOne;
using System.Xml;
using DIClassLib.OneByOne;
using DIClassLib.OboWebReference;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DagensIndustri.Templates.Public.Pages.bscampaign
{
    public partial class CampaignForm : EPiServer.UserControlBase
    {
        #region Properties

        public bool IsDigitalCampaign { get { return IsValue(PropertyPrefix + "IsDigital"); } }
        public bool IsStudentCampaign { get { return IsValue(PropertyPrefix + "IsStudent"); } }
        public bool IsOtherPayerForm { get; set; }
        public string PropertyPrefix { get; set; }
        public string FormHeading { get; set; }

        /// <summary>
        /// Flagged if a user clicked back button.
        /// Flag is used in mothership
        /// </summary>
        public bool GoBack { get; set; }

        /// <summary>
        /// Flagged if a postback is made within the form
        /// Flag is used in mothership
        /// </summary>
        public bool Postback { get; set; }

        /// <summary>
        /// Flagged if form is done with it's business
        /// Flag is used in mothership
        /// </summary>
        public bool FormDone { get; set; }

        /// <summary>
        /// Used to not mix up validation if multiple forms is used
        /// </summary>
        public string ValidationGroup { get; set; }

        public PaymentMethod.TypeOfPaymentMethod SelectedPayMethod
        {
            get
            {
                if(RbAutoPayment.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.DirectDebit;
                if(RbAutoWithdrawal.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal;
                if (RbInvoiceOtherPayer.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer;
                if (RbCard.Checked)
                    return PaymentMethod.TypeOfPaymentMethod.CreditCard;

                return PaymentMethod.TypeOfPaymentMethod.Invoice;
            }
        }

        #endregion

        #region Events

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
             
            if (!IsOtherPayerForm && !Postback)
                SetUpPaymentOptions();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //PAR-search not visible on student campaigns
            PhGetParInfo.Visible = !IsStudentCampaign;

            DataBind();
        }

        protected void BtnSubmitFormOnClick(object sender, EventArgs e)
        {
            Postback = true;

            if (IsDigitalCampaign && SelectedPayMethod != PaymentMethod.TypeOfPaymentMethod.Invoice)
            { 
                //If digital and not invoice (invoice shows address fields in form)
                //Disable validators, they are disabled on client by javascript but still active for .net
                ReqValStreetAddress.Enabled = false;
                RegValStairs.Enabled = false;
                ReqValPostalCode.Enabled = false;
                RegValPostalCode.Enabled = false;
                //ReqValCity.Enabled = false;               
            }

            if (Page.IsValid)
            {
                FormDone = true;
            }
        }

        protected void BtnBackOnClick(object sender, EventArgs e)
        {
            GoBack = true;
        }
        
        protected void LbPopulatePersonFormOnClick(object sender, EventArgs e)
        {
            Postback = true;

            var pnr = TxtPnoGet.Text;

            if (!new Regex(@"(^[\d]{12}$)").IsMatch(pnr))
            {
                DisplayParError("Felaktigt format på personnummer (ÅÅÅÅMMDDXXXX)");
                return;
            }

            //Google analytics tracking
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GATrack", "Track('Form', 'Private', 'Step2');", true);

            ClearForm();

            var person = Obo.GetPerson(pnr);

            if (person != null && !string.IsNullOrEmpty(person.FirstNames))
            {
                var streetAddress = person.StreetAddressRaw;
                TxtFirstName.Text = string.IsNullOrEmpty(person.GivenNames) ? person.FirstNames : person.GivenNames;
                TxtLastName.Text = person.LastNames;
                PopulateAddressFields(streetAddress);
                TxtPostalCode.Text = person.ZipCode;
                //TxtCity.Text = person.City;
                TxtMobilePhone.Text = person.PhoneMobile;
                TxtPno.Text = TxtPnoGet.Text;

                MsSqlHandler.InsertToLogPar(CurrentPage.PageLink.ID,
                    PropertyPrefix,
                    pnr,
                    string.Empty,
                    TxtFirstName.Text,
                    TxtLastName.Text,
                    string.Empty,
                    streetAddress ?? string.Empty,
                    TxtPostalCode.Text,
                    "", //TxtCity.Text,
                    TxtMobilePhone.Text);
            }
            else
            {
                DisplayParError("Vi hittar inga personuppgifter för " + pnr);
            }
        }

        protected void LbPopulateCompanyFormOnClick(object sender, EventArgs e)
        {
            Postback = true;

            var orgNo = TxtPnoGet.Text;

            if (!new Regex(@"(^[\d]{6}[\d]{4}$)").IsMatch(orgNo))
            {
                DisplayParError("Felaktigt format på organisationsnummer (XXXXXXXXXX)");
                return;
            }

            //Google analytics tracking
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "GATrack", "Track('Form', 'Company', 'Step2');", true);

            ClearForm();

            var company = Obo.GetCompany(orgNo);

            if (company != null && !string.IsNullOrEmpty(company.Name))
            {
                var streetAddress = company.StreetAddressRaw;
                TxtCompany.Text = company.Name;
                PopulateAddressFields(streetAddress);
                TxtPostalCode.Text = company.ZipCode;
                //TxtCity.Text = company.City;                
                TxtOrgNo.Text = TxtPnoGet.Text;

                MsSqlHandler.InsertToLogPar(CurrentPage.PageLink.ID, 
                    PropertyPrefix,
                    string.Empty,
                    orgNo,
                    string.Empty, 
                    string.Empty,
                    TxtCompany.Text,
                    streetAddress ?? string.Empty,
                    TxtPostalCode.Text,
                    "", //TxtCity.Text, 
                    string.Empty);
            }
            else
            {
                DisplayParError("Vi hittar inga företagsuppgifter för " + orgNo);
            }             
        }


        #endregion

        private void DisplayParError(string errorMsg)
        {
            PhParError.Visible = true;
            LblParError.Text = errorMsg;
        }

        /// <summary>
        /// Helper for fixing address from PAR to Cirix-format
        /// </summary>       
        private void PopulateAddressFields(string address)
        {
            //TEST DATA
            //5567832232 nearU, Fålhagsleden 57 2tr
            //9697608330 KJTH, Apelgatan 5 A
            //5560163429 SSAB, Klarabergsviadukten 70 Uppg D
            //5563687184 Swedia, SKINNARVIKSRINGEN 16 LGH 2
            //5567762686 Supplement company, ELISETORPSVÄGEN 15 C LGH 1602
            //5564128980 Bjersbo, Flintlåsvägen 18 3tr Lgh1302

            if (address == null)
                return;

            string streetAddress = string.Empty, streetNo = string.Empty, stairCase = string.Empty, stairs = string.Empty, appNo = string.Empty;        
   
            var nextIsAppNo = false;

            foreach (var arrayItem in address.Split(' '))
            {
                if (nextIsAppNo) {
                    appNo = arrayItem;
                    nextIsAppNo = false;
                    continue;
                }

                if (arrayItem.ToLower().EndsWith("tr"))
                    stairs = arrayItem.ToLower().Replace("tr", string.Empty);
                else if (arrayItem.ToLower().Equals("lgh"))
                    nextIsAppNo = true;
                else if (arrayItem.ToLower().StartsWith("lgh"))
                    appNo = arrayItem.ToLower().Replace("lgh", string.Empty);
                else if (MiscFunctions.IsNumeric(arrayItem) && string.IsNullOrEmpty(streetNo))
                    streetNo = arrayItem;
                else if (arrayItem.Length == 1 && !MiscFunctions.IsNumeric(arrayItem) && string.IsNullOrEmpty(stairCase))
                    stairCase = arrayItem;
                else if (!MiscFunctions.IsNumeric(arrayItem)) //As long it isn't numeric, it must be street address
                    streetAddress += arrayItem + " ";
            }

            TxtStreetAddress.Text = streetAddress.Trim();
            TxtHouseNo.Text = streetNo;
            TxtStairCase.Text = stairCase;
            TxtStairs.Text = stairs;
            TxtAppNo.Text = appNo;
        }

        private void SetUpPaymentOptions()
        {
            var showCard = !IsValue(PropertyPrefix + "HideCard");
            var showInvoice = !IsValue(PropertyPrefix + "HideInvoice");
            var showInvoiceOther = !IsValue(PropertyPrefix + "HideInvoiceOther");
            var showAutopay = !IsValue(PropertyPrefix + "HideAutopay");
            var showAutoWithdrawal = !IsValue(PropertyPrefix + "HideAutoWithdrawal");

            LblCard.Visible = showCard;
            LblInvoice.Visible = showInvoice;
            LblInvoiceOtherPayer.Visible = showInvoiceOther;
            LblAutoPayment.Visible = showAutopay;
            LblAutoWithdrawal.Visible = showAutoWithdrawal;

            ImgCreditCards.Visible = showCard;

            //Only one can be checked, this is the priority, low prio first
            if (showInvoiceOther)
                PaymethodChecker(false, true, false, false, false);
            if (showAutopay)
                PaymethodChecker(false, false, false, true, false);
            if (showAutoWithdrawal)
                PaymethodChecker(false, false, false, false, true);
            if (showCard)
                PaymethodChecker(true, false, false, false, false);
            if (showInvoice)
                PaymethodChecker(false, false, true, false, false);
            

            //If a digital campaign, add some click scripts that will show/hide adress fields
            if (IsDigitalCampaign)
            {
                RbCard.Attributes.Add("onclick", "javascript:FormHandler('.address',false)");
                RbInvoice.Attributes.Add("onclick", "javascript:FormHandler('.address',true)");
                RbInvoiceOtherPayer.Attributes.Add("onclick", "javascript:FormHandler('.address',false)");
                RbAutoPayment.Attributes.Add("onclick", "javascript:FormHandler('.address',false)");
                RbAutoWithdrawal.Attributes.Add("onclick", "javascript:FormHandler('.address',false)");
            }
            else
            {
                //Since form can reload with another campaign, we must clear attributes
                RbCard.Attributes.Remove("onclick");
                RbInvoice.Attributes.Remove("onclick");
                RbInvoiceOtherPayer.Attributes.Remove("onclick");
                RbAutoPayment.Attributes.Remove("onclick");
                RbAutoWithdrawal.Attributes.Remove("onclick");
            }        
        }

        private void PaymethodChecker(bool checkCard, bool checkInvoiceotherPayer, bool checkInvoice, bool checkAutoPayment, bool checkAutoWithdrawal)
        {
            RbCard.Checked = checkCard;
            RbInvoice.Checked = checkInvoice;
            RbInvoiceOtherPayer.Checked = checkInvoiceotherPayer;
            RbAutoPayment.Checked = checkAutoPayment;
            RbAutoWithdrawal.Checked = checkAutoWithdrawal;
        }

        /// <summary>
        /// Registers a scrip block that will scroll down to error message below form
        /// </summary>
        /// <param name="errorMsg">The error message to show</param>
        public void DisplayError(string errorMsg)
        {
            LblError.Text = errorMsg;
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Error", "$(document).ready(function () { displayArea('erroralert'); });", true);
        }

        public void HideParArea()
        {
            PhGetParInfo.Visible = false;
        }

        protected string GetValidationScript()
        {
            return "javascript:validate('" + ValidationGroup + "');";
        }

        #region Form input controls

        private void ClearForm()
        {
            TxtFirstName.Text = string.Empty;
            TxtLastName.Text = string.Empty;
            TxtMobilePhone.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtCompany.Text = string.Empty;
            TxtCo.Text = string.Empty;
            TxtStreetAddress.Text = string.Empty;
            TxtHouseNo.Text = string.Empty;
            TxtStairCase.Text = string.Empty;
            TxtStairs.Text = string.Empty;
            TxtAppNo.Text = string.Empty;
            TxtPostalCode.Text = string.Empty;
            //TxtCity.Text = string.Empty;
            TxtPno.Text = string.Empty;
        }


        public bool FormIsEmpty 
        {
            get {
                //if no firstname, lastname or company i treat it as empty and can populuate it with values from MDB
                return string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(Company);
            }
        }

        public string FirstName { get { return TxtFirstName.Text; } set { TxtFirstName.Text = value; } }
        public string LastName { get { return TxtLastName.Text; } set { TxtLastName.Text = value; } }
        public string PhoneMobile { get { return TxtMobilePhone.Text; } set { TxtMobilePhone.Text = value; } }
        public string Email { get { return TxtEmail.Text; } set { TxtEmail.Text = value; } }
        public string Company { get { return TxtCompany.Text; } set { TxtCompany.Text = value; } }
        public string Co { get { return TxtCo.Text; } set { TxtCo.Text = value; } }
        public string StreetAddress { get { return TxtStreetAddress.Text; } set { TxtStreetAddress.Text = value; } }
        public string HouseNo { get { return TxtHouseNo.Text; } set { TxtHouseNo.Text = value; } }
        public string StairCase { get { return TxtStairCase.Text; } set { TxtStairCase.Text = value; } }
        public string Stairs { get { return TxtStairs.Text; } set { TxtStairs.Text = value; } }
        public string AppNo { get { return TxtAppNo.Text; } set { TxtAppNo.Text = value; } }
        public string PostalCode { get { return TxtPostalCode.Text; } set { TxtPostalCode.Text = value; } }
        //public string City { get { return TxtCity.Text; } set { TxtCity.Text = value; } }
        //public string Password { get { return TxtPwd1.Text; } }
        public string BirthNo { get { return TxtPno.Text; } set { TxtPno.Text = value; } }
        public string Attention { get { return TxtAttention.Text; } set { TxtAttention.Text = value; } }
        public string OrganisationNumber { get { return TxtOrgNo.Text; } set { TxtOrgNo.Text = value; } }
        public string WantedStartDate { get { return TxtStartDate.Text; } set { TxtStartDate.Text = value; } }

        #endregion

    }
}