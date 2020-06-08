namespace DIClassLib.Subscriptions.AddCustAndSub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class AddCustAndSubReturnObject
    {
        public AddCustAndSubInData InData;
        
        public long CirixSubscriberCusno { get; set; }
        public long CirixPayerCusno { get; set; }
        public long CirixSubsno { get; set; }
        public bool SavedEntitlementInServicePlus { get; set; }

        public string EmailMessage { get; set; }

        public bool HasMessages { get { return (Messages.Count > 0); } }
        public List<Message> Messages = new List<Message>();
        
        
        public AddCustAndSubReturnObject() { }

        public AddCustAndSubReturnObject(string cirixCampaignId, string cirixTargetGroup, bool subscriberAddressIsRequired, PaymentMethod.TypeOfPaymentMethod paymentMethod,
                                         DateTime wantedStartDate, string servicePlusUserId, Subscriber subscriber, Payer payer = null,
                                         long? cardPayCustRefno = null)
        {
            InData = new AddCustAndSubInData(cirixCampaignId, cirixTargetGroup, subscriberAddressIsRequired, paymentMethod, wantedStartDate, servicePlusUserId, subscriber, payer, cardPayCustRefno);
        }
    
    
        public override string ToString()
        {
            //return base.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append("CirixSubscriberCusno: " + CirixSubscriberCusno + "<br>");
            sb.Append("CirixPayerCusno: " + CirixPayerCusno + "<br>");
            sb.Append("CirixSubsno: " + CirixSubsno + "<br>");
            sb.Append("SavedEntitlementInServicePlus: " + SavedEntitlementInServicePlus.ToString() + "<hr>");
            sb.Append("Messages:<br>");
            foreach (var mess in Messages)
                sb.Append(mess.ToString());

            sb.Append("<div style='background-color:#cccccc;'><hr>EmailMessage:<br>" + EmailMessage + "</div>");
            return sb.ToString();
        }
    
    }


    public class AddCustAndSubInData
    {
        public string CirixCampaignId { get; set; }
        public string CirixTargetGroup { get; set; }
        public bool SubscriberAddressIsRequired { get; set; }
        public PaymentMethod.TypeOfPaymentMethod PaymentMethod { get; set; }
        public DateTime WantedStartDate { get; set; }
        public string ServicePlusUserId { get; set; }
        public Subscriber Subscriber { get; set; }
        public Payer Payer { get; set; }
        public long? CardPayCustRefno { get; set; }

        public AddCustAndSubInData(string cirixCampaignId, string cirixTargetGroup, bool subscriberAddressIsRequired, PaymentMethod.TypeOfPaymentMethod paymentMethod,
                                   DateTime wantedStartDate, string servicePlusUserId, Subscriber subscriber, Payer payer = null,
                                   long? cardPayCustRefno = null)
        {
            CirixCampaignId = cirixCampaignId;
            CirixTargetGroup = cirixTargetGroup;
            SubscriberAddressIsRequired = subscriberAddressIsRequired;
            PaymentMethod = paymentMethod;
            WantedStartDate = wantedStartDate;
            ServicePlusUserId = servicePlusUserId;
            Subscriber = subscriber;
            Payer = payer;
            CardPayCustRefno = cardPayCustRefno;
        }
    }

    

    //public class CardPaymentFacts
    //{
    //    public long CustRefno { get; set; }
    //    public bool CardPayTableUpdated { get; set; }

    //    public CardPaymentFacts() { }
    //}
}
