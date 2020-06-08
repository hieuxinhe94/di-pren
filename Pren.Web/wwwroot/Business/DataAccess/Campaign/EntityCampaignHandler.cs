using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Pren.Web.Business.DataAccess.Campaign.Entities;

namespace Pren.Web.Business.DataAccess.Campaign
{
    public class EntityCampaignHandler : EntityDataHandlerBase, ICampaignDataHandler
    {
        public void AddOrUpdateCampaign(string campaignId, int campaignNumber, string packageId, string paperCode,
            string productNumber, decimal totalPriceExcludningVat, decimal totalPriceIncludningVat, decimal vatAmount,
            decimal vatPercent, string priceGroup, string subsKind, decimal priceForCustomerToPay)
        {
            AddOrUpdateEntities(dbContext =>
            {
                dbContext.Campaign.AddOrUpdate(new Data.Campaign
                {
                    CampaignId = campaignId,
                    CampaignNumber = campaignNumber,
                    PackageId = packageId,
                    PaperCode = paperCode,
                    ProductNumber = productNumber,
                    TotalPriceExcludningVat = totalPriceExcludningVat,
                    TotalPriceIncludningVat = totalPriceIncludningVat,
                    VatAmount = vatAmount,
                    VatPercent = vatPercent,
                    PriceGroup = priceGroup,
                    SubsKind = subsKind,
                    PriceForCustomerToPay = priceForCustomerToPay,
                    LastUpdated = DateTime.Now
                });

                dbContext.SaveChanges();
            });
        }

        public CampaignDataEntity GetCampaign(int campaignNumber, string campaignId)
        {
            return GetEntities(dbContext =>
            {
                var campaign = dbContext.Campaign.FirstOrDefault(t => t.CampaignNumber.Equals(campaignNumber) || t.CampaignId.Equals(campaignId));

                if (campaign != null)
                {
                    return new CampaignDataEntity
                    {
                        CampaignId = campaign.CampaignId,
                        CampaignNumber = campaign.CampaignNumber,
                        PackageId = campaign.PackageId,
                        PaperCode = campaign.PaperCode,
                        ProductNumber = campaign.ProductNumber,
                        TotalPriceExcludningVat = campaign.TotalPriceExcludningVat,
                        TotalPriceIncludningVat = campaign.TotalPriceIncludningVat,
                        VatAmount = campaign.VatAmount,
                        VatPercent = campaign.VatPercent,
                        SubsKind = campaign.SubsKind,
                        PriceGroup = campaign.PriceGroup,
                        PriceForCustomerToPay = campaign.PriceForCustomerToPay ?? campaign.TotalPriceIncludningVat
                    };
                }

                return null;
            });
        }
    }
}