using Di.ServicePlus.RestApi.ResponseModels.Offer;

namespace Di.ServicePlus.RestApi.Requests.Offers
{
    public interface IOffers
    {
        CreateOrUpdateOfferResponse CreateOrUpdateOffer(
            string userId, 
            string brandId, 
            string productId, 
            string token, 
            string offerType, 
            string subscriptionLenght, 
            bool forceDisplayed, 
            bool accepted,
            string offerOrigin);
    }
}
