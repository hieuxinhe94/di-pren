using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;

namespace DIClassLib.Subscriptions
{
    [Serializable]
    public class Subscription2
    {
        #region Properties
        public PaymentMethod.TypeOfPaymentMethod TypeOfPaymentMethod { get; private set; }
        public SubscriptionType.TypeOfSubscription TypeOfSubscription { get; private set; }

        public long CampaignNo { get; private set; }
        public DateTime SubsStartDate { get; set; }
        public DateTime SubsEndDate { get; set; }
        public string InvoiceMode { get; set; }

        public string Papercode { get; private set; }
        public string Substype { get; private set; }
        public string ProductNo { get; private set; }
        public string SubsKind { get; private set; }
        public int SubslenMons { get; private set; }
        public string Pricegroup { get; private set; }
        //public double GrossPrice { get; private set; }
        public double TotalPrice { get; private set; }
        //public double TotalPriceIncVAT { get; private set; }
        public double ItemPrice { get; private set; }
        public string DirectDebit { get; private set; }
        public int ItemQty { get; private set; }
        public int DiscPercent { get; private set; }

        public Person2 Subscriber { get; set; }
        public Person2 SubscriptionPayer { get; set; }

        //public long SubscriberCusNo { get; set; }
        public long SubscriberCusNo
        {
            get { return Subscriber.Cusno; }
            set { Subscriber.Cusno = value; }
        }

        //public long SubscriptionPayerCusNo { get; set; }
        public long SubscriptionPayerCusNo
        {
            get { return SubscriptionPayer.Cusno; }
            set { SubscriptionPayer.Cusno = value; }
        }

        public long SubscriptionNo { get; set; }
        #endregion

        #region Constructor
        public Subscription2(SubscriptionType.TypeOfSubscription subscriptionType, PaymentMethod.TypeOfPaymentMethod paymentMethod, long campaignNo, DateTime orderDate, string pageId)
        {
            TypeOfPaymentMethod = paymentMethod;
            TypeOfSubscription = subscriptionType;
            CampaignNo = campaignNo;

            PopulateMembers(orderDate, pageId);
        }
        #endregion

        #region Methods
        private void PopulateMembers(DateTime orderDate, string pageId)
        {
            bool isDirectDebit = TypeOfPaymentMethod == PaymentMethod.TypeOfPaymentMethod.DirectDebit;
            bool isWeekendProduct = TypeOfSubscription == SubscriptionType.TypeOfSubscription.DiWeekend;
            string paperCode = (TypeOfSubscription == SubscriptionType.TypeOfSubscription.DiPlus) ? Settings.PaperCode_IPAD : Settings.PaperCode_DI;
            string productNo = (TypeOfSubscription == SubscriptionType.TypeOfSubscription.DiWeekend) ? Settings.ProductNo_Weekend : Settings.ProductNo_Regular;

            CampaignObject campaignObject = CampaignObjectHandler.GetCampaignObject(CampaignNo, paperCode, productNo, orderDate, pageId, isDirectDebit);
            if (campaignObject != null)
            {
                Papercode = campaignObject.Papercode;
                Substype = campaignObject.Substype;
                ProductNo = campaignObject.ProductNo;
                SubsKind = campaignObject.SubsKind;
                SubslenMons = campaignObject.SubslenMons;
                Pricegroup = campaignObject.Pricegroup;
                //GrossPrice = campaignObject.GrossPrice;
                TotalPrice = campaignObject.TotalPrice;
                //TotalPriceIncVAT = campaignObject.TotalPriceIncVAT;
                ItemPrice = campaignObject.ItemPrice;
                DirectDebit = campaignObject.DirectDebit;
                ItemQty = campaignObject.ItemQty;
                DiscPercent = campaignObject.DiscPercent;

                //Set start and end subscription dates
                if (ProductNo == Settings.ProductNo_Regular)
                    SubsStartDate = GetRegularSubscriptionDate(orderDate);
                else
                    SubsStartDate = GetWeekendSubscriptionDate(orderDate);

                SubsEndDate = SubsStartDate.AddMonths(SubslenMons);

                //Set InvoiceMode based on type of payment method
                switch (TypeOfPaymentMethod)
                {
                    case PaymentMethod.TypeOfPaymentMethod.Invoice:
                    case PaymentMethod.TypeOfPaymentMethod.InvoiceAnotherAddressee:
                        InvoiceMode = Settings.InvoiceMode_BankGiro;
                        break;
                    case PaymentMethod.TypeOfPaymentMethod.DirectDebit:
                        InvoiceMode = Settings.InvoiceMode_AutoGiro;
                        break;
                    case PaymentMethod.TypeOfPaymentMethod.CreditCard:
                        InvoiceMode = Settings.InvoiceMode_KontoKort;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Rules for Weeked subscription
        /// Order date: Saturday – Tuesday: start date = closest Friday
        /// Order date: Wednesday-Friday: start date = Friday week after order day
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DateTime GetWeekendSubscriptionDate(DateTime dateTime)
        {
            DateTime dt;
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dt = dateTime.AddDays(4);
                    break;
                case DayOfWeek.Tuesday:
                    dt = dateTime.AddDays(3);
                    break;
                case DayOfWeek.Wednesday:
                    dt = dateTime.AddDays(9);
                    break;
                case DayOfWeek.Thursday:
                    dt = dateTime.AddDays(8);
                    break;
                case DayOfWeek.Friday:
                    dt = dateTime.AddDays(7);
                    break;
                case DayOfWeek.Saturday:
                    dt = dateTime.AddDays(6);
                    break;
                case DayOfWeek.Sunday:
                    dt = dateTime.AddDays(5);
                    break;
                default:
                    dt = dateTime;
                    break;
            }

            return Convert.ToDateTime(dt.ToShortDateString());
        }

        /// <summary>
        /// Rules for Regular subscription
        /// Orderdate Monday-Wednesday: start date = Order date + 3 days
        /// Orderdate Thursday – Sunday: start datum = Order date + 5 days
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private DateTime GetRegularSubscriptionDate(DateTime dateTime)
        {
            DateTime dt;
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                    dt = dateTime.AddDays(3);
                    break;
                default:
                    dt = dateTime.AddDays(5);
                    break;
            }

            return Convert.ToDateTime(dt.ToShortDateString());
        }

        /// <summary>
        /// Creates a string with all the values in this subscription
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("<b>{0}</b><br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/subscriptiondetails"));

