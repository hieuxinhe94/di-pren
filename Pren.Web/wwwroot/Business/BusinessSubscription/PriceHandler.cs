using System;
using System.Threading.Tasks;
using Di.Common.Logging;
using Di.Subscription.Logic.ExtraProducts;
using EPiServer.Framework.Cache;
using Pren.Web.Business.Cache;
using Pren.Web.Business.DataAccess;

namespace Pren.Web.Business.BusinessSubscription
{
    public class PriceHandler : IPriceHandler
    {
        private readonly IExtraProductsHandler _extraProductsHandler;
        private readonly IObjectCache _objectCache;
        private readonly ILogger _logService;
        private readonly IDataAccess _dataAccess;

        public PriceHandler(
            IExtraProductsHandler extraProductsHandler,
            IObjectCache objectCache, 
            ILogger logService, IDataAccess dataAccess)
        {
            _extraProductsHandler = extraProductsHandler;
            _objectCache = objectCache;
            _logService = logService;
            _dataAccess = dataAccess;
        }

        public int GetPrice(string externalProductCode)
        {
            // Try to get price from runtime cache
            var cacheKey = "bizOfferPrice" + externalProductCode;

            var cachedPrice = (int?)_objectCache.GetFromCache(cacheKey);

            if (cachedPrice > 0)
            {
                // Price found in runtime cache - return it
                return (int)cachedPrice;
            }

            // If no price in runtime cache - Try to get price from external source (Kayak)
            var price = GetPriceFromExternalSource(externalProductCode);

            if (price > 0)
            {
                // Add price to runtime cache
                AddToRunTimeCache(cacheKey, price, new TimeSpan(1, 0, 0)); //cache for 1 hour

                // Update local db cache async
                UpdateLocalCache(externalProductCode, price);
                return price;
            }
        
            // If price cannot be retrieved from external source (Kayak) we use our local db cache
            price = GetPriceFromLocalCache(externalProductCode);

            if (price > 0)
            {
                // Add price to runtime cache with lower time
                AddToRunTimeCache(cacheKey, price, new TimeSpan(0, 5, 0)); //cache for 5 minutes

                return price;
            }
            
            return price;
        }

        private int GetPriceFromExternalSource(string externalProductCode)
        {
            var price = 0;

            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        var extraProduct = _extraProductsHandler.GetExtraProduct(externalProductCode);

                        if (extraProduct != null)
                        {
                            int.TryParse(extraProduct.Price, out price);
                        }
                    }
                    catch (Exception exception)
                    {
                        _logService.Log(exception, "Failed to get biz Offer price from kayak for externalProductCode: " + externalProductCode, LogLevel.Error, typeof(PriceHandler));
                    }
                });

            task.Wait(TimeSpan.FromSeconds(5));

            return price;
        }

        private int GetPriceFromLocalCache(string externalProductCode)
        {
            return _dataAccess.BusinessSubscriptionHandler.GetPrice(externalProductCode);
        }

        private void AddToRunTimeCache(string cacheKey, int price, TimeSpan timeSpan)
        {
            _objectCache.AddToCache(cacheKey, price, new CacheEvictionPolicy(timeSpan, CacheTimeoutType.Absolute));
        }

        private void UpdateLocalCache(string externalProductCode, int price)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    _dataAccess.BusinessSubscriptionHandler.AddOrUpdatePrice(externalProductCode, price);
                }
                catch (Exception exception)
                {
                    _logService.Log(exception, "Failed to update local cache for product: " + externalProductCode + " with price: " + price, LogLevel.Error, typeof(PriceHandler));
                }
            });

            task.Wait(TimeSpan.FromSeconds(1));
        }
    }
}