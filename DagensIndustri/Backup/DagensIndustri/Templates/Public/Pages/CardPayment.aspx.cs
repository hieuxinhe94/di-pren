using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions;

using EPiServer;
using EPiServer.Core;
using DagensIndustri.Templates.Public.Units.Placeable.SignUpFlow;
using DagensIndustri.Tools.Classes.SignUp;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DIClassLib.CardPayment;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using System.Text;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class CardPayment : DiTemplatePage
    {

        #region properties

        public int MerchantId
        {
            get
            {
                int i = 0;
                int.TryParse(Settings.Nets_MerchantId, out i);
                
                //if (EPiFunctions.HasValue(CurrentPage, "MerchantId"))
                //    int.TryParse(CurrentPage["MerchantId"].ToString(), out i);

                return i;
            }
        }

        public string DiDepartment
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "DiDepartment"))
                    return CurrentPage["DiDepartment"].ToString();

                return string.Empty;
            }
        }

        public string ItemDescrPaytrans
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "ItemDescrPaytrans"))
                    return CurrentPage["ItemDescrPaytrans"].ToString();

                return string.Empty;
            }
        }

        /// <summary>
        /// 0 = infinite
        /// </summary>
        public int MaxNumItemsPerCust
        {
            get
            {
                int i = 1;
                if (EPiFunctions.HasValue(CurrentPage, "MaxNumItemsPerCust"))
                    int.TryParse(CurrentPage["MaxNumItemsPerCust"].ToString(), out i);

                return i;
            }
        }

        public bool HideFormAddress
        {
            get { return EPiFunctions.HasValue(CurrentPage, "HideFormAddress"); }
        }

        /// <summary>
        /// -1 = infinite
        /// </summary>
        public int NumItemsInStockFromStart
        {
            get
            {
                int i = -1;
                if (EPiFunctions.HasValue(CurrentPage, "ItemsInStock"))
                    int.TryParse(CurrentPage["ItemsInStock"].ToString(), out i);

                return i;
            }
        }

        public string NoItemsInStockText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "NoItemsInStockText"))
                    return CurrentPage["NoItemsInStockText"].ToString();

                return "Vi beklagar, det är tyvärr slutsålt";
            }
        }

        public string ThankYouText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "ThankYouText"))
                    return CurrentPage["ThankYouText"].ToString();

                return "<h2>Tack för ditt köp!</h2>";
            }
        }

        public double? PriceIncVat
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "PriceIncVat"))
                {
                    double i = 0;
                    if(double.TryParse(CurrentPage["PriceIncVat"].ToString(), out i))
                        return i;
                }
                return null;
            }
        }

        public double? PriceExVat
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "PriceExVat"))
                {
                    double i = 0;
                    if(double.TryParse(CurrentPage["PriceExVat"].ToString(), out i))
                        return i;
                }
                return null;
            }
        }

        public double? Vat
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "Vat"))
                {
                    double i = 0;
                    if(double.TryParse(CurrentPage["Vat"].ToString(), out i))
                        return i;
                }
                return null;
            }
        }

        public bool SendCustMail
        {
            get { return EPiFunctions.HasValue(CurrentPage, "SendCustMail"); }
        }

        public string CustMailText
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "CustMailText"))
                    return CurrentPage["CustMailText"].ToString();

                return string.Empty;
            }
        }

        public bool SendStaffMail
        {
            get { return EPiFunctions.HasValue(CurrentPage, "SendStaffMail"); }
        }

        public string StaffMailAddress
        {
            get
            {
                if (EPiFunctions.HasValue(CurrentPage, "StaffMailAddress"))
                    return CurrentPage["StaffMailAddress"].ToString();

                return MiscFunctions.GetAppsettingsValue("mailPrenDiSe");
            }
        }

        
        //non epi props
        private int _numItemsSold = -2;
        public int NumItemsSold 
        {
            get
            {
                if(_numItemsSold == -2)
                    _numItemsSold = MsSqlHandler.GetNumSoldCardPayItems(CurrentPage.PageLink.ID);

                return _numItemsSold;
            }
        }

        private int _numItemsInStock = -2;
        /// <summary>
        /// -1 = infinite
        /// </summary>
        public int NumItemsInStock 
        {
            get 
            {
                if (_numItemsInStock == -2)
                {
                    if (NumItemsInStockFromStart > 0)
                    {
                        int i = NumItemsInStockFromStart - NumItemsSold;
                        if (i < -1)
                            _numItemsInStock = 0;
                        else
                            _numItemsInStock = i;
                    }
                    else
                        _numItemsInStock = NumItemsInStockFromStart;
                }

                return _numItemsInStock;
            }
        }

        #endregion

        
        protected void Page_Load(object sender, EventArgs e)
        {
            UserMessageControl.ClearMessage();
            
            if (!IsPostBack)
            {
                #region return from Nets
                if (Request.QueryString["responseCode"] != null)
                {
                    NetsCardPayReturn ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                    CardPaymentDataHolder dh = (CardPaymentDataHolder)ret.NetsPreparePersisted.PersistedObj;

                    bool payOk = ret.HandleNetsReturn("", dh.Email);
                    if (payOk)
                    {
                        MsSqlHandler.SaveCardPayment(ret.NetsPreparePersisted.CustomerRefNo, dh);

                        if (SendCustMail && !string.IsNullOrEmpty(CustMailText))
                            MiscFunctions.SendMail("no-reply@di.se", dh.Email, "Di kortköp genomfört", CustMailText, true);

                        if (SendStaffMail && MiscFunctions.IsValidEmail(StaffMailAddress))
                            MiscFunctions.SendMail("no-reply@di.se", StaffMailAddress, "Di kortköp genomfört", ret.ToString(), true);

                        ShowMessInMainBody(ThankYouText);
                    }
                    return;
                }
                #endregion

                string err = GetOnLoadErr();
                if (!string.IsNullOrEmpty(err))
                {
                    UserMessageControl.ShowMessage(err, false, true);
                    PlaceHolderForm.Visible = false;
                    return;
                }

                PriceCalculator pc = new PriceCalculator(PriceIncVat, PriceExVat, Vat, 1);
                LabelPriceItem.Text = pc.PriceIncVat.ToString();
                LabelIncVatPrice.Text = pc.PriceIncVat.ToString();
                LabelExVatPrice.Text = pc.PriceExVat.ToString();

                HandlePlaceHolderNumItems();
                SubscriberDetails1.Visible = !HideFormAddress;
            }
        }

        private string GetOnLoadErr()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(MerchantId.ToString()))
                sb.Append("Ange butiks ID<br>");
            if (string.IsNullOrEmpty(DiDepartment))
                sb.Append("Ange vilken Di avdelning som säljer produkten<br>");
            if (string.IsNullOrEmpty(ItemDescrPaytrans))
                sb.Append("Ange en kortfattad produktbeskrivning<br>");
            if ((PriceIncVat == 0 && PriceExVat == 0) || (PriceIncVat == 0 && Vat == 0) || (PriceExVat == 0 && Vat == 0))
                sb.Append("Ange 2 av prisuppgifterna: pris ink moms / pris ex moms / moms<br>");
            if (NumItemsInStockFromStart == 0 || NumItemsInStock == 0)
                sb.Append(NoItemsInStockText + "<br>");

            return sb.ToString();
        }

        private void HandlePlaceHolderNumItems()
        {
            if (MaxNumItemsPerCust == 1)
            {
                PlaceHolderNumItems.Visible = false;
                return;
            }

            TextBoxNumItems.Text = "1";
            LabelMaxNumInfo.Visible = false;
            if (MaxNumItemsPerCust > 1)
            {
                LabelMaxNumInfo.Text = "(max " + MaxNumItemsPerCust.ToString() + " st/köp)";
                LabelMaxNumInfo.Visible = true;
            }
            PlaceHolderNumItems.Visible = true;
        }

        private void ShowMessInMainBody(string mess)
        {
            Heading1.Visible = false;
            Mainbody1.Text = mess;
            Mainbody1.Visible = true;
            Mainintro1.Visible = false;
            PlaceHolderForm.Visible = false;
        }


        protected void ButtonBuy_Click(object sender, EventArgs e)
        {
            string errNumItems = GetErrNumItems();
            if (!string.IsNullOrEmpty(errNumItems))
            {
                UserMessageControl.ShowMessage(errNumItems, false, true);
                return;
            }

            int numItems = GetNumItems();
            
            PriceCalculator pc = new PriceCalculator(PriceIncVat, PriceExVat, Vat, numItems);

            CardPaymentDataHolder dh = new CardPaymentDataHolder(CurrentPage.PageLink.ID, DiDepartment, ItemDescrPaytrans, numItems, SubscriberDetails1.IsStreetAddress, FirstNameInput.Text, LastNameInput.Text, MobilePhoneInput.Text, EmailInput.Text, pc);
            SubscriberDetails1.PopulateCardPaymentDataHolder(dh);
            
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string buyerName = dh.FirstName + " " + dh.LastName + ", " + dh.PhoneMobile;

            var prep = new NetsCardPayPrepare((double)pc.PriceIncVat, pc.VatAmount, pc.VatPct, true, false, url, "Avd: " + DiDepartment, ItemDescrPaytrans, buyerName, dh.Email, null, PaymentMethod.TypeOfPaymentMethod.CreditCard, dh);
        }

        private string GetErrNumItems()
        {
            if (TextBoxNumItems.Visible)
            {
                int numItems = 1;
                bool isNum = int.TryParse(TextBoxNumItems.Text, out numItems);
                
                if (!isNum || numItems <= 0)
                    return "Var god ange antal";

                if (MaxNumItemsPerCust > 0 && numItems > NumItemsInStock)
                    return "Var god ange antal mellan 1 och " + NumItemsInStock.ToString();

                if (MaxNumItemsPerCust > 0 && numItems > MaxNumItemsPerCust)
                    return "Var god ange antal mellan 1 och " + MaxNumItemsPerCust.ToString();
            }

            return string.Empty;
        }

        private int GetNumItems()
        {
            int numItems = 1;
            if (TextBoxNumItems.Visible)
                int.TryParse(TextBoxNumItems.Text, out numItems);

            return numItems;
        }


    }
}