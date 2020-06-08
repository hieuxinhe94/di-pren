using System.Web.Script.Serialization;
using Di.Common.WebRequests;
using Di.ServicePlus.RestApi.ResponseModels.Offer;
using Di.ServicePlus.Utils;

namespace Di.ServicePlus.RestApi.Requests.Offers
{
    internal class Offers : RequestBase, IOffers
    {
        public Offers(string servicePlusApiUrl) : base(servicePlusApiUrl)
        {
        }

        public Offers(string servicePlusApiUrl, IRequestService requestService)
            : base(servicePlusApiUrl, requestService)
        {
        }

        public CreateOrUpdateOfferResponse CreateOrUpdateOffer(
            string userId, 
            string brandId, 
            string productId, 
            string token, 
            string offerType, 
            string subscriptionLenght, 
            bool forceDisplayed, 
            bool accepted,
            string offerOrigin)
        {
            string json;

            // Only add origin param if it has a value
            if (!string.IsNullOrEmpty(offerOrigin))
            {
                json = new JavaScriptSerializer().Serialize(new
                {
                    userId = userId,
                    brandId = brandId,
                    offerType = offerType,
                    productId = productId,
                    subscriptionLength = subscriptionLenght,
                    forceDisplayed = forceDisplayed,
                    accepted = accepted,
                    origin = offerOrigin
                });
            }
            else
            {
                json = new JavaScriptSerializer().Serialize(new
                {
                    userId = userId,
                    brandId = brandId,
                    offerType = offerType,
                    productId = productId,
                    subscriptionLength = subscriptionLenght,
                    forceDisplayed = forceDisplayed,
                    accepted = accepted,
                });
            }

            var response = CreateRequestWithToken(RequestVerb.Post, "offers/create-or-update", token, json);

            return response.ConvertServicePlusJsonToObject<CreateOrUpdateOfferResponse>();
        }
    }
}
