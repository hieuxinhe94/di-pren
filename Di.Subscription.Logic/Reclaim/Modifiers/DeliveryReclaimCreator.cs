using System;
using System.Globalization;
using System.Linq;
using Di.Subscription.DataAccess.Reclaim;
using Di.Subscription.Logic.Reclaim.Retrievers;

namespace Di.Subscription.Logic.Reclaim.Modifiers
{
    internal class DeliveryReclaimCreator : IDeliveryReclaimCreator
    {
        private readonly IReclaimRepository _reclaimRepository;
        private readonly IReclaimTypeRetriever _reclaimTypeRetriever;

        public DeliveryReclaimCreator(IReclaimRepository reclaimRepository, IReclaimTypeRetriever reclaimTypeRetriever)
        {
            _reclaimRepository = reclaimRepository;
            _reclaimTypeRetriever = reclaimTypeRetriever;
        }

        public string CreateDeliveryReclaim(
            long customerNumber, 
            long subscriptionNumber, 
            int extNo, 
            string paperCode, 
            string reasonId,
            DateTime date)
        {
            var reclaimType = _reclaimTypeRetriever.GetReclaimTypes().First(t => t.Id == int.Parse(reasonId));
            var deliveryReclaim = GetDeliveryReclaim(
                customerNumber,
                subscriptionNumber,
                date,
                extNo,
                paperCode,
                reclaimType);

            return CreateDeliveryReclaim(deliveryReclaim);
        }

        private Types.DeliveryReclaim GetDeliveryReclaim(
            long customerNumber, 
            long subscriptionNumber, 
            DateTime date, 
            int extNo, 
            string paperCode,
            Types.ReclaimType reclaimType)
        {
            return new Types.DeliveryReclaim(
                customerNumber,
                subscriptionNumber,
                date,
                DateTime.Now.AddDays(2),
                extNo,
                paperCode,
                reclaimType.Id,
                reclaimType.ReclaimKind,
                reclaimType.Compensation,
                reclaimType.CarrierMessage,
                reclaimType.OrderNumber.ToString(CultureInfo.InvariantCulture),
                reclaimType.ReclaimText,
                reclaimType.ReclaimPaper ? "Y" : "N");
        }

        private string CreateDeliveryReclaim(Types.DeliveryReclaim deliveryReclaim)
        {
            return _reclaimRepository.CreateDeliveryReclaim(
                deliveryReclaim.CustomerNumber,
                deliveryReclaim.SubscriptionNumber,
                deliveryReclaim.ExtNo,
                deliveryReclaim.PaperCode,
                deliveryReclaim.ReclaimItem,
                deliveryReclaim.ReclaimChannel,
                deliveryReclaim.ReclaimKind,
                deliveryReclaim.PublishDate,
                deliveryReclaim.CreditSubscriber,
                deliveryReclaim.ReclaimMessage,
                deliveryReclaim.DeliveryMessageDate,
                deliveryReclaim.ReclaimCode,
                deliveryReclaim.ReclaimText,
                deliveryReclaim.ResponsiblePerson,
                deliveryReclaim.Language,
                deliveryReclaim.ReclaimPaper,
                deliveryReclaim.UserId,
                deliveryReclaim.DoorCode);
        }
    }
}