            sb.AppendFormat("{0}: {1}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/typeofsubscription"), MiscFunctions.GetAttributeDescription(TypeOfSubscription));
            sb.AppendFormat("{0}: {1}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/paymentmethod"), MiscFunctions.GetAttributeDescription(TypeOfPaymentMethod));
            sb.AppendFormat("CampaignNo: {0}<br>", CampaignNo);
            sb.AppendFormat("SubsStartDate: {0}<br>", SubsStartDate.ToShortDateString());
            sb.AppendFormat("SubsEndDate: {0}<br>", SubsEndDate.ToShortDateString());
            sb.AppendFormat("InvoiceMode: {0}<br>", InvoiceMode);
            sb.AppendFormat("Papercode: {0}<br>", Papercode);
            sb.AppendFormat("Substype: {0}<br>", Substype);
            sb.AppendFormat("ProductNo: {0}<br>", ProductNo);
            sb.AppendFormat("SubsKind: {0}<br>", SubsKind);
            sb.AppendFormat("SubslenMons: {0}<br>", SubslenMons);
            sb.AppendFormat("Pricegroup: {0}<br>", Pricegroup);
            sb.AppendFormat("TotalPrice: {0}<br>", TotalPrice);
            //sb.AppendFormat("TotalPriceIncVAT: {0}<br>", TotalPriceIncVAT);
            sb.AppendFormat("ItemPrice: {0}<br>", ItemPrice);
            sb.AppendFormat("DirectDebit: {0}<br>", DirectDebit);
            sb.AppendFormat("ItemQty: {0}<br>", ItemQty);
            sb.AppendFormat("DiscPercent: {0}<br>", DiscPercent);
            sb.AppendFormat("{0} ({1}): {2}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/subscriber"), EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/cirixname1"), Subscriber.CirixName1);
            sb.AppendFormat("CusNo för ({0}): {1}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/subscriber").ToLower(), SubscriberCusNo);
            sb.AppendFormat("{0} ({1}): {2}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/payer"), EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/cirixname1"), SubscriptionPayer != null ? SubscriptionPayer.CirixName1 : string.Empty);
            sb.AppendFormat("Cusno för {0}: {1}<br>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/payer").ToLower(), SubscriptionPayerCusNo);
            sb.AppendFormat("{0}: {1}<hr>", EPiServer.Core.LanguageManager.Instance.Translate("/subscription/email/subscriptionno"), SubscriptionNo);

            return sb.ToString();
        }
        #endregion


        internal void PopulateSubsHistory()
        {
            Subscriber.PopulateSubsHistory();
            SubscriptionPayer.PopulateSubsHistory();
        }

        public bool HasActiveSubs
        {
            get 
            { 
                return (Subscriber.HasActiveSubs || SubscriptionPayer.HasActiveSubs) ? true : false; 
            }
        }

        public string HasActiveSubsMessage 
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Din prenumeration kunde tyvärr inte sparas. Anledning till detta:<br>");
                string hasActiveSubs = " har redan aktiva prenumerationer i vårt system.<br>";

                if (Subscriber.HasActiveSubs)
                    sb.Append("- Prenumeranten" + hasActiveSubs);

                if (SubscriptionPayer.HasActiveSubs)
                    sb.Append("- Betalaren" + hasActiveSubs);

                sb.Append("<br>Kontakta kundtjänst på telefonnummer 08-573 651 00 så hjälper vi dig.");
                
                return sb.ToString();
            }
        }
    }
}
