using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Pren.Web.Business.Data;

namespace Pren.Web.Business.DataAccess.BusinessSubscription
{
    public class EntityBusinessSubscriptionDataHandler : EntityDataHandlerBase, IBusinessSubscriptionDataHandler
    {
        public void AddOrUpdatePrice(string externalProductCode, int price)
        {
            AddOrUpdateEntities(dbContext =>
            {
                dbContext.BusinessSubscriptionPrice.AddOrUpdate(new BusinessSubscriptionPrice
                {
                    ExternalProductCode = externalProductCode,
                    Price = price,
                    LastUpdated = DateTime.Now
                });

                dbContext.SaveChanges();
            });
        }

        public int GetPrice(string externalProductCode)
        {
            return GetEntities(dbContext =>
            {
                var businessSubscriptionPrice = dbContext.BusinessSubscriptionPrice
                    .FirstOrDefault(epc => epc.ExternalProductCode.Equals(externalProductCode));

                if (businessSubscriptionPrice != null)
                {
                    return businessSubscriptionPrice.Price;
                }

                return 0;
            });
        }
    }
}