using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DIClassLib.CardPayment;


namespace DIClassLib.Subscriptions.DiPlus
{

    [Serializable]
    public class DiPlusSubscription
    {
        public int EpiPageId { get; set; }
        public string DiDepartment { get; set; }
        public string ItemDescr { get; set; }
        public PriceCalculator PriceCalc { get; set; }
        public Subscription SubToUpgrade { get; set; }

        public long Cusno { get; set; }
        public long SubsNo { get; set; }
        
        public bool IsNewSubs { get; set; }
        public PaymentMethod.TypeOfPaymentMethod PayMethod { get { return PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal; } }
        public DiPlusSubscriptionType.PlusSubsType SubsType { get; set; }
        
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneMob { get; set; }
        public string Email { get; set; }
        public string Passwd { get; set; }
        public string Zip = "10000";
        

        public DiPlusSubscription() { }


        public void SetPaymentMembers(int epiPageId, string diDepartment, string itemDescr, PriceCalculator priceCalc)
        { 
            EpiPageId = epiPageId;
            DiDepartment = diDepartment;
            ItemDescr = itemDescr;
            PriceCalc = priceCalc;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("EpiPageId: " + EpiPageId.ToString() + "<br>");
            sb.Append("DiDepartment: " + DiDepartment + "<br>");
            sb.Append("ItemDescr: " + ItemDescr + "<br>");

            sb.Append("Cusno:" + Cusno + "<br>");
            sb.Append("SubsNo:" + SubsNo + "<br>");
            sb.Append("IsNewSubs:" + IsNewSubs.ToString() + "<br>");
            sb.Append("PayMethod:" + PayMethod.ToString() + "<br>");
            sb.Append("SubsType:" + SubsType.ToString() + "<br>");
            
            sb.Append("Company:" + Company + "<br>");
            sb.Append("FirstName:" + FirstName + "<br>");
            sb.Append("LastName:" + LastName + "<br>");
            sb.Append("PhoneMob:" + PhoneMob + "<br>");
            sb.Append("Email:" + Email + "<br>");

            if (PriceCalc != null)
                sb.Append("<hr><b>PriceCalc:</b><br>" + PriceCalc.ToString() + "<hr>");

            if(SubToUpgrade != null)
                sb.Append("<hr><b>UpgradableSub:</b><br>" + SubToUpgrade.ToString() + "<br>");

            return sb.ToString();
        }



        #region old stuff
        //public string Adr_company { get; set; }
        //public string Adr_companyNum { get; set; }
        //public string Adr_streetAdress { get; set; }
        //public string Adr_streetNumber { get; set; }
        //public string Adr_entrance { get; set; }
        //public string Adr_stairs { get; set; }
        //public string Adr_appartmentNum { get; set; }
        //public string Adr_city { get; set; }
        //public string Adr_stopOrBox { get; set; }
        //public string Adr_stopOrBoxNum { get; set; }
        //public string Adr_stopOrBoxZip { get; set; }
        //public string Adr_stopOrBoxCity { get; set; }

        //public string GetInvoiceAddressInHtml()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(Adr_company + "<br>");

        //    if (string.IsNullOrEmpty(Adr_stopOrBox))
        //    {
        //        sb.Append(Adr_streetAdress + " " + Adr_streetNumber + Adr_entrance + "<br>");
        //        sb.Append(Adr_zip + " " + Adr_city + "<br>");
        //    }
        //    else
        //    {
        //        sb.Append(Adr_stopOrBox + " " + Adr_stopOrBoxNum + "<br>");
        //        sb.Append(Adr_stopOrBoxZip + " " + Adr_stopOrBoxCity + "<br>");
        //    }

        //    return sb.ToString();
        //}

        //sb.Append("Adr_company:" + Adr_company + "<br>");
        //sb.Append("Adr_companyNum:" + Adr_companyNum + "<br>");
        //sb.Append("Adr_streetAdress:" + Adr_streetAdress + "<br>");
        //sb.Append("Adr_streetNumber:" + Adr_streetNumber + "<br>");
        //sb.Append("Adr_entrance:" + Adr_entrance + "<br>");
        //sb.Append("Adr_stairs:" + Adr_stairs + "<br>");
        //sb.Append("Adr_appartmentNum:" + Adr_appartmentNum + "<br>");
        //sb.Append("Adr_zip:" + Adr_zip + "<br>");
        //sb.Append("Adr_city:" + Adr_city + "<br>");

        //sb.Append("Adr_stopOrBox:" + Adr_stopOrBox + "<br>");
        //sb.Append("Adr_stopOrBoxNum:" + Adr_stopOrBoxNum + "<br>");
        //sb.Append("Adr_stopOrBoxZip:" + Adr_stopOrBoxZip + "<br>");
        //sb.Append("Adr_stopOrBoxCity:" + Adr_stopOrBoxCity + "<br>");
        #endregion
        
    }
}
