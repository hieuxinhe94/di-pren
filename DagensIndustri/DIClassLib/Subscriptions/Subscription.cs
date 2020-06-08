using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using Microsoft.VisualBasic;


namespace DIClassLib.Subscriptions
{
    [Serializable]
    public class Subscription : IComparable<Subscription>
    {
        #region Properties
        public PaymentMethod.TypeOfPaymentMethod PayMethod { get; private set; }

        public string CancelReason { get; set; }
        public string PackageId { get; set; }
        public long SubsNo { get; set; }
        public long CampNo { get; set; }                //1116
        public DateTime InvStartDate { get; set; }
        public DateTime SubsStartDate { get; set; }
        public DateTime SubsEndDate { get; set; }
        public string InvoiceMode { get; set; }

        public string PaperCode { get; set; }           //DI, IPAD
        public string Substype = "01";                  //Rabattkategori, always 01
        public string ProductNo { get; set; }           //01=full week, 05=weekend
        public string SubsKind { get; set; }

        public int SubsLenMons { get; set; }
        public int SubsLenDays { get; set; }

        public string Pricegroup { get; set; }
        public double TotalPriceExVat { get; set; }
        public double ItemPrice { get; set; }
        public string DirectDebit { get; set; }
        public int ItemQty { get; set; }
        public int DiscPercent { get; set; }

        public Person Subscriber { get; set; }
        public Person SubscriptionPayer { get; set; }

        public string CampId { get; set; }              //N100005E
        public string SubsState { get; set; }
        public DateTime SuspendDate { get; set; }

        public int ExtNo { get; set; }
        public DateTime SubsRealEndDate 
        {
            get
            {
                if (MiscFunctions.DateHasBeenSet(SuspendDate) && SuspendDate < SubsEndDate)
                    return SuspendDate;

                return SubsEndDate;
            }
        }
        public string TargetGroup { get; set; }

        public int StandItemQty { get; set; }
        public int PerItemQty { get; set; }
        public int SubsLength { get; set; }
        public string LengthUnit { get; set; }

        public string CoProdIncl { get; set; }          //'Y' if camp comes with knife etc

        public bool CreditCardPaymentOk = false;

        public String ProductName
        {
            get
            {
                return Settings.GetName_Product(PaperCode, ProductNo);
            }
        }

        public bool IsTrialPeriod { get; set; }

        public bool IsTrialFreePeriod { get; set; }

        #endregion


        #region Constructors

        public Subscription()
        {
            Subscriber = new Person(true);
            SubscriptionPayer = new Person(false);
        }


        /// <summary>
        /// constructor - campId OR campNo needed to populate this object
        /// </summary>
        public Subscription(string campId, long campNo, PaymentMethod.TypeOfPaymentMethod payMet, DateTime wantedStartDate, bool isTrialPeriod, bool isTrialFreePeriod = false)
        {
            DataSet dsCamp;
            if (!string.IsNullOrEmpty(campId))
            {
                CampId = campId;
                dsCamp = SubscriptionController.GetCampaign(CampId);
            }
            else
            {
                CampNo = campNo;
                dsCamp = SubscriptionController.GetCampaign(CampNo);
            }

            IsTrialPeriod = isTrialPeriod;
            IsTrialFreePeriod = isTrialFreePeriod;
            PayMethod = payMet;
            SetMembersByPayMethod(PayMethod);
            TargetGroup = "";
            PopulateMembersFromCirix(dsCamp, wantedStartDate);
        }

        /// <summary>
        /// constructor - campaigns
        /// </summary>
        //public Subscription(string campId, bool isAutogiro)
        //{
        //    if (isAutogiro)
        //    {
        //        PayMethod = PaymentMethod.TypeOfPaymentMethod.DirectDebit;
        //        SetMembersByPayMethod(PayMethod);
        //    }

        //    CampId = campId;
        //    PopulateMembers(CampId, DateTime.Now);
        //}

        #endregion


        #region Methods

