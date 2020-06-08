using System;
using System.Threading.Tasks;
using Di.Common.Logging;
using Di.Subscription.Logic.Campaign.Retrievers;
using EPiServer.Framework.Cache;
using Pren.Web.Business.DataAccess;

namespace Pren.Web.Business.Cache.Campaign.Retrievers
{
    public class CachedCampaignRetriever : ICampaignRetriever
    {
        private const int TimeoutInSeconds = 5;

        private readonly ICampaignRetriever _campaignRetriever;
        private readonly IObjectCache _objectCache;
        private readonly ILogger _logService;
        private readonly IDataAccess _dataAccess;

        public CachedCampaignRetriever(ICampaignRetriever campaignRetriever, IObjectCache objectCache, ILogger logService, IDataAccess dataAccess)
        {
            _campaignRetriever = campaignRetriever;
            _objectCache = objectCache;
            _logService = logService;
            _dataAccess = dataAccess;
        }

        public Di.Subscription.Logic.Campaign.Types.Campaign GetCampaign(long campaignNumber)
        {
            return GetCampaign((int)campaignNumber, string.Empty);
        }

        public Di.Subscription.Logic.Campaign.Types.Campaign GetCampaign(string campaignId)
        {
            return GetCampaign(0, campaignId);
        }

        private Di.Subscription.Logic.Campaign.Types.Campaign GetCampaign(int campaignNumber, string campaignId)
        {
            // Try to get campaign from runtime cache
            var cacheKey = "campaign" + campaignNumber + campaignId;

            var cachedCampaign = (Di.Subscription.Logic.Campaign.Types.Campaign)_objectCache.GetFromCache(cacheKey);

            if (cachedCampaign != null)
            {
                // Campaign found in runtime cache - return it
                return cachedCampaign;
            }

            var campaign = GetCampaignFromExternalSource(campaignNumber, campaignId);

            if (campaign != null)
            {
                // Add campaign to runtime cache                
                _objectCache.AddToCache(cacheKey, campaign, new CacheEvictionPolicy(new TimeSpan(1, 0, 0), CacheTimeoutType.Absolute)); //cache for 1 hour

                // Update local db cache async
                UpdateLocalCache(campaign);
                return campaign;
            }

            // If campaign cannot be retrieved from external source (Kayak) we use our local db cache
            campaign = GetCampaignFromLocalCache(campaignNumber, campaignId);

            if (campaign == null) return null;

            // Add campaign to runtime cache with lower time                
            _objectCache.AddToCache(cacheKey, campaign, new CacheEvictionPolicy(new TimeSpan(0, 5, 0), CacheTimeoutType.Absolute)); //cache for 5 minutes

            return campaign;
        }

        private Di.Subscription.Logic.Campaign.Types.Campaign GetCampaignFromExternalSource(int campaignNumber, string campaignId)
        {
            Di.Subscription.Logic.Campaign.Types.Campaign campaign = null;

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        campaign = campaignNumber < 1
                        ? _campaignRetriever.GetCampaign(campaignId)
                        : _campaignRetriever.GetCampaign(campaignNumber);
                    }
                    catch (Exception exception)
                    {
                        _logService.Log(exception, "Failed to get campaign from kayak for campaign: " + campaignId + "(" + campaignNumber + ")", LogLevel.Error, typeof(CachedCampaignRetriever));
                    }
                });

            task.Wait(TimeSpan.FromSeconds(TimeoutInSeconds));

            return campaign;
        }

        private Di.Subscription.Logic.Campaign.Types.Campaign GetCampaignFromLocalCache(int campaignNumber, string campaignId)
        {
            var campaign = _dataAccess.CampaignHandler.GetCampaign(campaignNumber, campaignId);

            if (campaign == null)
            {
                return null;
            }
           
            return new Di.Subscription.Logic.Campaign.Types.Campaign
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
                PriceForCustomerToPay = campaign.PriceForCustomerToPay
            };
        }

        private void UpdateLocalCache(Di.Subscription.Logic.Campaign.Types.Campaign campaign)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    _dataAccess.CampaignHandler.AddOrUpdateCampaign(campaign.CampaignId, campaign.CampaignNumber, campaign.PackageId, campaign.PaperCode, campaign.ProductNumber, campaign.TotalPriceExcludningVat, campaign.TotalPriceIncludningVat, campaign.VatAmount, campaign.VatPercent, campaign.PriceGroup, campaign.SubsKind, campaign.PriceForCustomerToPay);
                }
                catch (Exception exception)
                {
                    _logService.Log(exception, "Failed to update local cache for campaign: " + (campaign != null ? campaign.CampaignId : "null"), LogLevel.Error, typeof(CachedCampaignRetriever));
                }
            });

            task.Wait(TimeSpan.FromSeconds(1));
        }

    }
}