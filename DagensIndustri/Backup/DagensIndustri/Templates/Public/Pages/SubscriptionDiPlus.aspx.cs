using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;

using DIClassLib.CardPayment.Nets;

using EPiServer;
using DIClassLib.Subscriptions;
using DIClassLib.Subscriptions.DiPlus;
using DagensIndustri.Tools.Classes;
using DIClassLib.CardPayment;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using System.Text;
using System.Data;
using DIClassLib.CardPayment.Autowithdrawal;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class SubscriptionDiPlus : DiTemplatePage
    {

        public DiPlusCampaign SelectedCamp
        {
            get
            {
                if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs)
                    return DiPlusCampaigns.StandAlone;

                if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.UpgradeDi6DaySubs)
                    return DiPlusCampaigns.Upgrade6Days;

                if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.UpgradeWeekendSubs)
                    return DiPlusCampaigns.UpgradeWeekend;

                return null;
            }
        }

        public DiPlusSubscription PlusSub 
        {
            get 
            {
                if (ViewState["plusSub"] == null)
                    ViewState["plusSub"] = new DiPlusSubscription();

                return (DiPlusSubscription)ViewState["plusSub"];
            }
            set
            {
                ViewState["plusSub"] = value;
            }
        }

        public void SavePlusSubViewState()
        {
            ViewState["plusSub"] = PlusSub;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["responseCode"] != null)
                {
                    NetsCardPayReturn ret = new NetsCardPayReturn();
                    PlusSub = (DiPlusSubscription)ret.NetsPreparePersisted.PersistedObj;
                    bool payOk = ret.HandleNetsReturn("", PlusSub.Email);
                    
                    if (!payOk)
                    {
                        //HandleCardPayFailed(ret);
                        HandleCardPayFailed();
                        return;
                    }
                    else
                    {
                        Subscription sub = GetSubsObj();
                        sub.CreditCardPaymentOk = true;

                        #region add new cust and sub
                        if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs)
                        {
                            CirixDbHandler.TryInsertSubscription2(sub, PlusSub);
                            PlusSub.SubsNo = sub.SubsNo;
                        }
                        #endregion

                        #region add new sub on existing cust
                        if (PlusSub.SubsType != DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs)
                        {
                            string newSubsRet = CirixDbHandler.AddNewSubs2(sub, 0, PlusSub);  //new subsno returned on success
                            if (MiscFunctions.IsNumeric(newSubsRet))
                            {
                                PlusSub.SubsNo = long.Parse(newSubsRet);
                                CirixDbHandler.AddNewCusInvmode(PlusSub.Cusno, Settings.InvoiceMode_KontoKort, false);
                                SendStaffMailUpdateInvMode(sub);
                            }
                            else
                                MiscFunctions.SendStaffMailFailedSaveSubs("Lägga på ny pren på befintlig kund misslyckades. Cirixfel: " + newSubsRet, sub, PlusSub);
                        }
                        #endregion

                        PlusSub.Cusno = sub.Subscriber.Cusno;
                        CirixDbHandler.CreateNewInvoice(PlusSub.SubsNo, Settings.PaperCode_IPAD);
                        MsSqlHandler.InsertAwd2(ret, PlusSub.Cusno, PlusSub.SubsNo, SelectedCamp.Campno, sub.SubsEndDate);
                        MsSqlHandler.AppendToPayTransComment(ret.NetsPreparePersisted.CustomerRefNo, "cusno: " + PlusSub.Cusno);


                        string s = "1";
                        if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.UpgradeDi6DaySubs)
                            s = "2"; 
                        if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.UpgradeWeekendSubs)
                            s = "3";

                        Response.Redirect(CurrentPage.LinkURL + "&pay=" + s);
                    }
                }

                if (Request.QueryString["pay"] != null)
                {
                    ShowStep(DiPlusSubsSteps.Steps._3_ThankYou);
                    return;
                }


                ShowStep(DiPlusSubsSteps.Steps._1_SelectSubsType);
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //LiteralPlusSub.Text = PlusSub.ToString();
        }


        #region events called from user controls

        //step: select new or upgrade subs
        public void SetIsNewSubs(bool isNewSubs)
        {
            PlusSub.IsNewSubs = isNewSubs;
            
            if(isNewSubs) 
                PlusSub.SubsType = DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs;

            SavePlusSubViewState();
            ShowStep(DiPlusSubsSteps.Steps._2_PersonForm);
        }

        //step: go to Nets
        public void PersonFormButtonClicked(long cusno, string company, string firstName, string lastName, string phoneMob, string email, string passwd)
        {
            PlusSub.Cusno = cusno;
            PlusSub.Company = company;
            PlusSub.FirstName = firstName;
            PlusSub.LastName = lastName;
            PlusSub.PhoneMob = phoneMob;
            PlusSub.Email = email;
            PlusSub.Passwd = passwd;
            SavePlusSubViewState();

            PriceCalculator pc = new PriceCalculator(null, SelectedCamp.PriceExVat, Settings.VatIpad, 1);
            PlusSub.SetPaymentMembers(CurrentPage.PageLink.ID, "Avd: Upplaga", "Di+ pren via Di+ flödet, autobetalas nästa gång", pc);
            SavePlusSubViewState();
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string buyerName = PlusSub.FirstName + " " + PlusSub.LastName + ", " + PlusSub.PhoneMob;
                
            //send user to Nets
            var prep = new NetsCardPayPrepare((double)pc.PriceIncVat, pc.VatAmount, pc.VatPct, true, false, url, PlusSub.DiDepartment, PlusSub.ItemDescr, buyerName, PlusSub.Email, null, PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal, PlusSub);
        }

        #endregion


        #region helpers

        private void ShowStep(DiPlusSubsSteps.Steps step)
        {
            PlaceHolderIpad.Visible = true;
            PlaceHolderInvoice.Visible = false;

            _1_SelectSubsType1.Visible = false;
            _2_PersonForm1.Visible = false;
            _3_ThankYou1.Visible = false;
            _4_Error1.Visible = false;

            switch (step)
            {
                case DiPlusSubsSteps.Steps._1_SelectSubsType:
                    _1_SelectSubsType1.Visible = true;
                    break;
                case DiPlusSubsSteps.Steps._2_PersonForm:
                    bool isNewSubs = PlusSub.IsNewSubs;
                    _2_PersonForm1.HandleFormVisibility(!isNewSubs, isNewSubs, !isNewSubs, isNewSubs);
                    if (!isNewSubs)
                    {
                        PlaceHolderIpad.Visible = false;
                        PlaceHolderInvoice.Visible = true;
                    }
                    _2_PersonForm1.Visible = true;
                    break;
                case DiPlusSubsSteps.Steps._3_ThankYou:
                    _3_ThankYou1.Visible = true;
                    break;
                case DiPlusSubsSteps.Steps._4_Error:
                    _4_Error1.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void HandleCardPayFailed()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Betalning med kort misslyckades.");

            //if (!string.IsNullOrEmpty(ret.AurigaMessage))
            //    sb.Append("<br><br>Detaljerat fel:<br>" + ret.AurigaMessage);

            DisplayErr(sb.ToString());
        }

        private Subscription GetSubsObj()
        {
            DiPlusSubscription ps = PlusSub;
            Subscription sub = new Subscription(string.Empty, SelectedCamp.Campno, ps.PayMethod, DateTime.Now, false);
            sub.Subscriber = new Person(true, true, ps.FirstName, ps.LastName, "", ps.Company, "", "", "", "", "", ps.Zip, "", ps.PhoneMob, ps.Email, "", "", "", "");
            sub.Subscriber.Cusno = ps.Cusno;
            
            if(string.IsNullOrEmpty(ps.Company))
                sub.Subscriber.CirixName2 = ps.Email;   //just to make user unique in Cirix

            return sub;
        }

        private void SendStaffMailUpdateInvMode(Subscription sub)
        {
            string mail = MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe");

            StringBuilder body = new StringBuilder();
            body.Append("Eftersom det inte går att sätta en viss betalningstyp på en specifik prenumeration från Cirix webbservice ");
            body.Append("måste detta göras manuellt. Sätt betalningstyp 'kontokort' för nyligen skapad IPAD-prenumeration nedan.<br><br>");
            body.Append("<hr>" + sub.ToString() + "<hr>");
            body.Append(PlusSub.ToString() + "<hr>");

            MiscFunctions.SendMail(mail, mail, "Ändra betalningstyp på pren", body.ToString(), true);
        }

        private void DisplayErr(string err)
        {
            _4_Error1.ErrMess = err;
            ShowStep(DiPlusSubsSteps.Steps._4_Error);
        }

        #endregion


        #region old code
        //public double PriceStandAloneDiPlus
        //{
        //    get
        //    {
        //        if (ViewState["Price1MonNew"] == null)
        //            ViewState["Price1MonNew"] = GetCampPriceExVat(CampnoDiPlusStandAlone);

        //        return (double)ViewState["Price1MonNew"];
        //    }
        //}

        //public double PriceUpgDi6DaySub
        //{
        //    get
        //    {
        //        if (ViewState["Price1MonUpg"] == null)
        //            ViewState["Price1MonUpg"] = GetCampPriceExVat(CampnoUpgDi6DaySubs);

        //        return (double)ViewState["Price1MonUpg"];
        //    }
        //}

        //public double PriceUpgWeekendSub
        //{
        //    get
        //    {
        //        if (ViewState["Price1MonUpgWeekend"] == null)
        //            ViewState["Price1MonUpgWeekend"] = GetCampPriceExVat(CampnoUpgWeekend);

        //        return (double)ViewState["Price1MonUpgWeekend"];
        //    }
        //}

        //private double GetCampPriceExVat(long campno)
        //{
        //    return 420;

        //    double price = 0;
        //    if (campno > 0)
        //    {
        //        DataSet ds = CirixDbHandler.GetCampaign(campno);
        //        double.TryParse(ds.Tables[0].Rows[0]["TOTALPRICE"].ToString(), out price);
        //    }

        //    return price;
        //}

        //public double GetPriceIncVat(double priceExVat)
        //{
        //    return Math.Round((priceExVat * (1 + (Settings.VatIpad / 100))), MidpointRounding.AwayFromZero);
        //}
        

        //public long CampnoDiPlusStandAlone
        //{
        //    get
        //    {
        //        return long.Parse(MiscFunctions.GetAppsettingsValue("diPlusSubsnoNewStandAlone"));
        //    }
        //}

        //public long CampnoUpgDi6DaySubs
        //{
        //    get
        //    {
        //        return long.Parse(MiscFunctions.GetAppsettingsValue("diPlusSubsnoUpgrade6Day"));
        //    }
        //}

        //public long CampnoUpgWeekend
        //{
        //    get
        //    {
        //        return long.Parse(MiscFunctions.GetAppsettingsValue("diPlusSubsnoUpgradeWeekend"));
        //    }
        //}

        //public long FinalCampno
        //{
        //    get
        //    {
        //        if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.StandAlonePlusSubs)
        //            return DiPlusCampaigns.StandAlone.Campno;       //CampnoDiPlusStandAlone;
        //        if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.UpgradeDi6DaySubs)
        //            return DiPlusCampaigns.Upgrade6Days.Campno;     //CampnoUpgDi6DaySubs;
        //        if (PlusSub.SubsType == DiPlusSubscriptionType.PlusSubsType.UpgradeWeekendSubs)
        //            return DiPlusCampaigns.UpgradeWeekend.Campno;   //CampnoUpgWeekend;

        //        return 0;
        //    }
        //}
        
        
        //if (EPiFunctions.HasValue(CurrentPage, "campno1MonNew"))
        //{
        //    long i = 0;
        //    if (long.TryParse(CurrentPage["campno1MonNew"].ToString(), out i))
        //        return i;
        //}
        //return 0;
        
        //if (EPiFunctions.HasValue(CurrentPage, "campno1MonUpg"))
        //{
        //    long i = 0;
        //    if (long.TryParse(CurrentPage["campno1MonUpg"].ToString(), out i))
        //        return i;
        //}
        //return 0;

        //    if (EPiFunctions.HasValue(CurrentPage, "campno1MonUpgWeekend"))
        //    {
        //        long i = 0;
        //        if (long.TryParse(CurrentPage["campno1MonUpgWeekend"].ToString(), out i))
        //            return i;
        //    }
        //    return 0;


        //step: select private or comp
        //public void SetIsCompanyCust(bool isCompanyCust)
        //{
        //PlusSub.IsCompanyCust = isCompanyCust;
        //SavePlusSubViewState();
        //ShowStep(DiPlusSubsSteps.Steps._2_SelectSubsType);
        //}

        //step: pay by invoice
        //public void CreateInvoiceSubs(string adr_company, string adr_companyNum, string adr_streetAdress, string adr_streetNumber, string adr_entrance, 
        //                                string adr_stairs, string adr_appartmentNum, string adr_zip, string adr_city, string adr_stopOrBox, 
        //                                string adr_stopOrBoxNum, string adr_stopOrBoxZip, string adr_stopOrBoxCity)
        //{
        //    PlusSub.Adr_company = adr_company;
        //    PlusSub.Adr_companyNum = adr_companyNum;
        //    PlusSub.Adr_streetAdress = adr_streetAdress;
        //    PlusSub.Adr_streetNumber = adr_streetNumber;
        //    PlusSub.Adr_entrance = adr_entrance;
        //    PlusSub.Adr_stairs = adr_stairs;
        //    PlusSub.Adr_appartmentNum = adr_appartmentNum;
        //    PlusSub.Adr_zip = adr_zip;
        //    PlusSub.Adr_city = adr_city;
        //    PlusSub.Adr_stopOrBox = adr_stopOrBox;
        //    PlusSub.Adr_stopOrBoxNum = adr_stopOrBoxNum;
        //    PlusSub.Adr_stopOrBoxZip = adr_stopOrBoxZip;
        //    PlusSub.Adr_stopOrBoxCity = adr_stopOrBoxCity;
        //    SavePlusSubViewState();

        //    Subscription sub = GetSubsObj(PlusSub);
        //    string err = TryAddSubToCirix(sub);
        //    if (err.Length > 0)
        //    {
        //        DisplayErr(err);
        //        return;
        //    }

        //    ShowStep(DiPlusSubsSteps.Steps._5_ThankYou);
        //}
        #endregion

    }
}