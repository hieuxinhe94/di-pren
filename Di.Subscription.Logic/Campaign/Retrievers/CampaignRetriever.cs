using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Di.Common.Utils;
using Di.Subscription.DataAccess.Campaign;
using Di.Subscription.Logic.Package.Retrievers;

namespace Di.Subscription.Logic.Campaign.Retrievers
{
    public class CampaignRetriever : ICampaignRetriever
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IPackageRetriever _packageRetriever;

        public CampaignRetriever(ICampaignRepository campaignRepository, IPackageRetriever packageRetriever)
        {
            _campaignRepository = campaignRepository;
            _packageRetriever = packageRetriever;
        }

        public Types.Campaign GetCampaign(long campaignNumber)
        {
            var campaignExt = _campaignRepository.GetCampaign(campaignNumber);
            return GetCampaign(campaignExt);
        }

        public Types.Campaign GetCampaign(string campaignId)
        {
            var campaignSimple = _campaignRepository.GetCampaignSimple(campaignId);
            return GetCampaign(campaignSimple.CampaignNumber);
        }

        private Types.Campaign GetCampaign(DataAccess.Campaign.Campaign campaignExt)
        {
            var campaign =  new Types.Campaign
            {
                CampaignNumber = campaignExt.CampaignNumber,
                CampaignId = campaignExt.CampaignId,
                PackageId = campaignExt.PackageId,
                PriceGroup = campaignExt.PriceGroup
            };

            var product = _packageRetriever.GetProductPackages().FirstOrDefault(t => t.PackageId.Equals(campaignExt.PackageId) && t.MainProduct);
            Contract.Assume(product != null, "No productPackage returned, cannot get VAT");

            campaign.ProductNumber = product.ProductNumber;
            campaign.PaperCode = product.PaperCode;
            campaign.VatPercent = GetProductVatPercent(campaign.PaperCode, campaign.ProductNumber);

            if (campaignExt.VatIncluded == "Y")
            {
                campaign.TotalPriceIncludningVat = campaignExt.TotalPrice;
                campaign.TotalPriceExcludningVat = VatUtil.GetAmountExcludingVat(campaign.TotalPriceIncludningVat, campaign.VatPercent);
            }
            else
            {
                campaign.TotalPriceExcludningVat = campaignExt.TotalPrice;
                campaign.TotalPriceIncludningVat = VatUtil.GetAmountIncludingVat(campaign.TotalPriceExcludningVat, campaign.VatPercent);
            }

            campaign.SubsKindOngoingEnabled = campaignExt.StandItemQuantity > 0;
            campaign.SubsKindTimedEnabled = campaignExt.PerItemQuantity > 0;

            campaign.SubsKind = campaign.SubsKindOngoingEnabled ? SubscriptionConstants.SubsKindOngoing : SubscriptionConstants.SubsKindTimed;

            campaign.VatAmount = campaign.TotalPriceIncludningVat - campaign.TotalPriceExcludningVat;

            if (campaignExt.StandItemQuantity > 1)
            {
                campaign.PriceForCustomerToPay = campaign.TotalPriceIncludningVat / campaignExt.StandItemQuantity;
                campaign.PriceExludingVatForCustomerToPay = campaign.TotalPriceExcludningVat / campaignExt.StandItemQuantity;
            }
            else if (campaignExt.PerItemQuantity > 1)
            {
                campaign.PriceForCustomerToPay = campaign.TotalPriceIncludningVat / campaignExt.PerItemQuantity;
                campaign.PriceExludingVatForCustomerToPay = campaign.TotalPriceExcludningVat / campaignExt.PerItemQuantity;
            }
            else
            {
                campaign.PriceForCustomerToPay = campaign.TotalPriceIncludningVat;
                campaign.PriceExludingVatForCustomerToPay = campaign.TotalPriceExcludningVat;
            }

            campaign.PriceForCustomerToPay = Math.Round(campaign.PriceForCustomerToPay, 0, MidpointRounding.AwayFromZero);
            campaign.PriceExludingVatForCustomerToPay = Math.Round(campaign.PriceExludingVatForCustomerToPay, 0, MidpointRounding.AwayFromZero);

            return campaign;
        }

        private decimal GetProductVatPercent(string paperCode, string productNo)
        {
            return (paperCode == SubscriptionConstants.PaperCodeDi && productNo != SubscriptionConstants.ProductNoIpad)
                ? SubscriptionConstants.VatPercentDi
                : SubscriptionConstants.VatPercentIpad;
        }
    }
}