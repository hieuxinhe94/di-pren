using System;

namespace Di.Subscription.Logic.Reclaim.Types
{
    internal class DeliveryReclaim
    {
        public DeliveryReclaim(
            long customerNumber,
            long subscriptionNumber,
            DateTime publishDate,
            DateTime deliveryMessageDate,
            int extNo,
            string paperCode,
            int reclaimTypeId,
            string reclaimKind, 
            bool creditSubscriber, 
            bool reclaimMessage,
            string reclaimCode, 
            string reclaimText, 
            string reclaimPaper 
            )
        {
            CustomerNumber = customerNumber;
            SubscriptionNumber = subscriptionNumber;
            PublishDate = publishDate;
            DeliveryMessageDate = deliveryMessageDate;
            ExtNo = extNo;
            PaperCode = paperCode;
            ReclaimItem = reclaimTypeId;
            ReclaimKind = reclaimKind;
            CreditSubscriber = creditSubscriber;
            ReclaimMessage = reclaimMessage;
            ReclaimCode = reclaimCode;
            ReclaimText = reclaimText;
            ReclaimPaper = reclaimPaper;

            // Default values
            Language = SubscriptionConstants.DefaultLanguage;
            ReclaimChannel = ReclaimConstants.DefaultReclaimChannel;
            UserId = SubscriptionConstants.DefaultUserId;
            ResponsiblePerson = ReclaimConstants.DefaultResponsiblePerson;
        }

        internal long CustomerNumber { get; set; }
        internal long SubscriptionNumber { get; set; }
        internal DateTime PublishDate { get; set; }
        internal int ExtNo { get; set; }
        internal string PaperCode { get; set; }
        internal DateTime DeliveryMessageDate { get; set; }
        internal string DoorCode { get; set; }
        internal string Language { get; set; }
        internal string ReclaimChannel { get; set; }
        internal string ResponsiblePerson { get; set; }
        internal string UserId { get; set; }

        internal int ReclaimItem { get; set; }
        internal string ReclaimKind { get; set; }
        internal bool CreditSubscriber { get; set; }
        internal bool ReclaimMessage { get; set; }
        internal string ReclaimCode { get; set; }
        internal string ReclaimText { get; set; }
        internal string ReclaimPaper { get; set; }
    }
}