        private void PopulateMembersFromCirix(DataSet dsCamp, DateTime wantedStartDate)
        {
            try
            {
                if (DbHelpers.DbHelpMethods.DataSetHasRows(dsCamp))
                {
                    PackageId = DbHelpMethods.ValueIfColumnExist(dsCamp.Tables[0], dsCamp.Tables[0].Rows[0], "PACKAGEID", string.Empty);
                    CampId = dsCamp.Tables[0].Rows[0]["CAMPID"].ToString();
                    CampNo = long.Parse(dsCamp.Tables[0].Rows[0]["CAMPNO"].ToString());
                    TotalPriceExVat = double.Parse(dsCamp.Tables[0].Rows[0]["TOTALPRICE"].ToString());
                    var packageProducts = SubscriptionController.GetPackageProducts(PackageId);
                    PaperCode = packageProducts[0].PaperCode; //dsCamp.Tables[0].Rows[0]["PAPERCODE"].ToString(); DO NOT USE PAPERCODE FROM CAMPAIGN
                    ProductNo = packageProducts[0].ProductNo; //dsCamp.Tables[0].Rows[0]["PRODUCTNO"].ToString(); DO NOT USE PRODUCTNO FROM CAMPAIGN
                    
                    var vatIncluded = DbHelpMethods.ValueIfColumnExist(dsCamp.Tables[0], dsCamp.Tables[0].Rows[0], "VATINCLUDED", "N");
                    if (vatIncluded.Equals("Y"))
                    {
                        var vat = (SubscriptionController.GetProductVat(PaperCode, ProductNo) / 100);                        
                        TotalPriceExVat = TotalPriceExVat/(vat + 1);
                    }

                    DiscPercent = DbHelpMethods.ValueIfColumnExist(dsCamp.Tables[0], dsCamp.Tables[0].Rows[0], "DISCPERCENT", 0);
                    StandItemQty = int.Parse(dsCamp.Tables[0].Rows[0]["STAND_ITEMQTY"].ToString());
                    PerItemQty = int.Parse(dsCamp.Tables[0].Rows[0]["PER_ITEMQTY"].ToString());
                    SubsKind = StandItemQty > 0 ? Settings.SubsKind_tillsvidare : Settings.SubsKind_tidsbestamd;
                    ItemQty = StandItemQty > 0 ? StandItemQty : PerItemQty;
                    ItemPrice = (ItemQty > 0)
                        ? Math.Round(TotalPriceExVat/ItemQty, MidpointRounding.AwayFromZero)
                        : TotalPriceExVat;

                    string co = dsCamp.Tables[0].Rows[0]["COPRODINCL"].ToString();
                    CoProdIncl = string.IsNullOrEmpty(co) ? "N" : co;

                    DateTime tmp_frst = DateTime.MinValue;
                    DateTime.TryParse(dsCamp.Tables[0].Rows[0]["STARTDATE_FRST"].ToString(), out tmp_frst);
                    if (tmp_frst > DateTime.Now.Date && tmp_frst > wantedStartDate)
                        wantedStartDate = tmp_frst;

                    SubsStartDate = SubscriptionController.GetNextIssueDateIncDiRules(wantedStartDate, PaperCode,
                        ProductNo);
                    //SubsEndDate = DateTime.Parse(dsCamp.Tables[0].Rows[0]["SUBSENDDATE"].ToString()); //1800-01-01
                    SubsEndDate = DbHelpMethods.ValueIfColumnExist(dsCamp.Tables[0], dsCamp.Tables[0].Rows[0], "SUBSENDDATE", new DateTime(1800, 1, 1));
                    LengthUnit = dsCamp.Tables[0].Rows[0]["LENGTHUNIT"].ToString();
                    SubsLength = int.Parse(dsCamp.Tables[0].Rows[0]["SUBSLENGTH"].ToString());
                    SetSubsDateMembers(false);
                  
                    //handle autogiro
                    if (PayMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit)
                    {
                        ItemQty = SubsLenMons;
                        ItemPrice = Math.Round(TotalPriceExVat/ItemQty, MidpointRounding.AwayFromZero);
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("Subscription.PopulateMembersFromCirix() failed", ex.Message);
            }
        }

        public void SetSubsDateMembers(bool forceNewEndDate)
        {
            //end date not set to real value
            if (forceNewEndDate || SubsEndDate == new DateTime(1800, 01, 01))
            {
                switch (LengthUnit)
                {
                    case "YY":
                        SubsEndDate = SubsStartDate.AddYears(SubsLength);
                        break;
                    case "WW":
                        SubsEndDate = SubsStartDate.AddDays(SubsLength * 7);
                        break;
                    case "DD":
                        SubsEndDate = SubsStartDate.AddDays(SubsLength);
                        break;
                    default:    //assuming "MM" for months
                        SubsEndDate = SubsStartDate.AddMonths(SubsLength);
                        break;
                }
            }

            SetSubsLenMembers();
        }

        private void SetSubsLenMembers()
        {
            int mons = 0;
            int days = 0;
            
            mons = int.Parse(DateAndTime.DateDiff(DateInterval.Month, SubsStartDate, SubsEndDate).ToString());
            if (SubsStartDate.Day > SubsEndDate.Day)
                mons--;

            days = int.Parse(DateAndTime.DateDiff(DateInterval.Day, SubsStartDate.AddMonths(mons), SubsEndDate).ToString());

            SubsLenMons = (mons > -1) ? mons : 0;
            SubsLenDays = (days > -1) ? days : 0;
        }


        /// <summary>
        /// Call from campaign buttonclick to set PayMethod, DirectDebit, Pricegroup, InvoiceMode
        /// </summary>
        public void SetMembersByPayMethod(PaymentMethod.TypeOfPaymentMethod payMethod)
        {
            PayMethod = payMethod;
            DirectDebit = (PayMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit) ? "Y" : "N";
            Pricegroup = GetPiceGroup();
            InvoiceMode = GetInvoiceMode(PayMethod);
        }

        private string GetPiceGroup()
        {
            if (IsTrialPeriod)
                return MiscFunctions.GetAppsettingsValue("PriceGroupTrialPeriod");

            if (IsTrialFreePeriod)
                return Settings.PriceGroupTrialFree;

            return (DirectDebit == "N") ? "00" : "25";    //old value from getSubchoices2
        }
        
        private string GetInvoiceMode(PaymentMethod.TypeOfPaymentMethod payMethod)
        {
            switch (payMethod)
            {
                case PaymentMethod.TypeOfPaymentMethod.Invoice:
                case PaymentMethod.TypeOfPaymentMethod.InvoiceOtherPayer:
                    return SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak ? Settings.InvoiceModeKayakBankGiro : Settings.InvoiceMode_BankGiro;
                case PaymentMethod.TypeOfPaymentMethod.DirectDebit:
                    return SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak ? Settings.InvoiceModeKayakAutoGiro : Settings.InvoiceMode_AutoGiro;
                case PaymentMethod.TypeOfPaymentMethod.CreditCard:
                case PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal:
                    return SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak ? Settings.InvoiceModeKayakCreditCard : Settings.InvoiceMode_KontoKort;
                default:
                    return string.Empty;
            }
        }


        /// <summary>
        /// used when List<Subscription>.Sort() is called
        /// </summary>
        public int CompareTo(Subscription other)
        {
            //return SubsRealEndDate.CompareTo(other.SubsRealEndDate);  //ASC
            return -SubsRealEndDate.CompareTo(other.SubsRealEndDate);   //DESC
        }

        
        private string GetCampId()
        {
            if (CampNo > 0)
            {
                try
                {
                    return SubscriptionController.GetCampaign(CampNo).Tables[0].Rows[0]["CAMPID"].ToString();
                }
                catch (Exception ex)
                {
                    new Logger("GetCampId() failed", ex.ToString());
                }
            }

            return string.Empty;
        }

        
        /// <summary>
        /// Creates a string with all the values in this subscription
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                if (string.IsNullOrEmpty(CampId))
                    CampId = GetCampId();

                StringBuilder sb = new StringBuilder();

                if(Subscriber != null)
                    sb.AppendFormat("{0}<hr>", Subscriber.ToString());

                if (SubscriptionPayer != null)
                    sb.AppendFormat("{0}<hr>", SubscriptionPayer.ToString());

                sb.Append("<b>Prenumerationsdetaljer</b><br>");
                sb.AppendFormat("Kampanj: {0}<br>", CampId);
                sb.AppendFormat("Prennr: {0}<br>", SubsNo);
                sb.AppendFormat("Målgrupp: {0}<br>", TargetGroup);
                sb.AppendFormat("Produkt: {0}<br>", Settings.GetName_Product(PaperCode, ProductNo));
                sb.AppendFormat("Prenstart: {0}<br>", SubsStartDate.ToShortDateString());
                sb.AppendFormat("Prenslut: {0}<br>", SubsEndDate.ToShortDateString());
                sb.AppendFormat("Prenlängd (mån): {0}<br>", SubsLenMons);
                sb.AppendFormat("Prentyp: {0}<br>", Settings.GetName_SubsKind(SubsKind));
                sb.AppendFormat("Pris: {0}<br>", TotalPriceExVat);
                sb.AppendFormat("Betalningssätt: {0}<br>", Settings.GetName_InvoiceMode(InvoiceMode));
                
                if(CreditCardPaymentOk)
                    sb.AppendFormat("<font color='red'>Kortbetalning har genomförts</font><br>");

                sb.Append("<i>");
                sb.AppendFormat("CampaignNo: {0}<br>", CampNo);
                sb.AppendFormat("Papercode: {0}<br>", PaperCode);
                sb.AppendFormat("ProductNo: {0}<br>", ProductNo);
                sb.AppendFormat("Substype: {0}<br>", Substype);
                sb.AppendFormat("Pricegroup: {0}<br>", Pricegroup);
                sb.AppendFormat("ItemPrice: {0}<br>", ItemPrice);
                sb.AppendFormat("DirectDebit: {0}<br>", DirectDebit);
                sb.AppendFormat("ItemQty: {0}<br>", ItemQty);
                sb.AppendFormat("DiscPercent: {0}<br>", DiscPercent);
                sb.Append("</i><hr>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                string exc = ex.ToString();
                new Logger("Subscription/ToString() - failed", exc);
                return "Subscription/ToString() misslyckades, fel:<br><br>" + exc;
            }
        }


        //private void PopulateMembers(long campNo, DateTime wantedStartDate)
        //{
        //    PopulateMembersFromCirixHelper(CirixDbHandler.GetCampaign(campNo), wantedStartDate);
        //}

        //private void PopulateMembers(string campId, DateTime wantedStartDate)
        //{
        //    PopulateMembersFromCirixHelper(CirixDbHandler.GetCampaign(campId), wantedStartDate);
        //}

        //private DateTime TryGetNextIssueDate(string paperCode, string productNo, DateTime minDate)
        //{
        //    try
        //    {
        //        DateTime dt = CirixDbHandler.Ws.GetNextIssueDate_(paperCode, productNo, minDate);
        //        return (dt > minDate) ? dt : minDate;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("TryGetNextIssueDate() failed for paperCode:" + paperCode + ", productNo:" + productNo + ", minDate:" + minDate, ex.ToString());
        //        return minDate;
        //    }
        //}

        //private DateTime GetSubsMinStartDateByDiRules(string productNo)
        //{
        //    DateTime now = DateTime.Now;
        //    DateTime dt;

        //    if (productNo == Settings.ProductNo_Regular)
        //    {
        //        #region regular subs
        //        switch (now.DayOfWeek)
        //        {
        //            case DayOfWeek.Monday:
        //            case DayOfWeek.Tuesday:
        //            case DayOfWeek.Wednesday:
        //                dt = now.AddDays(3);
        //                break;
        //            default:
        //                dt = now.AddDays(5);
        //                break;
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        #region weekend subs
        //        switch (now.DayOfWeek)
        //        {
        //            case DayOfWeek.Monday:
        //                dt = now.AddDays(4);
        //                break;
        //            case DayOfWeek.Tuesday:
        //                dt = now.AddDays(3);
        //                break;
        //            case DayOfWeek.Wednesday:
        //                dt = now.AddDays(9);
        //                break;
        //            case DayOfWeek.Thursday:
        //                dt = now.AddDays(8);
        //                break;
        //            case DayOfWeek.Friday:
        //                dt = now.AddDays(7);
        //                break;
        //            case DayOfWeek.Saturday:
        //                dt = now.AddDays(6);
        //                break;
        //            case DayOfWeek.Sunday:
        //                dt = now.AddDays(5);
        //                break;
        //            default:
        //                dt = now;
        //                break;
        //        }
        //        #endregion
        //    }

        //    return Convert.ToDateTime(dt.ToShortDateString());
        //}
        #endregion


    }
}
 
